// Note the VB example will give you the first entry of the array n times where n is the size of the array
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals
{
    public class TSManager
    {
        [DllImport("wtsapi32.dll")]
        static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

        [DllImport("wtsapi32.dll")]
        static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll")]
        static extern Int32 WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pMemory);

        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public Int32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;

            public WTS_CONNECTSTATE_CLASS State;
        }

        public enum WTS_INFO_CLASS
        {
            WTSInitialProgram,
            WTSApplicationName,
            WTSWorkingDirectory,
            WTSOEMId,
            WTSSessionId,
            WTSUserName,
            WTSWinStationName,
            WTSDomainName,
            WTSConnectState,
            WTSClientBuildNumber,
            WTSClientName,
            WTSClientDirectory,
            WTSClientProductId,
            WTSClientHardwareId,
            WTSClientAddress,
            WTSClientDisplay,
            WTSClientProtocolType
        }

        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformation(
            System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

        public enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        public const int WTS_CURRENT_SESSION = -1;

        public static IntPtr OpenServer(String name)
        {
            IntPtr server = WTSOpenServer(name);
            return server;
        }

        public static void CloseServer(IntPtr serverHandle)
        {
            WTSCloseServer(serverHandle);
        }

        public static string QuerySessionInfo(int sessionId, WTS_INFO_CLASS infoClass)
        {
            System.IntPtr buffer = IntPtr.Zero;
            uint bytesReturned;
            try
            {
                WTSQuerySessionInformation(System.IntPtr.Zero, sessionId, infoClass, out buffer, out bytesReturned);
                return Marshal.PtrToStringAnsi(buffer);
            }
            catch
            {
                return String.Empty;
            }
            finally
            {
                WTSFreeMemory(buffer);
                buffer = IntPtr.Zero;
            }
        }

        public static List<SessionInfo> ListSessions(string serverName)
        {
            IntPtr server = IntPtr.Zero;
            List<SessionInfo> sessions = new List<SessionInfo>();
            server = OpenServer(serverName);
            try
            {
                /*IntPtr ppSessionInfo = IntPtr.Zero;
                Int32 count = 0;
                Int32 retval = WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count);
                WTS_SESSION_INFO[] wtsSessionInfo = new WTS_SESSION_INFO[count];
                Marshal.Copy(ppSessionInfo,               

                Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                Int32 current = (int)ppSessionInfo;
                if (retval != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        SessionInfo sessionInfo = new SessionInfo();
                        WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));
                        current += dataSize;
                        sessionInfo.Id = si.SessionID;
                        sessionInfo.UserName = QuerySessionInfo(wtsSessionInfo[0].SessionID, WTS_INFO_CLASS.WTSUserName);
                        sessionInfo.DomainName = QuerySessionInfo(wtsSessionInfo[0].SessionID, WTS_INFO_CLASS.WTSDomainName);
                        sessionInfo.ClientName = QuerySessionInfo(wtsSessionInfo[0].SessionID, WTS_INFO_CLASS.WTSClientName);
                        sessions.Add(sessionInfo);
                    }
                    WTSFreeMemory(ppSessionInfo);
                }*/
            }
            finally
            {
                CloseServer(server);
            }
            return sessions;
        }

        /*public bool CheckForDoubleSessions(string domain, string server, string userName)
        {

        }*/
    }

    public class SessionInfo
    {
        private int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string domainName;

        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        private string clientName;

        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }
    }
}
