using UnityEngine;
using System.Collections;

/// トゲ
public class Spike : Token {

  public static TokenMgr<Spike> parent = null;
  public static Spike Add(float x, float y) {
    if(parent == null) {
      parent = new TokenMgr<Spike>("Spike", 32);
    }
    return parent.Add(x, y);
  }
  
  void Start () {
  }
  
  void Update () {
    // 回転する
    Angle += 90 * Time.deltaTime;
  }

  void OnTriggerEnter2D(Collider2D other) {
    string name = LayerMask.LayerToName(other.gameObject.layer);
    if(name == "Player") {
      // プレイヤーが衝突
      // ゲームオーバー
      GameMgr.GetInstance().GameOver();
      Player p = other.gameObject.GetComponent<Player>();
      for(int i = 0; i < 32; i++) {
        Particle.Add(p.X, p.Y);
      }
      // プレイヤー消滅
      p.Vanish();
    }
  }
}
