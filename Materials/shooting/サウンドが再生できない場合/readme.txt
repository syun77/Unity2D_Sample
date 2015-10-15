2015/10/16 追加

サウンドが再生できない場合は、このフォルダ内の Sound.cs を、Assets/Scripts/Sound.cs に上書きして使うようお願いします。

■3Dサウンドを無効にする設定について
Unity5から AudioClip では3Dサウンドの設定をすることができなくなりました。そのため、AudioSource で3Dサウンドの設定をします。
AudioSourceクラスの「spatialBlend」が3Dサウンドの有効な割合となります。そしてこの値が「0」であれば2Dサウンドのみ有効となります。
