using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;

public partial class Program
{
    // �Ώۂ̏G�ۂ̐��K�\���i��1�L���v�`���ɑ��΃p�X������z��j
    static string betaExePattern = @"aaaaaaaaaaaaaaaaaaa";
    static string releaseExePattern = @"bbbbbbbbbbbbbbbbbbb";

    // �擾���̃y�[�WURL����ё��΃p�X�����p�̊��URL
    private const string HidemaruPageUrl = "https://hide.maruo.co.jp/software/hidemaru.html";
    private static readonly Uri BaseSoftwareUri = new Uri("https://hide.maruo.co.jp/software/", UriKind.Absolute);

    // HTML �R�����g���������邽�߂̎��O�R���p�C���ς݃p�^�[��
    private static readonly Regex HtmlCommentRegex = new Regex("<!--.*?-->", RegexOptions.Singleline | RegexOptions.Compiled);

    // HTML �����񂩂�R�����g�̈����菜���B
    static string StripHtmlComments(string html)
    {
        return HtmlCommentRegex.Replace(html, string.Empty);
    }

    // ���J�y�[�W���擾���A�x�[�^/�����[�X�o���̃p�^�[���őΏ� exe �� URL �𒊏o���A
    // �ŏ��Ɍ����������̂�Ԃ��i�����d�l�j�B
    static string GetTargetExecutableUrl()
    {
        using (HttpClient client = new HttpClient())
        {
            string htmlContent = client.GetStringAsync(HidemaruPageUrl).Result;
            htmlContent = StripHtmlComments(htmlContent);
            Console.WriteLine("Web�y�[�W�̎擾���������܂����B");

            var urls = new List<string>();
            urls.AddRange(ExtractAbsoluteUrls(htmlContent, betaExePattern, BaseSoftwareUri));
            urls.AddRange(ExtractAbsoluteUrls(htmlContent, releaseExePattern, BaseSoftwareUri));

            if (urls.Count == 0)
            {
                throw new Exception("�Ώۂ̐��K�\���Ƀ}�b�`����EXE�͑��݂��܂���ł����B");
            }

            return urls[0];
        }
    }

    // �w��p�^�[���� HTML ���瑊�΃p�X�𒊏o���A���URL�ƌ����������URL�R���N�V������Ԃ��B
    // ���Ҏd�l: ��1�L���v�`���O���[�v�����΃p�X�ł��邱�ƁB
    private static IEnumerable<string> ExtractAbsoluteUrls(string htmlContent, string pattern, Uri baseUri)
    {
        MatchCollection matches = Regex.Matches(htmlContent, pattern);
        Console.WriteLine("���K�\��:" + pattern);

        List<string> found = new List<string>(matches.Count);
        foreach (Match match in matches)
        {
            string relative = match.Groups[1].Value;
            string absolute = new Uri(baseUri, relative).AbsoluteUri;
            found.Add(absolute);
        }
        return found;
    }

    // �w�� URL ���烍�[�J���t�@�C���փ_�E�����[�h����B�������ɐi�����b�Z�[�W���o�́B
    private static void DownloadFileTo(string url, string filename)
    {
        using (WebClient webClient = new WebClient())
        {
            webClient.DownloadFile(url, filename);
            Console.WriteLine($"�t�@�C�� '{filename}' ���_�E�����[�h���܂����B");
        }
    }
}
