using UnityEngine;
using System.Collections;

/// 敵
public class Enemy : Token
{
  /// スプライト
  public Sprite Spr0;
  public Sprite Spr1;
  public Sprite Spr2;
  public Sprite Spr3;
  public Sprite Spr4;
  public Sprite Spr5;

  /// 敵管理
  public static TokenMgr<Enemy> parent = null;
  
  /// 敵の追加
  public static Enemy Add(int id, float x, float y, float direction, float speed)
  {
    Enemy e = parent.Add(x, y, direction, speed);
    if(e == null)
    {
      return null;
    }
    
    // 初期パラメータ設定
    e.SetParam(id);
    return e;
  }

  /// 狙い撃ちするターゲット
  public static Player target = null;
  /// 狙い撃ち角度を取得する
  public float GetAim()
  {
    float dx = target.X - X;
    float dy = target.Y - Y;
    return Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
  }

  /// 敵のID
  int _id = 0;

  /// HP
  int _hp = 0;
  /// HPの取得
  public int Hp
  {
    get { return _hp; }
  }

  /// IDからパラメータを設定
  public void SetParam(int id)
  {
    if (_id != 0)
    {
      // 前回のコルーチンを終了する
      StopCoroutine("_Update" + _id);
    }
    if (id != 0)
    {
      // コルーチンを新しく開始する
      StartCoroutine("_Update" + id);
    }

    // IDを設定
    _id = id;

    //            0 ,  1   2   3   4   5
    // HPテーブル
    int[] hps = { 500, 30, 30, 30, 30, 30 };
    // スプライトテーブル
    Sprite[] sprs = { Spr0, Spr1, Spr2, Spr3, Spr4, Spr5 };

    // HPを設定
    _hp = hps[id];

    // スプライトを設定
    SetSprite(sprs[id]);

    // サイズ変更
    Scale = 0.5f;
  }

  /// ダメージを与える
  bool Damage(int v)
  {
    _hp -= v;
    if (_hp <= 0)
    {
      // HPがなくなったので死亡
      Vanish();
      // 倒した
      for (int i = 0; i < 4; i++)
      {
        Particle.Add(X, Y);
      }
      // 破壊SE再生
      Sound.PlaySe("destroy", 0);
      
      // ボスを倒したらザコ敵と敵弾を消す
      if (_id == 0)
      {
        // 生存しているザコ敵を消す
        Enemy.parent.ForEachExist(e => e.Damage(9999));

        // 敵弾をすべて消す
        if (Bullet.parent != null)
        {
          Bullet.parent.Vanish();
        }
      }

      return true;
    }

    // まだ生きている
    return false;
  }
  
  /// 弾を発射する
  void DoBullet(float direction, float speed)
  {
    Bullet.Add(X, Y, direction, speed);
  }

  /// 更新
  void Update()
  {
    if (_id == 4)
    {
      // だいこんのみ
      Vector2 min = GetWorldMin();
      Vector2 max = GetWorldMax();

      if (Y < min.y || max.y < Y)
      {
        // 上下ではみ出したら跳ね返るようにする
        ClampScreen();
        // 移動速度を反転
        VY *= -1;
      }
      if (X < min.x || max.x < X)
      {
        // 左右ではみ出したら消滅する
        Vanish();
      }

      // 移動方向を向くようにする
      Angle = Direction;
    }
  }

  /// 固定フレームで更新
  void FixedUpdate()
  {
    if (_id <= 3)
    {
      // 通常の敵だけ移動速度を減衰する
      MulVelocity(0.93f);
    }
  }

  /// 更新
  IEnumerator _Update1()
  {
    while (true)
    {
      // 2秒おきに弾を撃つ
      yield return new WaitForSeconds(2.0f);
      // 狙い撃ち角度を取得
      float dir = GetAim();
      Bullet.Add(X, Y, dir, 2);
    }
  }

  IEnumerator _Update2()
  {
    // 発射角度を回転しながら撃つ
    yield return new WaitForSeconds(2.0f);
    float dir = 0;
    while (true)
    {
      Bullet.Add(X, Y, dir, 2);
      dir += 16;
      yield return new WaitForSeconds(0.1f);
    }
  }

  IEnumerator _Update3()
  {
    // 3Way弾を撃つ
    while (true)
    {
      // 2秒おきに弾を撃つ
      yield return new WaitForSeconds(2.0f);
      DoBullet(180 - 2, 2);
      DoBullet(180, 2);
      DoBullet(180 + 2, 2);
    }
  }

  IEnumerator _Update4()
  {
    // 何もしない
    yield return new WaitForSeconds(1.0f);
  }

  IEnumerator _Update5()
  {
    // 1回の更新で旋回する角度
    const float ROT = 5.0f;
    // ホーミングする
    while (true)
    {
      // 0.02秒おきに更新する
      yield return new WaitForSeconds(0.02f);
      // 現在の角度
      float dir = Direction;
      // 狙い撃ち角度
      float aim = GetAim();
      // 角度差を求める
      float delta = Mathf.DeltaAngle(dir, aim);
      if (Mathf.Abs(delta) < ROT)
      {
        // 角度差が小さいので回転不要
      }
      else if (delta > 0)
      {
        // 左回り
        dir += ROT;
      }
      else
      {
        // 右回り
        dir -= ROT;
      }
      SetVelocity(dir, Speed);

      // 画像も回転させる
      Angle = dir;

      // 画面外に出たら消える
      if (IsOutside())
      {
        Vanish();
      }
    }
  }



  /// 衝突判定
  void OnTriggerEnter2D(Collider2D other)
  {
    // Layer名を取得する
    string name = LayerMask.LayerToName(other.gameObject.layer);

    if (name == "Shot")
    {
      // ショットであれば当たりとする
      Shot s = other.GetComponent<Shot>();
      // ショットを消す
      s.Vanish();
      // ダメージ処理
      Damage(1);
    }
  }
}
