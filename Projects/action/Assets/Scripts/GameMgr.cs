using UnityEngine;
using System.Collections;

/// ゲーム管理
public class GameMgr : MonoBehaviour
{
  /// 現在のステージ番号
  int nStage = 1;

  /// プレイヤーへの参照
  Player _player = null;
  /// プレイヤーのゲーム状態をチェックする
  void CheckPlayerGameState()
  {
    if (_player == null)
    {
      // プレイヤーへの参照を取得する
      GameObject obj = GameObject.Find("Player") as GameObject;
      _player = obj.GetComponent<Player>();
    }
    switch (_player.GetGameState())
    {
      case Player.eGameState.StageClear:
        // ステージクリア
        _state = eState.StageClear;
        break;
      case Player.eGameState.GameOver:
        // ゲームオーバー
        _state = eState.GameOver;
        break;
    }
  }

  /// 更新
  void Update()
  {
    switch (_state)
    {
      case eState.Main:
        // プレイヤーのゲーム状態をチェックする
        CheckPlayerGameState();
        break;
      case eState.StageClear:
        // ステージクリア
        if (Input.GetKeyDown(KeyCode.Space))
        {
          // Spaceキーを押したら次に進む
          Restore();
          // 次のステージに進む
          nStage++;
          if (nStage > 3)
          {
            // 全ステージクリア
            // ステージ1に戻る
            nStage = 1;
          }
          // マップデータ読み込み
          Load();
          _state = eState.Main;
        }
        break;
      case eState.GameOver:
        // ゲームオーバー
        if (Input.GetKeyDown(KeyCode.Space))
        {
          // Spaceキーを押したらやり直し
          Restore();
          // マップデータ読み込み
          Load();
          _state = eState.Main;
        }
        break;
    }
  }

  // 状態
  enum eState
  {
    Main, // メイン
    StageClear, // ステージクリア
    GameOver, // ゲームオーバー
  }

  /// 状態
  eState _state = eState.Main;

  /// マップをロードする
  void Load()
  {
    // マップデータの読み込み
    FieldMgr field = new FieldMgr();
    field.Load(nStage);
  }

  /// 開始
  void Start()
  {
    // 壁オブジェクト管理生成
    Wall.parent = new TokenMgr<Wall>("Wall", 128);
    // 移動床オブジェクト管理作成
    FloorMove.parent = new TokenMgr<FloorMove>("FloorMove", 32);
    // パーティクル管理生成
    Particle.parent = new TokenMgr<Particle>("Particle", 32);
    // トゲ管理生成
    Spike.parent = new TokenMgr<Spike>("Spike", 32);

    // マップデータの読み込み
    //FieldMgr field = new FieldMgr();
    //field.Load(nStage);
    Load();
  }
  
  // 各種オブジェクトを全部消す
  void Restore()
  {
    Wall.parent.Vanish();
    FloorMove.parent.Vanish();
    Particle.parent.Vanish();
    Spike.parent.Vanish();
    
    // プレイヤーを復活させる
    _player.Revive();
    // 初期状態に戻す
    _player.SetGameState(Player.eGameState.None);
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
      case eState.StageClear:
        DrawLabelCenter("STAGE CLEAR!");
        break;
      case eState.GameOver:
        DrawLabelCenter("GAME OVER");
        break;
    }
  }
}