using UnityEngine;
using System.Collections;

/// ゲーム管理
public class GameMgr : MonoBehaviour
{
  // 状態
  enum eState
  {
    Main,
    // メイン
    GameClear,
    // ゲームクリア
    GameOver,
    // ゲームオーバー
  }

  eState _state = eState.Main;

  void Start ()
  {
    // BGMの再生
    Sound.PlayBgm ("bgm");

    Enemy.SetTarget ();
  }

  void Update ()
  {
    switch (_state) {
    case eState.Main:
      if (Enemy.parent.Count () == 0) {
        // 敵が全滅したのでゲームクリア
        _state = eState.GameClear;
      } else if (Enemy.ExistsTarget () == false) {
        // ゲームオーバー
        _state = eState.GameOver;
      }
      break;
    case eState.GameClear:
      if(Input.GetKeyDown(KeyCode.Space)) {
        // タイトルへ戻る
        Application.LoadLevel("Title");
      }
      break;
    case eState.GameOver:
      if(Input.GetKeyDown(KeyCode.Space)) {
        // ゲームをやり直す
        Application.LoadLevel("Main");
      }
      break;
    }
  }

  void OnDestroy() {
    // TokenMgrを参照を消す
    Shot.parent = null;
    Enemy.parent = null;
    Bullet.parent = null;
    Particle.parent = null;
  }

  void OnGUI ()
  {
    switch (_state) {
    case eState.GameClear:
      Util.GUILabel (160, 160, 128, 32, "GAME CLEAR!");
      Util.GUILabel (160, 240, 128, 32, "スペースキーでタイトルへ戻る");
      break;
    case eState.GameOver:
      Util.GUILabel (160, 160, 128, 32, "GAME OVER");
      Util.GUILabel (160, 240, 128, 32, "スペースキーでやり直す");
      break;
    }
  }
}
