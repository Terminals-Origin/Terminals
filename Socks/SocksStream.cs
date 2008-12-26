/*
 * Created by SharpDevelop.
 * User: CableJ01
 * Date: 16/11/2008
 * Time: 15:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;

namespace socks
{
	/// <summary>
	/// Description of SocksStream.
	/// </summary>
	public class SocksStream : Stream
	{
		public SocksStream()
		{
		}
		public override void Write(byte[] buffer, int a, int b)
		{
			
		}
		public override int Read(byte[] buffer, int a, int b)
		{
			return 0;
		}
		public override void SetLength(long a)
		{
			
		}
		public override long Seek(long amount, SeekOrigin from)
		{
			return 0;
		}
		public override void Flush()
		{
			
		}
		
		public override long Position
		{
			get
			{
				return 0;
			}
			set
			{
				
			}
		}
		public override long Length
		{
			get
			{
				return 0;
			}
		}
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}
	}
}
