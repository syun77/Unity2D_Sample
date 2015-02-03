using UnityEngine;
using System.Collections;

/// ボス
public class Boss : Enemy
{
  /// ボスを倒したフラグ
  public static bool bDestroyed = false;

  /// コルーチンを開始したかどうか
  bool _bStart = false;
  /// 開始
  void Start()
  {
    // パラメータを設定
    SetParam(0);
    // ボス倒したフラグ初期化
    bDestroyed = false;
  }
  /// 消滅
  public override void Vanish()
  {
    // ボスを倒した
    bDestroyed = true;
    base.Vanish();
  }
  
  /// 更新
  void Update()
  {
    if (_bStart == false)
    {
      // 敵生成開始
      StartCoroutine("_GenerateEnemy");
      _bStart = true;
    }
  }

  /// 敵の生成
  Enemy AddEnemy(int id, float direction, float speed)
  {
    return Enemy.Add(id, X, Y, direction, speed);
  }

  /// だいこんを3方向に発射
  void BulletRadish()
  {
    // プレイヤーと±30度にだいこんを発射
    float aim = GetAim();
    AddEnemy(4, aim, 3);
    AddEnemy(4, aim - 30, 3);
    AddEnemy(4, aim + 30, 3);
  }

  /// にんじんを発射
  void BulletCarrot()
  {
    AddEnemy(5, 45, 3);
    AddEnemy(5, -45, 3);
  }

  /// 敵生成
  IEnumerator _GenerateEnemy() {
    while (true)
    {
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
  /// HPの描画
  void OnGUI()
  {
    // テキストを黒にする
    Util.SetFontColor(Color.black);
    // テキストを大きめにする
    Util.SetFontSize(24);
    // 中央揃えにする
    Util.SetFontAlignment(TextAnchor.MiddleCenter);
    // テキスト描画
    string text = string.Format("{0,3}", Hp);
    Util.GUILabel(380, 200, 120, 30, text);
  }

}