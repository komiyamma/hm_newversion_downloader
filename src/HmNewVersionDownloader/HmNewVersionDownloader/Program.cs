using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

public class Program
{
    static string getTargetExeUrl()
    {
        // HTTPリクエストの作成
        using (HttpClient client = new HttpClient())
        {

            // Webページの取得
            string htmlContent = client.GetStringAsync("https://hide.maruo.co.jp/software/hidemaru.html").Result;

            // beta版の状況を取得

            // 正規表現を使ってURLを抽出
            MatchCollection matches_beta = Regex.Matches(htmlContent, @"(bin3?/hm\d+b\d+_signed\.exe)");

            // URLのリストを作成する
            List<string> urls = new List<string>();
            foreach (Match match in matches_beta)
            {
                urls.Add(new Uri(new Uri("https://hide.maruo.co.jp/software/"), match.Groups[1].Value).AbsoluteUri);
            }

            // 正規表現を使ってURLを抽出
            MatchCollection matches_release = Regex.Matches(htmlContent, @"(bin/hm\d+_signed\.exe)");
            foreach (Match match in matches_release)
            {
                urls.Add(new Uri(new Uri("https://hide.maruo.co.jp/software/"), match.Groups[1].Value).AbsoluteUri);
            }

            if (urls.Count == 0)
            {
                throw new Exception("対象の正規表現にマッチするEXEは存在しませんでした。");
            }

            return urls[0];
        }
    }

    public static void Main(string[] args)
    {
        try
        {
            // 最初のURLからのダウンロード
            string url = getTargetExeUrl();

            // ダウンロードファイル名
            string filename = "HmSigned.zip";
            if (args.Length > 0)
            {
                filename = args[0];
            }

            downloadTargetFile(url, filename);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ダウンロードエラー: {ex}");
        }
    }

    private static void downloadTargetFile(string url, string filename)
    {
        using (WebClient webClient = new WebClient())
        {
            webClient.DownloadFile(url, filename);

            Console.WriteLine($"ファイル '{filename}' をダウンロードしました。");
        }
    }
}
