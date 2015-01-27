using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;

/// Tiled Map Editor(*.tmx)ファイル読み込みモジュール
/// 注意：*.tmxファイルは、Resourcesフォルダ以下に配置すること
public class TMXLoader
{
  /// レイヤー情報
  class _Layer {
    public Layer2D data = null;
    public Dictionary<string, string> properties = null;
    public _Layer() {
      data = new Layer2D();
      properties = new Dictionary<string, string>();
    }
  }

  /// レイヤー情報のリスト
  List<_Layer> _layerList = null;

  /// インデックスを指定してレイヤー情報を取得する
  _Layer _GetLayer(int idx) {
    if(_layerList == null) {
      Debug.LogError("TmxLoader::_layerList is null.");
    }
    if(idx < 0 || Count <= idx) {
      Debug.LogError("TmxLoader::Count["+Count+"] Invalid idx="+idx);
    }
    return _layerList[idx];
  }

  /// レイヤー情報を取得する.
  public Layer2D GetLayer (int idx)
  {
    _Layer layer = _GetLayer(idx);
    return layer.data;
  }

  /// プロパティを取得する
  public string GetProperty (int idx, string key)
  {
    _Layer layer = _GetLayer(idx);
    return layer.properties[key];
  }

  /// レイヤー数を取得する.
  public int Count {
    get {
      if(_layerList == null) {
        return 0;
      }
      return _layerList.Count;
    }
  }
  // レベルデータを読み込む.
  public void Load (string fLevel)
  {
    _layerList = new List<_Layer>();

    // レベルデータ取得.
    TextAsset tmx = Resources.Load (fLevel) as TextAsset;

    // XML解析開始.
    XmlDocument xmlDoc = new XmlDocument ();
    xmlDoc.LoadXml (tmx.text);
    XmlNodeList mapList = xmlDoc.GetElementsByTagName ("map");
    foreach (XmlNode map in mapList) {
      XmlNodeList childList = map.ChildNodes;
      foreach (XmlNode child in childList) {
        switch (child.Name) {
        case "layer":
          // layerノードを解析する
          var layer = new _Layer();
          _layerList.Add(layer);
          _ParseLayer (child, layer.data, layer.properties);
          break;
        }
      }
    }
  }

  /// Layerを解析する
  void _ParseLayer (XmlNode child, Layer2D layer, Dictionary<string, string> properties)
  {
    // マップ属性を取得.
    XmlAttributeCollection attrs = child.Attributes;
    int w = int.Parse (attrs.GetNamedItem ("width").Value); // 幅を取得.
    int h = int.Parse (attrs.GetNamedItem ("height").Value); // 高さを取得.
    // レイヤー生成.
    layer.Create (w, h);
    foreach (XmlNode node in child) {
      switch (node.Name) {
      case "properties":
        // プロパティ
        foreach(XmlNode n in node) {
          XmlAttributeCollection attr = n.Attributes;
          var name = attr.GetNamedItem ("name").Value;
          var value = attr.GetNamedItem ("value").Value;
          properties[name] = value;
        }
        break;
      case "data":
        // グリッドデータ
        XmlNode n = node.FirstChild; // テキストノードを取得.
        string val = n.Value; // テキストを取得.
        // CSV(マップデータ)を解析.
        int y = 0;
        foreach (string line in val.Split('\n')) {
          if (line == "") {
            continue;
          } // 空文字は除外.
          int x = 0;
          foreach (string s in line.Split(',')) {
            int v = 0;
            // ","で終わるのでチェックが必要.
            if (int.TryParse (s, out v) == false) {
              continue;
            }
            // 値を設定.
            layer.Set (x, y, v);
            x++;
          }
          y++;
        }
        break;
      }
    }
  }
}
