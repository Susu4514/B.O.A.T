using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Character : MonoBehaviour {
    // Start is called before the first frame update

    //基础属性，所有生物都有这些属性
    //生命值，护盾值，攻击力，暴击率，闪避率

    private int Hp;
    private int Shield;
    private int Damage;
    private int Critical;
    private int Agile;

    private List<int> SkillList = new List<int>();

     void Update()
    {
        
    }
    
}
