using System;
using System.IO;

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

        // 0.5秒待つ
        System.Threading.Thread.Sleep(500);

        try
        {
            // 全ての秀丸の終了
            KillHidemaruProcesses();

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

            Console.WriteLine("ファイルをコピーします。");
            UpdateHidemaruFiles(hm_folder);

            Console.WriteLine("解凍ファイルを削除します。");
            NormalizeTempFolder();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ダウンロードエラー: {ex}");
            return;
        }

    }

}
