#region
//
// Bdev.Net.Dns by Rob Philpott, Big Developments Ltd. Please send all bugs/enhancements to
// rob@bigdevelopments.co.uk  This file and the code contained within is freeware and may be
// distributed and edited without restriction.
// 

#endregion

using System;
using System.Net;

namespace Bdev.Net.Dns
{
	/// <summary>
	/// ANAME Resource Record (RR) (RFC1035 3.4.1)
	/// </summary>
	public class ANameRecord : RecordBase
	{
		// An ANAME records consists simply of an IP address
		internal IPAddress _ipAddress;

		// expose this IP address r/o to the world
		public IPAddress IPAddress
		{
			get { return _ipAddress; }
		}

		/// <summary>
		/// Constructs an ANAME record by reading bytes from a return message
		/// </summary>
		/// <param name="pointer">A logical pointer to the bytes holding the record</param>
		internal ANameRecord(Pointer pointer)
		{
			byte b1 = pointer.ReadByte();
			byte b2 = pointer.ReadByte();
			byte b3 = pointer.ReadByte();
			byte b4 = pointer.ReadByte();

			// this next line's not brilliant - couldn't find a better way though
			_ipAddress = IPAddress.Parse(string.Format("{0}.{1}.{2}.{3}", b1, b2, b3, b4));
		}

		public override string ToString()
		{
			return _ipAddress.ToString();
		}
	}
}
