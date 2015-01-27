using UnityEngine;
using System.Collections;

public class Boss : Enemy {

	void Start () {
		// パラメータを設定
		SetParam(0);

		// 敵生成開始
		StartCoroutine("_GenerateEnemy");
	}
	
	void Update () {
	}

	/// 敵生成
	IEnumerator _GenerateEnemy() {
		while(true) {
			AddEnemy(1, 135, 5);
			AddEnemy(1, 225, 5);
			yield return new WaitForSeconds(3);
			BulletRadish(); // だいこんを発射
			yield return new WaitForSeconds(2);
			AddEnemy(2, 90, 5);
			AddEnemy(2, 270, 5);
			BulletCarrot(); // にんじんを発射
			yield return new WaitForSeconds(3);
			AddEnemy(3, 45, 5);
			AddEnemy(3, -45, 5);
			yield return new WaitForSeconds(3);
			BulletRadish(); // だいこんを発射
			yield return new WaitForSeconds(2);
			BulletCarrot(); // にんじんを発射
		}
	}
	/// だいこんを3方向に発射
	void BulletRadish() {
		// プレイヤーと±30度にだいこんを発射
		float aim = GetAim();
		AddEnemy(4, aim, 3);
		AddEnemy(4, aim-30, 3);
		AddEnemy(4, aim+30, 3);
	}
	/// にんじんを発射
	void BulletCarrot() {
		AddEnemy(5, 45, 3);
		AddEnemy(5, -45, 3);
	}

	/// 敵の生成
	Enemy AddEnemy(int id, float direction, float speed) {
		return Enemy.Add(id, X, Y, direction, speed);
	}

	/// HPの描画
	void OnGUI() {
		// テキストを黒にする
		Util.SetFontColor(Color.black);
		// テキストを大きめにする
		Util.SetFontSize(24);
		// テキスト描画
		string text = string.Format("{0,3}", Hp);
		Util.GUILabel(370, 220, 120, 30, text);
	}
}
