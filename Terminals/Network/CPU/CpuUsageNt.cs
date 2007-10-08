/*
    Copyright © 2003, The KPD-Team
    All rights reserved.
    http://www.mentalis.org/

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions
  are met:

    - Redistributions of source code must retain the above copyright
       notice, this list of conditions and the following disclaimer. 

    - Neither the name of the KPD-Team, nor the names of its contributors
       may be used to endorse or promote products derived from this
       software without specific prior written permission. 

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
  LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS
  FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL
  THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT,
  INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
  SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION)
  HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT,
  STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
  ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED
  OF THE POSSIBILITY OF SUCH DAMAGE.
*/

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace Org.Mentalis.Utilities {
	/// <summary>
	/// Inherits the CPUUsage class and implements the Query method for Windows NT systems.
	/// </summary>
	/// <remarks>
	/// <p>This class works on Windows NT4, Windows 2000, Windows XP, Windows .NET Server and higher.</p>
	/// <p>You should not use this class directly in your code. Use the CPUUsage.Create() method to instantiate a CPUUsage object.</p>
	/// </remarks>
	internal sealed class CpuUsageNt : CpuUsage {
		/// <summary>
		/// Initializes a new CpuUsageNt instance.
		/// </summary>
		/// <exception cref="NotSupportedException">One of the system calls fails.</exception>
		public CpuUsageNt() {
			byte[] timeInfo = new byte[32];		// SYSTEM_TIME_INFORMATION structure
			byte[] perfInfo = new byte[312];	// SYSTEM_PERFORMANCE_INFORMATION structure
			byte[] baseInfo = new byte[44];		// SYSTEM_BASIC_INFORMATION structure
			int ret;
			// get new system time
			ret = NtQuerySystemInformation(SYSTEM_TIMEINFORMATION, timeInfo, timeInfo.Length, IntPtr.Zero);
			if (ret != NO_ERROR)
				throw new NotSupportedException();
			// get new CPU's idle time
			ret = NtQuerySystemInformation(SYSTEM_PERFORMANCEINFORMATION, perfInfo, perfInfo.Length, IntPtr.Zero);
			if (ret != NO_ERROR)
				throw new NotSupportedException();
			// get number of processors in the system
			ret = NtQuerySystemInformation(SYSTEM_BASICINFORMATION, baseInfo, baseInfo.Length, IntPtr.Zero);
			if (ret != NO_ERROR )
				throw new NotSupportedException();
			// store new CPU's idle and system time and number of processors
			oldIdleTime = BitConverter.ToInt64(perfInfo, 0); // SYSTEM_PERFORMANCE_INFORMATION.liIdleTime
			oldSystemTime = BitConverter.ToInt64(timeInfo, 8); // SYSTEM_TIME_INFORMATION.liKeSystemTime
			processorCount = baseInfo[40];
		}
		/// <summary>
		/// Determines the current average CPU load.
		/// </summary>
		/// <returns>An integer that holds the CPU load percentage.</returns>
		/// <exception cref="NotSupportedException">One of the system calls fails. The CPU time can not be obtained.</exception>
		public override int Query() {
			byte[] timeInfo = new byte[32];		// SYSTEM_TIME_INFORMATION structure
			byte[] perfInfo = new byte[312];	// SYSTEM_PERFORMANCE_INFORMATION structure
			double dbIdleTime, dbSystemTime;
			int ret;
			// get new system time
			ret = NtQuerySystemInformation(SYSTEM_TIMEINFORMATION, timeInfo, timeInfo.Length, IntPtr.Zero);
			if (ret !=  NO_ERROR)
				throw new NotSupportedException();
			// get new CPU's idle time
			ret = NtQuerySystemInformation(SYSTEM_PERFORMANCEINFORMATION, perfInfo, perfInfo.Length, IntPtr.Zero);
			if (ret != NO_ERROR)
				throw new NotSupportedException();
			// CurrentValue = NewValue - OldValue
			dbIdleTime = BitConverter.ToInt64(perfInfo, 0) - oldIdleTime;
			dbSystemTime = BitConverter.ToInt64(timeInfo, 8) - oldSystemTime;
			// CurrentCpuIdle = IdleTime / SystemTime
			if (dbSystemTime != 0)
				dbIdleTime = dbIdleTime / dbSystemTime;
			// CurrentCpuUsage% = 100 - (CurrentCpuIdle * 100) / NumberOfProcessors
			dbIdleTime = 100.0 - dbIdleTime * 100.0 / processorCount + 0.5;
			// store new CPU's idle and system time
			oldIdleTime = BitConverter.ToInt64(perfInfo, 0); // SYSTEM_PERFORMANCE_INFORMATION.liIdleTime
			oldSystemTime = BitConverter.ToInt64(timeInfo, 8); // SYSTEM_TIME_INFORMATION.liKeSystemTime
			return (int)dbIdleTime;
		}
		/// <summary>
		/// NtQuerySystemInformation is an internal Windows function that retrieves various kinds of system information.
		/// </summary>
		/// <param name="dwInfoType">One of the values enumerated in SYSTEM_INFORMATION_CLASS, indicating the kind of system information to be retrieved.</param>
		/// <param name="lpStructure">Points to a buffer where the requested information is to be returned. The size and structure of this information varies depending on the value of the SystemInformationClass parameter.</param>
		/// <param name="dwSize">Length of the buffer pointed to by the SystemInformation parameter.</param>
		/// <param name="returnLength">Optional pointer to a location where the function writes the actual size of the information requested.</param>
		/// <returns>Returns a success NTSTATUS if successful, and an NTSTATUS error code otherwise.</returns>
		[DllImport("ntdll", EntryPoint="NtQuerySystemInformation")]
		private static extern int NtQuerySystemInformation(int dwInfoType, byte[] lpStructure, int dwSize, IntPtr returnLength);
		/// <summary>Returns the number of processors in the system in a SYSTEM_BASIC_INFORMATION structure.</summary>
		private const int SYSTEM_BASICINFORMATION = 0;
		/// <summary>Returns an opaque SYSTEM_PERFORMANCE_INFORMATION structure.</summary>
		private const int SYSTEM_PERFORMANCEINFORMATION = 2;
		/// <summary>Returns an opaque SYSTEM_TIMEOFDAY_INFORMATION structure.</summary>
		private const int SYSTEM_TIMEINFORMATION = 3;
		/// <summary>The value returned by NtQuerySystemInformation is no error occurred.</summary>
		private const int NO_ERROR = 0;
		/// <summary>Holds the old idle time.</summary>
		private long oldIdleTime;
		/// <summary>Holds the old system time.</summary>
		private long oldSystemTime;
		/// <summary>Holds the number of processors in the system.</summary>
		private double processorCount;
	}
}