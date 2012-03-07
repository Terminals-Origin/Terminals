#region
//
// Bdev.Net.Dns by Rob Philpott, Big Developments Ltd. Please send all bugs/enhancements to
// rob@bigdevelopments.co.uk  This file and the code contained within is freeware and may be
// distributed and edited without restriction.
// 

#endregion

using System;
using System.Runtime.Serialization;

namespace Bdev.Net.Dns
{
	/// <summary>
	/// Thrown when the server delivers a response we are not expecting to hear
	/// </summary>
	[Serializable]
	public class InvalidResponseException : SystemException
	{
		public InvalidResponseException()
		{
			// no implementation
		}

		public InvalidResponseException(Exception innerException) :  base(null, innerException) 
		{
			// no implementation
		}

		public InvalidResponseException(string message, Exception innerException) : base (message, innerException)
		{
			// no implementation
		}
        
		protected InvalidResponseException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
			// no implementation
		}
	}
}
