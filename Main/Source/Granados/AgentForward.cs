using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Granados.IO;
using Granados.IO.SSH2;
using Granados.SSH2;
using Granados.Util;

namespace Granados {
    public enum AgentForwadPacketType {
   /* Messages sent by the client. */
#if false
    SSH_AGENT_REQUEST_VERSION                     =   1,
    SSH_AGENT_ADD_KEY                             = 202,
    SSH_AGENT_DELETE_ALL_KEYS                     = 203,
    SSH_AGENT_LIST_KEYS                           = 204,
    SSH_AGENT_PRIVATE_KEY_OP                      = 205,
    SSH_AGENT_FORWARDING_NOTICE                   = 206,
    SSH_AGENT_DELETE_KEY                          = 207,
    SSH_AGENT_LOCK                                = 208,
    SSH_AGENT_UNLOCK                              = 209,
    SSH_AGENT_PING                                = 212,
    SSH_AGENT_RANDOM                              = 213,
    SSH_AGENT_EXTENSION                           = 301,

   /* Messages sent by the agent. */
    SSH_AGENT_SUCCESS                             = 101,
    SSH_AGENT_FAILURE                             = 102,
    SSH_AGENT_VERSION_RESPONSE                    = 103,
    SSH_AGENT_KEY_LIST                            = 104,
    SSH_AGENT_OPERATION_COMPLETE                  = 105,
    SSH_AGENT_RANDOM_DATA                         = 106,
    SSH_AGENT_ALIVE                               = 150,
#endif

    /*
     * OpenSSH's SSH-2 agent messages.
     */
    SSH_AGENT_FAILURE                   = 5,
    SSH_AGENT_SUCCESS                   = 6,

    SSH2_AGENTC_REQUEST_IDENTITIES          = 11,
    SSH2_AGENT_IDENTITIES_ANSWER            = 12,
    SSH2_AGENTC_SIGN_REQUEST                = 13,
    SSH2_AGENT_SIGN_RESPONSE                = 14,
    SSH2_AGENTC_ADD_IDENTITY                = 17,
    SSH2_AGENTC_REMOVE_IDENTITY             = 18,
    SSH2_AGENTC_REMOVE_ALL_IDENTITIES       = 19
    }

    //the client must implement this interface and set SSHConnectionParameter
    public interface IAgentForward {
        bool CanAcceptForwarding(); //ask agent forwarding is available
        SSH2UserAuthKey[] GetAvailableSSH2UserAuthKeys(); //list key
        
        //notifications
        void NotifyPublicKeyDidNotMatch();
        void Close();
        void OnError(Exception ex);
    }
    
    //currently OpenSSH's SSH2 connections are only supported
    internal class AgentForwardingChannel : ISSHChannelEventReceiver {
        private SSHChannel _channel;
        private IAgentForward _client;
        private SimpleMemoryStream _buffer;
        private bool _closed;

        public AgentForwardingChannel(IAgentForward client) {
            _client = client;
            _buffer = new SimpleMemoryStream();
        }
        internal void SetChannel(SSHChannel channel) {
            _channel = channel;
        }

        public void OnData(byte[] data, int offset, int length) {
            _buffer.Write(data, offset, length);
            int expectedLength = SSHUtil.ReadInt32(_buffer.UnderlyingBuffer, 0);
            if(expectedLength + 4 <= _buffer.Length) {
                SSH2DataReader r = new SSH2DataReader(new DataFragment(_buffer.UnderlyingBuffer, 4, _buffer.Length-4));
                AgentForwadPacketType pt = (AgentForwadPacketType)r.ReadByte();
                //remaining len-1
                _buffer.SetOffset(0);

                switch(pt) {
                    case AgentForwadPacketType.SSH2_AGENTC_REQUEST_IDENTITIES:
                        SendKeyList();
                        break;
                    case AgentForwadPacketType.SSH2_AGENTC_SIGN_REQUEST:
                        SendSign(r);
                        break;
                    default:
                        //Debug.WriteLine("Unknown agent packet " + pt.ToString());
                        TransmitWriter(OpenWriter(AgentForwadPacketType.SSH_AGENT_FAILURE));
                        break;
                }
            }
        }

        public void OnExtendedData(int type, byte[] data) {
        }

        public void OnChannelClosed() {
            if(!_closed) {
                _closed = true;
                _client.Close();
            }
        }

        public void OnChannelEOF() {
            if(!_closed) {
                _closed = true;
                _client.Close();
            }
        }

        public void OnChannelError(Exception error) {
            _client.OnError(error);
        }

        public void OnChannelReady() {
        }

        public void OnMiscPacket(byte packet_type, byte[] data, int offset, int length) {
        }

        private void SendKeyList() {
            SSH2DataWriter wr = OpenWriter(AgentForwadPacketType.SSH2_AGENT_IDENTITIES_ANSWER);
            // keycount, ((blob-len, pubkey-blob, comment-len, comment) * keycount)
            SSH2UserAuthKey[] keys = _client.GetAvailableSSH2UserAuthKeys();
            wr.Write(keys.Length);
            foreach(SSH2UserAuthKey key in keys) {
                byte[] blob = key.GetPublicKeyBlob();
                wr.WriteAsString(blob);
                Debug.WriteLine("Userkey comment="+key.Comment);
                wr.WriteAsString(Encoding.UTF8.GetBytes(key.Comment));
            }
            TransmitWriter(wr);
        }
        private void SendSign(SSH2DataReader r) {
            byte[] blob = r.ReadString();
            byte[] data = r.ReadString();
            //Debug.WriteLine(String.Format("SignRequest blobsize={0} datasize={1}", blob.Length, data.Length));

            SSH2UserAuthKey[] keys = _client.GetAvailableSSH2UserAuthKeys();
            SSH2UserAuthKey key = FindKey(keys, blob);
            if(key==null) {
                TransmitWriter(OpenWriter(AgentForwadPacketType.SSH_AGENT_FAILURE));
                _client.NotifyPublicKeyDidNotMatch();
            }
            else {
                SSH2DataWriter signpack = new SSH2DataWriter();
                signpack.Write(SSH2Util.PublicKeyAlgorithmName(key.Algorithm));
                signpack.WriteAsString(key.Sign(data));

                SSH2DataWriter wr = OpenWriter(AgentForwadPacketType.SSH2_AGENT_SIGN_RESPONSE);
                wr.WriteAsString(signpack.ToByteArray());
                TransmitWriter(wr);
            }
        }

        //writer util
        private SSH2DataWriter OpenWriter(AgentForwadPacketType pt) {
            SSH2DataWriter wr = new SSH2DataWriter();
            wr.Write(0); //length field
            wr.Write((byte)pt);
            return wr;
        }
        private void TransmitWriter(SSH2DataWriter wr) {
            int o = wr.Length;
            wr.SetOffset(0);
            wr.Write(o - 4); //length of int32
            _channel.Transmit(wr.UnderlyingBuffer, 0, o);
        }

        private SSH2UserAuthKey FindKey(SSH2UserAuthKey[] keys, byte[] blob) {
            foreach(SSH2UserAuthKey key in keys) {
                byte[] t = key.GetPublicKeyBlob();
                if(t.Length==blob.Length && Util.SSHUtil.memcmp(t, blob)==0) return key;
            }
            return null;
        }
    }
}
