using System;
using System.Diagnostics;

public partial class Program
{
    private static void KillHidemaruProcesses()
    {
        Process[] ps1 = Process.GetProcessesByName("hidemaru");

        foreach (Process p in ps1)
        {
            //プロセスを強制的に終了させる
            p.Kill();
        }
    }

}
