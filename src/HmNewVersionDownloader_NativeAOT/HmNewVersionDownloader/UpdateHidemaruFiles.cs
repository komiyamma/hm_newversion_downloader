using System;
using System.IO;

public partial class Program
{
    private static void UpdateHidemaruFiles(string hm_folder)
    {
        if (String.IsNullOrEmpty(hm_folder))
        {
            throw new Exception("秀丸フォルダーが無い");
        }

        if (!File.Exists(Path.Combine(hm_folder, "hidemaru.exe")))
        {
            throw new Exception("アップデート先は秀丸フォルダではない");
        }

        CopyFiles(archive_extracted_folder, hm_folder);
    }

    // Aディレクトリのファイル群をBディレクトリに上書きコピーする
    static void CopyFiles(string srcDirectory, string dstDirectory)
    {
        int failCnt = 0;
        foreach (string file in Directory.GetFiles(srcDirectory))
        {
            string filename = Path.GetFileName(file);
            string dest = Path.Combine(dstDirectory, filename);
            try
            {
                File.Copy(file, dest, true);
            }
            catch(Exception e)
            {
                failCnt++;
                if (failCnt > 3)
                {
                    break;
                }
                Console.WriteLine($"ファイルのコピーに失敗: {file} {e.Message}");
            }
        }

        if (failCnt > 0)
        {
            Console.WriteLine("コピーは失敗したので管理者権限で再度実行を試みる");
            UpdateHidemaruFilesRunAsAdmin(srcDirectory, dstDirectory);
        }
        else
        {
            Console.WriteLine($"ファイルのコピーが完了: {srcDirectory} -> {dstDirectory}");
        }
    }

}
