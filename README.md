# FileCopyRenDel
フォルダ以下の全てのファイルに対して一括処理（コピー or リネーム or デリート）を行います。  

## Screen Shot
![image](https://user-images.githubusercontent.com/85425896/122248116-d11aa280-cf02-11eb-964e-9984654b0a1b.png)

## Target
**Windows10 / .Net Framework 4.6.1**  
（プロジェクトで切り替えればおそらく Win7 / .Net3.0 でも動作すると思います）

## 概要
* フォルダ以下の全てのファイルに対して一括処理（コピー or リネーム or デリート）を行います。
    * 名前を変えてファイルをコピー  
    * リネーム  
    * デリート  

* 対象となるファイルはキーワードによりフィルタリングする事ができます。

* 一括処理前に、処理するファイルの変更前、変更後の状態を確認する事ができます。

## 使い方
1. 一括処理を行うフォルダを設定する  
 サブディレクトリ以下も処理する場合、チェックボックスにチェック
2. コピー or リネーム or デリート を選択する  
 
3. 処理するファイル名の抽出キーワードを入力。`abc_00.png ～ abc_99.png` であれば `abc` とか  
 ディレクトリ下の全てのファイルの場合は未入力でOK  
 変更キーワードで名前を変更できる。`abcd` と入れれば `abcd_00.png ～ abcd_99.png` となる  
 「Regex を使用」にチェックを入れると正規表現が使える  
 ![image](https://user-images.githubusercontent.com/85425896/122248678-3373a300-cf03-11eb-92b9-5bad31d41dce.png)  
 ↓  
 ![image](https://user-images.githubusercontent.com/85425896/122248856-5900ac80-cf03-11eb-9227-b24065f4eceb.png)
 
4. 確認を押すと処理内容が右の確認ログに表示。問題なければ実行を押すと処理  
![image](https://user-images.githubusercontent.com/85425896/122249666-ffe54880-cf03-11eb-892b-08d4b2259469.png)  
 
5. 履歴に今まで処理した内容が保存され、ワンクリックで以前の処理内容をロードできる 

## ライセンス
[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)
