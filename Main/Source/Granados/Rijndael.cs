/* ---------------------------------------------------------------------------
 *
 * Copyright (c) Routrek Networks, Inc.    All Rights Reserved..
 * 
 * This file is a part of the Granados SSH Client Library that is subject to
 * the license included in the distributed package.
 * You may not use this file except in compliance with the license.
 * 
 * ---------------------------------------------------------------------------
 * 
 * Rijndael was written by <a href="mailto:rijmen@esat.kuleuven.ac.be">Vincent
 * Rijmen</a> and <a href="mailto:Joan.Daemen@village.uunet.be">Joan Daemen</a>.
 * 
 * ---------------------------------------------------------------------------
 * 
 * I implemented this algorithm with reference to following products though the algorithm is known publicly.
 *   * MindTerm ( AppGate Network Security )
 */
using System;
using Granados.Crypto;

namespace Granados.Algorithms
{
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class Rijndael
    {
        byte[] _IV;
        int[][] _Ke;			// encryption round keys
        int[][] _Kd;			// decryption round keys
        private int _rounds;

        public Rijndael() {
            _IV = new byte[GetBlockSize()];
        }

        public int GetBlockSize() {
            return BLOCK_SIZE;
        }

        ///////////////////////////////////////////////
        // set _IV
        ///////////////////////////////////////////////
        public void SetIV(byte[] newiv) {
            Array.Copy(newiv, 0, _IV, 0, _IV.Length);
        }

        ///////////////////////////////////////////////
        // set KEY
        ///////////////////////////////////////////////
        public void InitializeKey(byte[] key) {
            if (key == null)
                throw new Exception("Empty key");
            //128bit or 192bit or 256bit
            if (!(key.Length == 16 || key.Length == 24 || key.Length == 32))
                throw new Exception("Incorrect key length");

            _rounds = getRounds(key.Length, GetBlockSize());
            _Ke = new int[_rounds + 1][]; 
            _Kd = new int[_rounds + 1][]; 
            int i, j;
            for(i=0; i<_rounds + 1; i++) {
                _Ke[i] = new int[BC];
                _Kd[i] = new int[BC];
            }

            int ROUND_KEY_COUNT = (_rounds + 1) * BC;
            int KC = key.Length / 4;
            int[] tk = new int[KC];

            for (i = 0, j = 0; i < KC; ) {
                tk[i++] = (key[j++] & 0xFF) << 24 |
                          (key[j++] & 0xFF) << 16 |
                          (key[j++] & 0xFF) <<  8 |
                          (key[j++] & 0xFF);
            }

            int t = 0;
            for (j = 0; (j < KC) && (t < ROUND_KEY_COUNT); j++, t++) {
                _Ke[t / BC][t % BC] = tk[j];
                _Kd[_rounds - (t / BC)][t % BC] = tk[j];
            }
            int tt, rconpointer = 0;
            while (t < ROUND_KEY_COUNT) {
                tt = tk[KC - 1];
                tk[0] ^= (S[(tt >> 16) & 0xFF] & 0xFF) << 24 ^
                         (S[(tt >>  8) & 0xFF] & 0xFF) << 16 ^
                         (S[ tt	       & 0xFF] & 0xFF) <<  8 ^
                         (S[(tt >> 24) & 0xFF] & 0xFF)       ^
                         (rcon[rconpointer++]  & 0xFF) << 24;

                if (KC != 8) {
                    for (i = 1, j = 0; i < KC; )
                        tk[i++] ^= tk[j++];
                }
                else {
                    for (i = 1, j = 0; i < KC / 2; )
                        tk[i++] ^= tk[j++];
                    tt = tk[KC / 2 - 1];
                    tk[KC / 2] ^= (S[ tt        & 0xFF] & 0xFF)       ^
                                  (S[(tt >>  8) & 0xFF] & 0xFF) <<  8 ^
                                  (S[(tt >> 16) & 0xFF] & 0xFF) << 16 ^
                                  (S[(tt >> 24) & 0xFF] & 0xFF) << 24;
                    for (j = KC / 2, i = j + 1; i < KC; )
                        tk[i++] ^= tk[j++];
                }
                for (j = 0; (j < KC) && (t < ROUND_KEY_COUNT); j++, t++) {
                    _Ke[t / BC][t % BC] = tk[j];
                    _Kd[_rounds - (t / BC)][t % BC] = tk[j];
                }
            }
            for (int r = 1; r < _rounds; r++){
                for (j = 0; j < BC; j++) {
                    tt = _Kd[r][j];
                    _Kd[r][j] = U1[(tt >> 24) & 0xFF] ^
                               U2[(tt >> 16) & 0xFF] ^
                               U3[(tt >>  8) & 0xFF] ^
                               U4[ tt        & 0xFF];
                }
            }
        }

        public static int getRounds(int keySize, int blockSize) {
            switch (keySize) {
                case 16:
                    return blockSize == 16 ? 10 : (blockSize == 24 ? 12 : 14);
                case 24:
                    return blockSize != 32 ? 12 : 14;
                default: 
                    return 14;
            }
        }

        public void encryptCBC(byte[] input, int inputOffset, int inputLen, byte[] output, int outputOffset) {
            int block_size = GetBlockSize();
            int nBlocks = inputLen / block_size;
            for(int bc = 0; bc < nBlocks; bc++) {
                CipherUtil.BlockXor(input, inputOffset, block_size, _IV, 0);
                blockEncrypt(_IV, 0, output, outputOffset);
                Array.Copy(output, outputOffset, _IV, 0, block_size);
                inputOffset  += block_size;
                outputOffset += block_size;
            }
        }

        public void decryptCBC(byte[] input, int inputOffset, int inputLen, byte[] output, int outputOffset) {
            int block_size = GetBlockSize();
            byte[] tmpBlk = new byte[block_size];
            int nBlocks = inputLen / block_size;
            for(int bc = 0; bc < nBlocks; bc++) 
            {
                blockDecrypt(input, inputOffset, tmpBlk, 0);
                for(int i = 0; i < block_size; i++) 
                {
                    tmpBlk[i] ^= _IV[i];
                    _IV[i] = input[inputOffset + i];
                    output[outputOffset + i] = tmpBlk[i];
                }
                inputOffset  += block_size;
                outputOffset += block_size;
            }
        }

        public void blockEncrypt(byte[] src, int inOffset, byte[] dst, int outOffset) {
            int[] Ker  = _Ke[0];

            int t0 = ((src[inOffset++] & 0xFF) << 24 |
                      (src[inOffset++] & 0xFF) << 16 |
                      (src[inOffset++] & 0xFF) <<  8 |
                      (src[inOffset++] & 0xFF)        ) ^ Ker[0];
            int t1 = ((src[inOffset++] & 0xFF) << 24 |
                      (src[inOffset++] & 0xFF) << 16 |
                      (src[inOffset++] & 0xFF) <<  8 |
                      (src[inOffset++] & 0xFF)        ) ^ Ker[1];
            int t2 = ((src[inOffset++] & 0xFF) << 24 |
                      (src[inOffset++] & 0xFF) << 16 |
                      (src[inOffset++] & 0xFF) <<  8 |
                      (src[inOffset++] & 0xFF)        ) ^ Ker[2];
            int t3 = ((src[inOffset++] & 0xFF) << 24 |
                      (src[inOffset++] & 0xFF) << 16 |
                      (src[inOffset++] & 0xFF) <<  8 |
                      (src[inOffset++] & 0xFF)        ) ^ Ker[3];

            int a0, a1, a2, a3;
            for (int r = 1; r < _rounds; r++) {
                Ker = _Ke[r];
                a0 = (T1[(t0 >> 24) & 0xFF] ^
                      T2[(t1 >> 16) & 0xFF] ^
                      T3[(t2 >>  8) & 0xFF] ^
                      T4[ t3        & 0xFF]  ) ^ Ker[0];
                a1 = (T1[(t1 >> 24) & 0xFF] ^
                      T2[(t2 >> 16) & 0xFF] ^
                      T3[(t3 >>  8) & 0xFF] ^
                      T4[ t0        & 0xFF]  ) ^ Ker[1];
                a2 = (T1[(t2 >> 24) & 0xFF] ^
                      T2[(t3 >> 16) & 0xFF] ^
                      T3[(t0 >>  8) & 0xFF] ^
                      T4[ t1        & 0xFF]  ) ^ Ker[2];
                a3 = (T1[(t3 >> 24) & 0xFF] ^
                      T2[(t0 >> 16) & 0xFF] ^
                      T3[(t1 >>  8) & 0xFF] ^
                      T4[ t2        & 0xFF]  ) ^ Ker[3];
                t0 = a0;
                t1 = a1;
                t2 = a2;
                t3 = a3;
            }

            Ker = _Ke[_rounds];
            int tt = Ker[0];
            dst[outOffset + 0] = (byte)(S[(t0 >> 24) & 0xFF] ^ (tt >> 24));
            dst[outOffset + 1] = (byte)(S[(t1 >> 16) & 0xFF] ^ (tt >> 16));
            dst[outOffset + 2] = (byte)(S[(t2 >>  8) & 0xFF] ^ (tt >>  8));
            dst[outOffset + 3] = (byte)(S[ t3        & 0xFF] ^  tt	       );
            tt = Ker[1];
            dst[outOffset + 4] = (byte)(S[(t1 >> 24) & 0xFF] ^ (tt >> 24));
            dst[outOffset + 5] = (byte)(S[(t2 >> 16) & 0xFF] ^ (tt >> 16));
            dst[outOffset + 6] = (byte)(S[(t3 >>  8) & 0xFF] ^ (tt >>  8));
            dst[outOffset + 7] = (byte)(S[ t0		 & 0xFF] ^  tt		);
            tt = Ker[2];
            dst[outOffset + 8] = (byte)(S[(t2 >> 24) & 0xFF] ^ (tt >> 24));
            dst[outOffset + 9] = (byte)(S[(t3 >> 16) & 0xFF] ^ (tt >> 16));
            dst[outOffset +10] = (byte)(S[(t0 >>  8) & 0xFF] ^ (tt >>  8));
            dst[outOffset +11] = (byte)(S[ t1		 & 0xFF] ^  tt		);
            tt = Ker[3];
            dst[outOffset +12] = (byte)(S[(t3 >> 24) & 0xFF] ^ (tt >> 24));
            dst[outOffset +13] = (byte)(S[(t0 >> 16) & 0xFF] ^ (tt >> 16));
            dst[outOffset +14] = (byte)(S[(t1 >>  8) & 0xFF] ^ (tt >>  8));
            dst[outOffset +15] = (byte)(S[ t2		 & 0xFF] ^  tt		);
        }

        public void blockDecrypt(byte[] src, int inOffset, byte[] dst, int outOffset) {
            int[] Kdr = _Kd[0];

            int t0 = ((src[inOffset++] & 0xFF) << 24 |
                      (src[inOffset++] & 0xFF) << 16 |
                      (src[inOffset++] & 0xFF) <<  8 |
                      (src[inOffset++] & 0xFF)        ) ^ Kdr[0];
            int t1 = ((src[inOffset++] & 0xFF) << 24 |
                      (src[inOffset++] & 0xFF) << 16 |
                      (src[inOffset++] & 0xFF) <<  8 |
                      (src[inOffset++] & 0xFF)        ) ^ Kdr[1];
            int t2 = ((src[inOffset++] & 0xFF) << 24 |
                      (src[inOffset++] & 0xFF) << 16 |
                      (src[inOffset++] & 0xFF) <<  8 |
                      (src[inOffset++] & 0xFF)        ) ^ Kdr[2];
            int t3 = ((src[inOffset++] & 0xFF) << 24 |
                      (src[inOffset++] & 0xFF) << 16 |
                      (src[inOffset++] & 0xFF) <<  8 |
                      (src[inOffset++] & 0xFF)        ) ^ Kdr[3];

            int a0, a1, a2, a3;
            for (int r = 1; r < _rounds; r++) {
                Kdr = _Kd[r];
                a0 = (T5[(t0 >> 24) & 0xFF] ^
                      T6[(t3 >> 16) & 0xFF] ^
                      T7[(t2 >>  8) & 0xFF] ^
                      T8[ t1        & 0xFF]  ) ^ Kdr[0];
                a1 = (T5[(t1 >> 24) & 0xFF] ^
                      T6[(t0 >> 16) & 0xFF] ^
                      T7[(t3 >>  8) & 0xFF] ^
                      T8[ t2        & 0xFF]  ) ^ Kdr[1];
                a2 = (T5[(t2 >> 24) & 0xFF] ^
                      T6[(t1 >> 16) & 0xFF] ^
                      T7[(t0 >>  8) & 0xFF] ^
                      T8[ t3        & 0xFF]  ) ^ Kdr[2];
                a3 = (T5[(t3 >> 24) & 0xFF] ^
                      T6[(t2 >> 16) & 0xFF] ^
                      T7[(t1 >>  8) & 0xFF] ^
                      T8[ t0        & 0xFF]  ) ^ Kdr[3];
                t0 = a0;
                t1 = a1;
                t2 = a2;
                t3 = a3;
            }

            Kdr = _Kd[_rounds];
            int tt = Kdr[0];
            dst[outOffset + 0] = (byte)(Si[(t0 >> 24) & 0xFF] ^ (tt >> 24));
            dst[outOffset + 1] = (byte)(Si[(t3 >> 16) & 0xFF] ^ (tt >> 16));
            dst[outOffset + 2] = (byte)(Si[(t2 >>  8) & 0xFF] ^ (tt >>  8));
            dst[outOffset + 3] = (byte)(Si[ t1        & 0xFF] ^  tt       );
            tt = Kdr[1];
            dst[outOffset + 4] = (byte)(Si[(t1 >> 24) & 0xFF] ^ (tt >> 24));
            dst[outOffset + 5] = (byte)(Si[(t0 >> 16) & 0xFF] ^ (tt >> 16));
            dst[outOffset + 6] = (byte)(Si[(t3 >>  8) & 0xFF] ^ (tt >>  8));
            dst[outOffset + 7] = (byte)(Si[ t2        & 0xFF] ^  tt       );
            tt = Kdr[2];
            dst[outOffset + 8] = (byte)(Si[(t2 >> 24) & 0xFF] ^ (tt >> 24));
            dst[outOffset + 9] = (byte)(Si[(t1 >> 16) & 0xFF] ^ (tt >> 16));
            dst[outOffset +10] = (byte)(Si[(t0 >>  8) & 0xFF] ^ (tt >>  8));
            dst[outOffset +11] = (byte)(Si[ t3        & 0xFF] ^  tt	      );
            tt = Kdr[3];
            dst[outOffset +12] = (byte)(Si[(t3 >> 24) & 0xFF] ^ (tt >> 24));
            dst[outOffset +13] = (byte)(Si[(t2 >> 16) & 0xFF] ^ (tt >> 16));
            dst[outOffset +14] = (byte)(Si[(t1 >>  8) & 0xFF] ^ (tt >>  8));
            dst[outOffset +15] = (byte)(Si[ t0        & 0xFF] ^  tt       );
        }

        /// <summary>
        /// constants
        /// </summary>
        private const int BLOCK_SIZE = 16; 
        private const int BC         = 4; 

        private static readonly int[] alog  = new int[256];
        private static readonly int[] log   = new int[256];

        private static readonly byte[] S    = new byte[256];
        private static readonly byte[] Si   = new byte[256];
        private static readonly int[]  T1   = new int[256];
        private static readonly int[]  T2   = new int[256];
        private static readonly int[]  T3   = new int[256];
        private static readonly int[]  T4   = new int[256];
        private static readonly int[]  T5   = new int[256];
        private static readonly int[]  T6   = new int[256];
        private static readonly int[]  T7   = new int[256];
        private static readonly int[]  T8   = new int[256];
        private static readonly int[]  U1   = new int[256];
        private static readonly int[]  U2   = new int[256];
        private static readonly int[]  U3   = new int[256];
        private static readonly int[]  U4   = new int[256];
        private static readonly byte[] rcon = new byte[30];

        private static readonly int[,,] shifts = new int[,,] {
        { {0, 0}, {1, 3}, {2, 2}, {3, 1} },
        { {0, 0}, {1, 5}, {2, 4}, {3, 3} },
        { {0, 0}, {1, 7}, {3, 5}, {4, 4} }};

        ///////////////////////////////////
        //class initialization
        ///////////////////////////////////
        static Rijndael() {
            int ROOT = 0x11B;
            int i, j = 0;

            alog[0] = 1;
            for (i = 1; i < 256; i++) {
                j = (alog[i-1] << 1) ^ alog[i-1];
                if ((j & 0x100) != 0) j ^= ROOT;
                alog[i] = j;
            }
            for (i = 1; i < 255; i++) log[alog[i]] = i;
            byte[,] A = new byte[,] {
                {1, 1, 1, 1, 1, 0, 0, 0},
                {0, 1, 1, 1, 1, 1, 0, 0},
                {0, 0, 1, 1, 1, 1, 1, 0},
                {0, 0, 0, 1, 1, 1, 1, 1},
                {1, 0, 0, 0, 1, 1, 1, 1},
                {1, 1, 0, 0, 0, 1, 1, 1},
                {1, 1, 1, 0, 0, 0, 1, 1},
                {1, 1, 1, 1, 0, 0, 0, 1}};
            byte[] B = new byte[] { 0, 1, 1, 0, 0, 0, 1, 1};

            int t;
            byte[,] box = new byte[256,8];
            box[1,7] = 1;
            for (i = 2; i < 256; i++) {
                j = alog[255 - log[i]];
                for (t = 0; t < 8; t++) {
                    box[i,t] = (byte)((j >> (7 - t)) & 0x01);
                }
            }

            byte[,] cox = new byte[256,8];
            for (i = 0; i < 256; i++) {
                for (t = 0; t < 8; t++) {
                    cox[i,t] = B[t];
                    for (j = 0; j < 8; j++)
                        cox[i,t] ^= (byte)(A[t,j] * box[i,j]);
                }
            }

            for (i = 0; i < 256; i++) {
                S[i] = (byte)(cox[i,0] << 7);
                for (t = 1; t < 8; t++)
                    S[i] ^= (byte)(cox[i,t] << (7-t));
                Si[S[i] & 0xFF] = (byte) i;
            }
            byte[][] G = new byte[4][];
            G[0] = new byte[] {2, 1, 1, 3};
            G[1] = new byte[] {3, 2, 1, 1};
            G[2] = new byte[] {1, 3, 2, 1};
            G[3] = new byte[] {1, 1, 3, 2};

            byte[,] AA = new byte[4,8];
            for (i = 0; i < 4; i++) {
                for (j = 0; j < 4; j++) AA[i,j] = G[i][j];
                AA[i,i+4] = 1;
            }
            byte pivot, tmp;
            byte[][] iG = new byte[4][];
            for (i = 0; i < 4; i++)
                iG[i] = new byte[4];

            for (i = 0; i < 4; i++) {
                pivot = AA[i,i];
                if (pivot == 0) {
                    t = i + 1;
                    while ((AA[t,i] == 0) && (t < 4))
                        t++;
                    if (t != 4)	{
                        for (j = 0; j < 8; j++) {
                            tmp = AA[i,j];
                            AA[i,j] = AA[t,j];
                            AA[t,j] = (byte) tmp;
                        }
                        pivot = AA[i,i];
                    }
                }
                for (j = 0; j < 8; j++)
                    if (AA[i,j] != 0)
                        AA[i,j] = (byte)
                            alog[(255 + log[AA[i,j] & 0xFF] - log[pivot & 0xFF]) % 255];
                for (t = 0; t < 4; t++)
                    if (i != t) {
                        for (j = i+1; j < 8; j++)
                            AA[t,j] ^= (byte)mul(AA[i,j], AA[t,i]);
                        AA[t,i] = 0;
                    }
            }

            for (i = 0; i < 4; i++)
                for (j = 0; j < 4; j++) iG[i][j] = AA[i,j + 4];

            int s;
            for (t = 0; t < 256; t++) {
                s = S[t];
                T1[t] = mul4(s, G[0]);
                T2[t] = mul4(s, G[1]);
                T3[t] = mul4(s, G[2]);
                T4[t] = mul4(s, G[3]);

                s = Si[t];
                T5[t] = mul4(s, iG[0]);
                T6[t] = mul4(s, iG[1]);
                T7[t] = mul4(s, iG[2]);
                T8[t] = mul4(s, iG[3]);

                U1[t] = mul4(t, iG[0]);
                U2[t] = mul4(t, iG[1]);
                U3[t] = mul4(t, iG[2]);
                U4[t] = mul4(t, iG[3]);
            }

            rcon[0] = 1;
            int r = 1;
            for (t = 1; t < 30; ) rcon[t++] = (byte)(r = mul(2, r));
        }

        private static int mul(int a, int b) {
            return (a != 0 && b != 0) ?
                    alog[(log[a & 0xFF] + log[b & 0xFF]) % 255] :
                    0;
        }

        private static int mul4(int a, byte[] b) {
            if (a == 0) return 0;
            a = log[a & 0xFF];
            int a0 = (b[0] != 0) ? alog[(a + log[b[0] & 0xFF]) % 255] & 0xFF : 0;
            int a1 = (b[1] != 0) ? alog[(a + log[b[1] & 0xFF]) % 255] & 0xFF : 0;
            int a2 = (b[2] != 0) ? alog[(a + log[b[2] & 0xFF]) % 255] & 0xFF : 0;
            int a3 = (b[3] != 0) ? alog[(a + log[b[3] & 0xFF]) % 255] & 0xFF : 0;
            return a0 << 24 | a1 << 16 | a2 << 8 | a3;
        }
    }
}
