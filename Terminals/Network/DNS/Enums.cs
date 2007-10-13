#region
//
// Bdev.Net.Dns by Rob Philpott, Big Developments Ltd. Please send all bugs/enhancements to
// rob@bigdevelopments.co.uk  This file and the code contained within is freeware and may be
// distributed and edited without restriction.
// 

#endregion

using System;

namespace Bdev.Net.Dns
{
	/// <summary>
	/// The DNS TYPE (RFC1035 3.2.2/3) - 4 types are currently supported. Also, I know that this
	/// enumeration goes against naming guidelines, but I have done this as an ANAME is most
	/// definetely an 'ANAME' and not an 'Aname'
	/// </summary>
	public enum DnsType
	{
		None = 0, ANAME = 1, NS = 2, SOA = 6, MX = 15
	}

	/// <summary>
	/// The DNS CLASS (RFC1035 3.2.4/5)
	/// Internet will be the one we'll be using (IN), the others are for completeness
	/// </summary>
	public enum DnsClass
	{
		None = 0, IN = 1, CS = 2, CH = 3, HS = 4
	}

	/// <summary>
	/// (RFC1035 4.1.1) These are the return codes the server can send back
	/// </summary>
	public enum ReturnCode
	{
		Success = 0,
		FormatError = 1,
		ServerFailure = 2,
		NameError = 3,
		NotImplemented = 4,
		Refused = 5,
		Other = 6
	}

	/// <summary>
	/// (RFC1035 4.1.1) These are the Query Types which apply to all questions in a request
	/// </summary>
	public enum Opcode
	{
		StandardQuery = 0,
		InverseQuerty = 1,
		StatusRequest = 2,
		Reserverd3 = 3,
		Reserverd4 = 4,
		Reserverd5 = 5,
		Reserverd6 = 6,
		Reserverd7 = 7,
		Reserverd8 = 8,
		Reserverd9 = 9,
		Reserverd10 = 10,
		Reserverd11 = 11,
		Reserverd12 = 12,
		Reserverd13 = 13,
		Reserverd14 = 14,
		Reserverd15 = 15,
	}
}
