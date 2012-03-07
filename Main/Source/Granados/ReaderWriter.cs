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
using System.Text;
using System.IO;

using Granados.PKI;
using Granados.Util;

namespace Granados.IO {
	////////////////////////////////////////////////////////////
	/// read/write primitive types
	/// 
	internal abstract class SSHDataReader {
		
		protected byte[] _data;
		protected int _offset;
		protected int _limit;
		
		public SSHDataReader(byte[] image) {
			_data = image;
			_offset = 0;
			_limit = image.Length;
		}
		public SSHDataReader(DataFragment data) {
			Init(data);
		}
		public void Recycle(DataFragment data) {
			Init(data);
		}
		private void Init(DataFragment data) {
			_data = data.Data;
			_offset = data.Offset;
			_limit = _offset + data.Length;
		}

		public byte[] Image {
			get {
				return _data;
			}
		}
		public int Offset {
			get {
				return _offset;
			}
		}

		public int ReadInt32() {
			if(_offset+3>=_limit) throw new IOException(Strings.GetString("UnexpectedEOF"));

			int ret = (((int)_data[_offset])<<24) + (((int)_data[_offset+1])<<16) + (((int)_data[_offset+2])<<8) + _data[_offset+3];
			
			_offset += 4;
			return ret;
		}

		public byte ReadByte() {
			if(_offset>=_limit) throw new IOException(Strings.GetString("UnexpectedEOF"));
			return _data[_offset++];
		}
		public bool ReadBool() {
			if(_offset>=_limit) throw new IOException(Strings.GetString("UnexpectedEOF"));
			return _data[_offset++]==0? false : true;
		}
		/**
		* multi-precise integer
		*/
		public abstract BigInteger ReadMPInt();
		
		public byte[] ReadString() {
			int length = ReadInt32();
			return Read(length);
		}

		public byte[] Read(int length) {
			byte[] image = new byte[length];
			if(_offset+length>_limit) throw new IOException(Strings.GetString("UnexpectedEOF"));
			Array.Copy(_data, _offset, image, 0, length);
			_offset += length;
			return image;
		}

		public byte[] ReadAll() {
			byte[] t = new byte[_limit - _offset];
			Array.Copy(_data, _offset, t, 0, t.Length);
			_offset = _limit;
			return t;
		}
		
		public int Rest {
			get {
				return _limit - _offset;
			}
		}
	}


	internal abstract class SSHDataWriter : IKeyWriter {
		protected SimpleMemoryStream _strm; 
	
		public SSHDataWriter() {
			_strm = new SimpleMemoryStream();
		}

		public byte[] ToByteArray() { return _strm.ToNewArray(); }

		public int Length {
			get {
				return _strm.Length;
			}
		}
		public void Reset() {
			_strm.Reset();
		}
		public void SetOffset(int value) {
			_strm.SetOffset(value);
		}
		public byte[] UnderlyingBuffer {
			get {
				return _strm.UnderlyingBuffer;
			}
		}
	
		public void Write(byte[] data) { _strm.Write(data, 0, data.Length); }
		public void Write(byte[] data, int offset, int count) { _strm.Write(data, offset, count); }
		public void Write(byte data)   { _strm.WriteByte(data); }
		public void Write(bool data)   { _strm.WriteByte(data? (byte)1 : (byte)0); }
	
		public void Write(int data) {
			uint udata = (uint)data;
			uint a = udata & 0xFF000000;
			a >>= 24;
			_strm.WriteByte((byte)a);
				
			a = udata & 0x00FF0000;
			a >>= 16;
			_strm.WriteByte((byte)a);
				
			a = udata & 0x0000FF00;
			a >>= 8;
			_strm.WriteByte((byte)a);

			a = udata & 0x000000FF;
			_strm.WriteByte((byte)a);
		}

		public abstract void Write(BigInteger data);

		public void Write(string data) {
			Write(data.Length);
			if(data.Length>0) Write(Encoding.ASCII.GetBytes(data));
		}
			
		public void WriteAsString(byte[] data) {
			Write(data.Length);
			if(data.Length>0) Write(data);
		}
		public void WriteAsString(byte[] data, int offset, int length) {
			Write(length);
			if(length>0) Write(data, offset, length);
		}
	}
}

namespace Granados.IO.SSH1 {
	using Granados.SSH1;

	internal class SSH1DataReader : SSHDataReader {

		public SSH1DataReader(byte[] image) : base(image) {}
		public SSH1DataReader(DataFragment data) : base(data) {}

		public PacketType ReadPacketType() {
			return (PacketType)this.ReadByte();
		}

		public override BigInteger ReadMPInt() {
			//first 2 bytes describes the bit count
			int bits = (((int)_data[_offset])<<8) + _data[_offset+1];
			_offset += 2;
			
			return new BigInteger(Read((bits+7) / 8));
		}
	}

	internal class SSH1DataWriter : SSHDataWriter {
		public override void Write(BigInteger data) {
			byte[] image = data.getBytes();
			int off = (image[0]==0? 1 : 0);
			int len = (image.Length-off) * 8; 
				
			int a = len & 0x0000FF00;
			a >>= 8;
			_strm.WriteByte((byte)a);

			a = len & 0x000000FF;
			_strm.WriteByte((byte)a);
				
			_strm.Write(image,off,image.Length-off);
		}
	}
}

namespace Granados.IO.SSH2 {
	using Granados.SSH2;

	internal class SSH2DataReader : SSHDataReader {

		public SSH2DataReader(byte[] image) : base(image) {}
		public SSH2DataReader(DataFragment data) : base(data) {}

		//SSH2 Key File Only
		public BigInteger ReadBigIntWithBits() {
			int bits = ReadInt32();
			int bytes = (bits + 7) / 8;
			return new BigInteger(Read(bytes));
		}
		public override BigInteger ReadMPInt() {
			return new BigInteger(ReadString());
		}
		public PacketType ReadPacketType() {
			return (PacketType)ReadByte();
		}
	}

	internal class SSH2DataWriter : SSHDataWriter {
		//writes mpint in SSH2 format
		public override void Write(BigInteger data) {
			byte[] t = data.getBytes();
			int len = t.Length;
			if(t[0] >= 0x80) {
				Write(++len);
				Write((byte)0);
			}
			else
				Write(len);
			Write(t);
		}

		public void WriteBigIntWithBits(BigInteger bi) {
			Write(bi.bitCount());
			Write(bi.getBytes());
		}

		public void WritePacketType(PacketType pt) {
			Write((byte)pt);
		}
	}
}
