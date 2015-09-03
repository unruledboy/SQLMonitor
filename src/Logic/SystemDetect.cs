using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Xnlab.SQLMon.Logic
{
    class SystemDetect
    {

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        internal static bool Is64Bit()
        {
            bool retVal;
            IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);
            return retVal;
        }

        internal static bool Is64BitEx()
        {
            var result = false;
            var value = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            if (!string.IsNullOrEmpty(value) && value.IndexOf("64") != -1)
                result = true;
            if (!result)
            {
                value = Environment.GetEnvironmentVariable("ProgramW6432");
                if (!string.IsNullOrEmpty(value) && value.IndexOf("64") != -1)
                    result = true;
            }
            return result;
        }
    }
}
