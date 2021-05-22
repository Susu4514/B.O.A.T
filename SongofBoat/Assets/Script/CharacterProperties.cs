using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterProperties : Character {
    // 人物的属性先写死
    // 人物的技能属性取决与符文，最初也是写死的
    public int[] SkillList;
    //敌人的技能列表和权重
    //敌人携带的buff列表
    public HealthBar bar;
    private BuffBase skill;
    void Awake(){
        BuffList = new List<Buff>();


        SkillList = new int[6];
        for(int i = 0 ; i < SkillList.Length; i ++){
            SkillList[i] = 10001+i;
        }
        //SkillList[1] = 10007;

        this.Hp = 200;
        this.HpNow = this.Hp;
        this.Shield = 0;
        this.ShieldNow = this.Shield;
        this.Damage = 20;
        this.Critical= 20;
        this.Agile = 20;

        skill = GetComponent<BuffBase>();
        
        HealthBarInitial();
    }

    public void HealthBarInitial(){
        bar.transform.Find("StartBg").gameObject.SetActive(false);
        bar.transform.Find("StartText").gameObject.SetActive(false);
        //TODO:血条的setActive不生效
    }

    public void StartSkill(int SkillType){
        this.skill.cast(SkillList[SkillType-1]);
        //Debug.Log(SkillList[SkillType-1]);
    }
}
