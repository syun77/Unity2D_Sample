using UnityEngine;
using System.Collections;

/// サウンド管理
public class SoundMgr : MonoBehaviour
{
  /// 開始
  void Start()
  {
    // サウンドをロード
    // "bgm01"をロード。キーは"bgm"とする
    Sound.LoadBgm("bgm", "bgm01");
    // "damage"をロード。キーは"damage"とする
    Sound.LoadSe("damage", "damage");
    // "destroy"をロード。キーは"destroy"とする
    Sound.LoadSe("destroy", "destroy");
  }
}
