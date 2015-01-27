using UnityEngine;
using System.Collections;

/// <summary>
/// パーティクル
/// </summary>
public class Particle : Token {

	/// プレハブからインスタンスを生成
	static GameObject _prefab = null;
	public static Particle Add(float x, float y) {
		// プレハブを取得
		_prefab = GetPrefab(_prefab, "Particle");
		// プレハブからインスタンスを生成
		return CreateInstance2<Particle>(_prefab, x, y);
	}

	IEnumerator Start () {
		SetVelocity(Random.Range(0, 359), Random.Range(2.0f, 4.0f));

		// 見えなくなるまで小さくする
		while(ScaleX > 0.01f) {
			yield return new WaitForSeconds(0.1f);
			// だんだん小さくする
			MulScale(0.7f);
			MulVelocity(0.5f);
		}

		// 消滅
		DestroyObj();
	}
	
	// Update is called once per frame
	void Update () {
	}
}
