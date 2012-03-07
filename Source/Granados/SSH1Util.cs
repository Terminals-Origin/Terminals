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
using Granados.IO;

namespace Granados.SSH1
{
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public class SSHServerInfo {
        public byte[] anti_spoofing_cookie;
        public int    server_key_bits;
        public BigInteger server_key_public_exponent;
        public BigInteger server_key_public_modulus;
        public int        host_key_bits;
        public BigInteger  host_key_public_exponent;
        public BigInteger  host_key_public_modulus;

        internal SSHServerInfo(SSHDataReader reader) {
            anti_spoofing_cookie = reader.Read(8); //first 8 bytes are cookie
            
            server_key_bits = reader.ReadInt32();
            server_key_public_exponent = reader.ReadMPInt();
            server_key_public_modulus = reader.ReadMPInt();
            host_key_bits = reader.ReadInt32();
            host_key_public_exponent = reader.ReadMPInt();
            host_key_public_modulus = reader.ReadMPInt();
        }

    }
    /// <summary>
    /// 
    /// </summary>
    /// <exclude/>
    public enum PacketType {
        SSH_MSG_DISCONNECT = 1,
        SSH_SMSG_PUBLIC_KEY = 2,
        SSH_CMSG_SESSION_KEY = 3,
        SSH_CMSG_USER = 4,
        SSH_CMSG_AUTH_RSA = 6,
        SSH_SMSG_AUTH_RSA_CHALLENGE = 7,
        SSH_CMSG_AUTH_RSA_RESPONSE = 8,
        SSH_CMSG_AUTH_PASSWORD = 9,
        SSH_CMSG_REQUEST_PTY = 10,
        SSH_CMSG_WINDOW_SIZE = 11,
        SSH_CMSG_EXEC_SHELL = 12,
        SSH_CMSG_EXEC_CMD = 13,
        SSH_SMSG_SUCCESS = 14,
        SSH_SMSG_FAILURE = 15,
        SSH_CMSG_STDIN_DATA = 16,
        SSH_SMSG_STDOUT_DATA = 17,
        SSH_SMSG_STDERR_DATA = 18,
        SSH_CMSG_EOF = 19,
        SSH_SMSG_EXITSTATUS = 20,
        SSH_MSG_CHANNEL_OPEN_CONFIRMATION = 21,
        SSH_MSG_CHANNEL_OPEN_FAILURE = 22,
        SSH_MSG_CHANNEL_DATA = 23,
        SSH_MSG_CHANNEL_CLOSE = 24,
        SSH_MSG_CHANNEL_CLOSE_CONFIRMATION = 25,
        SSH_CMSG_PORT_FORWARD_REQUEST = 28,
        SSH_MSG_PORT_OPEN = 29,
        SSH_MSG_IGNORE = 32,
        SSH_CMSG_EXIT_CONFIRMATION = 33,
        SSH_MSG_DEBUG = 36
    }
}
