using System;
using System.Diagnostics;
using System.Security.Principal;

public partial class Program
{
    static bool IsAdministrator()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }
    static void UpdateHidemaruFilesRunAsAdmin(string sourcePath, string destinationPath)
    {
        // 管理者権限が必要かどうかを確認します。
        // 管理者として実行するためのプロセスを開始します。
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            Verb = "runas",
            FileName = "cmd.exe",
            Arguments = $"/c xcopy \"{sourcePath}\" \"{destinationPath}\" /s /e /y", //  /s, /e, /y オプションを追加しました。必要に応じて変更してください。
            UseShellExecute = true,
            CreateNoWindow = false // ウィンドウを表示するか否か。trueにするとコンソールウィンドウは表示されません。
        };

        try
        {
            Process process = Process.Start(startInfo);
            process.WaitForExit(); // プロセスが終了するまで待機します。

            if (process.ExitCode == 0)
            {
                Console.WriteLine("xcopyコマンドが正常に実行されました。");
            }
            else
            {
                Console.WriteLine($"xcopyコマンドの実行に失敗しました。 エラーコード: {process.ExitCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラーが発生しました: {ex.Message}");
        }
    }
}
