#region
//
// Bdev.Net.Dns by Rob Philpott, Big Developments Ltd. Please send all bugs/enhancements to
// rob@bigdevelopments.co.uk  This file and the code contained within is freeware and may be
// distributed and edited without restriction.
// 

#endregion

using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;

namespace Bdev.Net.Dns
{
	/// <summary>
	/// A Request logically consists of a number of questions to ask the DNS Server. Create a request and
	/// add questions to it, then pass the request to Resolver.Lookup to query the DNS Server. It is important
	/// to note that many DNS Servers DO NOT SUPPORT MORE THAN 1 QUESTION PER REQUEST, and it is advised that
	/// you only add one question to a request. If not ensure you check Response.ReturnCode to see what the
	/// server has to say about it.
	/// </summary>
	public class Request
	{
		// A request is a series of questions, an 'opcode' (RFC1035 4.1.1) and a flag to denote
		// whether recursion is required (don't ask..., just assume it is)
		private ArrayList	_questions;
		private bool		_recursionDesired;
		private Opcode		_opCode;

		public bool RecursionDesired
		{
			get { return _recursionDesired;		}
			set { _recursionDesired = value;	}
		}

		public Opcode Opcode
		{
			get { return _opCode;				}
			set { _opCode = value;				}
		}

		/// <summary>
		/// Construct this object with the default values and create an ArrayList to hold
		/// the questions as they are added
		/// </summary>
		public Request()
		{
			// default for a request is that recursion is desired and using standard query
			_recursionDesired = true;
			_opCode = Opcode.StandardQuery;

			// create an expandable list of questions
			_questions = new ArrayList();

		}
		
		/// <summary>
		/// Adds a question to the request to be sent to the DNS server.
		/// </summary>
		/// <param name="question">The question to add to the request</param>
		public void AddQuestion(Question question)
		{
			// abandon if null
			if (question == null) throw new ArgumentNullException("question");

			// add this question to our collection
			_questions.Add(question);
		}

		/// <summary>
		/// Convert this request into a byte array ready to send direct to the DNS server
		/// </summary>
		/// <returns></returns>
		public byte[] GetMessage()
		{
			// construct a message for this request. This will be a byte array but we're using
			// an arraylist as we don't know how big it will be
			ArrayList data = new ArrayList();
			
			// the id of this message - this will be filled in by the resolver
			data.Add((byte)0);
			data.Add((byte)0);
			
			// write the bitfields
			data.Add((byte)(((byte)_opCode<<3)  | (_recursionDesired?0x01:0)));
			data.Add((byte)0);

			// tell it how many questions
			unchecked
			{
				data.Add((byte)(_questions.Count >> 8));
				data.Add((byte)_questions.Count);
			}
			
			// the are no requests, name servers or additional records in a request
			data.Add((byte)0); data.Add((byte)0);
			data.Add((byte)0); data.Add((byte)0);
			data.Add((byte)0); data.Add((byte)0);

			// that's the header done - now add the questions
			foreach (Question question in _questions)
			{
				AddDomain(data, question.Domain);
				unchecked
				{
					data.Add((byte)0);
					data.Add((byte)question.Type);
					data.Add((byte)0);
					data.Add((byte)question.Class);
				}
			}

			// and convert that to an array
			byte[] message = new byte[data.Count];
			data.CopyTo(message);
			return message;
		}

		/// <summary>
		/// Adds a domain name to the ArrayList of bytes. This implementation does not use
		/// the domain name compression used in the class Pointer - maybe it should.
		/// </summary>
		/// <param name="data">The ArrayList representing the byte array message</param>
		/// <param name="domainName">the domain name to encode and add to the array</param>
		private static void AddDomain(ArrayList data, string domainName)
		{
			int position = 0;
			int length = 0;

			// start from the beginning and go to the end
			while (position < domainName.Length)
			{
				// look for a period, after where we are
				length = domainName.IndexOf('.', position) - position;
				
				// if there isn't one then this labels length is to the end of the string
				if (length < 0) length = domainName.Length - position;
				
				// add the length
				data.Add((byte)length);

				// copy a char at a time to the array
				while (length-- > 0)
				{
					data.Add((byte)domainName[position++]);
				}

				// step over '.'
				position++;
			}
				
			// end of domain names
			data.Add((byte)0);
		}
	}
}
