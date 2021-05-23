using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class EnemyProperties : Character
{
    // Start is called before the first frame update
    static private string EnemyPropertiespath = Application.streamingAssetsPath + "/EnemyConfig.csv";
    public int InitialAct;
    public struct Skill{
        public int SkillID;
        public int SkillWeight;
    }
    //敌人的技能列表和权重
    private Skill[] SkillList{get;set;}
    //敌人携带的buff列表

    public HealthBar bar;

    //private LevelSystem levelsystem;


    void Awake(){
        SkillList = new Skill[3];
        BuffList = new List<Buff>();
    }
    void Start(){
        base.Start();
    }
    //这里读取敌人的各项基本属性
    public void readPropertiesFromFile(int ID){
        string[] BattleLine = File.ReadAllLines(EnemyPropertiespath);
        //然后把变量行提取出来
        string[] keys = BattleLine[1].Split(',');
        //建立字典映射，这里关卡可以没有
        for (int i = 3; i < BattleLine.Length; i++) {
            /* 每一行的内容都是逗号分隔，读取每一列的值 */

            string[] lineData = BattleLine[i].Split(',');

            if(Convert.ToInt32(lineData[0]) == ID){ //如果关卡ID对上了那么继续，如果对不上就不存
            //csv类对应
                for (int j = 0; j < lineData.Length; j++) {
                    if (keys[j] == "Hp") {
                        this.Hp = Convert.ToInt32(lineData[j]);
                        this.HpNow = this.Hp;
                    }
                    else if (keys[j] == "Shield") {
                        this.Shield = Convert.ToInt32(lineData[j]);
                        this.Shield = 0;
                    }
                    else if (keys[j] == "Damage") {
                        //   Debug.Log(lineData[j]);
                        this.Damage = Convert.ToInt32(lineData[j]);
                    }
                    //TODO:这个地方填表一定要填-1才没有，还有优化的地方哦
                    else if (keys[j] == "Critical") {
                            this.Critical = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "Agile") {
                            this.Agile = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "InitialAct") {
                            this.InitialAct = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "SkillFirst"){
                        this.SkillList[0].SkillID = Convert.ToInt32(lineData[j]); 
                        this.SkillList[0].SkillWeight = Convert.ToInt32(lineData[j+1]);
                    }
                    else if (keys[j] == "SkillSecond"){
                        this.SkillList[1].SkillID = Convert.ToInt32(lineData[j]); 
                        this.SkillList[1].SkillWeight = Convert.ToInt32(lineData[j+1]);
                    }
                    else if (keys[j] == "SkillThird"){
                        this.SkillList[2].SkillID = Convert.ToInt32(lineData[j]); 
                        this.SkillList[2].SkillWeight = Convert.ToInt32(lineData[j+1]);
                    }
                }
            }
        }
    }

    public void HealthBarInitial(){
        bar.startText.text = InitialAct.ToString();
    }
    
        // healthNowimage.fillAmount = enemy.HpNow/enemy.Hp;
        // if(healthEffect.fillAmount>healthNowimage.fillAmount){
        //     healthEffect.fillAmount -= 0.003f;
        // }
        // else{
        //     healthEffect.fillAmount = healthNowimage.fillAmount;
        // }

    private void OnMouseDown() {
        levelsystem.ChangeSelect(this);
    }


    public void actionDecrease(int x ){
        if(this.InitialAct<=6){
            this.InitialAct += x;
        }
    }


}
