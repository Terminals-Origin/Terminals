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
using System.Text;
using System.Security.Cryptography;

namespace SSHClient
{
	/// <summary>
	///  subclass of Granados class with added string conversion.
	/// </summary>
    public class SSH2UserAuthKey : Granados.SSH2.SSH2UserAuthKey
    {
        public SSH2UserAuthKey(Granados.PKI.KeyPair kp) : base(kp)
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

        public string toBase64String()
        {
    		byte[] plaintext = this.ToByteArray(null);
       		byte[] cyphertext = ProtectedData.Protect(plaintext, null, DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(cyphertext);
        }

        public static SSH2UserAuthKey FromBase64String(string value)
        {
    		byte[] cyphertext = Convert.FromBase64String(value);
            byte[] plaintext = ProtectedData.Unprotect(cyphertext, null, DataProtectionScope.CurrentUser);
            Granados.SSH2.SSH2UserAuthKey k =
                Granados.SSH2.SSH2UserAuthKey.FromByteArray(plaintext, null);
            return new SSH2UserAuthKey(k.KeyPair);
        }
        
    ///  PuTTY's own format for SSH-2 keys is as follows:
    ///  The file is text. Lines are terminated by CRLF, although CR-only
    ///  and LF-only are tolerated on input.
    ///  The first line says "PuTTY-User-Key-File-2: " plus the name of the
    ///  algorithm ("ssh-dss", "ssh-rsa" etc).
    ///  The next line says "Encryption: " plus an encryption type.
    ///  Currently the only supported encryption types are "aes256-cbc"
    ///  and "none".
    ///  The next line says "Comment: " plus the comment string.
    /// 
    ///  Next there is a line saying "Public-Lines: " plus a number N.
    ///  The following N lines contain a base64 encoding of the public
    ///  part of the key. This is encoded as the standard SSH-2 public key
    ///  blob (with no initial length): so for RSA, for example, it will
    ///  read
    /// 
    ///     string "ssh-rsa"
    ///     mpint  exponent
    ///     mpint  modulus
    /// 
    ///  Next, there is a line saying "Private-Lines: " plus a number N,
    ///  and then N lines containing the (potentially encrypted) private
    ///  part of the key. For the key type "ssh-rsa", this will be
    ///  composed of
    /// 
    ///     mpint  private_exponent
    ///     mpint  p                  (the larger of the two primes)
    ///     mpint  q                  (the smaller prime)
    ///     mpint  iqmp               (the inverse of q modulo p)
    ///     data   padding            (to reach a multiple of the cipher block size)
    /// 
    ///  And for "ssh-dss", it will be composed of
    /// 
    ///     mpint  x                  (the private key parameter)
    ///   [ string hash   20-byte hash of mpints p || q || g   only in old format ]
    ///  
    ///  Finally, there is a line saying "Private-MAC: " plus a hex
    ///  representation of a HMAC-SHA-1 of:
    /// 
    ///     string  name of algorithm ("ssh-dss", "ssh-rsa")
    ///     string  encryption type
    ///     string  comment
    ///     string  public-blob
    ///     string  private-plaintext (the plaintext version of the
    ///                                private part, including the final
    ///                                padding)
    ///  
    ///  The key to the MAC is itself a SHA-1 hash of:
    ///  
    ///     data    "putty-private-key-file-mac-key"
    ///     data    passphrase
    /// 
    ///  (An empty passphrase is used for unencrypted keys.)
    /// 
    ///  If the key is encrypted, the encryption key is derived from the
    ///  passphrase by means of a succession of SHA-1 hashes. Each hash
    ///  is the hash of:
    /// 
    ///     uint32  sequence-number
    ///     data    passphrase
    /// 
    ///  where the sequence-number increases from zero. As many of these
    ///  hashes are used as necessary.
    /// 
    ///  For backwards compatibility with snapshots between 0.51 and
    ///  0.52, we also support the older key file format, which begins
    ///  with "PuTTY-User-Key-File-1" (version number differs). In this
    ///  format the Private-MAC: field only covers the private-plaintext
    ///  field and nothing else (and without the 4-byte string length on
    ///  the front too). Moreover, the Private-MAC: field can be replaced
    ///  with a Private-Hash: field which is a plain SHA-1 hash instead of
    ///  an HMAC (this was generated for unencrypted keys).


        public static void FromPuttyFile(string filename, string passphrase)
        {
		    FileStream strm = new FileStream(filename, FileMode.Open, FileAccess.Read);
		    if (strm == null)
		    	throw new Exception("can't open "+filename);
			StreamReader r = new StreamReader(strm, Encoding.ASCII);
			string l = r.ReadLine();
		    if (l == null)
		    	throw new Exception("can't read "+filename);
		    string[] ll = l.Split(' ');
		    if(ll[0] != "PuTTY-User-Key-File-2:")
		    	throw new Exception("not a putty key file: "+filename);
		    string keytype = ll[1];
			l = r.ReadLine();
		    ll = l.Split(' ');
		    if(ll[0] != "Encryption:")
		    	throw new Exception("not a putty key file: "+filename);
		    string encryptiontype = ll[1];
			l = r.ReadLine();
		    ll = l.Split(' ');
		    if(ll[0] != "Comment:")
		    	throw new Exception("not a putty key file: "+filename);
		    string comment = ll[1];
			l = r.ReadLine();
		    ll = l.Split(' ');
		    if(ll[0] != "Public-Lines:")
		    	throw new Exception("not a putty key file: "+filename);
		    int public_lines =  int.Parse(ll[1]);
		    string pub = "";
		    for(int i=0; i<public_lines; i++)
		    	pub += r.ReadLine();
			l = r.ReadLine();
		    ll = l.Split(' ');
		    if(ll[0] != "Private-Lines:")
		    	throw new Exception("not a putty key file: "+filename);
		    int private_lines =  int.Parse(ll[1]);
		    string pri = "";
		    for(int i=0; i<private_lines; i++)
		    	pri += r.ReadLine();
			l = r.ReadLine();
		    ll = l.Split(' ');
		    if(ll[0] != "Private-MAC:")
		    	throw new Exception("not a putty key file: "+filename);
		    byte[] mac = Convert.FromBase64String(ll[1]);
		    byte[] pub_part = Convert.FromBase64String(pub);
		    byte[] pri_part = Convert.FromBase64String(pri);
		    string s = Encoding.ASCII.GetString(pri_part);
			r.Close();
        }
    }
}
