using UnityEngine;
using System.Collections;

public class Wall : Token {
  public static TokenMgr<Wall> parent = null;
  /// 壁を作る
  public static Wall Add(float x, float y) {
    if(parent == null) {
      parent = new TokenMgr<Wall>("Wall", 128);
    }
    Wall w = parent.Add(x, y);
    return w;
  }
}
