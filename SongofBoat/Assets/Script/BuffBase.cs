using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class BuffBase:MonoBehaviour
{
    public enum BuffType{
    DirectBuff = 1,         //直接生效
    GivingBuff = 2,         //派发类
    CalculateBuff = 3,      //计算类
    StartBuff = 4,          //回合开始类
    }

    public enum DirectBuffType{
        Hp = 1,
        Shield = 2,
        Action = 3,
    }

    //buff消失和重叠的类型
    public enum BuffDestroyType{
    HiteedBuff = 1,         //受击触发
    StartBuff = 2,          //回合开始触发
    }

    public enum BuffRestartIndex{
    refreshBuff = 1,        //刷新型
    addBuff = 2,            //叠加型
    }

    private int buffID;
    //private BuffType Bufftype;
    private int buffTarget;
    private string buffDescription;
    private int buffStartIndex;
    private string buffIcon;
    //private BuffDestroyType buffDestroytype;
    //private BuffRestartIndex buffRestartindex;
    private int[] buffParam;
    private int buffAoe;
    private int buffType;
    private int buffDestroytype;
    private int buffRestartindex;

    private CharacterProperties character;

    private LevelSystem levelsystem;
    static private string Buffpath = Application.streamingAssetsPath + "/battleBuffconfig.csv";


    public void cast(int buffID){
        //Debug.Log(Bufftype);
        if(buffID > 10000){ //保证一下必须是一个buffID才能读
            readBuffFromFile(buffID);
            // Debug.Log(buffID);
            //this.buffID = buffID;
            switch(buffType){
                case (int)BuffType.DirectBuff :
                    castDirect();
                    break;
                case (int)BuffType.GivingBuff :
                    castGiving();
                    break;
                case (int)BuffType.StartBuff :
                    castStart();
                    break;
                case (int)BuffType.CalculateBuff :
                    castCalculate();
                    break;
                default:
                    Debug.Log("读取技能type出现错误");
                    break;  
            }
        }
        Debug.Log(buffID);
    }

    //从csv文件中读取buff的数据
    //BuffTarget：技能目标               BuffDestroy Buff销毁类型
    //BuffType：技能类型                 BuffRestart Buff叠加类型
    //BuffStartIndex：Buff起始层数       BuffAoe 是否为Aoe技能
    //参数1-6的含义，见各项Buff的说明
    public void readBuffFromFile(int buffID){
        string[] BuffLine = File.ReadAllLines(Buffpath);
        string[] keys = BuffLine[1].Split(',');
        for (int i = 3; i < BuffLine.Length; i++) {
            string[] lineData = BuffLine[i].Split(',');
            //Debug.Log(Convert.ToInt32(lineData[0]));
//            Debug.Log(buffID);
            if(Convert.ToInt64(lineData[0]) == buffID){ 
                for (int j = 0; j < lineData.Length; j++) {
                    if (keys[j] == "BuffTarget") {
                        this.buffTarget = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffType") {
                        //Debug.Log("读到了一个杰尼龟type"+lineData[j]);
                        this.buffType = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffStartIndex") {
                        this.buffStartIndex = Convert.ToInt32(lineData[j]);
                    }
                      //TODO:这个地方填表一定要填-1才没有，还有优化的地方哦
                      else if (keys[j] == "BuffDestroy") {
                        this.buffDestroytype = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffRestart") {
                        this.buffRestartindex = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffAoe") { //这个buff是否为AOE范畴
                        buffAoe = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex1") {
                        buffParam[0] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex2") {
                        buffParam[1] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex3") {
                        buffParam[2] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex4") {
                        buffParam[3] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex5") {
                        buffParam[4] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex6") {
                        buffParam[5] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex7") {
                        buffParam[6] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex8") {
                        buffParam[7] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "BuffIndex9") {
                        buffParam[8] = Convert.ToInt32(lineData[j]);
                    }

                }
            }
        }
    }

    /*直接作用类技能，
    参数1：发动几率                
    参数2：对目标作用的属性 1、生命值 2、护盾值 3、行动力值
    参数3：对于攻击的百分比加成
    参数4：对于生命值的百分比加成
    参数5：对于护盾值的百分比加成
    参数6：对于行动力的百分比加成
    参数7：固定值
    参数8：
    参数9：
                    */
    public void castDirect() {
        //判断是否发动
        //如果是对自己就加，对别人就减，这个是没有做出来的
        if (UnityEngine.Random.Range(0, 100) <= buffParam[0]) {
            //根据参数值计算伤害             //计数决定是第几个被选中的敌人
            int Pointed = levelsystem.PointedEnemy;
            float Damage = character.Damage * buffParam[2] / 100 + character.HpNow * buffParam[3] / 100 + character.ShieldNow * buffParam[4] / 100 + buffParam[5] * levelsystem.battleInitial.Enemyem[Pointed].InitialAct + buffParam[6];
            //buff对自己生效
            if (buffTarget == 1) {
                switch (buffParam[1]) {
                    //BuffIndex2是判断对目标的什么属性生效
                    case (int)DirectBuffType.Hp:
                        levelsystem.battleInitial.hero.GetComponent<CharacterProperties>().hpDecrease(Damage * -1);
                        break;
                    case (int)DirectBuffType.Shield:
                        levelsystem.battleInitial.hero.GetComponent<CharacterProperties>().shieldDecrease(Damage * -1);
                        break;
                    default:
                        break;
                }
                //不为AOE攻击
            } else if(buffTarget ==2) {
                if (buffAoe == 0) {
                    switch (buffParam[1]) {
                        //BuffIndex2是判断对目标的什么属性生效
                        case (int)DirectBuffType.Hp:
                            levelsystem.battleInitial.Enemyem[Pointed].hpDecrease(Damage);
                            break;
                        case (int)DirectBuffType.Shield:
                            levelsystem.battleInitial.Enemyem[Pointed].shieldDecrease(Damage);
                            break;
                        case (int)DirectBuffType.Action:
                            levelsystem.battleInitial.Enemyem[Pointed].actionDecrease((int)Damage);
                            break;
                        default:
                            break;
                    }
                } else if (buffAoe == 1) {
                    switch (buffParam[1]) { //BuffIndex2是判断对目标的什么属性生效
                        case (int)DirectBuffType.Hp:
                            for (int i = 0; i < levelsystem.battleInitial.Enemyem.Length; i++) {
                                levelsystem.battleInitial.Enemyem[i].hpDecrease(Damage);
                            }
                            break;
                            //这个地方以后一定会出错。。敌人死了之后会报错
                        case (int)DirectBuffType.Shield:
                            for (int i = 0; i < levelsystem.battleInitial.Enemyem.Length; i++) {
                                levelsystem.battleInitial.Enemyem[i].shieldDecrease(Damage);
                            }
                            break;

                        case (int)DirectBuffType.Action:
                            for (int i = 0; i < levelsystem.battleInitial.Enemyem.Length; i++) {
                                levelsystem.battleInitial.Enemyem[i].actionDecrease((int)Damage);
                            }
                            break;

                        default:
                            break;
                    }
                }
            }
        }
    }
    
    /* 同时叠加多个buff类技能
    参数1：发动几率
    参数2-9 ： buffID
    */
    public void castGiving(){
        //这里是因为进到下面的函数里，变量的值会变，所以存在栈里
        int[] buffParams = new int[8];

        if(UnityEngine.Random.Range(0, 100)<=this.buffParam[0]){
            for (int i = 0; i < 8; i++) {
                buffParams[i] = this.buffParam[i+1];
                this.cast(buffParams[i]);
            }
        }
    }

    /* 回合开始时发动作用的技能
    参数1：回合开始时的发动概率
    参数2-6：派发的buffID
    */

    public void castStart(){
        Character.Buff newbuff = new Character.Buff();
        newbuff.buffType = 4;
        newbuff.buffDestroyType = buffDestroytype;
        newbuff.buffRefreshType = buffRestartindex;
        newbuff.buffStartIndex = buffStartIndex;
        newbuff.buffNowIndex = buffStartIndex;
        for (int i = 0; i < 8; i++) {
            newbuff.Param[i] = buffParam[i + 1];
        }


        int Pointed = levelsystem.PointedEnemy;
        if (UnityEngine.Random.Range(0, 100) <= buffParam[0]) {
            if (buffAoe == 1) {
                for (int i = 0; i < levelsystem.battleInitial.Enemyem.Length; i++) {
                    levelsystem.battleInitial.Enemyem[i].addBuff(newbuff);
                }
            } else if (buffAoe == 0) {
                levelsystem.battleInitial.Enemyem[Pointed].addBuff(newbuff);
            }
        }
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
    参数9: 绑定buff的ID
    */

    public void castCalculate(){
        Character.Buff newbuff = new Character.Buff();
        newbuff.Param = new int[8];
        newbuff.buffType = 3;
        newbuff.buffDestroyType = buffDestroytype;
        newbuff.buffRefreshType = buffRestartindex;
        newbuff.buffStartIndex = buffStartIndex;
        newbuff.buffNowIndex = buffStartIndex;
        for (int i = 0; i < 8; i++) {
            newbuff.Param[i] = buffParam[i + 1];
        }
        //Debug.Log(newbuff);
        int Pointed = levelsystem.PointedEnemy;
        if(UnityEngine.Random.Range(0, 100) <= buffParam[0]){
            if( buffAoe == 1 ){
                for(int i = 0 ; i < levelsystem.battleInitial.Enemyem.Length; i++){
                    levelsystem.battleInitial.Enemyem[i].addBuff(newbuff);
                }
            }
            else if( buffAoe == 0 ){
                levelsystem.battleInitial.Enemyem[Pointed].addBuff(newbuff);
            }
        }
    }

    void Awake(){
        character = this.GetComponent<CharacterProperties>();
    }

    void Start(){
        levelsystem = GetComponentInParent<LevelSystem>();
        buffParam = new int[9];
    }
}