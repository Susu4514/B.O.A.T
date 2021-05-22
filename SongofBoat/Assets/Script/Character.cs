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

    public struct Buff{
        public int buffType;
        public int buffDestroyType;
        public int buffRefreshType;
        public int buffStartIndex;
        public int buffNowIndex;
        public int[] Param;
        public int buffisEffected;
    }

    protected List<Buff> BuffList;




    public void shieldDecrease(float damage) {
        if (BuffList.Count != 0) {
            foreach (Buff buff in BuffList) {
                if (buff.buffType == 3) { //是3表明是受击类的buff，所以要在算伤害的时候进行判定。
                    Debug.Log(buff.buffType);
                    Debug.Log(buff.Param[2]);
                }
            }
        }
        int x = (int)damage;

        this.ShieldNow = this.ShieldNow + x > Shield ? Shield : ShieldNow + x;

    }
    /* 受到攻击时进行结算的buff
    参数1：buff的发动概率
    参数2：对目标作用的属性 1、伤害值 2、护盾值 3、行动力值
    参数3：减免的百分比值
    参数4：增加的百分比值
    参数5：增加的固定值
    参数6：减免的固定值
    参数7：受击对自身释放的buffid
    参数8：受击对敌方释放的buffid

    */
    public void hpDecrease(float damage){
        if (BuffList.Count != 0) {
            foreach (Buff buff in BuffList) {
                if (buff.buffType == 3) { //是3表明是受击类的buff，所以要在算伤害的时候进行判定。
                    if (UnityEngine.Random.Range(0, 100) <= buff.Param[0]) {
                        switch (buff.Param[1]) {
                            case 1: //伤害，正常走
                                damage = damage * (100 + buff.Param[2]) / 100 + buff.Param[3];
                                break;
                            case 2:

                                break;
                            default:
                                break;
                        }
                    }

                }
            }
        }
        int x = (int)damage;
        this.HpNow = this.HpNow-x > 0 ? this.HpNow-x : 0;
//        Debug.Log(this.HpNow);        
        if(this.HpNow<=0){
            Destroy(gameObject);  
        }
    }

    public void HpIncrease(float health) {
        int x = (int)health;
        this.HpNow = this.HpNow + x > this.Hp ? this.Hp : this.HpNow + x;
    }

    public void ShieldIncrease(float shield) {
        int x = (int)shield;
        this.ShieldNow = this.ShieldNow + x > this.Hp ? this.Hp : this.ShieldNow + x;
    }

    void Awake(){
        //this.BuffList = new List<Buff>();
    }

    public void addBuff(Buff buff) {
        this.BuffList.Add(buff);
    }
}
