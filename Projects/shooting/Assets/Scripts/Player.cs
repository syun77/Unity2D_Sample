using UnityEngine;
using System.Collections;

/// プレイヤー
public class Player : Token {

	public Sprite Spr0; // 待機画像1
	public Sprite Spr1; // 待機画像2

	/// アニメーションタイマー
	int _tAnim = 0;
	void Start () {
		// 画面からはみ出ないようにする
		var w = SpriteWidth / 2;
		var h = SpriteHeight / 2;
		SetSize(w, h);
	}

	/// 更新
	void Update () {
		// 移動処理
		Vector2 v = Util.GetInputVector();
		float speed = 5.0f * Time.deltaTime;
		ClampScreenAndMove(v * speed);

		// Spaceキーでショットを撃つ
		if(Input.GetKey(KeyCode.Space)) {
			// X座標をランダムでずらす
			float px = X + Random.Range(0, SpriteWidth/2);
			// 移動角度を±3する
			float direction = Random.Range(-3.0f, 3.0f);
			Shot.Add(px, Y, direction, 10);
		}
	}

	/// 固定フレームでの更新
	void FixedUpdate() {
		_tAnim++;
		if(_tAnim%48 < 24) {
			SetSprite(Spr0);
		}
		else {
			SetSprite(Spr1);
		}
	}

	/// 衝突判定
	void OnTriggerEnter2D(Collider2D other) {
		string name = LayerMask.LayerToName(other.gameObject.layer);
		switch(name) {
		case "Enemy":
		case "Bullet":
			// ゲームオーバー
			DestroyObj();
      // 敵からの参照を消す
      Enemy.ClearTarget();
      // やられSE再生
      Sound.PlaySe("damage");
      // BGMを止める
      Sound.StopBgm();
			for(int i = 0; i < 8; i++) {
				Particle.Add(X, Y);
			}
			break;
		}
	}
}
