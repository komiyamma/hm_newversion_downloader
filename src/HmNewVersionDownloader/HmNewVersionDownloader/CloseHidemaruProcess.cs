using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class Program
{
    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

    // EnumWindows のコールバックシグネチャ。
    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    private const uint WM_CLOSE = 0x0010;

    // 指定プロセス（複数インスタンス想定）に紐づくトップレベルウィンドウハンドルを列挙する。
    // 仕組み: プロセスID集合を先に作り、EnumWindows で各ウィンドウの所属プロセスIDを照合。
    private static List<IntPtr> GetWindowHandlesByProcessName(string processName)
    {
        var handles = new List<IntPtr>();
        var processes = Process.GetProcessesByName(processName);
        var processIds = new HashSet<int>();
        foreach (var process in processes)
        {
            processIds.Add(process.Id);
        }

        EnumWindows((hWnd, lParam) =>
        {
            int processId;
            GetWindowThreadProcessId(hWnd, out processId);
            if (processIds.Contains(processId))
            {
                handles.Add(hWnd);
            }
            return true; // 列挙継続
        }, IntPtr.Zero);

        return handles;
    }

    // 指定ウィンドウに対して WM_CLOSE を送信。アプリに通常終了の機会を与える。
    private static void SendCloseMessage(IntPtr hWnd)
    {
        SendMessage(hWnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
    }

    // 秀丸の全トップレベルウィンドウへ WM_CLOSE を送信し、自然終了を促す。
    // 非同期で送信し、全送信完了まで待機する。
    public static async Task CloseHidemaruProcess()
    {
        string processName = HidemaruProcessName;

        var windowHandles = GetWindowHandlesByProcessName(processName);
        if (windowHandles.Count == 0)
        {
            Console.WriteLine($"{processName} のウィンドウが見つかりませんでした。");
            return;
        }

        Console.WriteLine($"{processName} のウィンドウを閉じます。");

        var tasks = new List<Task>();
        foreach (var handle in windowHandles)
        {
            Console.WriteLine(handle);
            tasks.Add(Task.Run(() => SendCloseMessage(handle)));
        }
        await Task.WhenAll(tasks); // 全送信完了待ち

        Console.WriteLine($"{processName} に終了メッセージを送信完了");
    }
}