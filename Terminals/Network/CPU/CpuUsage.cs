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

namespace Org.Mentalis.Utilities {
	/// <summary>
	/// Defines an abstract base class for implementations of CPU usage counters.
	/// </summary>
	public abstract class CpuUsage {
		/// <summary>
		/// Creates and returns a CpuUsage instance that can be used to query the CPU time on this operating system.
		/// </summary>
		/// <returns>An instance of the CpuUsage class.</returns>
		/// <exception cref="NotSupportedException">This platform is not supported -or- initialization of the CPUUsage object failed.</exception>
		public static CpuUsage Create() {
			if (m_CpuUsage == null) {
				if (Environment.OSVersion.Platform == PlatformID.Win32NT)
					m_CpuUsage = new CpuUsageNt();
				else if (Environment.OSVersion.Platform == PlatformID.Win32Windows)
					m_CpuUsage = new CpuUsage9x();
				else
					throw new NotSupportedException();
			}
			return m_CpuUsage;
		}
		/// <summary>
		/// Determines the current average CPU load.
		/// </summary>
		/// <returns>An integer that holds the CPU load percentage.</returns>
		/// <exception cref="NotSupportedException">One of the system calls fails. The CPU time can not be obtained.</exception>
		public abstract int Query();
		/// <summary>
		/// Holds an instance of the CPUUsage class.
		/// </summary>
		private static CpuUsage m_CpuUsage= null;
	}
}