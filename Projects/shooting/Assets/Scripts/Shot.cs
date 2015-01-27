using UnityEngine;
using System.Collections;

/// ショット
public class Shot : Token {

	/// 親オブジェクト
	public static TokenMgr<Shot> parent = null;
	/// インスタンスの取得
	public static Shot Add(float x, float y, float direction, float speed) {
		if(parent == null) {
			// 最大数を32とする
			parent = new TokenMgr<Shot>("Shot", 32);
		}
		return parent.Add(x, y, direction, speed);
	}

	void Start () {
	}

	/// 更新
	void Update () {
		if(IsOutside()) {
			// 画面外に出たので消す
			Vanish();
		}
	}
}
