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
using Microsoft.Win32;

namespace Org.Mentalis.Utilities {
	/// <summary>
	/// Inherits the CPUUsage class and implements the Query method for Windows 9x systems.
	/// </summary>
	/// <remarks>
	/// <p>This class works on Windows 98 and Windows Millenium Edition.</p>
	/// <p>You should not use this class directly in your code. Use the CPUUsage.Create() method to instantiate a CPUUsage object.</p>
	/// </remarks>
	internal sealed class CpuUsage9x : CpuUsage {
		/// <summary>
		/// Initializes a new CPUUsage9x instance.
		/// </summary>
		/// <exception cref="NotSupportedException">One of the system calls fails.</exception>
		public CpuUsage9x() {
			try {
				// start the counter by reading the value of the 'StartStat' key
				RegistryKey startKey = Registry.DynData.OpenSubKey(@"PerfStats\StartStat", false);
				if (startKey == null)
					throw new NotSupportedException();
				startKey.GetValue(@"KERNEL\CPUUsage");
				startKey.Close();
				// open the counter's value key
				m_StatData = Registry.DynData.OpenSubKey(@"PerfStats\StatData", false);
				if (m_StatData == null)
					throw new NotSupportedException();
			} catch (NotSupportedException e) {
				throw e;
			} catch (Exception e) {
				throw new NotSupportedException("Error while querying the system information.", e);
			}
		}
		/// <summary>
		/// Determines the current average CPU load.
		/// </summary>
		/// <returns>An integer that holds the CPU load percentage.</returns>
		/// <exception cref="NotSupportedException">One of the system calls fails. The CPU time can not be obtained.</exception>
		public override int Query() {
			try {
				return (int)m_StatData.GetValue(@"KERNEL\CPUUsage");
			} catch (Exception e) {
				throw new NotSupportedException("Error while querying the system information.", e);
			}
		}
		/// <summary>
		/// Closes the allocated resources.
		/// </summary>
		~CpuUsage9x() {
			try {
				m_StatData.Close();
			} catch {}
			// stopping the counter
			try {
				RegistryKey stopKey = Registry.DynData.OpenSubKey(@"PerfStats\StopStat", false);
				stopKey.GetValue(@"KERNEL\CPUUsage", false);
				stopKey.Close();
			} catch {}
		}
		/// <summary>Holds the registry key that's used to read the CPU load.</summary>
		private RegistryKey m_StatData;
	}
}
