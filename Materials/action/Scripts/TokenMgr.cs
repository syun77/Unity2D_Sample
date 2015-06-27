using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// Token管理クラス
public class TokenMgr<Type> where Type : Token {
	int _size = 0;
	GameObject _prefab = null;
	List<Type> _pool = null;
	/// Order in Layer
	int _order = 0;
  /// ForEach関数に渡す関数の型
  public delegate void FuncT(Type t);
	/// コンストラクタ
	/// プレハブは必ず"Resources/Prefabs/"に配置すること
	public TokenMgr(string prefabName, int size=0) {
		_size = size;
		_prefab = Resources.Load("Prefabs/"+prefabName) as GameObject;
    if(_prefab == null) {
      Debug.LogError("Not found prefab. name="+prefabName);
    }
		_pool = new List<Type>();

		if(size > 0) {
			// サイズ指定があれば固定アロケーションとする
			for(int i = 0; i < size; i++) {
				GameObject g = GameObject.Instantiate(_prefab, new Vector3(), Quaternion.identity) as GameObject;
				Type obj = g.GetComponent<Type>();
				if(obj == null) {
					Debug.LogError(prefabName + "にスクリプトが未設定です");
				}
				obj.VanishCannotOverride();
				_pool.Add(obj);
			}
		}
	}
	/// オブジェクトを再利用する
	Type _Recycle(Type obj, float x, float y, float direction, float speed) {
		// 復活
		obj.Revive();
		obj.SetPosition(x, y);
		if(obj.RigidBody != null) {
			// Rigidbody2Dがあるときのみ速度を設定する
			obj.SetVelocity(direction, speed);
		}
		obj.Angle = 0;
		// Order in Layerをインクリメントして設定する
		obj.SortingOrder = _order;
		_order++;
		return obj;
	}

	/// インスタンスを取得する
	public Type Add(float x, float y, float direction=0.0f, float speed=0.0f) {
		foreach(Type obj in _pool) {
			if(obj.Exists == false) {
				// 未使用のオブジェクトを見つけた
				return _Recycle(obj, x, y, direction, speed);
			}
		}

		if(_size == 0) {
			// 自動で拡張
			GameObject g = GameObject.Instantiate(_prefab, new Vector3(), Quaternion.identity) as GameObject;
			Type obj = g.GetComponent<Type>();
			_pool.Add(obj);
			return _Recycle(obj, x, y, direction, speed);
		}
		return null;
	}

  /// 生存するインスタンスに対してラムダ式を実行する
  public void ForEachExist(FuncT func) {
    foreach(var obj in _pool) {
      if(obj.Exists) {
        func(obj);
      }
    }
  }

  /// 生存しているインスタンスをすべて破棄する
  public void Vanish() {
    ForEachExist(t => t.Vanish());
  }

  /// インスタンスの生存数を取得する
  public int Count() {
    int ret = 0;
    ForEachExist(t => ret++);

    return ret;
  }
}
