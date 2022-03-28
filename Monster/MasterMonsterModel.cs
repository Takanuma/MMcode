using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MonsterDex",menuName ="Create MonsterDex")]
public class MasterMonsterModel : ScriptableObject
{
    public new string name;
    public Sprite thumbnail;

    public int hp;
    public int strength;

}