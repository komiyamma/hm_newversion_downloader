using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Program { 

    static bool ReplaceIcon()
    {
        string temp_hidemaru_exe = Path.Combine(archive_extracted_folder, "hidemaru.exe");
        string temp_hidemaru_replaced = Path.Combine(archive_extracted_folder, "hidemaru_replaced.exe");
        string resourceFile = "HmNewVersionDownloader.res"; // これは絶対パスか相対パスで指定する必要があります


        // ResourceHacker.exe のパスを適切に設定する
        string resourceHackerPath = "ResourceHacker.exe"; // ResourceHacker.exe のパスをここに指定してください.  実行ファイルと同じフォルダにあればこのままでOK


        try
        {
            if (!File.Exists(resourceHackerPath))
            {
                throw new FileNotFoundException(resourceHackerPath);
            }

            // 1. ResourceHacker を実行
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = resourceHackerPath,
                Arguments = $"-open \"{temp_hidemaru_exe}\" -resource \"{resourceFile}\" -action modify -save \"{temp_hidemaru_replaced}\"",
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            Console.WriteLine(resourceHackerPath + " " + $"-open \"{temp_hidemaru_exe}\" -resource \"{resourceFile}\" -action modify -save \"{temp_hidemaru_replaced}\"");

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


            // 2. 元のファイルを削除
            if (File.Exists(temp_hidemaru_exe))
            {
                File.Delete(temp_hidemaru_exe);
            }
            else
            {
                Console.WriteLine("元のファイルが見つかりません: " + temp_hidemaru_exe);
            }

            // 3. ファイル名を変更
            if (File.Exists(temp_hidemaru_replaced))
            {
                File.Move(temp_hidemaru_replaced, temp_hidemaru_exe);
            }
            else
            {
                Console.WriteLine("変更後のファイルが見つかりません: " + temp_hidemaru_replaced);
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
