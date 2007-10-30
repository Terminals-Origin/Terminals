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
/*
	* structure of packet
	* 
	* length(4) padding(1-8) type(1) data(0+) crc(4)    
	* 
	* 1. length = type+data+crc
	* 2. the length of padding+type+data+crc must be a multiple of 8
	* 3. padding length must be 1 at least
	* 4. crc is calculated from padding,type and data
	*
 */

using System;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using Routrek.Crypto;
using Routrek.SSHC;

namespace Routrek.SSHCV1
{
	
	internal class SSH1Packet
	{
		private byte _type;
		private byte[] _data;
		private uint _CRC;

		/**
		* reads type, data, and crc from byte array.
		* an exception is thrown if crc check fails.
		*/
		internal void ConstructAndCheck(byte[] buf, int packet_length, int padding_length, bool check_crc) {
			_type = buf[padding_length];
			//System.out.println("Type: " + _type);
			if(packet_length > 5) //the body is not empty
			{
				_data = new byte[packet_length-5]; //5 is the length of [type] and [crc]
				Array.Copy(buf, padding_length+1, _data, 0, packet_length-5);
			}
			_CRC = (uint)SSHUtil.ReadInt32(buf, buf.Length-4);
			if(check_crc) {
				uint c = CRC.Calc(buf, 0, buf.Length-4);
				if(_CRC != c)
					throw new SSHException("CRC Error", buf);
			}
		}
		
		/**
		* constructs from the packet type and the body
		*/
		public static SSH1Packet FromPlainPayload(PacketType type, byte[] data) {
			SSH1Packet p = new SSH1Packet();
			p._type = (byte)type;
			p._data = data;
			return p;
		}
		public static SSH1Packet FromPlainPayload(PacketType type) {
			SSH1Packet p = new SSH1Packet();
			p._type = (byte)type;
			p._data = new byte[0];
			return p;
		}
		/**
		* creates a packet as the input of shell
		*/
		static SSH1Packet AsStdinString(byte[] input) {
			SSH1DataWriter w = new SSH1DataWriter();
			w.WriteAsString(input);
			SSH1Packet p = SSH1Packet.FromPlainPayload(PacketType.SSH_CMSG_STDIN_DATA, w.ToByteArray());
			return p;
		}
		
		private byte[] BuildImage() {
			int packet_length = (_data==null? 0 : _data.Length) + 5; //type and CRC
			int padding_length = 8 - (packet_length % 8);
			
			byte[] image = new byte[packet_length + padding_length + 4];
			SSHUtil.WriteIntToByteArray(image, 0, packet_length);
			
			for(int i=0; i<padding_length; i++) image[4+i]=0; //padding: filling by random values is better
			image[4+padding_length] = _type;
			if(_data!=null)
				Array.Copy(_data, 0, image, 4+padding_length+1, _data.Length);
			
			_CRC = CRC.Calc(image, 4, image.Length-8);
			SSHUtil.WriteIntToByteArray(image, image.Length-4, (int)_CRC);
			
			return image;
		}
		
		/**
		* writes to plain stream
		*/
		public void WriteTo(AbstractSocket output) {
			byte[] image = BuildImage();
			output.Write(image, 0, image.Length);
		}
		/**
		* writes to encrypted stream
		*/
		public void WriteTo(AbstractSocket output, Cipher cipher) {
			byte[] image = BuildImage();
			//dumpBA(image);
			byte[] encrypted = new byte[image.Length-4];
			cipher.Encrypt(image, 4, image.Length-4, encrypted, 0); //length field must not be encrypted

			Array.Copy(encrypted, 0, image, 4, encrypted.Length);
			output.Write(image, 0, image.Length);
		}
		
		public PacketType Type {
			get {
				return (PacketType)_type;
			}
		}
		public byte[] Data {
			get {
				return _data;
			}
		}
		public int DataLength {
			get {
				return _data==null? 0 : _data.Length;
			}
		}
	}

	internal interface ISSH1PacketHandler : IHandlerBase {
		void OnPacket(SSH1Packet packet);
	}

	internal class SynchronizedSSH1PacketHandler : SynchronizedHandlerBase, ISSH1PacketHandler {
		internal ArrayList _packets;

		internal SynchronizedSSH1PacketHandler() {
			_packets = new ArrayList();
		}

		public void OnPacket(SSH1Packet packet) {
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
		public SSH1Packet PopPacket() {
			lock(this) {
				if(_packets.Count==0)
					return null;
				else {
					SSH1Packet p = null;
					p = (SSH1Packet)_packets[0];
					_packets.RemoveAt(0);
					if(_packets.Count==0) _event.Reset();
					return p;
				}
			}
		}
	}
	internal class CallbackSSH1PacketHandler : ISSH1PacketHandler {
		internal SSH1Connection _connection;

		internal CallbackSSH1PacketHandler(SSH1Connection con) {
			_connection = con;
		}
		public void OnPacket(SSH1Packet packet) {
			_connection.AsyncReceivePacket(packet);
		}
		public void OnError(Exception error, string msg) {
			_connection.EventReceiver.OnError(error, msg);
		}
		public void OnClosed() {
			_connection.EventReceiver.OnConnectionClosed();
		}
	}

	internal class SSH1PacketBuilder : IByteArrayHandler {
		private ISSH1PacketHandler _handler;
		private byte[] _buffer;
		private int _readOffset;
		private int _writeOffset;
		private Cipher _cipher;
		private bool _checkMAC;
		private ManualResetEvent _event;

		public SSH1PacketBuilder(ISSH1PacketHandler handler) {
			_handler = handler;
			_buffer = new byte[0x1000];
			_readOffset = 0;
			_writeOffset = 0;
			_cipher = null;
			_checkMAC = false;
			_event = null;
		}

		public void SetSignal(bool value) {
			if(_event==null) _event = new ManualResetEvent(true);

			if(value)
				_event.Set();
			else
				_event.Reset();
		}

		public void SetCipher(Cipher c, bool check_mac) {
			_cipher = c;
			_checkMAC = check_mac;
		}
		public ISSH1PacketHandler Handler {
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

				SSH1Packet p = ConstructPacket();
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
		private SSH1Packet ConstructPacket() {
			if(_event!=null && !_event.WaitOne(3000, false))
				throw new Exception("waithandle timed out");

			if(_writeOffset-_readOffset<4) return null;
			int packet_length = SSHUtil.ReadInt32(_buffer, _readOffset);
			int padding_length = 8 - (packet_length % 8); //padding length
			int total = packet_length + padding_length;
			if(_writeOffset-_readOffset<4+total) return null;

			byte[] decrypted = new byte[total];
			if(_cipher!=null)
				_cipher.Decrypt(_buffer, _readOffset+4, total, decrypted, 0);
			else
				Array.Copy(_buffer, _readOffset+4, decrypted, 0, total);
			_readOffset += 4 + total;
			
			SSH1Packet p = new SSH1Packet();
			p.ConstructAndCheck(decrypted, packet_length, padding_length, _checkMAC);
			return p;
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
