using UnityEngine;
using System.Collections;

/// 移動床
public class FloorMove : Token
{
  public static TokenMgr<FloorMove> parent = null;
  public static FloorMove Add(float x, float y) {
    if(parent == null) {
      parent = new TokenMgr<FloorMove>("FloorMove", 32);
    }
    FloorMove floor = parent.Add(x, y);
    floor.SetVelocityXY (0.5f, 0);

    return floor;
  }
  
  // 前回の更新時のX座標
  float _xprevious = 0;
  // プレイヤーの参照
  Player _target = null;

  void Update ()
  {
    // 前回の座標からの差分を求める
    float dx = X - _xprevious;
    if (_target != null) {
      // 上にプレイヤーが乗っていたら動かす
      _target.X += dx;
    }
    // 現在の座標を次に使うように保存
    _xprevious = X;
  }
  void OnTriggerEnter2D (Collider2D other)
  {
    string name = LayerMask.LayerToName (other.gameObject.layer);
    if (name == "Ground") {
      // 他の床と当たったら反転
      VX *= -1;
      // 反対方向に少しだけ移動させる
      X += VX * Time.deltaTime;
    } else if (name == "Player") {
      // プレイヤーに当たったので参照を保持
      _target = other.gameObject.GetComponent<Player> ();
    }
  }
  
  void OnTriggerExit2D (Collider2D other)
  {
    string name = LayerMask.LayerToName (other.gameObject.layer);
    if (name == "Player") {
      // プレイヤーがいなくなったので参照を消す
      _target = null;
    }
  }

  public override void Vanish() {
    _target = null;
    base.Vanish();
  }

}
