using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

public partial class Program
{
    // 対象の秀丸の正規表現
    static string hm_exe_beta_regexp = @"aaaaaaaaaaaaaaaaaaa";

    // 対象の秀丸の正規表現
    static string hm_exe_release_regexp = @"bbbbbbbbbbbbbbbbbbb";

    static string removeHtmlComments(string html)
    {
        return Regex.Replace(html, @"<!--.*?-->", "", RegexOptions.Singleline);
    }

    static string getTargetExeUrl()
    {
        // HTTPリクエストの作成
        using (HttpClient client = new HttpClient())
        {

            // Webページの取得
            string htmlContent = client.GetStringAsync("https://hide.maruo.co.jp/software/hidemaru.html").Result;
            htmlContent = removeHtmlComments(htmlContent);
            Console.WriteLine("Webページの取得が完了しました。");
            // beta版の状況を取得

            // 正規表現を使ってURLを抽出
            MatchCollection matches_beta = Regex.Matches(htmlContent, hm_exe_beta_regexp);
            Console.WriteLine("正規表現:" + hm_exe_beta_regexp);

            // URLのリストを作成する
            List<string> urls = new List<string>();
            foreach (Match match in matches_beta)
            {
                urls.Add(new Uri(new Uri("https://hide.maruo.co.jp/software/"), match.Groups[1].Value).AbsoluteUri);
            }

            // 正規表現を使ってURLを抽出
            MatchCollection matches_release = Regex.Matches(htmlContent, hm_exe_release_regexp);
            Console.WriteLine("正規表現:" + hm_exe_release_regexp);

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

    private static void downloadTargetFile(string url, string filename)
    {
        using (WebClient webClient = new WebClient())
        {
            webClient.DownloadFile(url, filename);
            Console.WriteLine($"ファイル '{filename}' をダウンロードしました。");
        }
    }
}
