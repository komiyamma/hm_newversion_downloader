using System;
using System.Net.Http;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;
using System.Collections.Generic;
using System.Security.Policy;

public class Program
{
    public static void Main(string[] args)
    {
        try
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
                    return;
                }

                foreach(var u in urls)
                {
                    Console.WriteLine(u);
                }

                // 最初のURLからのダウンロード
                string url = urls[0];
                using (WebClient webClient = new WebClient())
                {
                    // ファイルをダウンロード
                    string filename = "HmSigned.zip";
                    webClient.DownloadFile(url, filename);

                    Console.WriteLine($"ファイル '{filename}' をダウンロードしました。");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ダウンロードエラー: {ex}");
        }
    }

}
