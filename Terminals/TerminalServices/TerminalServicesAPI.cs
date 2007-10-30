using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace Terminals.TerminalServices
{
    public class TerminalServicesAPI
    {

        [DllImport("WtsApi32.dll", EntryPoint = "WTSQuerySessionInformationW", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern bool WTSQuerySessionInformation(System.IntPtr hServer, int sessionId, WTS_INFO_CLASS wtsInfoClass, ref System.IntPtr ppBuffer, ref int pBytesReturned);

        [DllImport("wtsapi32.dll", SetLastError=true)]
            static extern bool WTSEnumerateProcesses(
                IntPtr serverHandle, // Handle to a terminal server. 
                Int32  reserved,     // must be 0
                Int32  version,      // must be 1
                ref IntPtr ppProcessInfo, // pointer to array of WTS_PROCESS_INFO
                ref Int32  pCount);       // pointer to number of processes
            

        [DllImport("WtsApi32.dll", EntryPoint = "WTSQuerySessionInformationW", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool WTSQuerySessionInformation2(System.IntPtr hServer, int SessionId, WTS_INFO_CLASS WTSInfoClass, ref IntPtr ppBuffer, ref Int32 pCount);
        [DllImport("Kernel32.dll", EntryPoint = "GetCurrentProcessId", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern Int32 GetCurrentProcessId();
        [DllImport("Kernel32.dll", EntryPoint = "ProcessIdToSessionId", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern bool ProcessIdToSessionId(Int32 processID, ref Int32 sessionID);
        [DllImport("Kernel32.dll", EntryPoint = "WTSGetActiveConsoleSessionId", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        private static extern Int32 WTSGetActiveConsoleSessionId();

        [DllImport("wtsapi32.dll", SetLastError = true)]
        static extern bool WTSSendMessage(
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


        //Function for TS Client IP Address 

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern bool WTSLogoffSession(IntPtr hServer, int SessionId, bool bWait);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool LookupAccountSid(
            string lpSystemName,
            [MarshalAs(UnmanagedType.LPArray)] byte[] Sid,
            System.Text.StringBuilder lpName,
            ref uint cchName,
            System.Text.StringBuilder ReferencedDomainName,
            ref uint cchReferencedDomainName,
            out SID_NAME_USE peUse);    



        [DllImport("wtsapi32.dll", BestFitMapping = true, CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto, EntryPoint = "WTSEnumerateSessions", SetLastError = true, ThrowOnUnmappableChar = true)]
        private static extern Int32 WTSEnumerateSessions(
            [MarshalAs(UnmanagedType.SysInt)] 
            IntPtr hServer, 
            [MarshalAs(UnmanagedType.U4)] 
            int Reserved,
           [MarshalAs(UnmanagedType.U4)] 
            int Vesrion, 
            [MarshalAs(UnmanagedType.SysInt)] 
            ref IntPtr ppSessionInfo,
           [MarshalAs(UnmanagedType.U4)] 
            ref int pCount);

        [DllImport("wtsapi32.dll")]
        private static extern void WTSFreeMemory(IntPtr pMemory);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr WTSOpenServer(string pServerName);

        [DllImport("wtsapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern void WTSCloseServer(IntPtr hServer);
        [DllImport("wtsapi32.dll", SetLastError = true)]
        public static extern int WTSShutdownSystem(IntPtr ServerHandle, long ShutdownFlags);
        private const long WTS_WSD_REBOOT = 0x00000004;
        private const long WTS_WSD_SHUTDOWN = 0x00000002;

        public static void ShutdownSystem(TerminalServer Server, bool Reboot)
        {
            long action = WTS_WSD_REBOOT;
            if(!Reboot) action = WTS_WSD_SHUTDOWN;
            System.IntPtr server = WTSOpenServer(Server.ServerName);
            if(server!=System.IntPtr.Zero) TerminalServicesAPI.WTSShutdownSystem(server, action);
        }

        public static bool SendMessage(Session Session, string Title, string Message, int Style, int Timeout, bool Wait)
        {
            System.IntPtr server = WTSOpenServer(Session.ServerName);
            if(server != System.IntPtr.Zero)
            {
                int respose = 0;
                return TerminalServicesAPI.WTSSendMessage(server, Session.SessionID, Title, Title.Length, Message, Message.Length, Style, Timeout, out respose, Wait);
            }
            return false;
        }



        public static bool LogOffSession(Session Session, bool Wait) {
            System.IntPtr server = WTSOpenServer(Session.ServerName);
            if(server != System.IntPtr.Zero)
            {
                return TerminalServicesAPI.WTSLogoffSession(server, Session.SessionID, Wait);
            }
            return false;
        }

        public static TerminalServer GetSessions(string ServerName)
        {

            TerminalServer Data = new TerminalServer();
            Data.ServerName = ServerName;


            IntPtr ptrOpenedServer = IntPtr.Zero;
            try
            {
                ptrOpenedServer = WTSOpenServer(ServerName);
                if(ptrOpenedServer == System.IntPtr.Zero)
                {
                    Data.IsATerminalServer = false;
                    return Data;
                }
                Data.ServerPointer = ptrOpenedServer;
                Data.IsATerminalServer = true;

                Int32 FRetVal;
                IntPtr ppSessionInfo = IntPtr.Zero;
                Int32 Count = 0;
                try
                {
                    FRetVal = WTSEnumerateSessions(ptrOpenedServer, 0, 1, ref ppSessionInfo, ref Count);

                    if(FRetVal != 0)
                    {
                        Data.Sessions = new List<Session>();
                        WTS_SESSION_INFO[] sessionInfo = new WTS_SESSION_INFO[Count + 1];
                        int i;
                        System.IntPtr session_ptr;
                        for(i = 0; i <= Count - 1; i++)
                        {
                            session_ptr = new System.IntPtr(ppSessionInfo.ToInt32() + (i * Marshal.SizeOf(sessionInfo[i])));
                            sessionInfo[i] = (WTS_SESSION_INFO)Marshal.PtrToStructure(session_ptr, typeof(WTS_SESSION_INFO));
                            Session s = new Session();
                            s.SessionID = sessionInfo[i].SessionID;
                            s.State = (ConnectionStates)(int)sessionInfo[i].State;
                            s.WindowsStationName = sessionInfo[i].pWinStationName;
                            s.ServerName = ServerName;
                            Data.Sessions.Add(s);                                                      
                        }
                        WTSFreeMemory(ppSessionInfo);
                        strSessionsInfo[] tmpArr = new strSessionsInfo[sessionInfo.GetUpperBound(0) + 1];
                        for(i = 0; i <= tmpArr.GetUpperBound(0); i++)
                        {
                            tmpArr[i].SessionID = sessionInfo[i].SessionID;
                            tmpArr[i].StationName = sessionInfo[i].pWinStationName;
                            tmpArr[i].ConnectionState = GetConnectionState(sessionInfo[i].State);
                            //MessageBox.Show(tmpArr(i).StationName & " " & tmpArr(i).SessionID & " " & tmpArr(i).ConnectionState) 
                        }
                        // ERROR: Not supported in C#: ReDimStatement 
                    }
                }
                catch(Exception ex)
                {
                    Terminals.Logging.Log.Info("", ex);
                    Data.Errors.Add(ex.Message + "\r\n" + System.Runtime.InteropServices.Marshal.GetLastWin32Error());
                }
            }
            catch(Exception ex)
            {
                Terminals.Logging.Log.Info("", ex);
                Data.Errors.Add(ex.Message + "\r\n" + System.Runtime.InteropServices.Marshal.GetLastWin32Error());
            }

            WTS_PROCESS_INFO[] plist = WTSEnumerateProcesses(ptrOpenedServer, Data);

            //Get ProcessID of TS Session that executed this TS Session 
            Int32 active_process = GetCurrentProcessId();
            Int32 active_session = 0;
            bool success1 = ProcessIdToSessionId(active_process, ref active_session);

            foreach(Session s in Data.Sessions)
            {
                if(s.Client == null) s.Client = new Client();
                
                WTS_CLIENT_INFO ClientInfo = LoadClientInfoForSession(Data.ServerPointer, s.SessionID);
                s.Client.Address = ClientInfo.Address;
                s.Client.AddressFamily = ClientInfo.AddressFamily;
                s.Client.ClientName = ClientInfo.WTSClientName;
                s.Client.DomianName = ClientInfo.WTSDomainName;
                s.Client.StationName = ClientInfo.WTSStationName;
                s.Client.Status = ClientInfo.WTSStatus;
                s.Client.UserName = ClientInfo.WTSUserName;
                s.IsTheActiveSession=false;
                if(s.SessionID == active_session) s.IsTheActiveSession = true;
            }



            WTSCloseServer(ptrOpenedServer);
            return Data;
        }


        public static WTS_PROCESS_INFO[] WTSEnumerateProcesses(IntPtr WTS_CURRENT_SERVER_HANDLE, TerminalServer Data)
        {
            IntPtr pProcessInfo = IntPtr.Zero;
            int processCount = 0;

            if(!WTSEnumerateProcesses(WTS_CURRENT_SERVER_HANDLE, 0, 1, ref pProcessInfo, ref processCount))
                return null;

            const int NO_ERROR = 0;
            const int ERROR_INSUFFICIENT_BUFFER = 122;
            int err = NO_ERROR;
            IntPtr pMemory = pProcessInfo;
            WTS_PROCESS_INFO[] processInfos = new WTS_PROCESS_INFO[processCount];
            for(int i = 0; i < processCount; i++)
            {
                processInfos[i] = (WTS_PROCESS_INFO)Marshal.PtrToStructure(pProcessInfo, typeof(WTS_PROCESS_INFO));
                pProcessInfo = (IntPtr)((int)pProcessInfo + Marshal.SizeOf(processInfos[i]));

                SessionProcess p = new SessionProcess();
                p.ProcessID = processInfos[i].ProcessID;
                p.ProcessName = Marshal.PtrToStringAnsi(processInfos[i].ProcessName);
                
                if(processInfos[i].UserSid != IntPtr.Zero)
                {
                    byte[] Sid = new byte[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0};
                    Marshal.Copy(processInfos[i].UserSid, Sid, 0, 14);
                    System.Text.StringBuilder name = new StringBuilder();
                    uint cchName = (uint)name.Capacity;
                    SID_NAME_USE sidUse;
                    StringBuilder referencedDomainName = new StringBuilder();
                    uint cchReferencedDomainName = (uint)referencedDomainName.Capacity;
                    if(LookupAccountSid(Data.ServerName, Sid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                    {
                        err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        if(err == ERROR_INSUFFICIENT_BUFFER)
                        {
                            name.EnsureCapacity((int)cchName);
                            referencedDomainName.EnsureCapacity((int)cchReferencedDomainName);
                            err = NO_ERROR;
                            if(!LookupAccountSid(null, Sid, name, ref cchName, referencedDomainName, ref cchReferencedDomainName, out sidUse))
                                err = System.Runtime.InteropServices.Marshal.GetLastWin32Error();
                        }


                        p.UserType = sidUse.ToString();
                        p.User = name.ToString();
                    }
                }
                //string userSID = Marshal.PtrToStringAuto(processInfos[i].UserSid);
                p.SessionID = processInfos[i].SessionID;
                
                //LookupAccountSid(Data.ServerName, 
                //p.User = Marshal.PtrToStringAnsi(processInfos[i].UserSid);
                foreach(Session s in Data.Sessions)
                {
                    if(s.SessionID == p.SessionID)
                    {
                        if(s.Processes == null) s.Processes = new List<SessionProcess>();
                        s.Processes.Add(p);
                        break;
                    }
                }
            }

            

            WTSFreeMemory(pMemory);
            return processInfos;
        }

        private static WTS_CLIENT_INFO LoadClientInfoForSession(IntPtr ptrOpenedServer, int active_session)
        {

            int returned = 0;
            IntPtr str = IntPtr.Zero;

            WTS_CLIENT_INFO ClientInfo = new WTS_CLIENT_INFO();
            ClientInfo.WTSStationName = "";
            ClientInfo.WTSClientName = "";
            ClientInfo.Address = new byte[6];
            ClientInfo.Address[2] = 0;
            ClientInfo.Address[3] = 0;
            ClientInfo.Address[4] = 0;
            ClientInfo.Address[5] = 0;

            ClientInfo.WTSClientName = GetString(ptrOpenedServer, active_session, WTS_INFO_CLASS.WTSClientName);
            ClientInfo.WTSStationName = GetString(ptrOpenedServer, active_session, WTS_INFO_CLASS.WTSWinStationName);
            ClientInfo.WTSDomainName = GetString(ptrOpenedServer, active_session, WTS_INFO_CLASS.WTSDomainName);

            //Get client IP address 
            IntPtr addr = System.IntPtr.Zero;
            if(WTSQuerySessionInformation2(ptrOpenedServer, active_session, WTS_INFO_CLASS.WTSClientAddress, ref addr, ref returned) == true)
            {
                _WTS_CLIENT_ADDRESS obj = new _WTS_CLIENT_ADDRESS();
                obj = (_WTS_CLIENT_ADDRESS)Marshal.PtrToStructure(addr, obj.GetType());
                ClientInfo.Address[2] = obj.Address[2];
                ClientInfo.Address[3] = obj.Address[3];
                ClientInfo.Address[4] = obj.Address[4];
                ClientInfo.Address[5] = obj.Address[5];
            }
            return ClientInfo;
        }
        private static string GetString(System.IntPtr ptrOpenedServer, int active_session, WTS_INFO_CLASS whichOne)
        {
            System.IntPtr str = System.IntPtr.Zero;
            int returned = 0;
            if(WTSQuerySessionInformation(ptrOpenedServer, active_session, whichOne, ref str, ref returned) == true)
            {
                return Marshal.PtrToStringAuto(str);
            }
            return "";
        }
        private static string GetConnectionState(WTS_CONNECTSTATE_CLASS State)
        {
            string RetVal;
            switch(State)
            {
                case WTS_CONNECTSTATE_CLASS.WTSActive:
                    RetVal = "Active";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSConnected:
                    RetVal = "Connected";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSConnectQuery:
                    RetVal = "Query";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSDisconnected:
                    RetVal = "Disconnected";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSDown:
                    RetVal = "Down";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSIdle:
                    RetVal = "Idle";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSInit:
                    RetVal = "Initializing.";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSListen:
                    RetVal = "Listen";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSReset:
                    RetVal = "reset";
                    break;
                case WTS_CONNECTSTATE_CLASS.WTSShadow:
                    RetVal = "Shadowing";
                    break;
                default:
                    RetVal = "Unknown connect state";
                    break;
            }
            return RetVal;
        }
        public struct WTS_PROCESS_INFO
        {
            public int SessionID;
            public int ProcessID;
            //This is a pointer to string...
            public IntPtr ProcessName;
            public IntPtr UserSid;
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public struct WTS_SESSION_INFO
        {
            //DWORD integer 
            public Int32 SessionID;
            // integer LPTSTR - Pointer to a null-terminated string containing the name of the WinStation for this session 
            public string pWinStationName;
            public WTS_CONNECTSTATE_CLASS State;
        }

        internal struct strSessionsInfo
        {
            public int SessionID;
            public string StationName;
            public string ConnectionState;
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
            WTSClientProtocolType,
            WTSIdleTime,
            WTSLogonTime,
            WTSIncomingBytes,
            WTSOutgoingBytes,
            WTSIncomingFrames,
            WTSOutgoingFrames
        }
        //Structure for TS Client IP Address 
        [StructLayout(LayoutKind.Sequential)]
        public struct _WTS_CLIENT_ADDRESS
        {
            public int AddressFamily;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 20)]
            public byte[] Address;
        }
        //Structure for TS Client Information 
        public struct WTS_CLIENT_INFO
        {
            [MarshalAs(UnmanagedType.Bool)] 
            public bool WTSStatus;
            [MarshalAs(UnmanagedType.LPWStr)] 
            public string WTSUserName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string WTSStationName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string WTSDomainName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public string WTSClientName;
            [MarshalAs(UnmanagedType.LPWStr)]
            public int AddressFamily;
            [MarshalAs(UnmanagedType.ByValArray)]
            public byte[] Address;
        }
       public enum SID_NAME_USE
        {
            User = 1,
            Group,
            Domain,
            Alias,
            WellKnownGroup,
            DeletedAccount,
            Invalid,
            Unknown,
            Computer
        }
    }
}