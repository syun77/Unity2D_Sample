using UnityEngine;
using System.Collections;

public class GameMgr : MonoBehaviour {
  
  // 状態
  enum eState {
    Main, // メイン
    StageClear, // ステージクリア
    GameOver, // ゲームオーバー
  }
  
  /// 状態
  eState _state = eState.Main;
  /// 現在のステージ数
  public int Stage = 1;
  /// プレイヤーの参照を保持しておく
  Player _player = null;

  /// インスタンスの取得
  public static GameMgr GetInstance() {
    GameObject obj = GameObject.Find ("GameMgr") as GameObject;
    return obj.GetComponent<GameMgr>();
  }
  
  /// ステージクリア
  public void StageClear() {
    _state = eState.StageClear;
  }
  
  /// ゲームオーバー
  public void GameOver() {
    _state = eState.GameOver;
  }

  void Start() {
    // 各種オブジェクトの管理クラスを生成
    FloorMove.parent = new TokenMgr<FloorMove>("FloorMove", 32);
    Particle.parent = new TokenMgr<Particle>("Particle", 32);
    Spike.parent = new TokenMgr<Spike>("Spike", 32);
    Wall.parent = new TokenMgr<Wall>("Wall", 128);
    GameObject obj = GameObject.Find("Player") as GameObject;
    // プレイヤーの参照を保持
    _player = obj.GetComponent<Player>();

    // マップデータ読み込み
    Load();
  }

  void Restore() {
    // 各種オブジェクトを全部消す
    FloorMove.parent.Vanish();
    Particle.parent.Vanish();
    Spike.parent.Vanish();
    Wall.parent.Vanish();
    // プレイヤーを復活させる
    _player.Revive();
  }

  void Load() {
    // マップデータ読み込み
    GameObject obj = GameObject.Find("FieldMgr");
    FieldMgr field = obj.GetComponent<FieldMgr>();
    field.Load(Stage);
  }

  void Update() {
    switch(_state) {
    case eState.StageClear:
      if(Input.GetKeyDown(KeyCode.Space)) {
        // Spaceキーを押したら次に進む
        Restore();
        // 次のステージに進む
        Stage++;
        if(Stage > 3) {
          // 全ステージクリア
          // ステージ1に戻る
          Stage = 1;
        }
        // マップデータ読み込み
        Load();
        _state = eState.Main;
        Update();
      }
      break;
    case eState.GameOver:
      if(Input.GetKeyDown(KeyCode.Space)) {
        // Spaceキーを押したらやり直し
        Restore();
        // マップデータ読み込み
        Load();
        _state = eState.Main;
        // マップデータ読み込み
      }
      break;
    }
  }
  
  /// テキスト表示
  void OnGUI() {
    string text = "";
    switch(_state) {
    case eState.StageClear:
      text = "STAGE CLEAR";
      break;
    case eState.GameOver:
      text = "GAME OVER";
      break;
    }
    Util.SetFontSize(36);
    Util.GUILabel(160, 160, 128, 32, text);
  }
}