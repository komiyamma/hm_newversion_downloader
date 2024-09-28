import re
import requests
import os
from urllib.parse import urljoin

def download_files():

    response = requests.get('https://hide.maruo.co.jp/software/hidemaru.html')
    response.raise_for_status()  # HTTPエラーチェック
    html_content = response.text

    # 正規表現を使ってURLを抽出
    urls = []
    matches: list = re.findall(r'"(https://hide.maruo.co.jp/software/bin3/hm\d+b\d+_signed\.exe)"', html_content)
    print(matches);
    for match in matches:
        urls.append(urljoin('https://hide.maruo.co.jp', match))

    if (len(matches) == 0):
        return

    url = matches[0]
    try:
        response = requests.get(url, stream=True)
        response.raise_for_status()  # HTTPエラーが発生した場合、例外を発生させる

        filename = os.path.basename(url)
        filename = "HmSigned.zip"

        with open(filename, 'wb') as f:
            for chunk in response.iter_content(chunk_size=8192):
                f.write(chunk)

        print(f"ファイル '{filename}' をダウンロードしました。")

    except requests.exceptions.RequestException as e:
        print(f"ダウンロードエラー: {e}")

download_files()
