using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

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

        try
        {
            // 対象となるexeのURLの取得
            string download_exe_url = getTargetExeUrl();

            // 対象のファイルをダウンロード
            downloadTargetFile(download_exe_url, archive_fullpath);

            // ダウンロードしたものを展開するためのフォルダを作成
            // 作成済みならフォルダ内を空にする
            normalizeTempFolder();

            // ダウンロードしたものを解凍
            executeSevenZip("7z.exe", archive_fullpath, archive_extracted_folder);

            if (args.Length > 0)
            {
                // 引数がない場合は、秀丸エディタのフォルダを探す
                hm_folder = args[0];

                updateHidemaruFiles(hm_folder);

                normalizeTempFolder();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ダウンロードエラー: {ex}");
            return;
        }

    }

}
