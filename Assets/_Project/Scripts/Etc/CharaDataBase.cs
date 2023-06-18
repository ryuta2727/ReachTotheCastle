using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class CharaDataBase : ScriptableObject
{
    public List<CharaStatus> charaList = new List<CharaStatus>();
}
