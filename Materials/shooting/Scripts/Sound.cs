using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// サウンド管理
public class Sound {

  /// サウンド種別
  enum eType {
    Bgm, // BGM
    Se,  // SE
  }

  static Sound _singleton = null;
  public static Sound GetInstance() {
    return _singleton ?? (_singleton = new Sound());
  }

  GameObject _object = null;
  AudioSource _sourceBgm = null;
  AudioSource _sourceSe = null;
  Dictionary<string, _Data> _poolBgm = new Dictionary<string, _Data>();
  Dictionary<string, _Data> _poolSe = new Dictionary<string, _Data>();

  /// 保持するデータ
  class _Data {
    /// アクセス用のキー
    public string Key;
    /// リソース名
    public string ResName;
    /// AudioClip
    public AudioClip Clip;

    /// コンストラクタ
    public _Data(string key, string res) {
      Key = key;
      ResName = "Sounds/" + res;
      // AudioClipの取得
      Clip = Resources.Load(ResName) as AudioClip;
    }
  }

  /// コンストラクタ
  public Sound() {
    // ここにロードするサウンドを一括登録する
    //_LoadBgm("bgm", "bgm01");
    //_LoadSe("damage", "damage");
  }

  /// AudioSourceを取得する
  AudioSource _GetAudioSource(eType type) {
    if(_object == null) {
      // GameObjectがなければ作る
      _object = new GameObject("Sound");
      // 破棄しないようにする
      GameObject.DontDestroyOnLoad(_object);
      // AudioSourceを作成
      _sourceBgm = _object.AddComponent<AudioSource>();
      _sourceSe = _object.AddComponent<AudioSource>();
    }

    if(type == eType.Bgm) {
      // BGM
      return _sourceBgm;
    }
    else {
      // SE
      return _sourceSe;
    }
  }

  // サウンドのロード
  // ※Resources/Soundsフォルダに配置すること
  public static void LoadBgm(string key, string resName) {
    GetInstance()._LoadBgm(key, resName);
  }
  public static void LoadSe(string key, string resName) {
    GetInstance()._LoadSe(key, resName);
  }
  void _LoadBgm(string key, string resName) {
    _poolBgm.Add(key, new _Data(key, resName));
  }
  void _LoadSe(string key, string resName) {
    _poolSe.Add(key, new _Data(key, resName));
  }

  /// BGMの再生
  /// ※事前にLoadBgmでロードしておくこと
  public static bool PlayBgm(string key) {
    return GetInstance()._PlayBgm(key);
  }
  bool _PlayBgm(string key) {
    if(_poolBgm.ContainsKey(key) == false) {
      // 対応するキーがない
      return false;
    }

    // いったん止める
    _StopBgm();

    // リソースの取得
    var _data = _poolBgm[key];

    // 再生
    var source = _GetAudioSource(eType.Bgm);
    source.loop = true;
    source.clip = _data.Clip;
    source.Play();

    return true;
  }
  /// BGMの停止
  public static bool StopBgm() {
    return GetInstance()._StopBgm();
  }
  bool _StopBgm() {
    _GetAudioSource(eType.Bgm).Stop();

    return true;
  }

  /// SEの再生
  /// ※事前にLoadSeでロードしておくこと
  public static bool PlaySe(string key) {
    return GetInstance()._PlaySe(key);
  }
  bool _PlaySe(string key) {
    if(_poolSe.ContainsKey(key) == false) {
      // 対応するキーがない
      return false;
    }

    // リソースの取得
    var _data = _poolSe[key];

    var source = _GetAudioSource(eType.Se);
    // ワンショット再生
    source.PlayOneShot(_data.Clip);
    //source.clip = _data.Clip;
    //source.Play();

    return true;
  }
}
