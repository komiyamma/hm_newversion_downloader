using System;
using System.Diagnostics;
using System.Security.Principal;

public partial class Program
{
    // 現在のプロセスが管理者権限で実行中かを判定する補助関数。
    // なお本ツールのフローでは直接の分岐には用いていない（権限不足時は UpdateHidemaruFilesRunAsAdmin で明示的に昇格実行する）。
    static bool IsAdministrator()
    {
        using (WindowsIdentity identity = WindowsIdentity.GetCurrent())
        {
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
    }

    // UAC 昇格を要求して xcopy を実行し、ファイルの上書きコピーを行う。
    // - sourcePath: コピー元（ディレクトリ）。
    // - destinationPath: コピー先（ディレクトリ）。
    // xcopy オプション:
    //   /s: 空でないディレクトリを含める
    //   /e: 空ディレクトリも含める（/s と併用して完全ツリーを対象）
    //   /y: 上書き確認を抑止
    // ここでは確実なオペレーションの可視性のためコンソールウィンドウ表示 (CreateNoWindow=false) としている。
    static void UpdateHidemaruFilesRunAsAdmin(string sourcePath, string destinationPath)
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            Verb = "runas",
            FileName = "cmd.exe",
            Arguments = $"/c xcopy \"{sourcePath}\" \"{destinationPath}\" /s /e /y",
            UseShellExecute = true,
            CreateNoWindow = false
        };

        try
        {
            Process process = Process.Start(startInfo);
            process.WaitForExit(); // xcopy 完了まで待機

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
