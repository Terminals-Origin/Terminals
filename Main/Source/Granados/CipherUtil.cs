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

namespace Granados.Crypto
{
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class CipherUtil
    {
        //Little Endian
        internal static uint GetIntLE(byte[] src, int offset) {
            return ((uint)src[offset	  ]        |
                ((uint)(src[offset + 1]) << 8)  |
                ((uint)(src[offset + 2]) << 16) |
                ((uint)(src[offset + 3]) << 24));
        }
        
        internal static void PutIntLE(uint val, byte[] dest, int offset) {
            dest[offset	]    = (byte)( val        & 0xff);
            dest[offset + 1] = (byte)((val >> 8 ) & 0xff);
            dest[offset + 2] = (byte)((val >> 16) & 0xff);
            dest[offset + 3] = (byte)((val >> 24) & 0xff);
        }
        
        //Big Endian
        internal static uint GetIntBE(byte[] src, int offset) {
            return (((uint)(src[offset    ]) << 24) |
                ((uint)(src[offset + 1]) << 16) |
                ((uint)(src[offset + 2]) << 8)  |
                ((uint)src[offset + 3]));
        }
        
        internal static void PutIntBE(uint val, byte[] dest, int offset) {
            dest[offset    ] = (byte)((val >> 24) & 0xff);
            dest[offset + 1] = (byte)((val >> 16) & 0xff);
            dest[offset + 2] = (byte)((val >> 8 ) & 0xff);
            dest[offset + 3] = (byte)( val        & 0xff);
        }
        
        internal static void BlockXor(byte[] src, int s_offset, int len, byte[] dest, int d_offset) {
            for(; len > 0; len--)
                dest[d_offset++] ^= src[s_offset++];
        }
    
        public static int memcmp(byte[] d1, int o1, byte[] d2, int o2, int len) {
            for(int i=0; i<len; i++) {
                byte b1 = d1[o1+i];
                byte b2 = d2[o2+i];
                if(b1<b2) return -1;
                else if(b1>b2) return 1;
            }
            return 0;
        }
    }
}