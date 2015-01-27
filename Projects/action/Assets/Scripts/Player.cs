using UnityEngine;
using System.Collections;

public class Player : Token
{
  // 状態
  enum eState {
    Idle, // 待機
    Run,  // 走り状態
    Jump, // ジャンプ
  }
  /// 状態
  eState _state = eState.Idle;
  /// アニメーションタイマー
  int _tAnim = 0;

  /// 走る速さ
  public float RunSpeed = 2;
  /// ジャンプの速さ
  public float JumpSpeed = 4;
  /// ■各種スプライト
  /// 待機状態
  public Sprite Sprite0;
  /// 待機状態（まばたき）
  public Sprite Sprite1;
  /// 走り１
  public Sprite Sprite2;
  /// 走り２
  public Sprite Sprite3;

  /// 地面に着地しているかどうか
  bool _bGround = false;

  /// 左を向いているかどうか
  bool _bFacingLeft = false;

  void Start ()
  {
  }

  void Update ()
  {
    // 左右キーで移動する
    Vector2 v = Util.GetInputVector ();
    VX = v.x * RunSpeed;
    if(VX <= -1.0f) {
      // 左を向く
      _bFacingLeft = true;
    }
    if(VX >= 1.0f) {
      // 右を向く
      _bFacingLeft = false;
    }

    // 着地チェック
    _bGround = CheckGround();

    // ジャンプ判定
    if(Input.GetKeyDown(KeyCode.Space)) {
      if(_bGround) {
        // 着地できているのでジャンプできる
        VY = JumpSpeed;
      }
    }
  }

  void FixedUpdate() {
    // 左右の向きを切り替える
    if(_bFacingLeft) {
      // 左向き
      ScaleX = -1.0f;
    }
    else {
      // 右向き
      ScaleX = 1.0f;
    }

    // アニメーションタイマーを更新
    _tAnim++;
    // 状態更新
    if(_bGround == false) {
      // 空中にいるのでジャンプ状態
      _state = eState.Jump;
    }
    else if(Mathf.Abs(VX) >= 1.0f) {
      // 移動しているので走り状態
      _state = eState.Run;
    }
    else {
      // 待機状態
      _state = eState.Idle;
    }

    // アニメーション更新
    switch(_state) {
    case eState.Idle:
      if(_tAnim%200 < 10) {
        // たまにまばたきする
        SetSprite(Sprite1);
      }
      else {
        SetSprite(Sprite0);
      }
      break;

    case eState.Run:
      // 走り状態
      if(_tAnim%12 < 6) {
        SetSprite(Sprite2);
      }
      else {
        SetSprite(Sprite3);
      }
      break;

    case eState.Jump:
      // ジャンプ中
      SetSprite(Sprite2);
      break;
    }
  }

  /// 地面に着地しているかどうか
  bool CheckGround() {
    // Groundグループのみチェックする
    int mask = 1 << LayerMask.NameToLayer("Ground");
    // キャラクタの半分よりちょっと下までレイを飛ばす
    float distance = SpriteHeight * 0.6f;
    // キャラクターの横半分よりちょっと大きめのサイズを取得する
    float width = BoxColliderWidth * 0.6f;
    float[] xList = {X-width, X, X+width};
    foreach(float px in xList) {
      // チェック実行
      RaycastHit2D hit = Physics2D.Raycast(new Vector2(px, Y), -Vector2.up, distance, mask);
      if(hit.collider != null) {
        // 着地できた
        return true;
      }
    }

    // 着地できてない
    return false;
  }
}
