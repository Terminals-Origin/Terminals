#region
//
// Bdev.Net.Dns by Rob Philpott, Big Developments Ltd. Please send all bugs/enhancements to
// rob@bigdevelopments.co.uk  This file and the code contained within is freeware and may be
// distributed and edited without restriction.
// 

#endregion

using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;

namespace Bdev.Net.Dns
{
	/// <summary>
	/// Summary description for Dns.
	/// </summary>
	public sealed class Resolver
	{
		const int		_dnsPort = 53;
		const int		_udpRetryAttempts = 2;
		static int		_uniqueId;

		/// <summary>
		/// Private constructor - this static class should never be instantiated
		/// </summary>
		private Resolver()
		{
			// no implementation
		}	

		/// <summary>
		/// Shorthand form to make MX querying easier, essentially wraps up the retreival
		/// of the MX records, and sorts them by preference
		/// </summary>
		/// <param name="domain">domain name to retreive MX RRs for</param>
		/// <param name="dnsServer">the server we're going to ask</param>
		/// <returns>An array of MXRecords</returns>
		public static MXRecord[] MXLookup(string domain, IPAddress dnsServer)
		{
			// check the inputs
			if (domain == null) throw new ArgumentNullException("domain");
			if (dnsServer == null)  throw new ArgumentNullException("dnsServer");

			// create a request for this
			Request request = new Request();

			// add one question - the MX IN lookup for the supplied domain
			request.AddQuestion(new Question(domain, DnsType.MX, DnsClass.IN));
			
			// fire it off
			Response response = Lookup(request, dnsServer);

			// if we didn't get a response, then return null
			if (response == null) return null;
				
			// create a growable array of MX records
			ArrayList resourceRecords = new ArrayList();

			// add each of the answers to the array
			foreach (Answer answer in response.Answers)
			{
				// if the answer is an MX record
				if (answer.Record.GetType() == typeof(MXRecord))
				{
					// add it to our array
					resourceRecords.Add(answer.Record);
				}
			}

			// create array of MX records
			MXRecord[] mxRecords = new MXRecord[resourceRecords.Count];

			// copy from the array list
			resourceRecords.CopyTo(mxRecords);

			// sort into lowest preference order
			Array.Sort(mxRecords);

			// and return
			return mxRecords;
		}

		/// <summary>
		/// The principal look up function, which sends a request message to the given
		/// DNS server and collects a response. This implementation re-sends the message
		/// via UDP up to two times in the event of no response/packet loss
		/// </summary>
		/// <param name="request">The logical request to send to the server</param>
		/// <param name="dnsServer">The IP address of the DNS server we are querying</param>
		/// <returns>The logical response from the DNS server or null if no response</returns>
		public static Response Lookup(Request request, IPAddress dnsServer)
		{
			// check the inputs
			if (request == null) throw new ArgumentNullException("request");
			if (dnsServer == null) throw new ArgumentNullException("dnsServer");
			
			// We will not catch exceptions here, rather just refer them to the caller

			// create an end point to communicate with
			IPEndPoint server = new IPEndPoint(dnsServer, _dnsPort);
		
			// get the message
			byte[] requestMessage = request.GetMessage();

			// send the request and get the response
			byte[] responseMessage = UdpTransfer(server, requestMessage);

			// and populate a response object from that and return it
			return new Response(responseMessage);
		}

		private static byte[] UdpTransfer(IPEndPoint server, byte[] requestMessage)
		{
			// UDP can fail - if it does try again keeping track of how many attempts we've made
			int attempts = 0;

			// try repeatedly in case of failure
			while (attempts <= _udpRetryAttempts)
			{
				// firstly, uniquely mark this request with an id
				unchecked
				{
					// substitute in an id unique to this lookup, the request has no idea about this
					requestMessage[0] = (byte)(_uniqueId >> 8);
					requestMessage[1] = (byte)_uniqueId;
				}

				// we'll be send and receiving a UDP packet
				Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			
				// we will wait at most 1 second for a dns reply
				socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 1000);

				// send it off to the server
				socket.SendTo(requestMessage, requestMessage.Length, SocketFlags.None, server);
		
				// RFC1035 states that the maximum size of a UDP datagram is 512 octets (bytes)
				byte[] responseMessage = new byte[512];

				try
				{
					// wait for a response upto 1 second
					socket.Receive(responseMessage);

					// make sure the message returned is ours
					if (responseMessage[0] == requestMessage[0] && responseMessage[1] == requestMessage[1])
					{
						// its a valid response - return it, this is our successful exit point
						return responseMessage;
					}
				}
				catch (SocketException ex)
				{
                    //Terminals.Logging.Error("Socket Exception on Receive", ex);
					// failure - we better try again, but remember how many attempts
					attempts++;
				}
				finally
				{
					// increase the unique id
					_uniqueId++;

					// close the socket
					socket.Close();
				}
			}
		
			// the operation has failed, this is our unsuccessful exit point
			throw new NoResponseException();
		}
	}
}
