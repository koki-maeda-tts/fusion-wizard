# Fusion Wizard

このリポジトリは Fusion Wizard というUnity製2DシューティングゲームのUnityプロジェクト、ビルドファイルと、その開発に用いたSource Generatorのソリューションが含まれたリポジトリです。

## プロジェクトの概要

制作人数: 1人  
制作期間: 2025/8/24 ～ 2025/9/13 (約3週間)  
制作の所要時間: 約80時間  
担当範囲: 企画、設計、プログラム、グラフィック制作 (サウンドおよびフォントは外部アセットを使用)  
使用ツール・技術:

- Unity
- Source Generator
- UniTask
- DOTween
- Visual Studio

使用言語: C#  
使用Unityバージョン: 6000.3.14f1  

## リポジトリ構成

`BuildFiles` : Windows用の各種ビルドファイル  
`GetOnlyPropertyGenerator` : Source Generatorのソースコードが含まれるVisual Studioのソリューション  
`LICENSES` : 各種ライセンス表記  
`UnityProject` : Unityのプロジェクトファイル  
`UnityProject/Assets/Scripts` : 実際のソースコードが含まれるフォルダ

## プレイ方法

※想定プレイ時間: 約10分

### Windows PC でプレイする場合

※Windows環境が必要になります。  

1. BuildFilesの中から、お使いのPC(intel-64bit/intel-32bit/ARM-64bit)に対応したビルドフォルダをダウンロードします。

2. ダウンロードしたフォルダの中にある「CombinationMagician.exe」をダブルクリックすることで起動できます。

### ブラウザでプレイする場合

Unityroomでプレイすることもできます。  
URL: <https://unityroom.com/games/fusion-wizard>

### 操作方法

WASD/矢印キー: プレイヤーの移動

マウスクリック/ドラッグ: プレイヤーの攻撃

Q/Eキー、マウスホイール: 攻撃方法(魔法の種類)の切り替え

Tabキー: メニューの表示

## クレジット

### BGM

魔王魂(<https://maou.audio/category/bgm/bgm-8bit/>)  
「8bit22」「8bit25」「8bit13」

### SE

魔王魂(<https://maou.audio/category/se/>)  
効果音ラボ(<https://soundeffect-lab.info/>)

### Font

「ノスタルドット（M+）」  
<https://logotype.jp/nosutaru-dot.html>

## ライセンス

このプロジェクトでは

- UniTask (<https://github.com/Cysharp/UniTask>)
- DOTween (<https://github.com/Demigiant/dotween>)

を使用しています。  
これらのライセンスについてはLICENSESフォルダを参照してください。
