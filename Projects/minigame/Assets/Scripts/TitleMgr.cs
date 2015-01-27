using UnityEngine;
using System.Collections;

public class TitleMgr : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnGUI() {
		// フォントサイズ設定
		GUIStyle style = new GUIStyle();
		style.fontSize = 32;
		// 中央揃え
		style.alignment = TextAnchor.MiddleCenter;
		// フォントの色を設定
		style.normal.textColor = Color.white;

		// ラベルの配置情報
		// ラベルの幅
		int w = 120;
		// ラベルの高さ
		int h = 30;
		Rect rect = new Rect();
		rect.x = Screen.width/2;  // 画面の中央(X)
		rect.y = Screen.height/2; // 画面の中央(Y)
		rect.x -= w/2;
		rect.y -= h/2;
		rect.width = w;
		rect.height = h;

		rect.y -= 30;

		// フォント描画
		GUI.Label(rect, "MINI GAME", style);

		rect.y += 60;
		if(GUI.Button(rect, "START")) {
			// ボタンを押した
			Application.LoadLevel("Main");
		}
	}
}
