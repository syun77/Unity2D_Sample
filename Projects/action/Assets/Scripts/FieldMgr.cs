using UnityEngine;
using System.Collections;

/// マップデータ管理
public class FieldMgr : MonoBehaviour {

  /// プレイヤー
  const int CHIP_PLAYER = 1;
  /// 壁
  const int CHIP_WALL = 2;
  /// トゲ
  const int CHIP_SPIKE = 3;
  /// 移動床
  const int CHIP_FLOOR_MOVE = 4;
  /// ゴール
  const int CHIP_GOAL = 5;

  /// チップX座標からワールドX座標を取得する
  float GetChipX(int i) {
    Vector2 min = Camera.main.ViewportToWorldPoint(Vector2.zero);
    var spr = Util.GetSprite("Levels/tileset", "tileset_0");
    var sprW = spr.bounds.size.x;
    return min.x + (sprW * i) + sprW/2;
  }
  /// チップY座標からワールドY座標を取得する
  float GetChipY(int j) {
    Vector2 max = Camera.main.ViewportToWorldPoint(Vector2.one);
    var spr = Util.GetSprite("Levels/tileset", "tileset_0");
    var sprH = spr.bounds.size.y;
    return max.y - (sprH * j) - sprH/2;
  }

  /// ステージ数を指定してマップデータをロードする
  public void Load (int nStage) {
    TMXLoader tmx = new TMXLoader();
    tmx.Load(string.Format("Levels/{0:D3}", nStage));
    Layer2D layer = tmx.GetLayer(0);
    
    // タイルの配置
    for(int j = 0; j < layer.Height; j++) {
      for(int i = 0; i < layer.Width; i++) {
        int v = layer.Get(i, j);
        float x = GetChipX(i);
        float y = GetChipY(j);
        switch(v) {
        case CHIP_PLAYER:
        {
          // プレイヤーを移動させる
          GameObject obj = GameObject.Find("Player") as GameObject;
          Player player = obj.GetComponent<Player>();
          player.SetPosition(x, y);
        }
          break;
        case CHIP_WALL:
          // 壁を作成
          Wall.Add(x, y);
          break;
        case CHIP_SPIKE:
          // トゲを生成
          Spike.Add(x, y);
          break;
        case CHIP_FLOOR_MOVE:
          // 移動床を作成
          FloorMove.Add(x, y);
          break;
        case CHIP_GOAL:
        {
          // ゴールを移動させる
          GameObject obj = GameObject.Find ("Goal") as GameObject;
          Goal goal = obj.GetComponent<Goal>();
          goal.SetPosition(x, y);
        }
          break;
        }
      }
    }
  }


  void Update () {
  }
}
