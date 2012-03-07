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
using Granados.Crypto;
using Granados.IO;
using Granados.IO.SSH1;
using Granados.Util;

namespace Granados.SSH1
{
	
	internal class SSH1Packet
	{
		private byte _type;
		private byte[] _data;
		private uint _CRC;

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
		public void WriteTo(AbstractGranadosSocket output) {
			byte[] image = BuildImage();
			output.Write(image, 0, image.Length);
		}
		/**
		* writes to encrypted stream
		*/
		public void WriteTo(AbstractGranadosSocket output, Cipher cipher) {
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


	internal class CallbackSSH1PacketHandler : IDataHandler {
		internal SSH1Connection _connection;

		internal CallbackSSH1PacketHandler(SSH1Connection con) {
			_connection = con;
		}
		public void OnData(DataFragment data) {
			_connection.AsyncReceivePacket(data);
		}
		public void OnError(Exception error) {
			_connection.EventReceiver.OnError(error);
		}
		public void OnClosed() {
			_connection.EventReceiver.OnConnectionClosed();
		}

	}

	internal class SSH1PacketBuilder : FilterDataHandler {
		private byte[] _buffer;
		private int _readOffset;
		private int _writeOffset;
		private Cipher _cipher;
		private bool _checkMAC;

		public SSH1PacketBuilder(IDataHandler handler) : base(handler) {
			_buffer = new byte[0x1000];
			_readOffset = 0;
			_writeOffset = 0;
			_cipher = null;
			_checkMAC = false;
		}

		public void SetCipher(Cipher c, bool check_mac) {
			_cipher = c;
			_checkMAC = check_mac;
		}

		public override void OnData(DataFragment data) {
			try {
				while(_buffer.Length - _writeOffset < data.Length)
					ExpandBuffer();
				Array.Copy(data.Data, data.Offset, _buffer, _writeOffset, data.Length);
				_writeOffset += data.Length;

				DataFragment p = ConstructPacket();
				while(p!=null) {
					_inner_handler.OnData(p);
					p = ConstructPacket();
				}
				ReduceBuffer();
			}
			catch(Exception ex) {
				_inner_handler.OnError(ex);
			}
		}
		//returns true if a new packet could be obtained
		private DataFragment ConstructPacket() {

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
			return ConstructAndCheck(decrypted, packet_length, padding_length, _checkMAC);
		}

		/**
		* reads type, data, and crc from byte array.
		* an exception is thrown if crc check fails.
		*/
		private DataFragment ConstructAndCheck(byte[] buf, int packet_length, int padding_length, bool check_crc) {
			int body_len = packet_length-4;
			byte[] body = new byte[body_len];
			Array.Copy(buf, padding_length, body, 0, body_len);

			uint received_crc = (uint)SSHUtil.ReadInt32(buf, buf.Length-4);
			if(check_crc) {
				uint crc = CRC.Calc(buf, 0, buf.Length-4);
				if(received_crc != crc)
					throw new SSHException("CRC Error", buf);
			}

			return new DataFragment(body, 0, body_len);
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

	}
}
