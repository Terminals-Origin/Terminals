/* ---------------------------------------------------------------------------
 *
 * Copyright (c) Routrek Networks, Inc.    All Rights Reserved..
 * 
 * This file is a part of the Granados SSH Client Library that is subject to
 * the license included in the distributed package.
 * You may not use this file except in compliance with the license.
 * 
 * ---------------------------------------------------------------------------
 */
using System;
using System.IO;
using System.Threading;
using System.Text;

namespace Granados {
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class SSHException : Exception {
        private byte[] _data;

        public SSHException(string msg, byte[] data) : base(msg) {
            _data = data;
        }
    
        public SSHException(string msg) : base(msg) {
        }
    }

    /// <summary>
    /// <ja>SSHプロトコルの種類を示します</ja>
    /// <en>Kind of SSH protocol</en>
    /// </summary>
    public enum SSHProtocol {
        /// <summary>
        /// <ja>SSH1</ja>
        /// <en>SSH1</en>
        /// </summary>
        SSH1,
        /// <summary>
        /// <ja>SSH2</ja>
        /// <en>SSH2</en>
        /// </summary>
        SSH2
    }
    
    /// <summary>
    /// <ja>
    /// アルゴリズムの種類を示します。
    /// </ja>
    /// <en>
    /// Kind of algorithm
    /// </en>
    /// </summary>
    public enum CipherAlgorithm {
        /// <summary>
        /// TripleDES
        /// </summary>
        TripleDES = 3,
        /// <summary>
        /// BlowFish
        /// </summary>
        Blowfish = 6,
        /// <summary>
        /// <ja>AES128（SSH2のみ有効）</ja>
        /// <en>AES128（SSH2 only）</en>
        /// </summary>
        AES128 = 10 //SSH2 ONLY
    }
    
    /// <summary>
    /// <ja>認証方式を示します。</ja>
    /// <en>Kind of authentification method</en>
    /// </summary>
    public enum AuthenticationType {
        /// <summary>
        /// <ja>公開鍵方式</ja>
        /// <en>Public key cryptosystem</en>
        /// </summary>
        PublicKey = 2, //uses identity file
        /// <summary>
        /// <ja>パスワード方式</ja>
        /// <en>Password Authentication</en>
        /// </summary>
        Password = 3,
        /// <summary>
        /// <ja>キーボードインタラクティブ</ja>
        /// <en>KeyboardInteractive</en>
        /// </summary>
        KeyboardInteractive = 4
    }

    /// <summary>
    /// <ja>
    /// 認証結果を示します。
    /// </ja>
    /// <en>
    /// Result of authentication.
    /// </en>
    /// </summary>
    public enum AuthenticationResult {
        /// <summary>
        /// <ja>成功</ja>
        /// <en>Succeed</en>
        /// </summary>
        Success,
        /// <summary>
        /// <ja>失敗</ja>
        /// <en>Failed</en>
        /// </summary>
        Failure,
        /// <summary>
        /// <ja>プロンプト</ja>
        /// <en>Prompt</en>
        /// </summary>
        Prompt
    }


    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum MACAlgorithm {
        HMACSHA1
    }
}

namespace Granados.Util {

    internal class SSHUtil {

        public static string ClientVersionString(SSHProtocol p) {
            return p==SSHProtocol.SSH1? "SSH-1.5-Granados-2.0" : "SSH-2.0-Granados-2.0";
        }

        public static int ReadInt32(Stream input)	{
            byte[] t = new byte[4];
            ReadAll(input, t, 0, t.Length);
            return ReadInt32(t, 0);
        }
        public static int ReadInt32(byte[] data, int offset) {
            int ret =0;
            ret |= (int)(data[offset]);
            ret <<= 8;
            ret |= (int)(data[offset + 1]);
            ret <<= 8;
            ret |= (int)(data[offset + 2]);
            ret <<= 8;
            ret |= (int)(data[offset + 3]);
            return ret;
        }
        /**
        * Network-byte-orderで32ビット値を書き込む。
        */
        public static void WriteIntToByteArray(byte[] dst, int pos, int data) {
            uint udata = (uint)data;
            uint a = udata & 0xFF000000;
            a >>= 24;
            dst[pos] = (byte)a;
            
            a = udata & 0x00FF0000;
            a >>= 16;
            dst[pos+1] = (byte)a;
            
            a = udata & 0x0000FF00;
            a >>= 8;
            dst[pos+2] = (byte)a;

            a = udata & 0x000000FF;
            dst[pos+3] = (byte)a;
            
        }
        public static void WriteIntToStream(Stream input, int data)	{
            byte[] t = new byte[4];
            WriteIntToByteArray(t, 0, data);
            input.Write(t, 0, t.Length);
        }

        public static void ReadAll(Stream input, byte[] buf, int offset, int len) {
            do {
                int fetched = input.Read(buf, offset, len);
                len -= fetched;
                offset += fetched;
            } while(len > 0);
        }

        public static bool ContainsString(string[] s, string v) {
            foreach(string x in s)
                if(x==v) return true;

            return false;
        }

        public static int memcmp(byte[] d1, byte[] d2) {
            for(int i = 0; i<d1.Length; i++) {
                if(d1[i]!=d2[i]) return (int)(d2[i]-d1[i]);
            }
            return 0;
        }
        public static int memcmp(byte[] d1, int o1, byte[] d2, int o2, int len) {
            for(int i = 0; i<len; i++) {
                if(d1[o1+i]!=d2[o2+i]) return (int)(d2[o2+i]-d1[o1+i]);
            }
            return 0;
        }
        public static void ZeroMemory(byte[] t, int offset, int length) {
            for(int i=0; i<length; i++)
                t[offset+length] = 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class Strings {
        private static StringResources _strings;
        public static string GetString(string id) {
            if(_strings==null) Reload();
            return _strings.GetString(id);
        }

        //load resource corresponding to current culture
        public static void Reload() {
            _strings = new StringResources("Granados.strings", typeof(Strings).Assembly);
        }
    }

    internal class DebugUtil {
        public static string DumpByteArray(byte[] data) {
            return DumpByteArray(data, 0, data.Length);
        }
        public static string DumpByteArray(byte[] data, int offset, int length) {
            StringBuilder bld = new StringBuilder();
            for(int i=0; i<length; i++) {
                bld.Append(data[offset+i].ToString("X2"));
                if((i % 4)==3) bld.Append(' ');
            }
            return bld.ToString();
        }

        public static string CurrentThread() {
            Thread t = Thread.CurrentThread;
            return t.GetHashCode().ToString();
        }
    }
}
