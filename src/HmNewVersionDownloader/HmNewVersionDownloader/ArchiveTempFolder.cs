using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Program
{

    private static void NormalizeTempFolder()
    {
        try
        {
            if (!Directory.Exists(archive_extracted_folder))
            {
                Directory.CreateDirectory(archive_extracted_folder);
            }

            // archive_extracted_folder内のファイルを削除
            foreach (string file in Directory.GetFiles(archive_extracted_folder))
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"解凍用フォルダのファイル操作エラー: {ex}");
            return;
        }
    }
}
