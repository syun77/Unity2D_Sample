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
  /// 管理オブジェクト
  public static TokenMgr<Enemy> parent = null;

  public static Enemy Add (int id, float x, float y, float direction, float speed)
  {
    // 管理オブジェクトを生成
    if (parent == null) {
      parent = new TokenMgr<Enemy> ("Enemy", 64);
    }
    Enemy e = parent.Add (x, y, direction, speed);
    // IDを設定する
    e.SetParam (id);
    return e;
  }

  /// 狙い撃ちするターゲット
  static Player _target = null;

  Player _Target {
    get { return _target; }
  }

  /// 狙い撃ち角度を取得する
  static float _targetX = 0.0f;
  // ターゲット座標(X)
  static float _targetY = 0.0f;
  // ターゲット座標(Y)
  public float GetAim ()
  {
    if (_Target) {
      // 存在していれば座標を取得する
      _targetX = _Target.X;
      _targetY = _Target.Y;
    }
    float dx = _targetX - X;
    float dy = _targetY - Y;
    return Mathf.Atan2 (dy, dx) * Mathf.Rad2Deg;
  }

  /// ターゲットの消去
  public static void ClearTarget ()
  {
    _target = null;
  }

  /// ターゲットが存在するかどうか
  public static bool ExistsTarget ()
  {
    return _target != null;
  }

  /// ターゲットの設定
  public static void SetTarget ()
  {
    _target = GameObject.Find ("Player").GetComponent<Player> ();
  }

  /// HP
  int _hp = 0;

  /// HPの取得
  public int Hp {
    get { return _hp; }
  }

  /// ダメージを与える
  bool Damage (int v)
  {
    _hp -= v;
    if (_hp <= 0) {
      // HPがなくなったので死亡
      Vanish ();
      // 破壊SE再生
      Sound.PlaySe ("destroy");
      // 倒した
      for (int i = 0; i < 4; i++) {
        Particle.Add (X, Y);
      }
      // ボスを倒したらザコ敵と敵弾を消す
      if (_id == 0) {
        // 生存しているザコ敵を消す
        Enemy.parent.ForEachExist (e => e.Damage (9999));

        // 敵弾をすべて消す
        if (Bullet.parent != null) {
          Bullet.parent.Vanish ();
        }
      }
      return true;
    }

    // まだ生きている
    return false;
  }

  /// ID
  int _id = 0;

  /// 開始
  void Start ()
  {
    // 何もしない
  }

  /// IDからパラメータを設定
  public void SetParam (int id)
  {

    if (_id != 0) {
      // 前回のコルーチンを終了する
      StopCoroutine ("_Update" + _id);
    }
    if (id != 0) {
      // コルーチンを新しく開始する
      StartCoroutine ("_Update" + id);
    }

    // IDを設定
    _id = id;

    //                0 ,   1      2    3     4     5
    int[] hps = { 5,  30,   30,   30,   30,   30 }; // HP
    float[] scales = { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f }; // スケール値
    Sprite[] sprs = { Spr0, Spr1, Spr2, Spr3, Spr4, Spr5 }; // スプライト

    // HPを設定
    _hp = hps [id];
    // スケール値を設定
    Scale = scales [id];
    // スプライトを設定
    SetSprite (sprs [id]);
  }

  /// 更新
  void Update ()
  {
    if (_id == 4) {
      // だいこんのみ
      Vector2 min = GetWorldMin ();
      Vector2 max = GetWorldMax ();

      if (Y < min.y || max.y < Y) {
        // 上下ではみ出したら跳ね返るようにする
        ClampScreen ();
        // 移動速度を反転
        VY *= -1;
      }
      if (X < min.x || max.x < X) {
        // 左右ではみ出したら消滅する
        Vanish ();
      }

      // 移動方向を向くようにする
      Angle = Direction;
    }
  }

  IEnumerator _Update1 ()
  {
    while (true) {
      // 2秒おきに弾を撃つ
      yield return new WaitForSeconds (2.0f);
      // 狙い撃ち角度を取得
      float dir = GetAim ();
      DoBullet (dir, 2);
    }
  }

  IEnumerator _Update2 ()
  {
    // 発射角度を回転しながら撃つ
    yield return new WaitForSeconds (2.0f);
    float dir = 0;
    while (true) {
      DoBullet (dir, 2);
      dir += 16;
      yield return new WaitForSeconds (0.1f);
    }
  }

  IEnumerator _Update3 ()
  {
    // 3Way弾を撃つ
    while (true) {
      // 2秒おきに弾を撃つ
      yield return new WaitForSeconds (2.0f);
      DoBullet (180 - 2, 2);
      DoBullet (180, 2);
      DoBullet (180 + 2, 2);
    }
  }

  IEnumerator _Update4 ()
  {
    yield return new WaitForSeconds (1);
  }

  IEnumerator _Update5 ()
  {
    // 1回の更新で旋回する角度
    const float ROT = 5.0f;
    // ホーミングする
    while (true) {
      // 0.2秒おきに更新する
      yield return new WaitForSeconds (0.02f);
      // 現在の角度
      float dir = Direction;
      // 狙い撃ち角度
      float aim = GetAim ();
      // 角度差を求める
      float delta = Mathf.DeltaAngle (dir, aim);
      if (Mathf.Abs (delta) < ROT) {
        // 角度差が小さいので回転不要
      } else if (delta > 0) {
        // 左回り
        dir += ROT;
      } else {
        // 右回り
        dir -= ROT;
      }
      SetVelocity (dir, Speed);

      // 画像も回転させる
      Angle = dir;

      // 画面外に出たら消える
      if (IsOutside ()) {
        Vanish ();
      }
    }
  }

  /// 固定フレームで更新
  void FixedUpdate ()
  {
    if (_id <= 3) {
      // 通常の敵だけ移動速度を減衰する
      MulVelocity (0.93f);
    }
  }

  /// 弾を発車する
  void DoBullet (float direction, float speed)
  {
    Bullet.Add (X, Y, direction, speed);
  }

  /// 衝突判定
  void OnTriggerEnter2D (Collider2D other)
  {
    // Layer名を取得する
    string name = LayerMask.LayerToName (other.gameObject.layer);

    if (name == "Shot") {
      // ショットであれば当たりとする
      Shot s = other.GetComponent<Shot> ();
      // ダメージ処理
      Damage (1);
      // ショットを消す
      s.Vanish ();
      // パーティクル生成
      Particle p = Particle.Add (s.X, s.Y);
      if (p) {
        // 青色にする
        p.SetColor (0.1f, 0.1f, 1);
        // 速度をすこし遅くする
        p.MulVelocity (0.7f);
      }
    }
  }
}
