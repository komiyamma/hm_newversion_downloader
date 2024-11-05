using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public partial class Program
{
    public static int executeSevenZip(string sevenZipPath, string archivePath, string outputDirectory)
    {
        // 7-Zip実行ファイルのパス、アーカイブファイルのパス、出力ディレクトリのパスをチェック
        if (string.IsNullOrEmpty(sevenZipPath) || !System.IO.File.Exists(sevenZipPath))
        {
            Console.Error.WriteLine("7-Zip実行ファイルが見つかりません: " + sevenZipPath);
            return -1; // エラーコードを返す
        }
        if (string.IsNullOrEmpty(archivePath) || !System.IO.File.Exists(archivePath))
        {
            Console.Error.WriteLine("アーカイブファイルが見つかりません: " + archivePath);
            return -2; // エラーコードを返す
        }
        if (string.IsNullOrEmpty(outputDirectory))
        {
            Console.Error.WriteLine("出力ディレクトリが指定されていません。");
            return -3; // エラーコードを返す
        }

        try
        {
            // プロセスを開始する
            using (Process process = new Process())
            {
                process.StartInfo.FileName = sevenZipPath;
                process.StartInfo.Arguments = $"x \"{archivePath}\" -o\"{outputDirectory}\"";
                process.StartInfo.UseShellExecute = false; // シェルを使用しない
                process.StartInfo.RedirectStandardOutput = true; // 出力をリダイレクト
                process.StartInfo.RedirectStandardError = true; // エラー出力をリダイレクト
                process.StartInfo.CreateNoWindow = true; // ウィンドウを表示しない

                process.Start();

                // 出力とエラー出力を読み取る
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                // 出力とエラー出力を出力する (デバッグ用)
                Console.WriteLine("7-Zip出力:");
                Console.WriteLine(output);
                Console.WriteLine("7-Zipエラー出力:");
                Console.WriteLine(error);

                return process.ExitCode; // 終了コードを返す
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("7-Zip実行中にエラーが発生しました: " + ex.Message);
            return -4; // エラーコードを返す
        }
    }
}