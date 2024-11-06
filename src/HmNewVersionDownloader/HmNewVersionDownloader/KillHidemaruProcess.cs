using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Program
{
    private static void KillHidemaruProcesses()
    {
        System.Diagnostics.Process[] ps1 = System.Diagnostics.Process.GetProcessesByName("hidemaru");

        foreach (System.Diagnostics.Process p in ps1)
        {
            //プロセスを強制的に終了させる
            p.Kill();
        }
    }

}
