using System.Runtime.InteropServices;

namespace AntiVirus.COMInterop
{
    [ComVisible(false)]
    enum HResult : uint
    {
        S_OK = 0,
        S_FALSE = 1,
        ERROR_FILE_NOT_FOUND = 0x80070002,
        INET_E_SECURITY_PROBLEM = 0x800c000e,
        E_FAIL = 0x80004005
    }
}
