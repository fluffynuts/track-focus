using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace track_focus;

public static class FindForegroundWindow
{
    [DllImport("user32.dll")]
    private static extern IntPtr GetForegroundWindow();
    
    [DllImport("user32.dll", SetLastError = true)]
    private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

    public static Process GetForegroundProcess()
    {
        uint processID = 0;
        var hWnd = GetForegroundWindow(); // Get foreground window handle
        var threadID = GetWindowThreadProcessId(hWnd, out processID); // Get PID from window handle
        if (processID < 1)
        {
            return null;
        }

        var fgProc = Process.GetProcessById(Convert.ToInt32(processID)); // Get it as a C# obj.
        // NOTE: In some rare cases ProcessID will be NULL. Handle this how you want. 
        return fgProc;
    }
}