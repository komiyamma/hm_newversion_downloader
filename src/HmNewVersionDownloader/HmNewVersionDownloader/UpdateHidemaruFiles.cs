using System;
using System.IO;

public partial class Program
{
    // 秀丸エディタのインストール先へ、解凍済みの新バージョンファイル群を上書き配置する。
    // 事前条件:
    //  - installDirectory は秀丸のインストールディレクトリであること（hidemaru.exe が存在する）。
    // 例外:
    //  - 引数不備やインストール先の妥当性が確認できない場合は例外を送出する（上位で捕捉してユーザー通知）。
    private static void UpdateHidemaruFiles(string installDirectory)
    {
        if (String.IsNullOrEmpty(installDirectory))
        {
            throw new Exception("秀丸フォルダーが無い");
        }

        if (!File.Exists(Path.Combine(installDirectory, "hidemaru.exe")))
        {
            throw new Exception("アップデート先は秀丸フォルダではない");
        }

        CopyFiles(ExtractedArchiveDirectory, installDirectory);
    }

    // srcDirectory 直下のファイルを dstDirectory へ上書きコピーする。
    // 仕様:
    //  - 失敗が一定回数（>3）を超えた時点でループを打ち切る。
    //  - 一件でも失敗が発生した場合は、管理者権限での再試行（xcopy を runas で起動）にフォールバックする。
    //    → 既存仕様を尊重し、ディレクトリ配下の再帰コピーはフォールバック時に xcopy の /s /e で担保。
    static void CopyFiles(string srcDirectory, string dstDirectory)
    {
        int failedCount = 0;
        foreach (string filePath in Directory.GetFiles(srcDirectory))
        {
            string fileName = Path.GetFileName(filePath);
            string destinationPath = Path.Combine(dstDirectory, fileName);
            try
            {
                File.Copy(filePath, destinationPath, true);
            }
            catch(Exception e)
            {
                failedCount++;
                if (failedCount > 3)
                {
                    break;
                }
                Console.WriteLine($"ファイルのコピーに失敗: {filePath} {e.Message}");
            }
        }

        if (failedCount > 0)
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
