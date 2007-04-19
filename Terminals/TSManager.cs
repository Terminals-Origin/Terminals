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

        public static string QuerySessionInfo(IntPtr server, int sessionId, WTS_INFO_CLASS infoClass)
        {
            System.IntPtr buffer = IntPtr.Zero;
            uint bytesReturned;
            try
            {
                WTSQuerySessionInformation(server, sessionId, infoClass, out buffer, out bytesReturned);
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
            return ListSessions(serverName, null, null, null, null);
        }

        public static List<SessionInfo> ListSessions(string serverName, string userName, string domainName, 
            string clientName, WTS_CONNECTSTATE_CLASS? state)
        {
            IntPtr server = IntPtr.Zero;
            List<SessionInfo> sessions = new List<SessionInfo>();
            server = OpenServer(serverName);
            try
            {
                IntPtr ppSessionInfo = IntPtr.Zero;
                Int32 count = 0;
                Int32 retval = WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count);
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
                        sessionInfo.UserName = QuerySessionInfo(server, sessionInfo.Id, WTS_INFO_CLASS.WTSUserName);
                        sessionInfo.DomainName = QuerySessionInfo(server, sessionInfo.Id, WTS_INFO_CLASS.WTSDomainName);
                        sessionInfo.ClientName = QuerySessionInfo(server, sessionInfo.Id, WTS_INFO_CLASS.WTSClientName);
                        sessionInfo.State = si.State;

                        if (userName != null || domainName!=null || clientName != null || state!=null) //In this case, the caller is asking to return only matching sessions
                        {
                            if (userName != null && !String.Equals(userName, sessionInfo.UserName, StringComparison.CurrentCultureIgnoreCase))
                                continue; //Not matching
                            if (clientName != null && !String.Equals(clientName, sessionInfo.ClientName, StringComparison.CurrentCultureIgnoreCase))
                                continue; //Not matching
                            if (domainName != null && !String.Equals(domainName, sessionInfo.DomainName, StringComparison.CurrentCultureIgnoreCase))
                                continue; //Not matching
                            if (state != null && sessionInfo.State != state.Value)
                                continue;
                        }

                        sessions.Add(sessionInfo);
                    }
                    WTSFreeMemory(ppSessionInfo);
                }
            }
            finally
            {
                CloseServer(server);
            }
            return sessions;
        }

        public static SessionInfo GetCurrentSession(string serverName, string userName, string domainName, string clientName)
        {
            List<SessionInfo> sessions = ListSessions(serverName, userName, domainName, clientName, WTS_CONNECTSTATE_CLASS.WTSActive);
            if (sessions.Count == 0)
                return null;
            if (sessions.Count > 1)
                throw new Exception("Duplicate sessions found for user");
            return sessions[0];
        }
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

        private TSManager.WTS_CONNECTSTATE_CLASS state;

        public TSManager.WTS_CONNECTSTATE_CLASS State
        {
            get { return state; }
            set { state = value; }
        }
    }
}
