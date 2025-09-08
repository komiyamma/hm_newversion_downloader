using System;
using System.IO;

public partial class Program
{

    // 解凍用ワークディレクトリを「存在し、かつ空」に正規化する。
    // 既存仕様: 既存ファイルの削除でエラーが発生しても握りつぶす（後続処理を継続）。
    private static void NormalizeExtractionDirectory()
    {
        try
        {
            if (!Directory.Exists(ExtractedArchiveDirectory))
            {
                Directory.CreateDirectory(ExtractedArchiveDirectory);
            }

            foreach (string file in Directory.GetFiles(ExtractedArchiveDirectory))
            {
                try
                {
                    File.Delete(file);
                }
                catch
                {
                    // 既存仕様: 失敗しても継続（ログのみ）。
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"解凍用フォルダのファイル操作エラー: {ex}");
            return;
        }
    }
}
