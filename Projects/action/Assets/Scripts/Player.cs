using UnityEngine;
using System.Collections;

/// プレイヤー
public class Player : Token
{
  /// ゲーム状態
  public enum eGameState
  {
    None, // なし
    StageClear, // ステージクリア
    GameOver, // ゲームオーバー
  }
  eGameState _gameState = eGameState.None;
  /// ゲーム状態を設定する
  public void SetGameState(eGameState s)
  {
    _gameState = s;
  }
  /// ゲーム状態を取得する
  public eGameState GetGameState()
  {
    return _gameState;
  }

  /// 左を向いているかどうか
  bool _bFacingLeft = false;

  // 状態
  enum eState
  {
    Idle, // 待機
    Run,  // 走り状態
    Jump, // ジャンプ
  }
  /// 状態
  eState _state = eState.Idle;
  /// アニメーションタイマー
  int _tAnim = 0;

  /// ■各種スプライト
  /// 待機状態
  public Sprite Sprite0;
  /// 待機状態（まばたき）
  public Sprite Sprite1;
  /// 走り１
  public Sprite Sprite2;
  /// 走り２
  public Sprite Sprite3;

  /// 走る速さ
  [SerializeField]
  float _RunSpeed = 2;
  /// ジャンプの速さ（力）
  [SerializeField]
  float _JumpSpeed = 4;

  /// 地面に着地しているかどうか
  bool _bGround = false;

  /// 更新
  void Update()
  {
    // 左右キーで移動する
    Vector2 v = Util.GetInputVector();
    VX = v.x * _RunSpeed;

    // 向いている方向チェック
    if (VX <= -1.0f)
    {
      // 左を向く
      _bFacingLeft = true;
    }
    if (VX >= 1.0f)
    {
      // 右を向く
      _bFacingLeft = false;
    }
    
    // 着地チェック
    _bGround = CheckGround();

    // ジャンプ判定
    if (Input.GetKeyDown(KeyCode.Space))
    {
      if (_bGround)
      {
        // 着地できているのでジャンプできる
        VY = _JumpSpeed;
      }
    }
  }

  /// 地面に着地しているかどうか
  bool CheckGround()
  {
    // Groundグループのみチェックする
    int mask = 1 << LayerMask.NameToLayer("Ground");
    // キャラクタの半分よりちょっと下までレイを飛ばす
    float distance = SpriteHeight * 0.6f;
    // キャラクターの横半分よりちょっと大きめのサイズを取得する
    float width = BoxColliderWidth * 0.6f;
    float[] xList = { X - width, X, X + width };
    foreach (float px in xList)
    {
      // チェック実行
      RaycastHit2D hit = Physics2D.Raycast(new Vector2(px, Y), -Vector2.up, distance, mask);
      if (hit.collider != null)
      {
        // 着地できた
        return true;
      }
    }

    // 着地できてない
    return false;
  }

  /// 固定フレームで更新
  void FixedUpdate()
  {
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
    if (_bGround == false)
    {
      // 空中にいるのでジャンプ状態
      _state = eState.Jump;
    }
    else if (Mathf.Abs(VX) >= 1.0f)
    {
      // 移動しているので走り状態
      _state = eState.Run;
    }
    else
    {
      // 待機状態
      _state = eState.Idle;
    }

    // アニメーション更新
    switch (_state)
    {
      case eState.Idle:
        if (_tAnim % 200 < 10)
        {
          // たまにまばたきする
          SetSprite(Sprite1);
        }
        else
        {
          SetSprite(Sprite0);
        }
        break;

      case eState.Run:
        // 走り状態
        if (_tAnim % 12 < 6)
        {
          SetSprite(Sprite2);
        }
        else
        {
          SetSprite(Sprite3);
        }
        break;

      case eState.Jump:
        // ジャンプ中
        SetSprite(Sprite2);
        break;
    }
  }
  
  /// 消滅
  public override void Vanish()
  {
    // パーティクル生成
    for (int i = 0; i < 32; i++)
    {
      Particle.Add(X, Y);
    }
    base.Vanish();
  }
}
