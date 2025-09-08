using System;
using System.Diagnostics;

public partial class Program
{
    // 残存している秀丸プロセスを全て強制終了する（最終手段）。
    // 先に CloseHidemaruProcess() により自然終了を試みてから呼び出す想定。
    private static void KillHidemaruProcesses()
    {
        Process[] processes = Process.GetProcessesByName(HidemaruProcessName);
        foreach (Process process in processes)
        {
            process.Kill();
        }
    }

}
