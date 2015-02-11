using UnityEngine;
using System.Collections;
using System.Text;
using System.Xml;
using System.IO;

public class TMXLoader
{
  Layer2D _layer = null;
  public Layer2D GetLayer(int idx)
  {
    return _layer;
  }
  // レベルデータを読み込む
  public void Load(string fLevel)
  {
    // レイヤー生成.
    _layer = new Layer2D();
    // レベルデータ取得.
    TextAsset tmx = Resources.Load(fLevel) as TextAsset;

    // XML解析開始.
    XmlDocument xmlDoc = new XmlDocument();
    xmlDoc.LoadXml(tmx.text);
    XmlNodeList mapList = xmlDoc.GetElementsByTagName("map");
    foreach (XmlNode map in mapList)
    {
      XmlNodeList childList = map.ChildNodes;
      foreach (XmlNode child in childList)
      {
        if (child.Name != "layer") { continue; } // layerノード以外は見ない.

        // マップ属性を取得.
        XmlAttributeCollection attrs = child.Attributes;
        int w = int.Parse(attrs.GetNamedItem("width").Value); // 幅を取得.
        int h = int.Parse(attrs.GetNamedItem("height").Value); // 高さを取得.
        // レイヤー生成.
        _layer.Create(w, h);
        XmlNode node = child.FirstChild; // 子ノードは<data>のみ.
        XmlNode n = node.FirstChild; // テキストノードを取得.
        string val = n.Value; // テキストを取得.
        // CSV(マップデータ)を解析.
        int y = 0;
        foreach (string line in val.Split('\n'))
        {
          // 空白文字を削除
          var line2 = line.Trim();
          if (line2 == "") { continue; } // 空文字は除外.
          int x = 0;
          foreach (string s in line2.Split(','))
          {
            int v = 0;
            // ","で終わるのでチェックが必要.
            if (int.TryParse(s, out v) == false) { continue; }
            // 値を設定.
            _layer.Set(x, y, v);
            x++;
          }
          y++;
        }
      }
    }
  }
}
