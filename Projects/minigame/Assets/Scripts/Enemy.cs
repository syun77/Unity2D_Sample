using UnityEngine;
using System.Collections;

public class Enemy : Token {

	/// 生存数
	static int _count = 0;
	public static int Count {
		get { return _count; }
	}
	/// 開始
	void Start () {
		// 生存数を増やす
		_count++;

		// サイズを設定
		SetSize(0.3f, 0.3f);
		// ランダムな方向に移動する
		SetVelocity(Random.Range(0, 359), 2);
	}

	/// マウスクリックした
	void OnMouseDown() {
		// 生存数を減らす
		_count--;
		Destroy(gameObject);

		// パーティクルを32個生成
		for(int i = 0; i < 32; i++) {
			Particle.Add(X, Y);
		}
	}

	/// <summary>
	/// 更新
	/// </summary>
	void Update () {

		// 画面端で反転する
		Vector2 min = GetWorldMin();
		Vector2 max = GetWorldMax();

		if(X < min.x || max.x < X) {
			// 反転する
			VX *= -1;
			// 画面内に移動する
			ClampScreen();
		}
		if(Y < min.y || max.y < Y) {
			// 反転する
			VY *= -1;
			// 画面内に移動する
			ClampScreen();
		}
	}

}
