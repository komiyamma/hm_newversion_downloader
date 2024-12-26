using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

public partial class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    //コールバック関数の型
    delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    private const uint WM_CLOSE = 0x0010;

    static List<IntPtr> GetWindowHandlesByProcessName(string processName)
    {
        var handles = new List<IntPtr>();
        var processes = Process.GetProcessesByName(processName);
        var processIds = new HashSet<int>();
        foreach (var p in processes)
        {
            processIds.Add(p.Id);
        }


        EnumWindows(new EnumWindowsProc((hWnd, lParam) =>
        {
            int processId;
            GetWindowThreadProcessId(hWnd, out processId);
            if (processIds.Contains(processId))
            {
                handles.Add(hWnd);
            }
            return true; // 列挙を続ける
        }), IntPtr.Zero);

        return handles;
    }

    static void SendCloseMessage(IntPtr hWnd)
    {
        SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
    }


    public static async Task CloseHidemaruProcess()
    {
        string processName = "hidemaru"; // プロセス名

        //プロセス名からウィンドウハンドルを取得する
        var windowHandles = GetWindowHandlesByProcessName(processName);

        if (windowHandles.Count == 0)
        {
            Console.WriteLine($"{processName} のウィンドウが見つかりませんでした。");
            return;
        }

        Console.WriteLine($"{processName} のウィンドウを閉じます。");

        //取得したすべてのウィンドウハンドルに対してWM_CLOSEメッセージを送信
        var tasks = new List<Task>();
        foreach (var handle in windowHandles)
        {
            Console.WriteLine(handle);
            tasks.Add(Task.Run(() => SendCloseMessage(handle)));
        }
        await Task.WhenAll(tasks); // 全てのメッセージ送信終了を待つ

        Console.WriteLine($"{processName} に終了メッセージを送信完了");
    }
}