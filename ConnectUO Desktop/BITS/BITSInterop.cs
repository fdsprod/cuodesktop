//============================================================================================================
// Microsoft Updater Application Block for .NET
//  http://msdn.microsoft.com/library/en-us/dnbda/html/updater.asp
//	
// BITSInterop.cs
//
// Interop definitions for BITS COM interop.
// 
// For more information see the Updater Application Block Implementation Overview. 
// 
//============================================================================================================
// Copyright (C) 2000-2001 Microsoft Corporation
// All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR
// FITNESS FOR A PARTICULAR PURPOSE.
//============================================================================================================
//
// Added definitions to support IBackgroundCopyJob2::SetCredentials()
//
// 25/7/04 Eddie Tse (eddietse@hotmail.com)
//
//============================================================================================================

using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;


namespace BITS
{
	// COM Interop C# classes for accessing BITS API.
	// Refer to MSDN for Details: 
	// http://msdn.microsoft.com/library/en-us/bits/bits/bits_reference.asp
	// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/bits/bits/service_accounts_and_bits.asp 
	// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/bits/bits/enumerating_jobs_in_the_transfer_queue.asp
	// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/bits/bits/handling_errors.asp?frame=true
	// http://msdn.microsoft.com/library/default.asp?url=/library/en-us/dnwxp/html/WinXP_BITS.asp 
	// http://msdn.microsoft.com/msdnmag/issues/03/02/BITS/default.aspx 

	/// <summary>
	/// BackgroundCopyManager Class
	/// </summary>
	[GuidAttribute("4991D34B-80A1-4291-83B6-3328366B9097")]
	[ClassInterfaceAttribute(ClassInterfaceType.None)]
	[ComImportAttribute()]
	[SecurityPermission(SecurityAction.Demand, UnmanagedCode = true)]
	internal class BackgroundCopyManager
	{
	}

	/// <summary>
	/// Use the IBackgroundCopyManager interface to create transfer jobs, 
	/// retrieve an enumerator object that contains the jobs in the queue, 
	/// and to retrieve individual jobs from the queue.
	/// </summary>
	[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	[GuidAttribute("5CE34C0D-0DC9-4C1F-897C-DAA1B78CEE7C")]
	[ComImportAttribute()]
	internal interface IBackgroundCopyManager 
	{
		
		/// <summary>
		/// Creates a new transfer job
		/// </summary>
		void CreateJob([MarshalAs(UnmanagedType.LPWStr)] string DisplayName, BG_JOB_TYPE Type, out Guid pJobId, [MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyJob ppJob);
		
		/// <summary>
		/// Retrieves a given job from the queue
		/// </summary>
		void GetJob(ref Guid jobID, [MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyJob ppJob);
		
		/// <summary>
		/// Retrieves an enumerator object that you use to enumerate jobs in the queue
		/// </summary>
		void EnumJobs(uint dwFlags, [MarshalAs(UnmanagedType.Interface)] out IEnumBackgroundCopyJobs ppenum);
		
		/// <summary>
		/// Retrieves a description for the given error code
		/// </summary>
		void GetErrorDescription([MarshalAs(UnmanagedType.Error)] int hResult, uint LanguageId, [MarshalAs(UnmanagedType.LPWStr)] out string pErrorDescription);
	}

 
	/// <summary>
	/// Use the IBackgroundCopyJob interface to add files to the job, 
	/// set the priority level of the job, determine the state of the
	/// job, and to start and stop the job.
	/// </summary>
	[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	[GuidAttribute("37668D37-507E-4160-9316-26306D150B12")]
	[ComImport]
	internal interface IBackgroundCopyJob 
	{
		/// <summary>
		/// Adds multiple files to the job
		/// </summary>
		void AddFileSet(uint cFileCount, ref _BG_FILE_INFO pFileSet);

		/// <summary>
		/// Adds a single file to the job
		/// </summary>
		void AddFile([MarshalAs(UnmanagedType.LPWStr)] string RemoteUrl, [MarshalAs(UnmanagedType.LPWStr)] string LocalName);

		
		/// <summary>
		/// Returns an interface pointer to an enumerator
		/// object that you use to enumerate the files in the job
		/// </summary>
		void EnumFiles([MarshalAs(UnmanagedType.Interface)] out IEnumBackgroundCopyFiles pEnum);

		
		/// <summary>
		/// Pauses the job
		/// </summary>
		void Suspend();

		
		/// <summary>
		/// Restarts a suspended job
		/// </summary>
		void Resume();

		
		/// <summary>
		/// Cancels the job and removes temporary files from the client
		/// </summary>
		void Cancel();
		
		/// <summary>
		/// Ends the job and saves the transferred files on the client
		/// </summary>
		void Complete();
		
		/// <summary>
		/// Retrieves the identifier of the job in the queue
		/// </summary>
		void GetId(out Guid pVal);
		
		/// <summary>
		/// Retrieves the type of transfer being performed, 
		/// such as a file download
		/// </summary>
		void GetType(out BG_JOB_TYPE pVal);
		
		/// <summary>
		/// Retrieves job-related progress information, 
		/// such as the number of bytes and files transferred 
		/// to the client
		/// </summary>
		void GetProgress(out _BG_JOB_PROGRESS pVal);
		
		/// <summary>
		/// Retrieves timestamps for activities related
		/// to the job, such as the time the job was created
		/// </summary>
		void GetTimes(out _BG_JOB_TIMES pVal);
		
		/// <summary>
		/// Retrieves the state of the job
		/// </summary>
		void GetState(out BG_JOB_STATE pVal);
		
		/// <summary>
		/// Retrieves an interface pointer to 
		/// the error object after an error occurs
		/// </summary>
		void GetError([MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyError ppError);
		
		/// <summary>
		/// Retrieves the job owner's identity
		/// </summary>
		void GetOwner([MarshalAs(UnmanagedType.LPWStr)] out string pVal);
		
		/// <summary>
		/// Specifies a display name that identifies the job in 
		/// a user interface
		/// </summary>
		void SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string Val);
		
		/// <summary>
		/// Retrieves the display name that identifies the job
		/// </summary>
		void GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pVal);
		
		/// <summary>
		/// Specifies a description of the job
		/// </summary>
		void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string Val);
		
		/// <summary>
		/// Retrieves the description of the job
		/// </summary>
		void GetDescription([MarshalAs(UnmanagedType.LPWStr)] out string pVal);
		
		/// <summary>
		/// Specifies the priority of the job relative to 
		/// other jobs in the transfer queue
		/// </summary>
		void SetPriority(BG_JOB_PRIORITY Val);

		/// <summary>
		/// Retrieves the priority level you have set for the job.
		/// </summary>
		void GetPriority(out BG_JOB_PRIORITY pVal);
		
		/// <summary>
		/// Specifies the type of event notification to receive
		/// </summary>
		void SetNotifyFlags([MarshalAs(UnmanagedType.U4)] BG_JOB_NOTIFICATION_TYPE Val);
		
		/// <summary>
		/// Retrieves the event notification (callback) flags 
		/// you have set for your application.
		/// </summary>
		void GetNotifyFlags(out uint pVal);
		
		/// <summary>
		/// Specifies a pointer to your implementation of the 
		/// IBackgroundCopyCallback interface (callbacks). The 
		/// interface receives notification based on the event 
		/// notification flags you set
		/// </summary>
		void SetNotifyInterface([MarshalAs(UnmanagedType.IUnknown)] object Val);
		
		/// <summary>
		/// Retrieves a pointer to your implementation 
		/// of the IBackgroundCopyCallback interface (callbacks).
		/// </summary>
		void GetNotifyInterface([MarshalAs(UnmanagedType.IUnknown)] out object pVal);
		
		/// <summary>
		/// Specifies the minimum length of time that BITS waits after 
		/// encountering a transient error condition before trying to 
		/// transfer the file
		/// </summary>
		void SetMinimumRetryDelay(uint Seconds);
		
		/// <summary>
		/// Retrieves the minimum length of time that BITS waits after 
		/// encountering a transient error condition before trying to 
		/// transfer the file
		/// </summary>
		void GetMinimumRetryDelay(out uint Seconds);
		
		/// <summary>
		/// Specifies the length of time that BITS continues to try to 
		/// transfer the file after encountering a transient error 
		/// condition
		/// </summary>
		void SetNoProgressTimeout(uint Seconds);
		
		/// <summary>
		/// Retrieves the length of time that BITS continues to try to 
		/// transfer the file after encountering a transient error condition
		/// </summary>
		void GetNoProgressTimeout(out uint Seconds);
		
		/// <summary>
		/// Retrieves the number of times the job was interrupted by 
		/// network failure or server unavailability
		/// </summary>
		void GetErrorCount(out uint Errors);
		
		/// <summary>
		/// Specifies which proxy to use to transfer the files
		/// </summary>
		void SetProxySettings(BG_JOB_PROXY_USAGE ProxyUsage, [MarshalAs(UnmanagedType.LPWStr)] string ProxyList, [MarshalAs(UnmanagedType.LPWStr)] string ProxyBypassList);
		
		/// <summary>
		/// Retrieves the proxy settings the job uses to transfer the files
		/// </summary>
		void GetProxySettings(out BG_JOB_PROXY_USAGE pProxyUsage, [MarshalAs(UnmanagedType.LPWStr)] out string pProxyList, [MarshalAs(UnmanagedType.LPWStr)] out string pProxyBypassList);
		
		/// <summary>
		/// Changes the ownership of the job to the current user
		/// </summary>
		void TakeOwnership();
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("54B50739-686F-45EB-9DFF-D6A9A0FAA9AF")]
	internal interface IBackgroundCopyJob2 : IBackgroundCopyJob
	{
		/// <summary>
		/// Adds multiple files to the job
		/// </summary>
		new void AddFileSet(uint cFileCount, ref _BG_FILE_INFO pFileSet);

		/// <summary>
		/// Adds a single file to the job
		/// </summary>
		new void AddFile([MarshalAs(UnmanagedType.LPWStr)] string RemoteUrl, [MarshalAs(UnmanagedType.LPWStr)] string LocalName);
		
		/// <summary>
		/// Returns an interface pointer to an enumerator
		/// object that you use to enumerate the files in the job
		/// </summary>
		new void EnumFiles([MarshalAs(UnmanagedType.Interface)] out IEnumBackgroundCopyFiles pEnum);
		
		/// <summary>
		/// Pauses the job
		/// </summary>
		new void Suspend();
		
		/// <summary>
		/// Restarts a suspended job
		/// </summary>
		new void Resume();
		
		/// <summary>
		/// Cancels the job and removes temporary files from the client
		/// </summary>
		new void Cancel();
		
		/// <summary>
		/// Ends the job and saves the transferred files on the client
		/// </summary>
		new void Complete();
		
		/// <summary>
		/// Retrieves the identifier of the job in the queue
		/// </summary>
		new void GetId(out Guid pVal);
		
		/// <summary>
		/// Retrieves the type of transfer being performed, 
		/// such as a file download
		/// </summary>
		new void GetType(out BG_JOB_TYPE pVal);
		
		/// <summary>
		/// Retrieves job-related progress information, 
		/// such as the number of bytes and files transferred 
		/// to the client
		/// </summary>
		new void GetProgress(out _BG_JOB_PROGRESS pVal);
		
		/// <summary>
		/// Retrieves timestamps for activities related
		/// to the job, such as the time the job was created
		/// </summary>
		new void GetTimes(out _BG_JOB_TIMES pVal);
		
		/// <summary>
		/// Retrieves the state of the job
		/// </summary>
		new void GetState(out BG_JOB_STATE pVal);
		
		/// <summary>
		/// Retrieves an interface pointer to 
		/// the error object after an error occurs
		/// </summary>
		new void GetError([MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyError ppError);
		
		/// <summary>
		/// Retrieves the job owner's identity
		/// </summary>
		new void GetOwner([MarshalAs(UnmanagedType.LPWStr)] out string pVal);
		
		/// <summary>
		/// Specifies a display name that identifies the job in 
		/// a user interface
		/// </summary>
		new void SetDisplayName([MarshalAs(UnmanagedType.LPWStr)] string Val);
		
		/// <summary>
		/// Retrieves the display name that identifies the job
		/// </summary>
		new void GetDisplayName([MarshalAs(UnmanagedType.LPWStr)] out string pVal);
		
		/// <summary>
		/// Specifies a description of the job
		/// </summary>
		new void SetDescription([MarshalAs(UnmanagedType.LPWStr)] string Val);
		
		/// <summary>
		/// Retrieves the description of the job
		/// </summary>
		new void GetDescription([MarshalAs(UnmanagedType.LPWStr)] out string pVal);
		
		/// <summary>
		/// Specifies the priority of the job relative to 
		/// other jobs in the transfer queue
		/// </summary>
		new void SetPriority(BG_JOB_PRIORITY Val);

		/// <summary>
		/// Retrieves the priority level you have set for the job.
		/// </summary>
		new void GetPriority(out BG_JOB_PRIORITY pVal);
		
		/// <summary>
		/// Specifies the type of event notification to receive
		/// </summary>
		new void SetNotifyFlags([MarshalAs(UnmanagedType.U4)] BG_JOB_NOTIFICATION_TYPE Val);
		
		/// <summary>
		/// Retrieves the event notification (callback) flags 
		/// you have set for your application.
		/// </summary>
		new void GetNotifyFlags(out uint pVal);
		
		/// <summary>
		/// Specifies a pointer to your implementation of the 
		/// IBackgroundCopyCallback interface (callbacks). The 
		/// interface receives notification based on the event 
		/// notification flags you set
		/// </summary>
		new void SetNotifyInterface([MarshalAs(UnmanagedType.IUnknown)] object Val);
		
		/// <summary>
		/// Retrieves a pointer to your implementation 
		/// of the IBackgroundCopyCallback interface (callbacks).
		/// </summary>
		new void GetNotifyInterface([MarshalAs(UnmanagedType.IUnknown)] out object pVal);
		
		/// <summary>
		/// Specifies the minimum length of time that BITS waits after 
		/// encountering a transient error condition before trying to 
		/// transfer the file
		/// </summary>
		new void SetMinimumRetryDelay(uint Seconds);
		
		/// <summary>
		/// Retrieves the minimum length of time that BITS waits after 
		/// encountering a transient error condition before trying to 
		/// transfer the file
		/// </summary>
		new void GetMinimumRetryDelay(out uint Seconds);
		
		/// <summary>
		/// Specifies the length of time that BITS continues to try to 
		/// transfer the file after encountering a transient error 
		/// condition
		/// </summary>
		new void SetNoProgressTimeout(uint Seconds);
		
		/// <summary>
		/// Retrieves the length of time that BITS continues to try to 
		/// transfer the file after encountering a transient error condition
		/// </summary>
		new void GetNoProgressTimeout(out uint Seconds);
		
		/// <summary>
		/// Retrieves the number of times the job was interrupted by 
		/// network failure or server unavailability
		/// </summary>
		new void GetErrorCount(out uint Errors);
		
		/// <summary>
		/// Specifies which proxy to use to transfer the files
		/// </summary>
		new void SetProxySettings(BG_JOB_PROXY_USAGE ProxyUsage, [MarshalAs(UnmanagedType.LPWStr)] string ProxyList, [MarshalAs(UnmanagedType.LPWStr)] string ProxyBypassList);
		
		/// <summary>
		/// Retrieves the proxy settings the job uses to transfer the files
		/// </summary>
		new void GetProxySettings(out BG_JOB_PROXY_USAGE pProxyUsage, [MarshalAs(UnmanagedType.LPWStr)] out string pProxyList, [MarshalAs(UnmanagedType.LPWStr)] out string pProxyBypassList);
		
		/// <summary>
		/// Changes the ownership of the job to the current user
		/// </summary>
		new void TakeOwnership();


		///
		/// Starts definition of IBackgroundCopyJob2
		///
		void SetNotifyCmdLine([In, MarshalAs(UnmanagedType.LPWStr)] string Program, [In, MarshalAs(UnmanagedType.LPWStr)] string Parameters);

		void GetNotifyCmdLine([MarshalAs(UnmanagedType.LPWStr)] out string pProgram, [MarshalAs(UnmanagedType.LPWStr)] out string pParameters);

		void GetReplyProgress([Out] out _BG_JOB_REPLY_PROGRESS pProgress);

		void GetReplyData([In, Out] IntPtr ppBuffer, out ulong pLength);

		void SetReplyFileName([In, MarshalAs(UnmanagedType.LPWStr)] string ReplyFileName);

		void GetReplyFileName([MarshalAs(UnmanagedType.LPWStr)] out string pReplyFileName);

		void SetCredentials([In] ref BG_AUTH_CREDENTIALS Credentials);

		void RemoveCredentials(BG_AUTH_TARGET Target, BG_AUTH_SCHEME Scheme);
	}

	/// <summary>
	/// Use the information in the IBackgroundCopyError interface to 
	/// determine the cause of the error and if the transfer process 
	/// can proceed
	/// </summary>
	[GuidAttribute("19C613A0-FCB8-4F28-81AE-897C3D078F81")]
	[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImportAttribute()]
	internal  interface IBackgroundCopyError 
	{
		/// <summary>
		/// Retrieves the error code and identify the context 
		/// in which the error occurred
		/// </summary>
		void GetError(out BG_ERROR_CONTEXT pContext, [MarshalAs(UnmanagedType.Error)] out int pCode);

		/// <summary>
		/// Retrieves an interface pointer to the file object 
		/// associated with the error
		/// </summary>
		void GetFile([MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyFile pVal);

		/// <summary>
		/// Retrieves the error text associated with the error
		/// </summary>
		void GetErrorDescription(uint LanguageId, [MarshalAs(UnmanagedType.LPWStr)] out string pErrorDescription);

		/// <summary>
		/// Retrieves a description of the context in which the error occurred
		/// </summary>
		void GetErrorContextDescription(uint LanguageId, [MarshalAs(UnmanagedType.LPWStr)] out string pContextDescription);

		/// <summary>
		/// Retrieves the protocol used to transfer the file
		/// </summary>
		void GetProtocol([MarshalAs(UnmanagedType.LPWStr)] out string pProtocol);
	}

	/// <summary>
	/// Use the IEnumBackgroundCopyJobs interface to enumerate the list 
	/// of jobs in the transfer queue. To get an IEnumBackgroundCopyJobs 
	/// interface pointer, call the IBackgroundCopyManager::EnumJobs method
	/// </summary>
	[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	[GuidAttribute("1AF4F612-3B71-466F-8F58-7B6F73AC57AD")]
	[ComImportAttribute()]
	internal  interface IEnumBackgroundCopyJobs 
	{
		/// <summary>
		/// Retrieves a specified number of items in the enumeration sequence
		/// </summary>
		void Next(uint celt, [MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyJob rgelt, out uint pceltFetched);

		/// <summary>
		/// Skips a specified number of items in the enumeration sequence
		/// </summary>
		void Skip(uint celt);

		/// <summary>
		/// Resets the enumeration sequence to the beginning.
		/// </summary>
		void Reset();

		/// <summary>
		/// Creates another enumerator that contains the same 
		/// enumeration state as the current one
		/// </summary>
		void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumBackgroundCopyJobs ppenum);

		/// <summary>
		/// Returns the number of items in the enumeration
		/// </summary>
		void GetCount(out uint puCount);
	}

	/// <summary>
	/// Use the IEnumBackgroundCopyFiles interface to enumerate the files 
	/// that a job contains. To get an IEnumBackgroundCopyFiles interface 
	/// pointer, call the IBackgroundCopyJob::EnumFiles method
	/// </summary>
	[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	[GuidAttribute("CA51E165-C365-424C-8D41-24AAA4FF3C40")]
	[ComImportAttribute()]
	internal  interface IEnumBackgroundCopyFiles 
	{
		/// <summary>
		/// Retrieves a specified number of items in the enumeration sequence
		/// </summary>
		void Next(uint celt, [MarshalAs(UnmanagedType.Interface)] out IBackgroundCopyFile rgelt, out uint pceltFetched);
		
		/// <summary>
		/// Skips a specified number of items in the enumeration sequence
		/// </summary>
		void Skip(uint celt);

		/// <summary>
		/// Resets the enumeration sequence to the beginning
		/// </summary>
		void Reset();

		/// <summary>
		/// Creates another enumerator that contains the same 
		/// enumeration state as the current enumerator
		/// </summary>
		void Clone([MarshalAs(UnmanagedType.Interface)] out IEnumBackgroundCopyFiles ppenum);

		/// <summary>
		/// Retrieves the number of items in the enumeration
		/// </summary>
		void GetCount(out uint puCount);
	}

	/// <summary>
	///  The IBackgroundCopyFile interface contains information about a file 
	///  that is part of a job. For example, you can use the interfaces methods
	///  to retrieve the local and remote names of the file and transfer progress
	///  information
	/// </summary>
	[GuidAttribute("01B7BD23-FB88-4A77-8490-5891D3E4653A")]
	[InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImportAttribute()]
	internal  interface IBackgroundCopyFile 
	{
		/// <summary>
		/// Retrieves the remote name of the file
		/// </summary>
		void GetRemoteName([MarshalAs(UnmanagedType.LPWStr)] out string pVal);

		/// <summary>
		/// Retrieves the local name of the file
		/// </summary>
		void GetLocalName([MarshalAs(UnmanagedType.LPWStr)] out string pVal);
		
		/// <summary>
		/// Retrieves the progress of the file transfer
		/// </summary>
		void GetProgress(out _BG_FILE_PROGRESS pVal);
	}

	[ComImport]
	[Guid("97EA99C7-0186-4AD4-8DF9-C5B4E0ED6B22")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IBackgroundCopyCallback
	{
		// Methods
		void JobTransferred([In, MarshalAs(UnmanagedType.Interface)] IBackgroundCopyJob pJob);

		void JobError([In, MarshalAs(UnmanagedType.Interface)] IBackgroundCopyJob pJob, [In, MarshalAs(UnmanagedType.Interface)] IBackgroundCopyError pError);

		void JobModification([In, MarshalAs(UnmanagedType.Interface)] IBackgroundCopyJob pJob, [In] uint dwReserved);
		
	}

	/// <summary>
	/// The BG_JOB_STATE enumeration type defines constant values for the 
	/// different states of a job
	/// </summary>
	public enum BG_JOB_STATE 
	{
		/// <summary>
		/// Specifies that the job is in the queue and waiting to run. 
		/// If a user logs off while their job is transferring, the job 
		/// transitions to the queued state
		/// </summary>
		BG_JOB_STATE_QUEUED = 0,

		/// <summary>
		/// Specifies that BITS is trying to connect to the server. If the 
		/// connection succeeds, the state of the job becomes 
		/// BG_JOB_STATE_TRANSFERRING; otherwise, the state becomes 
		/// BG_JOB_STATE_TRANSIENT_ERROR
		/// </summary>
		BG_JOB_STATE_CONNECTING = 1,

		/// <summary>
		/// Specifies that BITS is transferring data for the job
		/// </summary>
		BG_JOB_STATE_TRANSFERRING = 2,

		/// <summary>
		/// Specifies that the job is suspended (paused)
		/// </summary>
		BG_JOB_STATE_SUSPENDED = 3,

		/// <summary>
		/// Specifies that a non-recoverable error occurred (the service is 
		/// unable to transfer the file). When the error can be corrected, 
		/// such as an access-denied error, call the IBackgroundCopyJob::Resume 
		/// method after the error is fixed. However, if the error cannot be 
		/// corrected, call the IBackgroundCopyJob::Cancel method to cancel 
		/// the job, or call the IBackgroundCopyJob::Complete method to accept 
		/// the portion of a download job that transferred successfully.
		/// </summary>
		BG_JOB_STATE_ERROR = 4,

		/// <summary>
		/// Specifies that a recoverable error occurred. The service tries to 
		/// recover from the transient error until the retry time value that 
		/// you specify using the IBackgroundCopyJob::SetNoProgressTimeout method 
		/// expires. If the retry time expires, the job state changes to 
		/// BG_JOB_STATE_ERROR
		/// </summary>
		BG_JOB_STATE_TRANSIENT_ERROR = 5,

		/// <summary>
		/// Specifies that your job was successfully processed
		/// </summary>
		BG_JOB_STATE_TRANSFERRED = 6,

		/// <summary>
		/// Specifies that you called the IBackgroundCopyJob::Complete method 
		/// to acknowledge that your job completed successfully
		/// </summary>
		BG_JOB_STATE_ACKNOWLEDGED = 7,

		/// <summary>
		/// Specifies that you called the IBackgroundCopyJob::Cancel method to 
		/// cancel the job (remove the job from the transfer queue)
		/// </summary>
		BG_JOB_STATE_CANCELLED = 8,
	}

	/// <summary>
	/// The BG_JOB_TYPE enumeration type defines constant values that you 
	/// use to specify the type of transfer job, such as download
	/// </summary>
	public enum BG_JOB_TYPE 
	{
		/// <summary>
		/// Specifies that the job downloads files to the client
		/// </summary>
		BG_JOB_TYPE_DOWNLOAD = 0,

		/// <summary>
		/// Specifies that the job uploads a file to the server.
		/// </summary>
		BG_JOB_TYPE_UPLOAD = 1,

		/// <summary>
		/// BG_JOB_TYPE_UPLOAD_REPLY
		/// </summary>
		BG_JOB_TYPE_UPLOAD_REPLY = 2,
	}

	[Flags]
	internal enum BG_JOB_NOTIFICATION_TYPE : uint
	{
		BG_NOTIFY_JOB_TRANSFERRED = 0x0001,
		BG_NOTIFY_JOB_ERROR = 0x0002,
		BG_NOTIFY_DISABLE = 0x0004,
		BG_NOTIFY_JOB_MODIFICATION = 0x0008,
	}

	/// <summary>
	/// The BG_JOB_PROXY_USAGE enumeration type defines constant values 
	/// that you use to specify which proxy to use for file transfers
	/// </summary>
	internal  enum BG_JOB_PROXY_USAGE 
	{
		/// <summary>
		/// Use the proxy and proxy bypass list settings defined by each 
		/// user to transfer files
		/// </summary>
		BG_JOB_PROXY_USAGE_PRECONFIG = 0,

		/// <summary>
		/// Do not use a proxy to transfer files
		/// </summary>
		BG_JOB_PROXY_USAGE_NO_PROXY = 1,

		/// <summary>
		/// Use the application's proxy and proxy bypass list to transfer files
		/// </summary>
		BG_JOB_PROXY_USAGE_OVERRIDE = 2,
	}

	
	/// <summary>
	/// The BG_JOB_PRIORITY enumeration type defines the constant values 
	/// that you use to specify the priority level of the job
	/// </summary>
	public enum BG_JOB_PRIORITY 
	{
		/// <summary>
		/// Transfers the job in the foreground
		/// </summary>
		BG_JOB_PRIORITY_FOREGROUND = 0,

		/// <summary>
		/// Transfers the job in the background. This is the highest background 
		/// priority level. 
		/// </summary>
		BG_JOB_PRIORITY_HIGH = 1,

		/// <summary>
		/// Transfers the job in the background. This is the default priority 
		/// level for a job
		/// </summary>
		BG_JOB_PRIORITY_NORMAL = 2,

		/// <summary>
		/// Transfers the job in the background. This is the lowest background 
		/// priority level
		/// </summary>
		BG_JOB_PRIORITY_LOW = 3,
	}

	internal enum BG_AUTH_SCHEME
	{
		// Fields
		BG_AUTH_SCHEME_BASIC = 1,
		BG_AUTH_SCHEME_DIGEST = 2,
		BG_AUTH_SCHEME_NTLM = 3,
		BG_AUTH_SCHEME_NEGOTIATE = 4,
		BG_AUTH_SCHEME_PASSPORT = 5
	}
 
	internal enum BG_AUTH_TARGET
	{
		// Fields
		BG_AUTH_TARGET_SERVER = 1,
		BG_AUTH_TARGET_PROXY = 2,
	}

	/// <summary>
	/// The BG_ERROR_CONTEXT enumeration type defines the constant values 
	/// that specify the context in which the error occurred
	/// </summary>
	internal  enum BG_ERROR_CONTEXT 
	{
		/// <summary>
		/// An error has not occurred
		/// </summary>
		BG_ERROR_CONTEXT_NONE = 0,

		/// <summary>
		/// The error context is unknown
		/// </summary>
		BG_ERROR_CONTEXT_UNKNOWN = 1,

		/// <summary>
		/// The transfer queue manager generated the error
		/// </summary>
		BG_ERROR_CONTEXT_GENERAL_QUEUE_MANAGER = 2,

		/// <summary>
		/// The error was generated while the queue manager was 
		/// notifying the client of an event
		/// </summary>
		BG_ERROR_CONTEXT_QUEUE_MANAGER_NOTIFICATION = 3,

		/// <summary>
		/// The error was related to the specified local file. For example, 
		/// permission was denied or the volume was unavailable
		/// </summary>
		BG_ERROR_CONTEXT_LOCAL_FILE = 4,

		/// <summary>
		/// The error was related to the specified remote file. 
		/// For example, the URL is not accessible
		/// </summary>
		BG_ERROR_CONTEXT_REMOTE_FILE = 5,

		/// <summary>
		/// The transport layer generated the error. These errors are general 
		/// transport failures; errors not specific to the remote file
		/// </summary>
		BG_ERROR_CONTEXT_GENERAL_TRANSPORT = 6,
	}

	[StructLayout(LayoutKind.Explicit, Size=16, Pack=4)]
	internal struct BG_AUTH_CREDENTIALS
	{
		[FieldOffset(0)]
		public BG_AUTH_TARGET Target;

		[FieldOffset(4)]
		public BG_AUTH_SCHEME Scheme;

		[FieldOffset(8)]
		public BG_AUTH_CREDENTIALS_UNION Credentials;
	}

	[StructLayout(LayoutKind.Explicit, Size=8, Pack=4)]
	internal struct BG_AUTH_CREDENTIALS_UNION
	{
		[FieldOffset(0)]
		public BG_BASIC_CREDENTIALS Basic;
	}

	[StructLayout(LayoutKind.Explicit, Size=8, Pack=4)]
	internal struct BG_BASIC_CREDENTIALS
	{
		[FieldOffset(0)]
		[MarshalAs(UnmanagedType.LPWStr)]
		public string UserName;

		[FieldOffset(4)]
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Password;
	}
 
	/// <summary>
	/// The BG_FILE_INFO structure provides the local and 
	/// remote names of the file to transfer
	/// </summary>
	[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0)]
	internal  struct _BG_FILE_INFO 
	{
		/// <summary>
		/// Remote Name for the File
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RemoteName;

		/// <summary>
		/// Local Name for the file
		/// </summary>
		[MarshalAs(UnmanagedType.LPWStr)]
		public string LocalName;
	}

	/// <summary>
	/// The BG_JOB_PROGRESS structure provides job-related progress information, 
	/// such as the number of bytes and files transferred
	/// </summary>
	[StructLayoutAttribute(LayoutKind.Sequential, Pack=8, Size=0)]
	internal struct _BG_JOB_PROGRESS 
	{
		/// <summary>
		/// Total number of bytes to transfer for the job.
		/// </summary>
		public ulong BytesTotal;

		/// <summary>
		/// Number of bytes transferred
		/// </summary>
		public ulong BytesTransferred;

		/// <summary>
		/// Total number of files to transfer for this job
		/// </summary>
		public uint FilesTotal;

		/// <summary>
		/// Number of files transferred. 
		/// </summary>
		public uint FilesTransferred;
	}


	[StructLayout(LayoutKind.Sequential, Pack=8)]
	internal struct _BG_JOB_REPLY_PROGRESS
	{
		// Fields
		public ulong BytesTotal;
		public ulong BytesTransferred;
	}

	/// <summary>
	/// The BG_JOB_TIMES structure provides job-related timestamps
	/// </summary>
	[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0)]
	internal  struct _BG_JOB_TIMES 
	{
		/// <summary>
		/// Time the job was created
		/// </summary>
		public _FILETIME CreationTime;

		/// <summary>
		/// Time the job was last modified or bytes were transferred
		/// </summary>
		public _FILETIME ModificationTime;

		/// <summary>
		/// Time the job entered the BG_JOB_STATE_TRANSFERRED state
		/// </summary>
		public _FILETIME TransferCompletionTime;
	}

	
	/// <summary>
	/// FILETIME Structure
	/// </summary>
	[StructLayoutAttribute(LayoutKind.Sequential, Pack=4, Size=0)]
	internal  struct _FILETIME 
	{
		/// <summary>
		/// Description
		/// </summary>
		public uint dwLowDateTime;

		/// <summary>
		/// Description
		/// </summary>
		public uint dwHighDateTime;
	}

	/// <summary>
	/// The BG_FILE_PROGRESS structure provides file-related progress information, 
	/// such as the number of bytes transferred
	/// </summary>
	[StructLayoutAttribute(LayoutKind.Sequential, Pack=8, Size=0)]
	internal  struct _BG_FILE_PROGRESS 
	{
		/// <summary>
		/// Size of the file in bytes
		/// </summary>
		public ulong BytesTotal;

		/// <summary>
		/// Number of bytes transferred. 
		/// </summary>
		public ulong BytesTransferred;

		/// <summary>
		/// For downloads, the value is TRUE if the file is available to the user; 
		/// otherwise, the value is FALSE
		/// </summary>
		public int Completed;
	}

}
