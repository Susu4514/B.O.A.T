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
        public int BuffType;
        public int BuffDestroyType;
        public int BuffRefreshType;
        public int BuffStartIndex;
        public int BuffNowIndex;
        public int[] Param;
        public int BuffIsEffected;
    }

    public List<Buff> BuffList{get;set;}
    
    
    public void shieldDecrease(int x){
        foreach(Buff buff in BuffList){
            if(buff.BuffType == 3){ //是3表明是受击类的buff，所以要在算伤害的时候进行判定。
                Debug.Log(buff.BuffType);
                Debug.Log(buff.Param[2]);
            }            
        }
        this.ShieldNow = this.ShieldNow + x > Shield ? Shield : ShieldNow + x;

    }
    /* 受到攻击时进行结算的buff
    参数1：buff的发动概率
    参数2：对目标作用的属性 1、伤害值 2、行动力值 3、其它 如果为3则参数4-6有意义
    参数3：减免/增加的百分比值
    参数4：增加/减免的固定值
    参数4：受击对自身释放的buffid
    参数5：受击对敌方释放的buffid
    */
    public void hpDecrease(int x){
        foreach(Buff buff in BuffList){
            if(buff.BuffType == 3){ //是3表明是受击类的buff，所以要在算伤害的时候进行判定。

            }            
        }
        this.HpNow = this.HpNow-x > 0 ? this.HpNow-x : 0;
//        Debug.Log(this.HpNow);        
        if(this.HpNow<=0){
            Destroy(gameObject);
        }
    }

    void Awake(){
       
    }
}
