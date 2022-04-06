using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using AntiVirus.COMInterop;

namespace AntiVirus
{
    /// <summary>
    /// Provides a constants to indicate the result of scanning virus.
    /// </summary>
    [ComVisible(true)]
    public enum ScanResult
    {
        /// <summary>
        /// Virus is not found.
        /// </summary>
        VirusNotFound,
        
        /// <summary>
        /// Virus is found, and may be cleaned.
        /// </summary>
        VirusFound,
        
        /// <summary>
        /// Specified file to be scan is not found.
        /// </summary>
        FileNotExist,
        
        /// <summary>
        /// File is blocked due to security restrictions
        /// </summary>
        BlockedByPolicy
    }

    /// <summary>
    /// Scan and clean virus.
    /// </summary>
    [ComVisible(true)]
    public class Scanner
    {
        /// <summary>
        /// Get or set the client GUID of this scanner.
        /// </summary>
        public Guid ClientGuid { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scanner"/> class.
        /// </summary>
        public Scanner()
        {
            this.ClientGuid = new Guid(0xC467440F, 0x8ACB, 0x449B, 0xA7, 0xB1, 0x05, 0xB7, 0x40, 0x5C, 0x37, 0x53);
        }

        /// <summary>
        /// Scan the specified file, and clean if infected.
        /// </summary>
        /// <param name="path">The path of file to be scan.</param>
        public ScanResult ScanAndClean(string path)
        {
            if (!Path.IsPathRooted(path))
                throw new ArgumentException("Path is not rooted.", "path");

            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
            {
                return ScanAndCleanCore(path);
            }
            else
            {
                var result = default(ScanResult);
                var exception = default(Exception);
                var satThread = new Thread(() => {
                    try
                    {
                        result = ScanAndCleanCore(path);
                    }
                    catch (Exception e)
                    {
                        exception = e;
                    }
                });
                satThread.SetApartmentState(ApartmentState.STA);
                satThread.Start();
                satThread.Join();
                if (exception != null) throw new Exception("unexpected exception.", exception);
                return result;
            }
        }

        private ScanResult ScanAndCleanCore(string path)
        {
            var atsvc = new AttachmentServices();
            var iae = (IAttachmentExecute)atsvc;
            try
            {
                var clientGuid = this.ClientGuid;
                iae.SetClientGuid(ref clientGuid);
                iae.SetFileName(path);
                iae.SetLocalPath(path);
                iae.SetSource("about:internet");
                
                var result = iae.Save();

                switch ((HResult)result)
                {
                    case HResult.S_OK: 
                        // Scan completed, virus not found
                        return ScanResult.VirusNotFound;
                    case HResult.INET_E_SECURITY_PROBLEM: 
                        // This is returned if the download was blocked due to security
                        // restrictions. E.g. if the source URL was in the Restricted Sites zone
                        // and downloads are blocked on that zone, then the download would be
                        // deleted and this error code is returned.
                        return ScanResult.BlockedByPolicy;
                    case HResult.ERROR_FILE_NOT_FOUND: 
                        // Returned when file is not found
                        return ScanResult.FileNotExist;
                    case HResult.E_FAIL: 
                        // Returned if an anti-virus product reports an infection in the
                        // downloaded file during IAE::Save().
                        return ScanResult.VirusFound;
                    default:
                        throw new COMException("unexpected exception.", (int)result);
                }
            }
            finally
            {
                iae.ClearClientState();
                Marshal.ReleaseComObject(iae);
                Marshal.ReleaseComObject(atsvc);
            }
        }
    }
}
