/*
 * Created by SharpDevelop.
 * User: CableJ01
 * Date: 18/01/2009
 * Time: 15:40
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Globalization;
using System.IO;

namespace SSHClient
{
	/// <summary>
	///  subclass of Granados class with added string conversion.
	/// </summary>
    public class SSH2UserAuthKey : Routrek.SSHCV2.SSH2UserAuthKey
    {
        public SSH2UserAuthKey(Routrek.PKI.KeyPair kp) : base(kp)
        {
        }

        public string PublicPartInOpenSSHStyle()
        {
            Byte[] mk = new Byte[2048];
            MemoryStream ks = new MemoryStream(mk);
            base.WritePublicPartInOpenSSHStyle(ks);
            ks = new MemoryStream(mk);
            StreamReader sr = new StreamReader(ks);
            return sr.ReadLine();
        }

        public string toSECSHStyle(string comment)
        {
            Byte[] mk = new Byte[2048];
            MemoryStream ks = new MemoryStream(mk);
            base.WritePrivatePartInSECSHStyleFile(ks, comment, "");
            ks = new MemoryStream(mk);
            StreamReader sr = new StreamReader(ks);
            return sr.ReadToEnd();
        }

        public static SSH2UserAuthKey FromSECSHStyle(string value)
        {
            MemoryStream ks = new MemoryStream();
            StreamWriter sw = new StreamWriter(ks);
            sw.Write(value);
            ks.Seek(0, SeekOrigin.Begin);
            Routrek.SSHCV2.SSH2UserAuthKey k =
            	Routrek.SSHCV2.SSH2UserAuthKey.FromSECSHStyleStream(ks, "");
            return new SSH2UserAuthKey(k.KeyPair);
        }
    }
}
