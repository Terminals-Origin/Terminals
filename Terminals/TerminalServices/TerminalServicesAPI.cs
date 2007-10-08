using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals.TerminalServices
{
    public class TerminalServicesAPI
    {
        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr WTSOpenServer(string pServerName);

        [DllImport("wtsapi32.dll")]
        public static extern void WTSCloseServer(IntPtr hServer);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentProcessId();

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSDisconnectSession(IntPtr hServer, int sessionId, bool bWait);


        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSEnumerateProcesses(
            IntPtr serverHandle, // Handle to a terminal server. 
            Int32 reserved,     // must be 0
            Int32 version,      // must be 1
            ref IntPtr ppProcessInfo, // pointer to array of WTS_PROCESS_INFO
            ref Int32 pCount);       // pointer to number of processes

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSEnumerateSessions(
                System.IntPtr hServer,
                int Reserved,
                int Version,
                ref System.IntPtr ppSessionInfo,
                ref int pCount);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern void WTSEnumerateSessions(
                        System.IntPtr hServer,
                        ref System.IntPtr ppSessionInfo,
                        ref int pCount);
        //{
        //    WTSEnumerateSessions(hServer, 0, 1, ppSessionInfo, pCount);
        //}

        [DllImport("wtsapi32.dll", ExactSpelling = true, SetLastError = false)]
        public static extern void WTSFreeMemory(IntPtr memory);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSLogoffSession(IntPtr hServer, int SessionId, bool bWait);

        /// <summary>
        /// The WTSQuerySessionInformation function retrieves session information for the specified 
        /// session on the specified terminal server. 
        /// It can be used to query session information on local and remote terminal servers.
        /// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/termserv/termserv/wtsquerysessioninformation.asp
        /// </summary>
        /// <param name="hServer">Handle to a terminal server. Specify a handle opened by the WTSOpenServer function, 
        /// or specify <see cref="WTS_CURRENT_SERVER_HANDLE"/> to indicate the terminal server on which your application is running.</param>
        /// <param name="sessionId">A Terminal Services session identifier. To indicate the session in which the calling application is running 
        /// (or the current session) specify <see cref="WTS_CURRENT_SESSION"/>. Only specify <see cref="WTS_CURRENT_SESSION"/> when obtaining session information on the 
        /// local server. If it is specified when querying session information on a remote server, the returned session 
        /// information will be inconsistent. Do not use the returned data in this situation.</param>
        /// <param name="wtsInfoClass">Specifies the type of information to retrieve. This parameter can be one of the values from the <see cref="WTSInfoClass"/> enumeration type. </param>
        /// <param name="ppBuffer">Pointer to a variable that receives a pointer to the requested information. The format and contents of the data depend on the information class specified in the <see cref="WTSInfoClass"/> parameter. 
        /// To free the returned buffer, call the <see cref="WTSFreeMemory"/> function. </param>
        /// <param name="pBytesReturned">Pointer to a variable that receives the size, in bytes, of the data returned in ppBuffer.</param>
        /// <returns>If the function succeeds, the return value is a nonzero value.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("Wtsapi32.dll")]
        public static extern bool WTSQuerySessionInformation(
            System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, out System.IntPtr ppBuffer, out uint pBytesReturned);
        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern bool WTSSendMessage(
                    IntPtr hServer,
                    [MarshalAs(UnmanagedType.I4)] int SessionId,
                    String pTitle,
                    [MarshalAs(UnmanagedType.U4)] int TitleLength,
                    String pMessage,
                    [MarshalAs(UnmanagedType.U4)] int MessageLength,
                    [MarshalAs(UnmanagedType.U4)] int Style,
                    [MarshalAs(UnmanagedType.U4)] int Timeout,
                    [MarshalAs(UnmanagedType.U4)] out int pResponse,
                    bool bWait);


        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSShutdownSystem(IntPtr ServerHandle, long ShutdownFlags);
        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern Int32 WTSVirtualChannelClose(IntPtr hChannelHandle);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr WTSVirtualChannelOpen(IntPtr hServer, Int32 dwSessionID, IntPtr pChannelName);
        [DllImport("kernel32.dll")]
        public static extern bool ProcessIdToSessionId(uint dwProcessId, out uint pSessionId);
    }
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

    public enum WTS_INFO_CLASS
    {
        WTSInitialProgram = 0,
        WTSApplicationName = 1,
        WTSWorkingDirectory = 2,
        WTSOEMId = 3,
        WTSSessionId = 4,
        WTSUserName = 5,
        WTSWinStationName = 6,
        WTSDomainName = 7,
        WTSConnectState = 8,
        WTSClientBuildNumber = 9,
        WTSClientName = 10,
        WTSClientDirectory = 11,
        WTSClientProductId = 12,
        WTSClientHardwareId = 13,
        WTSClientAddress = 14,
        WTSClientDisplay = 15,
        WTSClientProtocolType = 16,
        WTSIdleTime = 17,
        WTSLogonTime = 18,
        WTSIncomingBytes = 19,
        WTSOutgoingBytes = 20,
        WTSIncomingFrames = 21,
        WTSOutgoingFrames = 22
    }
    public struct WTS_PROCESS_INFO
    {
        public int SessionID;
        public int ProcessID;
        //This is a pointer to string...
        public int ProcessName;
        public int UserSid;
    }

}