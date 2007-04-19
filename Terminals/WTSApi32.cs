using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals
{
    public static class WTSApi32
    {
        [DllImport("wtsapi32.dll")]
        static extern IntPtr WTSOpenServer([MarshalAs(UnmanagedType.LPStr)] String pServerName);

        [DllImport("wtsapi32.dll")]
        static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("wtsapi32.dll")]
        static extern bool WTSEnumerateSessions(
            IntPtr hServer,
            [MarshalAs(UnmanagedType.U4)] Int32 Reserved,
            [MarshalAs(UnmanagedType.U4)] Int32 Version,
            ref IntPtr ppSessionInfo,
            [MarshalAs(UnmanagedType.U4)] ref Int32 pCount);

        [DllImport("wtsapi32.dll")]
        static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("Wtsapi32.dll")]
        static extern bool WTSQuerySessionInformation(
            System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);

        [DllImport("Kernel32.dll")]
        static extern int WTSGetActiveConsoleSessionId();

        [StructLayout(LayoutKind.Sequential)]
        struct WTS_SESSION_INFO
        {
            public Int32 SessionID;
            [MarshalAs(UnmanagedType.LPStr)]
            public String pWinStationName;
            public WTS_CONNECTSTATE_CLASS State;
        }

        enum WTS_INFO_CLASS
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

        public enum WTS_CONNECTSTATE_CLASS : int
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

        public const int WTS_CURRENT_SERVER_HANDLE = -1;

        /*public static bool GetIsRunningLocally()
        {
            System.IntPtr buffer = IntPtr.Zero;
            uint bytesReturned;

            int sessionID;

            try
            {
                bool sessionInfo = WTSQuerySessionInformation(System.IntPtr.Zero, WTS_CURRENT_SERVER_HANDLE, WTSInfoClass.WTSSessionId, out buffer, out bytesReturned);
                sessionID = Marshal.ReadInt32(buffer);

            }
            catch
            {
                return true;
            }
            finally
            {
                WTSFreeMemory(buffer);
                buffer = IntPtr.Zero;
            }

            int currentSessionId = WTSGetActiveConsoleSessionId();

            if (currentSessionId == sessionID)
            {
                return true;
            }
            else
            {
                return false;
            }
        }*/

        public static IntPtr OpenServer(String Name)
        {
            IntPtr server = WTSOpenServer(Name);
            return server;
        }

        public static void CloseServer(IntPtr ServerHandle)
        {
            WTSCloseServer(ServerHandle);
        }

        /*public static List<String> ListSessions(String ServerName)
        {
            IntPtr server = IntPtr.Zero;
            List<String> ret = new List<string>();
            try
            {
                server = OpenServer(ServerName);
                IntPtr ppSessionInfo = IntPtr.Zero;
                Int32 count = 0;
                Int32 retval = WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count);
                Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                Int32 current = (int)ppSessionInfo;
                if (retval != 0)
                {
                    for (int i = 0; i < count; i++)
                    {
                        WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));
                        current += dataSize;
                        ret.Add(si.SessionID + " " + si.State + " " + si.pWinStationName);
                    }
                    WTSFreeMemory(ppSessionInfo);
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                CloseServer(server);
            }
            return ret;
        }*/

        private static T QuerySessionInfo<T>(System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass)
        {
            IntPtr ppBuffer = IntPtr.Zero;
            uint pBytesReturned;

            if (WTSQuerySessionInformation(hServer, sessionId, wtsInfoClass, out ppBuffer, out pBytesReturned))
            {
                try
                {
                    T result;
                    if (typeof(T).IsEnum)
                    {
                        Type underlyingType = Enum.GetUnderlyingType(typeof(T));
                        result = (T)Marshal.PtrToStructure(ppBuffer, underlyingType);
                    }
                    else
                        result = (T)Marshal.PtrToStructure(ppBuffer, typeof(T));
                    
                    return result;
                }
                finally
                {
                    WTSFreeMemory(ppBuffer);
                    ppBuffer = IntPtr.Zero;

                }
            }
            return default(T);
        }

        private static string QuerySessionInfo(System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass)
        {
            IntPtr ppBuffer = IntPtr.Zero;
            uint pBytesReturned;

            if (WTSQuerySessionInformation(hServer, sessionId, wtsInfoClass, out ppBuffer, out pBytesReturned))
            {
                try
                {
                    return Marshal.PtrToStringAnsi(ppBuffer);
                }
                finally
                {
                    WTSFreeMemory(ppBuffer);
                    ppBuffer = IntPtr.Zero;

                }
            }
            return String.Empty;
        }

        public static List<SessionInfo> GetServerActiveSessions(string serverName)
        {
            IntPtr server = IntPtr.Zero;
            List<SessionInfo> sessionInfos = new List<SessionInfo>();
            server = OpenServer(serverName);
            if (server != IntPtr.Zero)
            {
                try
                {
                    IntPtr ppSessionInfo = IntPtr.Zero;
                    Int32 count = 0;
                    if (WTSEnumerateSessions(server, 0, 1, ref ppSessionInfo, ref count))
                    {
                        Int32 dataSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                        Int32 current = (int)ppSessionInfo;
                        for (int i = 0; i < count; i++)
                        {
                            SessionInfo sessionInfo = new SessionInfo();
                            WTS_SESSION_INFO si = (WTS_SESSION_INFO)Marshal.PtrToStructure((System.IntPtr)current, typeof(WTS_SESSION_INFO));
                            current += dataSize;
                            sessionInfo.ApplicationName = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSApplicationName);
                            sessionInfo.ClientAddress = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSClientAddress);
                            sessionInfo.ClientBuildNumber = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSClientBuildNumber);
                            sessionInfo.ClientDirectory = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSClientDirectory);
                            sessionInfo.ClientDisplay = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSClientDisplay);
                            sessionInfo.ClientDirectory = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSClientDirectory);
                            sessionInfo.ClientHardwareId = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSClientHardwareId);
                            sessionInfo.ClientName = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSClientName);
                            sessionInfo.ClientProductId = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSClientProductId);
                            sessionInfo.ClientProtocolType = QuerySessionInfo<short>(server, si.SessionID, WTS_INFO_CLASS.WTSClientProtocolType);
                            sessionInfo.ConnectState = QuerySessionInfo<WTS_CONNECTSTATE_CLASS>(server, si.SessionID, WTS_INFO_CLASS.WTSConnectState);
                            sessionInfo.DomainName = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSDomainName);
                            sessionInfo.InitialProgram = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSInitialProgram);
                            sessionInfo.OEMId = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSOEMId);
                            sessionInfo.SessionId = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSSessionId);
                            sessionInfo.UserName = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSUserName);
                            sessionInfo.WinStationName = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSWinStationName);
                            sessionInfo.WorkingDirectory = QuerySessionInfo(server, si.SessionID, WTS_INFO_CLASS.WTSWorkingDirectory);
                        }
                        WTSFreeMemory(ppSessionInfo);
                    }
                }
                finally
                {
                    CloseServer(server);
                }
            }
            return sessionInfos;
        }
    }

    public class SessionInfo
    {
        private string userName;

        public string UserName
        {
            get { return userName; }
            set { userName = value; }
        }

        private string initialProgram;

        public string InitialProgram
        {
            get { return initialProgram; }
            set { initialProgram = value; }
        }

        private string applicationName;

        public string ApplicationName
        {
            get { return applicationName; }
            set { applicationName = value; }
        }

        private string clientAddress;

        public string ClientAddress
        {
            get { return clientAddress; }
            set { clientAddress = value; }
        }

        private string workingDirectory;

        public string WorkingDirectory
        {
            get { return workingDirectory; }
            set { workingDirectory = value; }
        }

        private string oemId;

        public string OEMId
        {
            get { return oemId; }
            set { oemId = value; }
        }

        private string sessionId;

        public string SessionId
        {
            get { return sessionId; }
            set { sessionId = value; }
        }

        private string winStationName;

        public string WinStationName
        {
            get { return winStationName; }
            set { winStationName = value; }
        }

        private string domainName;

        public string DomainName
        {
            get { return domainName; }
            set { domainName = value; }
        }

        private WTSApi32.WTS_CONNECTSTATE_CLASS connectState;

        public WTSApi32.WTS_CONNECTSTATE_CLASS ConnectState
        {
            get { return connectState; }
            set { connectState = value; }
        }

        private string clientBuildNumber;

        public string ClientBuildNumber
        {
            get { return clientBuildNumber; }
            set { clientBuildNumber = value; }
        }

        private string clientName;

        public string ClientName
        {
            get { return clientName; }
            set { clientName = value; }
        }

        private string clientDirectory;

        public string ClientDirectory
        {
            get { return clientDirectory; }
            set { clientDirectory = value; }
        }

        private string clientProductId;

        public string ClientProductId
        {
            get { return clientProductId; }
            set { clientProductId = value; }
        }

        private string clientHardwareId;

        public string ClientHardwareId
        {
            get { return clientHardwareId; }
            set { clientHardwareId = value; }
        }

        private string clientDisplay;

        public string ClientDisplay
        {
            get { return clientDisplay; }
            set { clientDisplay = value; }
        }

        private short clientProtocolType;

        public short ClientProtocolType
        {
            get { return clientProtocolType; }
            set { clientProtocolType = value; }
        }
    }
}

