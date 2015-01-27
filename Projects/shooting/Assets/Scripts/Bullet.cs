using UnityEngine;
using System.Collections;

/// 敵弾
public class Bullet : Token {

	public static TokenMgr<Bullet> parent = null;
	public static Bullet Add(float x, float y, float direction, float speed) {
		if(parent == null) {
			parent = new TokenMgr<Bullet>("Bullet", 256);
		}
		return parent.Add(x, y, direction, speed);
	}

	/// 更新
	void Update () {
		if(IsOutside()) {
			// 画面外に出たら消える
			Vanish();
		}
	}
}
