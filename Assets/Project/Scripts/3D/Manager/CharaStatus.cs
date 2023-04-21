using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CharaStatus
{
    public GameObject chara; //3Dオブジェクト
    public string explainText; //説明
    public int Hp; //体力
    public int Atk; //攻撃力
    public int Int; //魔法攻撃力
    public int def; //防御力
    public int Lev; //レベル
    public int Exp; //経験値
}

