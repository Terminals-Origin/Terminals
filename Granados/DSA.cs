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
 * I implemented this algorithm with reference to following products though the algorithm is known publicly.
 *   * MindTerm ( AppGate Network Security )
 */
using System;

namespace Granados.PKI
{
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class DSAKeyPair : KeyPair, ISigner, IVerifier {
        private DSAPublicKey _publickey;
        private BigInteger _x;

        public DSAKeyPair(BigInteger p, BigInteger g, BigInteger q, BigInteger y, BigInteger x) {
            _publickey = new DSAPublicKey(p, g, q, y);
            _x = x;
        }
        public override PublicKeyAlgorithm Algorithm {
            get {
                return PublicKeyAlgorithm.DSA;
            }
        }
        public override PublicKey PublicKey {
            get {
                return _publickey;
            }
        }
        public BigInteger X {
            get {
                return _x;
            }
        }

        public byte[] Sign(byte[] data) {
            BigInteger r = _publickey._g.modPow(_x, _publickey._p) % _publickey._q;
            BigInteger s = (_x.modInverse(_publickey._q) * (new BigInteger(data) + _x * r)) % _publickey._q;

            byte[] result = new byte[data.Length * 2];
            byte[] br = r.getBytes();
            byte[] bs = s.getBytes();
            Array.Copy(br, 0, result, data.Length - br.Length, br.Length);
            Array.Copy(bs, 0, result, data.Length*2 - bs.Length, bs.Length);

            return result;
        }
        public void Verify(byte[] data, byte[] expecteddata) {
            _publickey.Verify(data, expecteddata);
        }

        public static DSAKeyPair GenerateNew(int bits, Random random) {
            BigInteger one = new BigInteger(1);
            BigInteger[] pq = findRandomStrongPrime((uint)bits, 160, random);
            BigInteger p = pq[0], q=pq[1];
            BigInteger   g  = findRandomGenerator(q, p, random);

            BigInteger x;
            do {
                x = new BigInteger();
                x.genRandomBits(q.bitCount(), random);
            } while((x < one) || (x > q));

            BigInteger y = g.modPow(x, p);

            return new DSAKeyPair(p, g, q, y, x);
        }

        private static BigInteger[] findRandomStrongPrime(uint primeBits, int orderBits,	Random random) {
            BigInteger one = new BigInteger(1);
            BigInteger u, aux, aux2;
            long[] table_q, table_u, prime_table;
            PrimeSieve sieve = new PrimeSieve(16000);
            uint table_count  = sieve.AvailablePrimes() - 1;
            int i, j;
            bool flag;
            BigInteger prime = null, order = null;

            order = BigInteger.genPseudoPrime(orderBits, 20, random);

            prime_table = new long[table_count];
            table_q     = new long[table_count];
            table_u     = new long[table_count];

            i = 0;
            for(int pN = 2; pN != 0; pN = sieve.getNextPrime(pN), i++) {
                prime_table[i] = (long)pN;
            }

            for(i = 0; i < table_count; i++) {
                table_q[i] =
                    (((order % new BigInteger(prime_table[i])).LongValue()) *
                    (long)2) % prime_table[i];
            }

            while(true) {
                u = new BigInteger();
                u.genRandomBits((int)primeBits, random);
                u.setBit(primeBits - 1);
                aux = order << 1;
                aux2 = u % aux;
                u = u - aux2;
                u = u + one;

                if(u.bitCount() <= (primeBits - 1))
                    continue;

                for(j = 0; j < table_count; j++) {
                    table_u[j] =
                        (u % new BigInteger(prime_table[j])).LongValue();
                }

                aux2 = order << 1;

                for(i = 0; i < (1 << 24); i++) {
                    long cur_p;
                    long value;

                    flag = true;
                    for(j = 1; j < table_count; j++) {
                        cur_p = prime_table[j];
                        value = table_u[j];
                        if(value >= cur_p)
                            value -= cur_p;
                        if(value == 0)
                            flag = false;
                        table_u[j] = value + table_q[j];
                    }
                    if(!flag)
                        continue;

                    aux   = aux2 * new BigInteger(i);
                    prime = u + aux;

                    if(prime.bitCount() > primeBits)
                        continue;

                    if(prime.isProbablePrime(20))
                        break;
                }

                if(i < (1 << 24))
                    break;
            }

            return new BigInteger[] { prime, order };
        }

        private static BigInteger findRandomGenerator(BigInteger order, BigInteger modulo, Random random) {
            BigInteger one = new BigInteger(1);
            BigInteger aux = modulo - new BigInteger(1);
            BigInteger t   = aux % order;
            BigInteger generator;

            if(t.LongValue() != 0) {
                return null;
            }

            t = aux / order;

            while(true) {
                generator = new BigInteger();
                generator.genRandomBits(modulo.bitCount(), random);
                generator = generator % modulo;
                generator = generator.modPow(t, modulo);
                if(generator!=one)
                    break;
            }

            aux = generator.modPow(order, modulo);

            if(aux!=one) {
                return null;
            }

            return generator;
        }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class DSAPublicKey : PublicKey, IVerifier {
        internal BigInteger _p;
        internal BigInteger _g;
        internal BigInteger _q;
        internal BigInteger _y;

        public DSAPublicKey(BigInteger p, BigInteger g, BigInteger q, BigInteger y) {
            _p = p;
            _g = g;
            _q = q;
            _y = y;
        }
        public override PublicKeyAlgorithm Algorithm {
            get {
                return PublicKeyAlgorithm.DSA;
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
        public BigInteger G {
            get {
                return _g;
            }
        }
        public BigInteger Y {
            get {
                return _y;
            }
        }
        public override void WriteTo(IKeyWriter writer) {
            writer.Write(_p);
            writer.Write(_q);
            writer.Write(_g);
            writer.Write(_y);
        }
        public void Verify(byte[] data, byte[] expecteddata) {

            byte[] first  = new byte[data.Length/2];
            byte[] second = new byte[data.Length/2];
            Array.Copy(data, 0, first, 0, first.Length);
            Array.Copy(data, first.Length, second, 0, second.Length);
            BigInteger r = new BigInteger(first);
            BigInteger s = new BigInteger(second);

            BigInteger w  = s.modInverse(_q);
            BigInteger u1 = (new BigInteger(expecteddata) * w) % _q;
            BigInteger u2 = (r * w) % _q;
            BigInteger v  = ((_g.modPow(u1, _p) * _y.modPow(u2, _p)) % _p) % _q;
            if(v!=r) throw new VerifyException("Failed to verify");
        }
    }
}
