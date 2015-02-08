using UnityEngine;
using System.Collections;

/// 壁
public class Wall : Token
{
  public static TokenMgr<Wall> parent = null;
  /// 壁を作る
  public static Wall Add(float x, float y)
  {
    Wall w = parent.Add(x, y);
    return w;
  }
}