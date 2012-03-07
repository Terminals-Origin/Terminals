using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Org.Mentalis.Network.ProxySocket;

/*
 *   Using the ProxySocket class is very easy. It works exactely like
 *   an ordinary Socket, but it offers more functionality.
 *   If you connect to a remote host, you can specify a host/port pair
 *   instead of an IPEndPoint and the ProxySocket will resolve it for
 *   you.
 *   It can also connect trough firewall proxy servers (hence the name).
 *
 *   To use a ProxySocket object with your SOCK4/5 firewall, simply
 *   adjust the Proxy properties (ProxyEndPoint, ProxyUser, ProxyPass
 *   and ProxyType).
 */

class TestApp {
	static void Main(string[] args)	{
		// create a new ProxySocket
		ProxySocket s = new ProxySocket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
		// set the proxy settings
		s.ProxyEndPoint = new IPEndPoint(IPAddress.Parse("10.0.0.5"), 1080);
		s.ProxyUser = "username";
		s.ProxyPass = "password";
		s.ProxyType = ProxyTypes.Socks5;	// if you set this to ProxyTypes.None, 
											// the ProxySocket will act as a normal Socket
		// connect to the remote server
		// (note that the proxy server will resolve the domain name for us)
		s.Connect("www.mentalis.org", 80);
		// send an HTTP request
		s.Send(Encoding.ASCII.GetBytes("GET / HTTP/1.0\r\nHost: www.mentalis.org\r\n\r\n"));
		// read the HTTP reply
		int recv = 0;
		byte [] buffer = new byte[1024];
		recv = s.Receive(buffer);
		while (recv > 0) {
			Console.Write(Encoding.ASCII.GetString(buffer, 0, recv));
			recv = s.Receive(buffer);
		}
		// wait until the user presses enter
		Console.WriteLine("Press enter to continue...");
		Console.ReadLine();
	}
}
