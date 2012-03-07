/* ---------------------------------------------------------------------------
 *
 * Copyright (c) Routrek Networks, Inc.    All Rights Reserved..
 * 
 * This file is a part of the Granados SSH Client Library that is subject to
 * the license included in the distributed package.
 * You may not use this file except in compliance with the license.
 * 
 * ---------------------------------------------------------------------------
 * I implemented this algorithm with reference to following products and books though the algorithm is known publicly.
 *   * MindTerm ( AppGate Network Security )
 *   * Applied Cryptography ( Bruce Schneier )
 */
using System;
using System.Security.Cryptography;

using Granados.Util;

namespace Granados.PKI
{
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class RSAKeyPair : KeyPair, ISigner, IVerifier {

        private RSAPublicKey _publickey;
        private BigInteger _d;
        private BigInteger _u;
        private BigInteger _p;
        private BigInteger _q;

        public RSAKeyPair(BigInteger e, BigInteger d, BigInteger n, BigInteger u, BigInteger p, BigInteger q) {
            _publickey = new RSAPublicKey(e, n);
            _d = d;
            _u = u;
            _p = p;
            _q = q;
        }
        public BigInteger D {
            get { 
                return _d;
            }
        }
        public BigInteger U {
            get { 
                return _u;
            }
        }
        public BigInteger P {
            get { 
                return _p;
            }
        }
        public BigInteger Q {
            get { 
                return _q;
            }
        }
        public override PublicKeyAlgorithm Algorithm {
            get {
                return PublicKeyAlgorithm.RSA;
            }
        }
        public byte[] Sign(byte[] data) {
            BigInteger pe = PrimeExponent(_d, _p);
            BigInteger qe = PrimeExponent(_d, _q);

            BigInteger result = SignCore(new BigInteger(data), pe, qe);

            return result.getBytes();
        }
        
        public void Verify(byte[] data, byte[] expected) {
            _publickey.Verify(data, expected);
        }

        public byte[] SignWithSHA1(byte[] data) {
            byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(data);

            byte[] buf = new byte[hash.Length + PKIUtil.SHA1_ASN_ID.Length];
            Array.Copy(PKIUtil.SHA1_ASN_ID, 0, buf, 0, PKIUtil.SHA1_ASN_ID.Length);
            Array.Copy(hash,        0, buf, PKIUtil.SHA1_ASN_ID.Length, hash.Length);
    
            BigInteger x = new BigInteger(buf);
            //Debug.WriteLine(x.ToHexString());
            int padLen = (_publickey._n.bitCount() + 7) / 8;

            x = RSAUtil.PKCS1PadType1(x, padLen);
            byte[] result = Sign(x.getBytes());
            return result;
        }

        private BigInteger SignCore(BigInteger input, BigInteger pe, BigInteger qe) {
            BigInteger p2 = (input % _p).modPow(pe, _p);
            BigInteger q2 = (input % _q).modPow(qe, _q);

            if(p2==q2)
                return p2;

            BigInteger k = (q2-p2) % _q;
            if(k.IsNegative) k += _q; //in .NET, k is negative when _q is negative
            k = (k * _u) % _q;

            BigInteger result = k * _p + p2;

            return result;
        }

        public override PublicKey PublicKey {
            get {
                return _publickey;
            }
        }

        private static BigInteger PrimeExponent(BigInteger privateExponent,	BigInteger prime) {
            BigInteger pe = prime - new BigInteger(1);
            return privateExponent % pe;

        }

        public RSAParameters ToRSAParameters() {
            RSAParameters p = new RSAParameters();
            p.D = _d.getBytes();
            p.Exponent = _publickey.Exponent.getBytes();
            p.Modulus = _publickey.Modulus.getBytes();
            p.P = _p.getBytes();
            p.Q = _q.getBytes();
            BigInteger pe = PrimeExponent(_d, _p);
            BigInteger qe = PrimeExponent(_d, _q);
            p.DP = pe.getBytes();
            p.DQ = qe.getBytes();
            p.InverseQ = _u.getBytes();
            return p;
        }

        public static RSAKeyPair GenerateNew(int bits, Random rnd) {
            BigInteger one = new BigInteger(1);
            BigInteger p   = null;
            BigInteger q   = null;
            BigInteger t   = null;
            BigInteger p_1 = null;
            BigInteger q_1 = null;
            BigInteger phi = null;
            BigInteger G   = null;
            BigInteger F   = null;
            BigInteger e   = null;
            BigInteger d   = null;
            BigInteger u   = null;
            BigInteger n   = null;

            bool finished = false;

            while(!finished) {
                p = BigInteger.genPseudoPrime(bits / 2, 64, rnd);
                q = BigInteger.genPseudoPrime(bits - (bits / 2), 64, rnd);

                if(p == 0) {
                    continue;
                } else if(q < p) {
                    t = q;
                    q = p;
                    p = t;
                }

                t = p.gcd(q);
                if(t != one) {
                    continue;
                }

                p_1 = p - one;
                q_1 = q - one;
                phi = p_1 * q_1;
                G   = p_1.gcd(q_1);
                F   = phi / G;

                e   = one << 5;
                e   = e - one;
                do {
                    e = e + (one + one);
                    t = e.gcd(phi);
                } while(t!=one);

                // !!! d = e.modInverse(F);
                d = e.modInverse(phi);
                n = p * q;
                u = p.modInverse(q);

                finished = true;
            }

            return new RSAKeyPair(e,d,n,u,p,q);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class RSAPublicKey : PublicKey, IVerifier {


        internal BigInteger _e;
        internal BigInteger _n;


        public RSAPublicKey(BigInteger exp, BigInteger mod) {
            _e = exp;
            _n = mod;
        }
        public override PublicKeyAlgorithm Algorithm {
            get {
                return PublicKeyAlgorithm.RSA;
            }
        }
        public BigInteger Exponent {
            get {
                return _e;
            }
        }
        public BigInteger Modulus {
            get {
                return _n;
            }
        }

        public void Verify(byte[] data, byte[] expected) {
            if(VerifyBI(data)!=new BigInteger(expected))
                throw new VerifyException("Failed to verify");
        }
        private BigInteger VerifyBI(byte[] data) {
            return new BigInteger(data).modPow(_e, _n);
        }
        public void VerifyWithSHA1(byte[] data, byte[] expected) {
            BigInteger result = VerifyBI(data);
            byte[] finaldata = RSAUtil.StripPKCS1Pad(result,1).getBytes();
            
            if(finaldata.Length != PKIUtil.SHA1_ASN_ID.Length+expected.Length)
                throw new VerifyException("result is too short");
            else {
                byte[] r = new byte[finaldata.Length];
                Array.Copy(PKIUtil.SHA1_ASN_ID, 0, r, 0, PKIUtil.SHA1_ASN_ID.Length);
                Array.Copy(expected, 0, r, PKIUtil.SHA1_ASN_ID.Length, expected.Length);
                if(SSHUtil.memcmp(r, finaldata)!=0)
                    throw new VerifyException("failed to verify");
            }
        }

        public override void WriteTo(IKeyWriter writer) {
            writer.Write(_e);
            writer.Write(_n);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class RSAUtil {
        
        public static BigInteger PKCS1PadType2(BigInteger input, int pad_len, Random rand) {
            int input_byte_length = (input.bitCount()+7)/8;
            //System.out.println(String.valueOf(pad_len) + ":" + input_byte_length);
            byte[] pad = new byte[pad_len - input_byte_length - 3];

            for(int i = 0; i < pad.Length; i++) {
                byte[] b = new byte[1];
                rand.NextBytes(b);
                while(b[0] == 0) rand.NextBytes(b); //0‚Å‚Í‚¾‚ß‚¾
                pad[i] = b[0];
            }

            BigInteger pad_int = new BigInteger(pad);
            pad_int = pad_int << ((input_byte_length + 1) * 8);
            BigInteger result = new BigInteger(2);
            result = result << ((pad_len - 2) * 8);
            result = result | pad_int;
            result = result | input;

            return result;
        }
        
        public static BigInteger PKCS1PadType1(BigInteger input, int pad_len) {
            int input_byte_length = (input.bitCount()+7)/8;
            //System.out.println(String.valueOf(pad_len) + ":" + input_byte_length);
            byte[] pad = new byte[pad_len - input_byte_length - 3];
            
            for(int i = 0; i < pad.Length; i++) {
                pad[i] = (byte)0xff;
            }

            BigInteger pad_int = new BigInteger(pad);
            pad_int = pad_int << ((input_byte_length + 1) * 8);
            BigInteger result = new BigInteger(1);
            result = result << ((pad_len - 2) * 8);
            result = result | pad_int;
            result = result | input;

            return result;
        }
        public static BigInteger StripPKCS1Pad(BigInteger input, int type) {
            byte[] strip = input.getBytes();
            int i;

            if(strip[0] != type) throw new Exception(String.Format("Invalid PKCS1 padding {0}", type));

            for(i = 1; i < strip.Length; i++) {
                if(strip[i] == 0) break;

                if(type == 0x01 && strip[i] != (byte)0xff)
                    throw new Exception("Invalid PKCS1 padding, corrupt data");
            }

            if(i == strip.Length)
                throw new Exception("Invalid PKCS1 padding, corrupt data");

            byte[] val = new byte[strip.Length - i];
            Array.Copy(strip, i, val, 0, val.Length);
            return new BigInteger(val);
        }
    }
}
