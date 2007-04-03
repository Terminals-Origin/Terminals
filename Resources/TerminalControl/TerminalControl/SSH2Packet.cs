/* ---------------------------------------------------------------------------
 *
 * Copyright (c) Routrek Networks, Inc.    All Rights Reserved..
 * 
 * This file is a part of the Granados SSH Client Library that is subject to
 * the license included in the distributed package.
 * You may not use this file except in compliance with the license.
 * 
 * ---------------------------------------------------------------------------
 */

using System;
using System.Collections;
using System.IO;
using System.Threading;
using System.Diagnostics;

using Routrek.SSHC;

namespace Routrek.SSHCV2
{
	/* SSH2 Packet Structure
	 * 
	 * uint32    packet_length
     * byte      padding_length
     * byte[n1]  payload; n1 = packet_length - padding_length - 1
     * byte[n2]  random padding; n2 = padding_length (max 255)
     * byte[m]   mac (message authentication code); m = mac_length
	 * 
	 * 4+1+n1+n2 must be a multiple of the cipher block size
	 */

	internal class SSH2Packet
	{
		private int _packetLength;
		private byte[] _payload;
		private byte[] _padding;
		private byte[] _mac;

		private const int MAX_PACKET_LENGTH = 0x80000; //there was the case that 64KB is insufficient

		public byte[] Data {
			get {
				return _payload;
			}
		}
		//constracts and appends mac
		public void CalcHash(MAC mac, int sequence) {
			byte[] buf = new byte[4+4+_packetLength];
			SSHUtil.WriteIntToByteArray(buf, 0, sequence);
			WriteTo(buf, 4, false);
			
			_mac = mac.Calc(buf);
		}
		public void WriteTo(AbstractSocket strm, Cipher cipher) {
			int bodylen = 4+_packetLength;
			byte[] buf = new byte[bodylen + (_mac==null? 0 : _mac.Length)];
			WriteTo(buf, 0, false);

			if(cipher!=null)
				cipher.Encrypt(buf, 0, bodylen, buf, 0);
			
			if(_mac!=null)
				Array.Copy(_mac, 0, buf, bodylen, _mac.Length);
			
			strm.Write(buf, 0, buf.Length);
			strm.Flush();
		}
		public void WriteTo(byte[] buf, int offset, bool includes_mac) {
			SSHUtil.WriteIntToByteArray(buf, offset, _packetLength);
			buf[offset+4] = (byte)_padding.Length;
			Array.Copy(_payload, 0, buf, offset+5, _payload.Length);
			Array.Copy(_padding, 0, buf, offset+5+_payload.Length, _padding.Length);
			if(includes_mac && _mac!=null)
				Array.Copy(_mac, 0, buf, offset+5+_payload.Length+_padding.Length, _mac.Length);
		}

		public static SSH2Packet FromPlainPayload(byte[] payload, int blocksize, Random rnd) {
			SSH2Packet p = new SSH2Packet();
			int r = 11 - payload.Length % blocksize;
			while(r < 4) r += blocksize;
			p._padding = new byte[r]; //block size is 8, and padding length is at least 4 bytes
			rnd.NextBytes(p._padding);
			p._payload = payload;
			p._packetLength = 1+payload.Length+p._padding.Length;
			return p;
		}
		//no decryption, no mac
		public static SSH2Packet FromPlainStream(byte[] buffer, int offset) {
			SSH2Packet p = new SSH2Packet();
			p._packetLength = SSHUtil.ReadInt32(buffer, offset);
			if(p._packetLength<=0 || p._packetLength>=MAX_PACKET_LENGTH) throw new SSHException(String.Format("packet size {0} is invalid", p._packetLength));
			offset += 4;

			byte pl = buffer[offset++];
			if(pl < 4) throw new SSHException(String.Format("padding length {0} is invalid", pl));
			p._payload = new byte[p._packetLength - 1 - pl];
			Array.Copy(buffer, offset, p._payload, 0, p._payload.Length);
			return p;
		}

		public static SSH2Packet FromDecryptedHead(byte[] head, byte[] buffer, int offset, Cipher cipher, int sequence, MAC mac) {

			SSH2Packet p = new SSH2Packet();
			p._packetLength = SSHUtil.ReadInt32(head, 0);
			if(p._packetLength<=0 || p._packetLength>=MAX_PACKET_LENGTH) throw new SSHException(String.Format("packet size {0} is invalid", p._packetLength));
			SSH2DataWriter buf = new SSH2DataWriter();
			buf.Write(sequence);
			buf.Write(head);
			if(p._packetLength > (cipher.BlockSize - 4)) {
				byte[] tmp = new byte[p._packetLength-(cipher.BlockSize - 4)];
				cipher.Decrypt(buffer, offset, tmp.Length, tmp, 0);
				offset += tmp.Length;
				buf.Write(tmp);
			}
			byte[] result = buf.ToByteArray();
			int padding_len = (int)result[8];
			if(padding_len<4) throw new SSHException("padding length is invalid");

			byte[] payload = new byte[result.Length-9-padding_len];
			Array.Copy(result, 9, payload, 0, payload.Length);
			p._payload = payload;

			if(mac!=null) {
				p._mac = mac.Calc(result);
				if(SSHUtil.memcmp(p._mac, 0, buffer, offset, mac.Size)!=0)
					throw new SSHException("MAC Error");
			}
			return p;
		}
	}

	internal interface ISSH2PacketHandler : IHandlerBase {
		void OnPacket(SSH2Packet packet);
	}

	internal class SynchronizedSSH2PacketHandler : SynchronizedHandlerBase, ISSH2PacketHandler {
		internal ArrayList _packets;

		internal SynchronizedSSH2PacketHandler() {
			_packets = new ArrayList();
		}

		public void OnPacket(SSH2Packet packet) {
			lock(this) {
				_packets.Add(packet);
				if(_packets.Count > 0)
					SetReady();
			}
		}
		public void OnError(Exception error, string msg) {
			base.SetError(msg);
		}
		public void OnClosed() {
			base.SetClosed();
		}

		public bool HasPacket {
			get {
				return _packets.Count>0;
			}
		}
		public SSH2Packet PopPacket() {
			lock(this) {
				if(_packets.Count==0)
					return null;
				else {
					SSH2Packet p = null;
					p = (SSH2Packet)_packets[0];
					_packets.RemoveAt(0);
					if(_packets.Count==0) _event.Reset();
					return p;
				}
			}
		}
	}
	internal class CallbackSSH2PacketHandler : ISSH2PacketHandler {
		internal SSH2Connection _connection;

		internal CallbackSSH2PacketHandler(SSH2Connection con) {
			_connection = con;
		}
		public void OnPacket(SSH2Packet packet) {
			_connection.AsyncReceivePacket(packet);
		}
		public void OnError(Exception error, string msg) {
			_connection.EventReceiver.OnError(error, msg);
		}
		public void OnClosed() {
			_connection.EventReceiver.OnConnectionClosed();
		}
	}

	internal class SSH2PacketBuilder : IByteArrayHandler {
		private ISSH2PacketHandler _handler;
		private byte[] _buffer;
		private byte[] _head;
		private int _readOffset;
		private int _writeOffset;
		private int _sequence;
		private Cipher _cipher;
		private MAC _mac;
		private ManualResetEvent _event;

		public SSH2PacketBuilder(ISSH2PacketHandler handler) {
			_handler = handler;
			_buffer = new byte[0x1000];
			_readOffset = 0;
			_writeOffset = 0;
			_sequence = 0;
			_cipher = null;
			_mac = null;
			_head = null;
		}
		public void SetSignal(bool value) {
			if(_event==null) _event = new ManualResetEvent(true);

			if(value)
				_event.Set();
			else
				_event.Reset();
		}

		public void SetCipher(Cipher c, MAC m) {
			_cipher = c;
			_mac = m;
		}
		public ISSH2PacketHandler Handler {
			get {
				return _handler;
			}
			set {
				_handler = value;
			}
		}

		public void OnData(byte[] data, int offset, int length) {
			try {
				while(_buffer.Length - _writeOffset < length)
					ExpandBuffer();
				Array.Copy(data, offset, _buffer, _writeOffset, length);
				_writeOffset += length;

				SSH2Packet p = ConstructPacket();
				while(p!=null) {
					_handler.OnPacket(p);
					p = ConstructPacket();
				}
				ReduceBuffer();
			}
			catch(Exception ex) {
				OnError(ex, ex.Message);
			}
		}
		//returns true if a new packet could be obtained
		private SSH2Packet ConstructPacket() {
			SSH2Packet packet = null;
			if(_event!=null && !_event.WaitOne(3000, false))
				throw new Exception("waithandle timed out");

			if(_cipher==null) {
				if(_writeOffset-_readOffset<4) return null;
				int len = SSHUtil.ReadInt32(_buffer, _readOffset);
				if(_writeOffset-_readOffset<4+len) return null;

				packet = SSH2Packet.FromPlainStream(_buffer, _readOffset);
				_readOffset += 4 + len;
				_sequence++;
			}
			else {
				if(_head==null) {
					if(_writeOffset-_readOffset<_cipher.BlockSize) return null;
					_head = new byte[_cipher.BlockSize];
					byte[] eh = new byte[_cipher.BlockSize];
					Array.Copy(_buffer, _readOffset, eh, 0, eh.Length);
					_readOffset += eh.Length;
					_cipher.Decrypt(eh, 0, eh.Length, _head, 0);
				}

				int len = SSHUtil.ReadInt32(_head, 0);
				if(_writeOffset-_readOffset < len+4-_head.Length+_mac.Size) return null;

				packet = SSH2Packet.FromDecryptedHead(_head, _buffer, _readOffset, _cipher, _sequence++, _mac);
				_readOffset += 4 + len - _head.Length + _mac.Size;
				_head = null;
			}

			return packet;
		}

		private void ExpandBuffer() {
			byte[] t = new byte[_buffer.Length*2];
			Array.Copy(_buffer, 0, t, 0, _buffer.Length);
			_buffer = t;
		}
		private void ReduceBuffer() {
			if(_readOffset==_writeOffset) {
				_readOffset = 0;
				_writeOffset = 0;
			}
			else {
				byte[] temp = new byte[_writeOffset - _readOffset];
				Array.Copy(_buffer, _readOffset, temp, 0, temp.Length);
				Array.Copy(temp, 0, _buffer, 0, temp.Length);
				_readOffset = 0;
				_writeOffset = temp.Length;
			}
		}

		public void OnError(Exception error, string msg) {
			_handler.OnError(error, msg);
		}
		public void OnClosed() {
			_handler.OnClosed();
			if(_event!=null) _event.Close();
		}
	}
}
