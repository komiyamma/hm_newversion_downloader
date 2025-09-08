using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

public partial class Program
{
    // 秀丸のプロセス名（共通定義）。
    private const string HidemaruProcessName = "hidemaru";

    // 非同期終了待ちのための遅延・ポーリング条件（既存仕様の数値を定数化）。
    private const int CloseGraceDelayMs = 500;
    private const int ClosePollCount = 3;
    private const int ClosePollIntervalMs = 200;

    // 一時領域と作業ファイル/ディレクトリの絶対パス。
    static readonly string TempDirectory = Path.GetTempPath();
    static readonly string ArchiveFilePath = Path.Combine(TempDirectory, "HmNewVersionArchive.zip");
    static readonly string ExtractedArchiveDirectory = Path.Combine(TempDirectory, "HmNewVersionArchive");

    // 秀丸本体のインストールディレクトリ（Main 引数で決定）。
    static string HidemaruInstallDirectory = null;

    public static void Main(string[] args)
    {
        // 引数1: 秀丸のインストールフォルダ。指定がなければ何もせず終了（既存仕様）。
        if (args.Length >= 1)
        {
            HidemaruInstallDirectory = args[0];
        }
        else
        {
            return;
        }

        // 引数2,3: 取得対象 exe の正規表現（ベータ/リリース）。指定時は上書き。
        if (args.Length >= 3)
        {
            betaExePattern = "(" + args[1] + ")";
            releaseExePattern = "(" + args[2] + ")";
        }

        // 非同期で「自然終了要求」と「ダウンロード/展開」を同時進行。
        var closeTask = Task.Run(async () =>
        {
            try
            {
                await Task.Delay(CloseGraceDelayMs); // 先に少し猶予

                await CloseHidemaruProcess(); // WM_CLOSE を送って自然終了を促す

                for (int count = 0; count < ClosePollCount; count++)
                {
                    Process[] remainingProcesses = Process.GetProcessesByName(HidemaruProcessName);
                    if (remainingProcesses.Length == 0)
                    {
                        break;
                    }
                    await Task.Delay(ClosePollIntervalMs);
                }

                KillHidemaruProcesses(); // まだ残っていれば最終手段
            }
            catch (Exception ex)
            {
                Console.WriteLine($"秀丸の終了エラー: {ex}");
            }
        });

        var workTask = Task.Run(() =>
        {
            try
            {
                string executableUrl = GetTargetExecutableUrl();

                DownloadFileTo(executableUrl, ArchiveFilePath);

                // 作業ディレクトリを空に整える
                NormalizeExtractionDirectory();
                Console.WriteLine("フォルダを正規化します。");

                // 7z 解凍
                ExtractArchiveWithSevenZip("7z.exe", ArchiveFilePath, ExtractedArchiveDirectory);
                Console.WriteLine("ファイルを解凍します。");

                ApplyResourceToHidemaruExecutable();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ダウンロードエラー: {ex}");
                return;
            }
        });

        // どちらも完了まで待機
        closeTask.Wait();
        workTask.Wait();

        try
        {
            Console.WriteLine("ファイルをコピーします。");
            UpdateHidemaruFiles(HidemaruInstallDirectory);

            Console.WriteLine("解凍ファイルを削除します。");
            NormalizeExtractionDirectory();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ファイル操作エラー: {ex}");
            return;
        }
    }
}
