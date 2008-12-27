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

namespace Routrek.Toolkit {

	public class Base64 {

		private static readonly int[] _fromBase64 = new int[] {
													   -1, -1, -1, -1, -1, -1, -1, -1,
													   -1, -1, -1, -1, -1, -1, -1, -1,
													   -1, -1, -1, -1, -1, -1, -1, -1,
													   -1, -1, -1, -1, -1, -1, -1, -1,
													   -1, -1, -1, -1, -1, -1, -1, -1,
													   -1, -1, -1, 62, -1, -1, -1, 63,
													   52, 53, 54, 55, 56, 57, 58, 59,
													   60, 61, -1, -1, -1, -1, -1, -1,
													   -1,  0,  1,  2,  3,  4,  5,  6,
													   7,  8,  9, 10, 11, 12, 13, 14,
													   15, 16, 17, 18, 19, 20, 21, 22,
													   23, 24, 25, -1, -1, -1, -1, -1,
													   -1, 26, 27, 28, 29, 30, 31, 32,
													   33, 34, 35, 36, 37, 38, 39, 40,
													   41, 42, 43, 44, 45, 46, 47, 48,
													   49, 50, 51, -1, -1, -1, -1, -1
												   };

		private static readonly byte[] _toBase64 = new byte[] {
													   // 'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P',
													   65, 66, 67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80,
													   // 'Q','R','S','T','U','V','W','X','Y','Z','a','b','c','d','e','f',
													   81, 82, 83, 84, 85, 86, 87, 88, 89, 90, 97, 98, 99,100,101,102,
													   // 'g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v',
													   103,104,105,106,107,108,109,110,111,112,113,114,115,116,117,118,
													   // 'w','x','y','z','0','1','2','3','4','5','6','7','8','9','+','/'
													   119,120,121,122, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57, 43, 47, (byte)'='
												   };

		private const int PAD = 64;

		public static byte[] Encode(byte[] data) {
			return Encode(data, 0, data.Length);
		}

		public static byte[] Decode(byte[] data) {
			return Decode(data, 0, data.Length);
		}

		public static byte[] Decode(byte[] encoded, int offset, int length) {
			int j, c, v;
			int bits = 0;

			int n    = offset + length;
			byte[] data = new byte[(length * 3) / 4];

			v = 0;
			j = 0;
			for(int i = offset; i < n; i++) {
				c = _fromBase64[encoded[i]];
				if(c < 0) {
					if(encoded[i] == _toBase64[PAD])
						break;
					continue;
				}
				v = (v << 6) | c;
				bits += 6;
				if(bits >= 8) {
					bits -= 8;
					data[j++] = (byte) ((v >> bits) & 0xff);
				}
			}

			if(data.Length > j) {
				byte[] temp = new byte[j];
				Array.Copy(data, 0, temp, 0, j);
				data = temp;
			}

			return data;
		}

		public static byte[] Encode(byte[] data, int offset, int length) {
			byte[] encoded;
			int x1, x2, x3, x4;
			int i, j, r, n;

			r       = length % 3;
			n       = offset + (length - r);
			encoded = new byte[((length / 3) * 4) + (r != 0 ? 4 : 0)];

			for(i = offset, j = 0; i < n; i += 3, j += 4) {
				x1 = (data[i] & 0xfc) >> 2;
				x2 = (((data[i    ] & 0x03) << 4) | ((data[i + 1] & 0xf0) >> 4));
				x3 = (((data[i + 1] & 0x0f) << 2) | ((data[i + 2] & 0xc0) >> 6));
				x4 = (data[i + 2] & 0x3f);
				encoded[j    ] = _toBase64[x1];
				encoded[j + 1] = _toBase64[x2];
				encoded[j + 2] = _toBase64[x3];
				encoded[j + 3] = _toBase64[x4];
			}

			if(r != 0) {
				x1 = (data[i] & 0xfc) >> 2;
				x2 = (data[i] & 0x03) << 4;
				x3 = PAD;
				x4 = PAD;
				if(r == 2) {
					x2 |= ((data[i + 1] & 0xf0) >> 4);
					x3 = (data[i + 1] & 0x0f) << 2;
				}
				encoded[j++] = _toBase64[x1];
				encoded[j++] = _toBase64[x2];
				encoded[j++] = _toBase64[x3];
				encoded[j++] = _toBase64[x4];
			}

			return encoded;
		}


		//for test
		public static void test() {
			string t = "hernan crespo";
			string s = System.Text.Encoding.ASCII.GetString(Decode(Encode(System.Text.Encoding.ASCII.GetBytes(t))));
		
		}
	}
}