using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Program
{
    private static void updateHidemaruFiles(string hm_folder)
    {
        if (String.IsNullOrEmpty(hm_folder))
        {
            throw new Exception("秀丸フォルダーが無い");
        }

        if (!File.Exists(Path.Combine(hm_folder, "hidemaru.exe")))
        {
            throw new Exception("アップデート先は秀丸フォルダではない");
        }

        copyFiles(archive_extracted_folder, hm_folder);
    }

    // Aディレクトリのファイル群をBディレクトリに上書きコピーする
    static void copyFiles(string srcDirectory, string dstDirectory)
    {
        bool isFail = true;
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
                isFail = true;
                Console.WriteLine($"ファイルのコピーに失敗: {file}");
            }
        }

        if (isFail)
        {
            Console.WriteLine("コピーは失敗したので管理者権限で再度実行を試みる");
            updateHidemaruFilesRunAsAdmin(srcDirectory, dstDirectory);
        }
        Console.WriteLine($"ファイルのコピーが完了: {srcDirectory} -> {dstDirectory}");
    }

}
