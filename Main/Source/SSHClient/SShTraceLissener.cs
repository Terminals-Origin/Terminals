using System;
using System.Diagnostics;
using Granados;

namespace SSHClient
{
    /// <summary>
    /// Debug logging for Granados ssh protocol.
    /// </summary>
    internal class SShTraceLissener : ISSHEventTracer
    {
        public void OnTranmission(String type, String detail)
        {
            WriteDebugMessage(type, detail);
        }

        public void OnReception(String type, String detail)
        {
            WriteDebugMessage(type, detail);
        }

        private static void WriteDebugMessage(String type, String detail)
        {
            Debug.WriteLine(String.Format("Granados message {0}: {1}", type, detail));
        }
    }
}
