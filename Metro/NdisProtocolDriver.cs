/******************************************************************************\
* Metro                                                                        *
*                                                                              *
* Copyright (C) 2004 Chris Waddell <IRBMe@icechat.net>		                   *
*                                                                              *
* This program is free software; you can redistribute it and/or modify         *
* it under the terms of the GNU General Public License as published by         *
* the Free Software Foundation; either version 2, or (at your option)          *
* any later version.                                                           *
*                                                                              *
* This program is distributed in the hope that it will be useful,              *
* but WITHOUT ANY WARRANTY; without even the implied warranty of               *
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the                *
* GNU General Public License for more details.                                 *
*                                                                              *
* You should have received a copy of the GNU General Public License            *
* along with this program; if not, write to the Free Software                  *
* Foundation, Inc., 675 Mass Ave, Cambridge, MA 02139, USA.                    *
*                                                                              *
* Please consult the LICENSE.txt file included with this project for           *
* more details                                                                 *
*                                                                              *
\******************************************************************************/

using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using Metro.LinkLayer;

namespace Metro
{
	#region Classes

	/// <summary>
	///		Represents a network interface which can be bound to by the NDIS
	///		protocol driver.
	/// </summary>
	public class NetworkAdapter
	{
		#region Private Fields
			
		/// <summary>
		///		Adapter index.
		/// </summary>
		private int m_index;
		
		/// <summary>
		///		Adapter id.
		/// </summary>
		private string m_adapterID;
		
		/// <summary>
		///		Adapter name.
		/// </summary>
		private string m_adapterName;
		
		/// <summary>
		/// 
		/// </summary>
		private MACAddress m_macAddress;
		
		/// <summary>
		/// 
		/// </summary>
		private NetworkInterface[] m_interfaces;
		
		#endregion
			
		#region Public Fields
			
		/// <summary>
		///		Adapter index.
		/// </summary>
		public int Index
		{
			get
			{
				return m_index;
			}
			set
			{
				m_index = value;
			}
		}
		
		
		/// <summary>
		///		Adapter id.
		/// </summary>
		public string AdapterID
		{
			get
			{
				return m_adapterID;
			}
			set
			{
				m_adapterID = value;
			}
		}
		
		
		/// <summary>
		///		Adapter name.
		/// </summary>
		public string AdapterName
		{
			get
			{
				return m_adapterName;
			}
			set
			{
				m_adapterName = value;
			}
		}
		
		
		/// <summary>
		///		MAC address associated with this device.
		/// </summary>
		public MACAddress MediaAccessControlAddress
		{
			get
			{
				return m_macAddress;
			}
			set
			{
				m_macAddress = value;
			}
		}
		
		
		/// <summary>
		///		Network interface list
		/// </summary>
		public NetworkInterface[] Interfaces
		{
			get
			{
				return m_interfaces;
			}
			set
			{
				m_interfaces = value;
			}
		}
		
		
		#endregion	
	}


	/// <summary>
	///		A class used for interfacing with the NDIS protocol driver included in
	///		the windows DDK.
	/// </summary>
	public class NdisProtocolDriverInterface : IDisposable
	{
		#region Structs
		
		/// <summary>
		///		NDIS protocol query binding structure.
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct NDISPROT_QUERY_BINDING
		{
			/// <summary>
			///		0 based binding number.
			/// </summary>
			public ulong BindingIndex;
			
			/// <summary>
			///		Device name offset from start of the struct.
			/// </summary>
			public ulong DeviceNameOffset;
			
			/// <summary>
			///		Device name length in bytes.
			/// </summary>
			public ulong DeviceNameLength;
			
			/// <summary>
			///		Device description offset from start of the struct.
			/// </summary>
			public ulong DeviceDescrOffset;
			
			/// <summary>
			///  Device description length in bytes.
			/// </summary>
			public ulong DeviceDescrLength;
		}
		

		#endregion
	
		#region Imports
		
		/// <summary>
		///		The CreateFile function creates or opens a file, directory, physical disk, 
		///		volume, console buffer, tape drive, communications resource, mailslot, or 
		///		named pipe. The function returns a handle that can be used to access the 
		///		object.
		/// </summary>
		/// <param name="_lpFileName">
		///		Pointer to a null-terminated string that specifies the name of the object 
		///		to create or open.
		///	</param>
		/// <param name="_dwDesiredAccess">
		///		Access to the object (reading, writing, or both).
		///	</param>
		/// <param name="_dwShareMode">
		///		Sharing mode of the object (reading, writing, both, or neither)
		///	</param>
		/// <param name="_lpSecurityAttributes">
		///		Pointer to a SECURITY_ATTRIBUTES structure that determines whether the 
		///		returned handle can be inherited by child processes. If 
		///		lpSecurityAttributes is NULL, the handle cannot be inherited.
		///	</param>
		/// <param name="_dwCreationDisposition">
		///		Action to take on files that exist, and which action to take when files do 
		///		not exist. For more information about this parameter, see the Remarks 
		///		section.
		///	</param>
		/// <param name="_dwFlagsAndAttributes">
		///		File attributes and flags.
		///	</param>
		/// <param name="_hTemplateFile">
		///		Handle to a template file, with the GENERIC_READ access right. The template
		///		file supplies file attributes and extended attributes for the file being 
		///		created. This parameter can be NULL.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is an open handle to the 
		///		specified file. If the specified file exists before the function call and 
		///		dwCreationDisposition is CREATE_ALWAYS or OPEN_ALWAYS, a call to 
		///		GetLastError returns ERROR_ALREADY_EXISTS (even though the function has 
		///		succeeded). If the file does not exist before the call, GetLastError 
		///		returns zero.
		///
		///		If the function fails, the return value is INVALID_HANDLE_VALUE. To get 
		///		extended error information, call GetLastError.
		///	</returns>
		[DllImport("kernel32", SetLastError=true)]
		private static extern IntPtr CreateFile (
			string	_lpFileName,
			uint	_dwDesiredAccess,
			uint	_dwShareMode,
			uint	_lpSecurityAttributes,
			uint	_dwCreationDisposition,
			uint	_dwFlagsAndAttributes,	
			uint	_hTemplateFile);

		/// <summary>
		///		The WriteFile function writes data to a file at the position specified by the 
		///		file pointer. This function is designed for both synchronous and asynchronous 
		///		operation.
		/// </summary>
		/// <param name="_hFile">
		///		Handle to the file. The file handle must have been created with the GENERIC_WRITE
		///		access right.
		///	</param>
		/// <param name="_lpBuffer">
		///		Pointer to the buffer containing the data to be written to the file.
		///	</param>
		/// <param name="_nNumberOfBytesToWrite">
		///		Number of bytes to be written to the file.
		///	</param>	
		/// <param name="_lpNumberOfBytesWritten">
		///		Pointer to the variable that receives the number of bytes written. 
		///	</param>
		/// <param name="_lpOverlapped">
		///		Pointer to an OVERLAPPED structure. This structure is required if hFile 
		///		was opened with FILE_FLAG_OVERLAPPED.
		/// </param>
		/// <returns>
		///		If the function succeeds, the return value is nonzero.
		///		
		///		If the function fails, the return value is zero. To get extended error 
		///		information, call GetLastError.
		///	</returns>
		[DllImport("kernel32", SetLastError=true)]
		private static extern unsafe bool WriteFile (
			IntPtr	_hFile,
			void*	_lpBuffer,
			uint	_nNumberOfBytesToWrite,
			uint*	_lpNumberOfBytesWritten,
			uint	_lpOverlapped);

		/// <summary>
		///		The ReadFile function reads data from a file, starting at the position 
		///		indicated by the file pointer. This function is designed for both 
		///		synchronous and asynchronous operation.
		/// </summary>
		/// <param name="_hFile">
		///		Handle to the file. The file handle must have been created with the GENERIC_READ
		///		access right.
		///	</param>
		/// <param name="_lpBuffer">
		///		Pointer to the buffer that receives the data read from the file.
		///	</param>
		/// <param name="_nNumberOfBytesToRead">
		///		Number of bytes to be read from the file.
		///	</param>	
		/// <param name="_lpNumberOfBytesRead">
		///		ointer to the variable that receives the number of bytes read.
		///	</param>
		/// <param name="_lpOverlapped">
		///		Pointer to an OVERLAPPED structure. This structure is required if hFile 
		///		was opened with FILE_FLAG_OVERLAPPED.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is nonzero.
		///		
		///		If the function fails, the return value is zero. To get extended error 
		///		information, call GetLastError.
		///	</returns>
		[DllImport("kernel32", SetLastError=true)]
		private static extern unsafe bool ReadFile (
			IntPtr	_hFile,					
			void*	_lpBuffer,				
			uint	_nNumberOfBytesToRead,		
			uint*	_lpNumberOfBytesRead,	
			uint	_lpOverlapped);			

		/// <summary>
		///		The CloseHandle function closes an open object handle.
		/// </summary>
		/// <param name="_hObject">
		///		Handle to an open object.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is nonzero.
		///
		///		If the function fails, the return value is zero. To get extended error 
		///		information, call GetLastError.
		///	</returns>
		[DllImport("kernel32", SetLastError=true)]
		private static extern bool CloseHandle (
			IntPtr	_hObject);

		/// <summary>
		///		The DeviceIoControl function sends a control code directly to a specified 
		///		device driver, causing the corresponding device to perform the 
		///		corresponding operation.
		/// </summary>
		/// <param name="_hDevice">
		///		Handle to the device on which the operation is to be performed. The device 
		///		is typically a volume, directory, file, or stream. To retrieve a device 
		///		handle, use the CreateFile function.
		///	</param>
		/// <param name="_dwIoControlCode">
		///		Control code for the operation. This value identifies the specific 
		///		operation to be performed and the type of device on which to perform it.
		///	</param>
		/// <param name="_lpInBuffer">
		///		Pointer to the input buffer that contains the data required to perform the 
		///		operation. The format of this data depends on the value of the 
		///		dwIoControlCode parameter.
		///	</param>
		/// <param name="_nInBufferSize">
		///		Size of the input buffer, in bytes.
		///	</param>
		/// <param name="lpOutBuffer">
		///		Pointer to the output buffer that is to receive the data returned by the 
		///		operation. The format of this data depends on the value of the 
		///		dwIoControlCode parameter.
		///	</param>
		/// <param name="_nOutBufferSize">
		///		 Size of the output buffer, in bytes.
		///	</param>
		/// <param name="_lpBytesReturned">
		///		Pointer to a variable that receives the size of the data stored in the 
		///		output buffer, in bytes.
		///	</param>	
		/// <param name="_lpOverlapped">
		///		Pointer to an OVERLAPPED structure.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is nonzero.
		///		
		///		If the function fails, the return value is zero. To get extended error 
		///		information, call GetLastError.
		///	</returns>
		[DllImport("kernel32", SetLastError=true)]
		private static extern unsafe bool DeviceIoControl (
			IntPtr	_hDevice,
			uint	_dwIoControlCode,
			void*	_lpInBuffer,
			uint	_nInBufferSize,
			void*	lpOutBuffer,	
			uint	_nOutBufferSize,
			uint*	_lpBytesReturned,
			uint	_lpOverlapped);
		
		/// <summary>
		///		The GetAdaptersInfo function retrieves adapter information for the 
		///		local computer.
		/// </summary>
		/// <param name="pAdapterInfo">
		///		Pointer to a buffer that receives a linked list of IP_ADAPTER_INFO 
		///		structures.
		///	</param>
		/// <param name="pOutBufLen">
		///		Pointer to a ULONG variable that specifies the size of the buffer 
		///		pointed to by the pAdapterInfo parameter. If this size is insufficient 
		///		to hold the adapter information, GetAdaptersInfo fills in this variable
		///		with the required size, and returns an error code of ERROR_BUFFER_OVERFLOW.
		///	</param>
		/// <returns>
		///		If the function succeeds, the return value is ERROR_SUCCESS.
		///	</returns>
		[DllImport("iphlpapi", SetLastError=true)]
		private static extern int GetAdaptersInfo (
			IntPtr pAdapterInfo, 
			ref int pOutBufLen);
		
		#endregion
		
		#region Private Fields
		
		/// <summary>
		///		Used to open the device with read access.
		/// </summary>
		private const uint GENERIC_READ  = 0x80000000;
		
		/// <summary>
		///		Used to open the device with write access.
		/// </summary>
		private const uint GENERIC_WRITE = 0x40000000;
			
		/// <summary>
		///		Open an existing device
		/// </summary>
		private const uint OPEN_EXISTING = 0x00000003;

		/// <summary>
		///		Normal file attributes.
		/// </summary>
		private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;

		/// <summary>
		///		represents an invalid handle.
		/// </summary>
		private const int INVALID_HANDLE_VALUE = -1;

		/// <summary>
		///		The control code used to query interfaces to bind to.
		/// </summary>
		private const uint IOCTL_NDISPROT_QUERY_BINDING = 0x12C80C;
		
		/// <summary>
		///		The control code used to bind to an interface.
		/// </summary>
		private const uint IOCTL_NDISPROT_OPEN_DEVICE = 0x12C800;
		
		/// <summary>
		/// 
		/// </summary>
		private const int MAX_ADAPTER_NAME_LENGTH = 260;
		
		/// <summary>
		/// 
		/// </summary>
		private const int MAX_ADAPTER_ADDRESS_LENGTH = 8;
		
		/// <summary>
		/// 
		/// </summary>
		private const int MAX_ADAPTER_DESCRIPTION_LENGTH = 132;
		
		/// <summary>
		/// 
		/// </summary>
		private const int ERROR_BUFFER_OVERFLOW = 111;
		
		/// <summary>
		/// 
		/// </summary>
		private const int MIB_IF_TYPE_ETHERNET = 6;

		/// <summary>
		///		The device handle.
		/// </summary>
		private IntPtr m_hDevice = IntPtr.Zero;
		
		/// <summary>
		///		Network adapters.
		/// </summary>
		private NetworkAdapter[] m_adapters;
		
		/// <summary>
		///		The adapter which has been bound to.
		/// </summary>
		private NetworkAdapter m_boundAdapter = null;
		
		/// <summary>
		///		Whether or not the class has been disposed.
		/// </summary>
		private bool m_disposed = false;
		
		#endregion
		
		#region Public Fields
		
		/// <summary>
		///		Returns the device handle.
		/// </summary>
		public IntPtr DeviceHandle
		{
			get
			{
				return m_hDevice;
			}
		}
		
		
		/// <summary>
		///		Network adapter list.
		/// </summary>
		public NetworkAdapter[] Adapters
		{
			get
			{	
				return m_adapters;
			}
		}
		

		/// <summary>
		///		Whether or not the driver has been successfuly started.
		/// </summary>
		public bool DriverStarted
		{
			get
			{
				return m_hDevice.ToInt32() > 0;
			}
		}


		/// <summary>
		///		Whether or not the driver has been successfuly bound to an adapter.
		/// </summary>
		public bool DriverBound
		{
			get
			{
				return m_boundAdapter != null;
			}
		}


		/// <summary>
		///		The adapter which has been bound to, if any.
		/// </summary>
		public NetworkAdapter BoundAdapter
		{
			get
			{
				return m_boundAdapter;
			}
		}
		
		
		/// <summary>
		///		Whether or not the class has been disposed.
		/// </summary>
		public bool Disposed
		{
			get
			{
				return m_disposed;
			}
		}


		#endregion
		
		#region Public Methods
		
		/// <summary>
		///		Open the device driver.
		/// </summary>
		/// <param name="deviceName">
		///		The device driver name.
		///	</param>
		/// <exception cref="SystemException">
		///		A system error occured when opening the driver.
		///	</exception>
		public void OpenDevice (string deviceName)
		{
			// attempt to open the device driver
			m_hDevice = CreateFile (deviceName, GENERIC_WRITE | GENERIC_READ, 0, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
			
			// if the handle is invalid then reset it back to 0 and return false
			if ((int)m_hDevice <= 0)
			{
				m_hDevice = IntPtr.Zero;
				throw new SystemException (Win32Error.GetErrorMessage (Marshal.GetLastWin32Error()));
			}
			
			// enumerate the adapters
			m_adapters = EnumerateAdapters ();
			
			// now find the adapter info associated with each device
			
			int bufferSize = 0;
			int ret = 0;
			
			// make an initial call to return the buffer size
			ret = GetAdaptersInfo (IntPtr.Zero, ref bufferSize);
			
			// if an error other than buffer overflow occured then return false
			if (ret != ERROR_BUFFER_OVERFLOW)
			{
				throw new SystemException (Win32Error.GetErrorMessage (ret));
			}
			
			// allocate some memory
			IntPtr pBuffer = Marshal.AllocHGlobal (bufferSize);
				
			// make another call, this time with the correct size
			ret = GetAdaptersInfo (pBuffer, ref bufferSize);

			// if it didn't succeed, return false
			if (ret != 0)
			{
				throw new SystemException (Win32Error.GetErrorMessage (ret));
			}

			do
			{
				// read the adapter id
				string AdapterID = Marshal.PtrToStringAnsi (new IntPtr(pBuffer.ToInt32() + 8));

				// loop through each bindable adapter we know about
				for (int i = 0; i < m_adapters.Length; i++)
				{
					// if this is the adapter we want
					if (m_adapters[i].AdapterID.Substring (m_adapters[i].AdapterID.LastIndexOf ("\\") + 1) == AdapterID)
					{
						// read the address length
						int addressLength = Marshal.ReadByte (new IntPtr(pBuffer.ToInt32() + MAX_ADAPTER_NAME_LENGTH + MAX_ADAPTER_DESCRIPTION_LENGTH + 8));
			
						// read the physical address
						byte[] physicalAddress = new byte[addressLength];
			
						for (int j = 0; j < addressLength; j++)
						{
							physicalAddress[j] = Marshal.ReadByte (new IntPtr(pBuffer.ToInt32() + MAX_ADAPTER_NAME_LENGTH + MAX_ADAPTER_DESCRIPTION_LENGTH + 12 + j));
						}

						m_adapters[i].MediaAccessControlAddress = new MACAddress (physicalAddress);

						int IpCount = 0;
						
						// calculate the pointer to the IP address list
						IntPtr pAddressList = new IntPtr(pBuffer.ToInt32() + MAX_ADAPTER_NAME_LENGTH + MAX_ADAPTER_DESCRIPTION_LENGTH + MAX_ADAPTER_ADDRESS_LENGTH + 28);
						
						IntPtr pIpAddressString = pAddressList;
						
						// follow the pointers until we reach a null pointer, so we know how
						// many there are
						do
						{
							IpCount++;
							pIpAddressString = Marshal.ReadIntPtr (pIpAddressString, 0);
						}
						while (pIpAddressString.ToInt32() > 0);
						
						// now we know how many to allocate
						m_adapters[i].Interfaces = new NetworkInterface[IpCount];
						
						// create an interface list enumeration
						NetworkInterfaceList m_interfaces = new NetworkInterfaceList ();
						
						// start at the beginning of the list again
						pIpAddressString = pAddressList;
						IpCount = 0;
						
						do
						{

							// read the IP
							IPAddress ipAddress = IPAddress.Parse (Marshal.PtrToStringAnsi (new IntPtr(pIpAddressString.ToInt32() + 4)));
							
							// find the interface which this IP belongs to
							for (int j = 0; j < m_interfaces.Interfaces.Length; j++)
							{
								if (m_interfaces.Interfaces[j].Address.Equals (ipAddress))
								{
									m_adapters[i].Interfaces[IpCount] = m_interfaces.Interfaces[j];
									break;
								}
							}
							
							IpCount++;
							
							pIpAddressString = Marshal.ReadIntPtr (pIpAddressString, 0);
						}
						while (pIpAddressString.ToInt32() > 0);
					}
				}
				
				pBuffer = Marshal.ReadIntPtr (pBuffer, 0);
			}
			while (pBuffer.ToInt32() > 0);

			
			// free the allocated memory
			Marshal.FreeHGlobal (pBuffer);
		}


		/// <summary>
		///		Closes the device driver
		/// </summary>
		public void CloseDevice ()
		{
			if (m_hDevice != IntPtr.Zero)
			{
				if (CloseHandle(m_hDevice))
				{
					m_hDevice = IntPtr.Zero;
				}
				else
				{
					throw new SystemException (Win32Error.GetErrorMessage (Marshal.GetLastWin32Error()));
				}
			}
		}
	
		
		/// <summary>
		///		Enumerate network adapters to bind to.
		/// </summary>
		/// <returns>
		///		Returns an array of network adapters.
		///	</returns>
		private NetworkAdapter[] EnumerateAdapters ()
		{
			int adapterIndex = 0;		// the current adapter index
			bool validAdapter = true;	// whether or not we are reading valid adapters

			// temporary array to store adapter info in
			NetworkAdapter[] adapters = new NetworkAdapter[10]; 

			do
			{
				// buffer to hold returned data
				byte[] buf = new byte[1024];
				
				// number of bytes read into the buffer
				uint bytesRead = 0;
				
				// the query binding struct
				NDISPROT_QUERY_BINDING ndisprot = new NDISPROT_QUERY_BINDING();
				
				// size of the query binding struct
				uint bufsize = (uint)Marshal.SizeOf(ndisprot);	
				
				// set the current index to the next adapter
				ndisprot.BindingIndex = (ulong)adapterIndex;
				
				// read the adapter info
				unsafe
				{
					fixed (void* vpBuf = buf)
					{
						validAdapter = DeviceIoControl (m_hDevice, IOCTL_NDISPROT_QUERY_BINDING, (void*)&ndisprot, bufsize, vpBuf, (uint)buf.Length, &bytesRead, 0);
					}
				}

				// if not a valid adapter, break out of the loop
				if (!validAdapter) 
				{
					break;
				}

				string tmpinfo = Encoding.Unicode.GetString(buf).Trim((char)0x00);
				tmpinfo = tmpinfo.Substring(tmpinfo.IndexOf("\\"));
				
				// store the adapter information
				adapters[adapterIndex] = new NetworkAdapter ();
				adapters[adapterIndex].Index		 = adapterIndex;
				adapters[adapterIndex].AdapterID	 = tmpinfo.Substring(0, tmpinfo.IndexOf("}")+1);
				adapters[adapterIndex].AdapterName   = tmpinfo.Substring(tmpinfo.IndexOf("}")+1).Trim((char)0x00);
					
				// increment the adapterIndex count
				adapterIndex++;

			} 
			while (validAdapter && adapterIndex < 10);
				
	
			// copy the temp adapter struct to the return struct
			NetworkAdapter[] returnInfo = new NetworkAdapter[adapterIndex];
			
			for (int i=0; i < returnInfo.Length; i++)
			{
				returnInfo[i] = adapters[i];
			}
			
			// return the adapters
			return returnInfo;
		}
		
		
		/// <summary>
		///		Bind the driver to an adapter for use. Note that an adapter
		///		can only be bound to by one driver at a time.
		/// </summary>
		/// <param name="adapter">
		///		The adapter to bind to.
		///	</param>
		/// <exception cref="SystemException">
		///		If the driver cannot be bound to, a system exception is thrown.
		///	</exception>
		public void BindAdapter (NetworkAdapter adapter)
		{
			// char array to hold the adapterID string
			char[] ndisAdapter = new char[256];
			
			// convert the string to a non-localized unicode char array
			int nameLength = 0;
			int i = 0;

			for (i=0; i < adapter.AdapterID.Length; i++)
			{
				ndisAdapter[i] = adapter.AdapterID[i];
				nameLength++;
			}
			
			// add the null char to terminate the string
			ndisAdapter[i] = '\0';
			
			uint bytesReturned;
			bool ret = false;
			
			// attempt to bind to the adapter
			unsafe 
			{
				fixed (void* vpNdisAdapter = &ndisAdapter[0])
				{
					ret = DeviceIoControl (m_hDevice, IOCTL_NDISPROT_OPEN_DEVICE, vpNdisAdapter, (uint)(nameLength*sizeof(char)), null, 0, &bytesReturned, 0);
				}
			}
			
			if (ret)
			{
				m_boundAdapter = adapter;
			}
			else
			{
				throw new SystemException (Win32Error.GetErrorMessage (Marshal.GetLastWin32Error()));
			}
		}

		
		/// <summary>
		///		Bind the driver to an adapter for use. Note that an adapter
		///		can only be bound to by one driver at a time.
		/// </summary>
		/// <param name="adapterID">
		///		The adapter id to bind to.
		///	</param>
		/// <exception cref="SystemException">
		///		If the driver cannot be bound to, a system exception is thrown.
		///	</exception>
		public void BindAdapter (string adapterID)
		{
			for (int i = 0; i < m_adapters.Length; i++)
			{
				if (m_adapters[i].AdapterID == adapterID)
				{
					BindAdapter (m_adapters[i]);
					return;
				}
			}
			
			throw new SystemException ("Could not find the specified adapter");
		}
		
		
		/// <summary>
		///		Bind the driver to an adapter for use. Note that an adapter
		///		can only be bound to by one driver at a time.
		/// </summary>
		/// <param name="networkInterface">
		///		The network interface to bind to.
		///	</param>
		/// <exception cref="SystemException">
		///		If the driver cannot be bound to, a system exception is thrown.
		///	</exception>
		public void BindAdapter (NetworkInterface networkInterface)
		{
			for (int i = 0; i < m_adapters.Length; i++)
			{
				for (int j = 0; j < m_adapters[i].Interfaces.Length; j++)
				{
					if (m_adapters[i].Interfaces[j].Address.Equals (networkInterface.Address))
					{
						BindAdapter (m_adapters[i].AdapterID);
						return;
					}
				}
			}
			
			throw new SystemException ("Could not find the specified adapter");
		}
		
		
		/// <summary>
		///		Bind the driver to an adapter for use. Note that an adapter
		///		can only be bound to by one driver at a time.
		/// </summary>
		/// <param name="networkInterface">
		///		The network interface to bind to.
		///	</param>
		/// <exception cref="SystemException">
		///		If the driver cannot be bound to, a system exception is thrown.
		///	</exception>
		public void BindAdapter (IPAddress networkInterface)
		{
			for (int i = 0; i < m_adapters.Length; i++)
			{
				for (int j = 0; j < m_adapters[i].Interfaces.Length; j++)
				{
					if (m_adapters[i].Interfaces[j].Address.Equals (networkInterface))
					{
						BindAdapter (m_adapters[i].AdapterID);
						return;
					}
				}
			}
			
			throw new SystemException ("Could not find the specified adapter");
		}
		
		
		/// <summary>
		///		Write a packet to the network interface.
		/// </summary>
		/// <param name="packet">
		///		The packet to write.
		///	</param>
		public void SendPacket (byte[] packet)
		{
			uint bytesSent = 0;
			bool ret = false;

			// attempt to send the packet
			unsafe
			{
				fixed (void* pvPacket = packet)
				{
					ret = WriteFile (m_hDevice, pvPacket, (uint)packet.Length, &bytesSent, 0);
				}
			}
			
			if (!ret)
			{
				throw new SystemException (Win32Error.GetErrorMessage (Marshal.GetLastWin32Error()));
			}
		}
		
		
		/// <summary>
		///		Recieve a packet from the network interface.
		/// </summary>
		/// <returns>
		///		The recieved packet or null if it failed to recieve.
		///	</returns>
		public byte[] RecievePacket ()
		{
			// allocate 10kb for the packet - should be far more than enough
			byte[] buffer = new byte[10240];
		
			uint bytesRead;
			bool ret = false;

			// attempt to recieve the packet
			unsafe
			{
				fixed (void* pvPacket = buffer)
				{
					ret = ReadFile (m_hDevice, pvPacket, (uint)buffer.Length, &bytesRead, 0);
				}
			}
			
			if (!ret)
			{
				throw new SystemException (Win32Error.GetErrorMessage (Marshal.GetLastWin32Error()));
			}
			
			// allocate the correct amount of space
			byte[] correctBuffer = new byte[bytesRead];
			
			// copy the packet in and return it
			Array.Copy (buffer, 0, correctBuffer, 0, bytesRead);
			return correctBuffer;
		}
		
		
		/// <summary>
		///		Dispose.
		/// </summary>
        public void Dispose()
        {
            Dispose (true);
            GC.SuppressFinalize (this);
        }
               
               
		#endregion
		
		#region Private Methods
		
		/// <summary>
		///		Dispose.
		/// </summary>
		/// <param name="disposing">
		///		If disposing equals true, the method has been called directly
        ///		or indirectly by a user's code. Managed and unmanaged resources
        ///		can be disposed.
        ///		If disposing equals false, the method has been called by the 
        ///		runtime from inside the finalizer and you should not reference 
        ///		other objects. Only unmanaged resources can be disposed.
		///	</param>
        private void Dispose (bool disposing)
        {
			// Check to see if Dispose has already been called.
            if (!m_disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
					// clean up managed resources
				}
				
				// clean up unmanaged resources
				try
				{
					CloseDevice ();
				}
				catch
				{
					// do nothing
				}
			}
			
			m_disposed = true;
		}
		
		
		/// <summary>
		///		Destructor.
		/// </summary>
		~NdisProtocolDriverInterface ()
		{
			Dispose (false);
		}
		
		
		#endregion 
	}
	
	
	#endregion
}