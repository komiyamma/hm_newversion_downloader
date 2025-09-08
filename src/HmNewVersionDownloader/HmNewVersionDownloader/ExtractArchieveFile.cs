using System;
using System.Diagnostics;

public partial class Program
{
    // 7-Zip (7z.exe) を子プロセスとして起動し、アーカイブを指定ディレクトリへ展開する。
    // 戻り値は 7-Zip のプロセス終了コード（0 は成功を意味するのが通例）。
    // 事前条件チェックでファイルやパス不備を判定し、問題があれば負の独自コードを返す。
    public static int ExtractArchiveWithSevenZip(string sevenZipPath, string archivePath, string outputDirectory)
    {
        // 7-Zip 実行ファイルの有無
        if (string.IsNullOrEmpty(sevenZipPath) || !System.IO.File.Exists(sevenZipPath))
        {
            Console.Error.WriteLine("7-Zip実行ファイルが見つかりません: " + sevenZipPath);
            return -1; // sevenZipPath 不正
        }
        // 対象アーカイブの有無
        if (string.IsNullOrEmpty(archivePath) || !System.IO.File.Exists(archivePath))
        {
            Console.Error.WriteLine("アーカイブファイルが見つかりません: " + archivePath);
            return -2; // archivePath 不正
        }
        // 出力先パスの妥当性（空文字チェックのみ。ディレクトリ作成は呼び出し側の責務）
        if (string.IsNullOrEmpty(outputDirectory))
        {
            Console.Error.WriteLine("出力ディレクトリが指定されていません。");
            return -3; // 出力先未指定
        }

        try
        {
            // 7-Zip を非対話モードで起動しログを収集
            using (Process process = new Process())
            {
                process.StartInfo.FileName = sevenZipPath;
                process.StartInfo.Arguments = $"x \"{archivePath}\" -o\"{outputDirectory}\"";
                process.StartInfo.UseShellExecute = false; // シェルを介さない（標準出力/標準エラーの取得のため）
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.CreateNoWindow = true; // コンソール非表示

                process.Start();

                // 出力/エラーを全量読み取り（7z 側の進捗はテキスト出力）
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                Console.WriteLine("7-Zip出力:");
                Console.WriteLine(output);
                Console.WriteLine("7-Zipエラー出力:");
                Console.WriteLine(error);

                return process.ExitCode; // 0:成功、非0:7z側のエラー
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("7-Zip実行中にエラーが発生しました: " + ex.Message);
            return -4; // 予期せぬ例外
        }
    }
}