using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class Character: MonoBehaviour{
    // Start is called before the first frame update

    //基础属性，所有生物都有这些属性
    //生命值，护盾值，攻击力，暴击率，闪避率

    public int Hp{get;set;}
    public int HpNow{get;set;}
    public int Shield{get;set;}
    public int ShieldNow{get;set;}
    public int Damage{get;set;}
    public int Critical{get;set;}
    public int Agile{get;set;}
    public List<BuffBase> BuffList{get;set;}

}
