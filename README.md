# Automatic_Door_Unit
VRChat World（SDK3）用自動ドアのギミックです。
「Player_React_Sensor」をプレイヤー感知ギミックとして独立して使用することも可能です。

# 必須項目
* Unity 2019.4.30f1
* 最新の VRChat SDK3
* 最新の UdonSharp

# 使用方法
1. インポート後、「Automatic_Door_Unit_V2」プレハブをシーンへ配置
2. 必要に応じて「Player_React_Sensor」のPosition・Box Colliderなどを調整
3.「Auto_Mover」下の「Door_Left」「Door_Right」にドアのオブジェクトを移動
4. Animationを調整
5. ワールドをビルドしてアップロードします。

# 補足事項
利用方法についてはこちらのWikiもご参照ください。
https://github.com/MinadukiSekina/Automatic_Door_Unit/wiki

# 更新履歴

### 2021年9月22日 v1.0.0 リリース

### 2021年9月26日 v2.0.0 リリース
* Animator形式のV2に移行
* プレイヤー感知部分の別ユニット化