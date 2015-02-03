using UnityEngine;
using System.Collections;

/// ゲーム管理
public class GameMgr : MonoBehaviour
{
  /// 状態
  enum eState
  {
    Init, // 初期化
    Main, // メイン
    GameClear, // ゲームクリア
    GameOver, // ゲームオーバー
  }
  /// 開始時は初期化状態にする
  eState _state = eState.Init;

  /// 開始
  void Start()
  {
    // ショットオブジェクトを32個確保しておく
    Shot.parent = new TokenMgr<Shot>("Shot", 32);
    // パーティクルオブジェクトを256個確保しておく
    Particle.parent = new TokenMgr<Particle>("Particle", 256);
    // 敵弾オブジェクトを256個確保しておく
    Bullet.parent = new TokenMgr<Bullet>("Bullet", 256);
    // 敵オブジェクトを64個確保しておく
    Enemy.parent = new TokenMgr<Enemy>("Enemy", 64);

    // プレイヤーの参照を敵に登録する
    Enemy.target = GameObject.Find("Player").GetComponent<Player>();
  }

  /// 更新
  void Update()
  {
    switch (_state)
    {
      case eState.Init:
        // BGM再生開始
        Sound.PlayBgm("bgm");
        // メイン状態へ遷移する
        _state = eState.Main;
        break;
      case eState.Main:
        if (Boss.bDestroyed)
        {
          // ボスを倒したのでゲームクリア
          // BGMを止める
          Sound.StopBgm();
          _state = eState.GameClear;
        }
        else if (Enemy.target.Exists == false)
        {
          // プレイヤーが死亡したのでゲームオーバー
          _state = eState.GameOver;
        }
        break;
      case eState.GameClear:
        if (Input.GetKeyDown(KeyCode.Space))
        {
          // タイトルへ戻る
          Application.LoadLevel("Title");
        }
        break;
      case eState.GameOver:
        if (Input.GetKeyDown(KeyCode.Space))
        {
          // ゲームをやり直す
          Application.LoadLevel("Main");
        }
        break;
    }
  }

  /// ラベルを画面中央に表示
  void DrawLabelCenter(string message)
  {
    // フォントサイズ設定
    Util.SetFontSize(32);
    // 中央揃え
    Util.SetFontAlignment(TextAnchor.MiddleCenter);

    // フォントの位置
    float w = 128; // 幅
    float h = 32; // 高さ
    float px = Screen.width / 2 - w / 2;
    float py = Screen.height / 2 - h / 2;

    // フォント描画
    Util.GUILabel(px, py, w, h, message);
  }

  void OnGUI()
  {
    switch (_state)
    {
      case eState.GameClear:
        DrawLabelCenter("GAME CLEAR!");
        break;
      case eState.GameOver:
        DrawLabelCenter("GAME OVER");
        break;
    }
  }

  /// 破棄
  void OnDestroy()
  {
    // TokenMgrの参照を消す
    Shot.parent = null;
    Enemy.parent = null;
    Bullet.parent = null;
    Particle.parent = null;

    Enemy.target = null;
  }

}
