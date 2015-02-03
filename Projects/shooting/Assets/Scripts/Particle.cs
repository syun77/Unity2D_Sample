using UnityEngine;
using System.Collections;

/// パーティクル
public class Particle : Token
{
  /// パーティクル管理
  public static TokenMgr<Particle> parent = null;
  /// パーティクルのインスタンスを取得する
  public static Particle Add(float x, float y)
  {
    Particle p = parent.Add(x, y);
    if (p)
    {
      // ランダムに移動する
      p.SetVelocity(Random.Range(0, 359), Random.Range(2.0f, 4.0f));
      // 初期のサイズを設定
      p.SetScale(0.25f, 0.25f);
    }

    return p;
  }

  /// 更新
  void Update()
  {
    MulVelocity(0.95f);
    MulScale(0.97f);
    if (Scale < 0.01f)
    {
      // 見えなくなったら消す
      Vanish();
    }
  }
}