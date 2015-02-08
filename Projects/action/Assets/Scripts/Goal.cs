using UnityEngine;
using System.Collections;

/// ゴール
public class Goal : Token
{
  /// ゴールスプライト
  public Sprite Spr0;
  public Sprite Spr1;
  public Sprite Spr2;
  public Sprite Spr3;
  /// アニメーションタイマー
  int _tAnim = 0;

  /// 固定フレームで更新
  void FixedUpdate()
  {
    // アニメーションタイマー更新
    _tAnim++;
    // スプライトテーブル
    Sprite[] sprTbl = { Spr0, Spr1, Spr2, Spr3 };
    // アニメ更新間隔
    const int INTERVAL = 16;
    int SIZE = sprTbl.Length;
    // アニメ番号計算
    int idx = (_tAnim % (INTERVAL * SIZE)) / INTERVAL;
    // スプライトを設定
    SetSprite(sprTbl[idx]);
  }

  /// ステージクリアチェック
  public void OnTriggerEnter2D(Collider2D other)
  {
    // レイヤー名を取得
    string name = LayerMask.LayerToName(other.gameObject.layer);
    if (name == "Player")
    {
      // ステージクリア
      Player p = other.gameObject.GetComponent<Player>();
      p.SetGameState(Player.eGameState.StageClear);
      // プレイヤー消滅
      p.Vanish();
    }
  }
}
