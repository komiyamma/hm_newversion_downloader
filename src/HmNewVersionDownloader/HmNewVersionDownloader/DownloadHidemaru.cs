using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

public partial class Program
{
    // 対象の秀丸の正規表現（第1キャプチャに相対パスが入る想定）
    static string betaExePattern = @"aaaaaaaaaaaaaaaaaaa";
    static string releaseExePattern = @"bbbbbbbbbbbbbbbbbbb";

    // 取得元のページURLおよび相対パス解決用の基底URL
    private const string HidemaruPageUrl = "https://hide.maruo.co.jp/software/hidemaru.html";
    private static readonly Uri BaseSoftwareUri = new Uri("https://hide.maruo.co.jp/software/", UriKind.Absolute);

    // HTML コメントを除去するための事前コンパイル済みパターン
    private static readonly Regex HtmlCommentRegex = new Regex("<!--.*?-->", RegexOptions.Singleline | RegexOptions.Compiled);

    // HTML 文字列からコメント領域を取り除く。
    static string StripHtmlComments(string html)
    {
        return HtmlCommentRegex.Replace(html, string.Empty);
    }

    // 公開ページを取得し、ベータ/リリース双方のパターンで対象 exe の URL を抽出し、
    // 最初に見つかったものを返す（既存仕様）。
    static string GetTargetExecutableUrl()
    {
        using (HttpClient client = new HttpClient())
        {
            string htmlContent = client.GetStringAsync(HidemaruPageUrl).Result;
            htmlContent = StripHtmlComments(htmlContent);
            Console.WriteLine("Webページの取得が完了しました。");

            var urls = new List<string>();
            urls.AddRange(ExtractAbsoluteUrls(htmlContent, betaExePattern, BaseSoftwareUri));
            urls.AddRange(ExtractAbsoluteUrls(htmlContent, releaseExePattern, BaseSoftwareUri));

            if (urls.Count == 0)
            {
                throw new Exception("対象の正規表現にマッチするEXEは存在しませんでした。");
            }

            return urls[0];
        }
    }

    // 指定パターンで HTML から相対パスを抽出し、基底URLと結合した絶対URLコレクションを返す。
    // 期待仕様: 第1キャプチャグループが相対パスであること。
    private static IEnumerable<string> ExtractAbsoluteUrls(string htmlContent, string pattern, Uri baseUri)
    {
        MatchCollection matches = Regex.Matches(htmlContent, pattern);
        Console.WriteLine("正規表現:" + pattern);

        List<string> found = new List<string>(matches.Count);
        foreach (Match match in matches)
        {
            string relative = match.Groups[1].Value;
            string absolute = new Uri(baseUri, relative).AbsoluteUri;
            found.Add(absolute);
        }
        return found;
    }

    // 指定 URL からローカルファイルへダウンロードする。完了時に進捗メッセージを出力。
    private static void DownloadFileTo(string url, string filename)
    {
        using (WebClient webClient = new WebClient())
        {
            webClient.DownloadFile(url, filename);
            Console.WriteLine($"ファイル '{filename}' をダウンロードしました。");
        }
    }
}
