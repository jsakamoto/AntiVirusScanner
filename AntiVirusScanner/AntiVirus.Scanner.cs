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
        /// Viirus is found, and may be cleaned.
        /// </summary>
        VirusFound,
        
        /// <summary>
        /// Specified file to be scan is not found.
        /// </summary>
        FileNotExist
    }

    /// <summary>
    /// Scan and clean virus.
    /// </summary>
    [ComVisible(true)]
    public class Scanner
    {
        /// <summary>
        /// Get or set the client GUID of this scannerr.
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
        /// <param name="path">The path of file ttoo be scan.</param>
        public ScanResult ScanAndClean(string path)
        {
            if (Path.IsPathRooted(path) == false)
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
                var result = iae.Save();

                switch ((HResult)result)
                {
                    case HResult.S_OK: return ScanResult.VirusNotFound;
                    case HResult.INET_E_SECURITY_PROBLEM: return ScanResult.VirusFound;
                    case HResult.ERROR_FILE_NOT_FOUND: return ScanResult.FileNotExist;
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
