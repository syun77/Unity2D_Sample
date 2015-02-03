using UnityEngine;
using System.Collections;

/// ショット
public class Shot : Token
{
  /// 親オブジェクト
  public static TokenMgr<Shot> parent = null;
  /// インスタンスの取得
  public static Shot Add(float x, float y, float direction, float speed)
  {
    return parent.Add(x, y, direction, speed);
  }

  /// 更新
  void Update()
  {
    if (IsOutside())
    {
      // 画面外に出たので消す
      //DestroyObj();
      Vanish();
    }
  }

  /// 消滅
  public override void Vanish()
  {
    // パーティクル生成
    Particle p = Particle.Add(X, Y);
    if (p != null)
    {
      // 青色にする
      p.SetColor(0.1f, 0.1f, 1);
      // 速度を少し遅くする
      p.MulVelocity(0.7f);
    }
    base.Vanish();
  }
}