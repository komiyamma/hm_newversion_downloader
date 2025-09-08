using System;
using System.Diagnostics;
using System.IO;

public partial class Program { 

    // 7-Zip で解凍した hidemaru.exe に対し、ResourceHacker を用いて .res を適用し、
    // 自然な形で置き換える（テンポラリへ保存 → 元 exe 削除 → 名前変更）。
    // 既存仕様: ResourceHacker のパスは呼び出し環境で解決可能であること（同一フォルダ等）。
    static bool ApplyResourceToHidemaruExecutable()
    {
        string targetExePath = Path.Combine(ExtractedArchiveDirectory, "hidemaru.exe");
        string replacedExePath = Path.Combine(ExtractedArchiveDirectory, "hidemaru_replaced.exe");
        string resourceFilePath = "HmNewVersionDownloader.res"; // 実行環境で解決可能な .res パス（相対/絶対）

        string resourceHackerPath = "ResourceHacker.exe"; // 同梱または PATH 上に配置されている想定

        try
        {
            if (!File.Exists(resourceHackerPath))
            {
                throw new FileNotFoundException(resourceHackerPath);
            }

            // ResourceHacker によるリソース更新
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = resourceHackerPath,
                Arguments = $"-open \"{targetExePath}\" -resource \"{resourceFilePath}\" -action modify -save \"{replacedExePath}\"",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            Console.WriteLine(resourceHackerPath + " " + $"-open \"{targetExePath}\" -resource \"{resourceFilePath}\" -action modify -save \"{replacedExePath}\"");

            using (Process process = new Process { StartInfo = startInfo })
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();
                process.WaitForExit();

                if (process.ExitCode != 0)
                {
                    throw new Exception($"ResourceHacker実行エラー: ExitCode={process.ExitCode}, Output={output}, Error={error}");
                }
            }

            // 置換の原子性は保証しない（既存仕様）。
            if (File.Exists(targetExePath))
            {
                File.Delete(targetExePath);
            }
            else
            {
                Console.WriteLine("元のファイルが見つかりません: " + targetExePath);
            }

            if (File.Exists(replacedExePath))
            {
                File.Move(replacedExePath, targetExePath);
            }
            else
            {
                Console.WriteLine("変更後のファイルが見つかりません: " + replacedExePath);
            }

            Console.WriteLine("処理が完了しました。");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"エラーが発生しました: {ex.Message}");
        }

        return true;
    }
}
