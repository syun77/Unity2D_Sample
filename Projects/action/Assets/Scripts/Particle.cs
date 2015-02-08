using UnityEngine;
using System.Collections;

/// パーティクル
public class Particle : Token
{

  public static TokenMgr<Particle> parent = null;
  public static Particle Add(float x, float y)
  {
    Particle p = parent.Add(x, y);
    // ランダムでサイズや飛ばす方向や速度を変えたりする
    p.Scale = Random.Range(0.5f, 1.0f);
    float dir = Random.Range(0, 359);
    float spd = Random.Range(3.0f, 6.0f);
    p.SetVelocity(dir, spd);
    // 生存フレーム数は40～60
    p.Timer = Random.Range(40, 60);
    return p;
  }

  // 消滅タイマー
  public int Timer;
  void FixedUpdate()
  {
    // 速度を減衰する
    MulVelocity(0.97f);
    // サイズを少しずつ小さくする
    MulScale(0.97f);
    Timer--;
    if (Timer < 1)
    {
      // 消滅
      Vanish();
    }
  }
}