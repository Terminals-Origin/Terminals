/*
 * Created by SharpDevelop.
 * User: CableJ01
 * Date: 19/11/2008
 * Time: 08:55
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Net;
using System.Net.Sockets;

namespace Terminals
{
	/// <summary>
	/// Description of TcpProtocol.
	/// </summary>
	public class TcpProtocol
	{
		#region Public Properties
		#endregion
		#region Public Enums
		#endregion
		#region Public Fields
		#endregion
		#region Public Delegates
		public delegate void DataIndicate(byte[] data);
		public delegate void Disconnect();
		#endregion
		#region Public Events
	    public event DataIndicate OnDataIndicated;
	    public event Disconnect OnDisconnect;
		#endregion
		#region Public Constructors
		public TcpProtocol(NetworkStream stream)
		{
			this.stream = stream;
			State s = new State(stream);
			this.stream.BeginRead(s.buffer, 0, s.buffer.Length,
				                      new AsyncCallback(OnRead), s);
		}
		#endregion
		#region Public Methods
		public void RequestData (byte[] data)
		{
			stream.Write(data, 0, data.Length);
		}
		#endregion
		#region Public Overrides
		#endregion
		#region Private Enums
		#endregion
		#region Private Fields
		private NetworkStream stream;
		#endregion
		#region Private Classes
		private class State
		{
			public NetworkStream stream;
			public byte[] buffer;
			public State(NetworkStream stream)
			{
				this.stream = stream;
				this.buffer = new byte[2048];
			}
		}
		#endregion
		#region Private Structs
		#endregion
		#region Private Methods
		private void OnRead(IAsyncResult ar)
		{
			State s = (State)ar.AsyncState;
			try
			{
				int n = s.stream.EndRead(ar);
				if(n > 0 && OnDataIndicated!=null)
				{
					byte[] obuf = new byte[n];
					System.Array.Copy(s.buffer, obuf, obuf.Length);
					OnDataIndicated(obuf);
				}
				this.stream.BeginRead(s.buffer, 0, s.buffer.Length,
					                      new AsyncCallback(OnRead), s);
			} catch(Exception e)
			{
				if(OnDisconnect!=null) OnDisconnect();
			}
		}
		#endregion
		#region Private Overrides
		#endregion
	}
}
