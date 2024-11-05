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

        if ( ! File.Exists(Path.Combine(hm_folder, "hidemaru.exe")) )
        {
            throw new Exception("秀丸ファイルのアップデート先が無い");
        }

        // 全ての秀丸の終了
        killHidemaruProcesses();

        updateHidemaruFiles(archive_extracted_folder, hm_folder);
    }

    // Aディレクトリのファイル群をBディレクトリに上書きコピーする
    static void updateHidemaruFiles(string srcDirectory, string dstDirectory)
    {
        foreach (string file in Directory.GetFiles(srcDirectory))
        {
            string filename = Path.GetFileName(file);
            string dest = Path.Combine(dstDirectory, filename);
            try
            {
                File.Copy(file, dest, true);
            }
            catch
            {
                Console.WriteLine($"ファイルのコピーに失敗: {file}");
            }
        }

        Console.WriteLine($"ファイルのコピーが完了: {srcDirectory} -> {dstDirectory}");
    }

}
