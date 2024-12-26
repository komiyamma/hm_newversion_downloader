using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public partial class Program
{
    static string temp_folder = System.IO.Path.GetTempPath();

    // ダウンロード先の絶対ファイル名の決定
    static string archive_fullpath = Path.Combine(temp_folder, "HmNewVersionArchive.zip");

    // 「HmNewVersionArchive」を解凍するフォルダ
    static string archive_extracted_folder = Path.Combine(temp_folder, "HmNewVersionArchive");

    // 秀丸本体のフォルダー
    static string hm_folder = null;

    public static void Main(string[] args)
    {
        // 秀丸が存在するフォルダの格納
        if (args.Length >= 1)
        {
            // 最初の引数は秀丸エディタのフォルダ
            hm_folder = args[0];
        }
        else
        {
            return;
        }

        // ダウンロードする秀丸のexeの正規表現
        if (args.Length >= 3)
        {
            hm_exe_beta_regexp = "(" + args[1] + ")";
            hm_exe_release_regexp = "(" + args[2] + ")";
        }

        var task1 = Task.Run(async () =>
        {
            try
            {
                // 0.5秒待つ
                await Task.Delay(500);

                // まずは自然なクローズを要求
                await CloseHidemaruProcess();

                for (int count = 0; count < 3; count++)
                {
                    Process[] remainProcess = Process.GetProcessesByName("hidemaru");
                    if (remainProcess.Length == 0)
                    {
                        break;
                    }
                    await Task.Delay(200);
                }

                // 全ての秀丸の強制終了
                KillHidemaruProcesses();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"秀丸の終了エラー: {ex}");
            }
        });

        var task2 = Task.Run(() =>
        {
            try
            {
                // 対象となるexeのURLの取得
                string download_exe_url = GetTargetExeUrl();

                // 対象のファイルをダウンロード
                DownloadTargetFile(download_exe_url, archive_fullpath);

                // ダウンロードしたものを展開するためのフォルダを作成
                // 作成済みならフォルダ内を空にする
                NormalizeTempFolder();
                Console.WriteLine("フォルダを正規化します。");

                // ダウンロードしたものを解凍
                ExecuteSevenZip("7z.exe", archive_fullpath, archive_extracted_folder);
                Console.WriteLine("ファイルを解凍します。");

                ReplaceIcon();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ダウンロードエラー: {ex}");
                return;
            }
        });

        // 非同期の方が遅くて、まだ終わっていないなら、終了を待つ
        task1.Wait();
        task2.Wait();

        try { 
            Console.WriteLine("ファイルをコピーします。");
            UpdateHidemaruFiles(hm_folder);

            Console.WriteLine("解凍ファイルを削除します。");
            NormalizeTempFolder();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ファイル操作エラー: {ex}");
            return;
        }

    }

}
