using UnityEngine;
using System.Collections;

/// タイトル画面管理
public class TitleMgr : MonoBehaviour
{
  /// Press to startの表示フラグ
  bool _bDrawPressStart = false;

  IEnumerator Start()
  {
    while (true)
    {
      // 0.6秒で点滅する
      _bDrawPressStart = !_bDrawPressStart;
      yield return new WaitForSeconds(0.6f);
    }
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.Space))
    {
      // Spaceキーを押したらゲーム開始
      Application.LoadLevel("Main");
    }
  }

  void OnGUI()
  {
    if (_bDrawPressStart)
    {
      // ゲーム開始メッセージの描画
      // フォントサイズ設定
      Util.SetFontSize(32);
      // 中央揃え
      Util.SetFontAlignment(TextAnchor.MiddleCenter);

      // フォントの位置
      float w = 128; // 幅
      float h = 32; // 高さ
      float px = Screen.width / 2 - w / 2;
      float py = Screen.height / 2 - h / 2;

      // 少し下にずらす
      py += 65;

      // フォント描画
      Util.GUILabel(px, py, w, h, "スペースキーでゲーム開始");
    }
  }
}
