using UnityEngine;
using System.Collections;

/// タイトル画面管理
public class TitleMgr : MonoBehaviour
{
  /// Press to startの表示フラグ
  bool _bDrawPressStart = false;

  IEnumerator Start ()
  {
    while (true) {
      // 0.6秒で点滅する
      _bDrawPressStart = !_bDrawPressStart;
      yield return new WaitForSeconds (0.6f);
    }
  }

  void Update ()
  {
    if (Input.GetKeyDown (KeyCode.Space)) {
      // Spaceキーを押したらゲーム開始
      Application.LoadLevel ("Main");
    }
  }

  void OnGUI ()
  {
    if (_bDrawPressStart) {
      // press to startの描画
      Util.GUILabel (160, 240, 128, 32, "スペースキーでゲーム開始");
    }
  }
}
