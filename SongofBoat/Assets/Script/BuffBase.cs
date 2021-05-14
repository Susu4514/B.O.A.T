using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
public class BuffBase:MonoBehaviour
{
    //buff类型
    public enum BuffType{
    DirectBuff = 1,
    GivingBuff = 2,
    CalculateBuff = 3,
    StartBuff = 4,
    HittedBuff = 5,
    }

    public enum DirectBuffType{
        Hp = 1,
        Shield = 2,
        Action = 3,
    }
    //buff消失和重叠的类型
    public enum BuffDestroyType{
    HiteedBuff = 1,
    StartBuff = 2,
    }

    public enum BuffRestartIndex{
    refreshBuff = 1,
    addBuff = 2,
    }

    private int buffID;
    //private BuffType Bufftype;
    private int BuffTarget;
    private string BuffDescription;
    private int BuffStartIndex;
    private string BuffIcon;
    //private BuffDestroyType BuffDestroytype;
    //private BuffRestartIndex BuffRestartindex;
    private int BuffIndex1;
    private int BuffIndex2;
    private int BuffIndex3;
    private int BuffIndex4;
    private int BuffIndex5;
    private int BuffIndex6;
    private int BuffAoe;
    private int Bufftype;
    private int BuffDestroytype;
    private int BuffRestartindex;

    private CharacterProperties character;

    private LevelSystem levelsystem;
    static private string Buffpath = Application.streamingAssetsPath + "/battleBuffconfig.csv";
    public void cast(int buffID){
        //Debug.Log(Bufftype);
        if(buffID > 10000){ //保证一下必须是一个buffID才能读
            readBuffFromFile(buffID);
            // Debug.Log(buffID);
            //this.buffID = buffID;
            switch(Bufftype){
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
    }

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
                        this.BuffTarget = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "BuffType") {
                        //Debug.Log("读到了一个杰尼龟type"+lineData[j]);
                        this.Bufftype = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "BuffStartIndex") {
                        this.BuffStartIndex = Convert.ToInt32(lineData[j]);
                    }
                    //TODO:这个地方填表一定要填-1才没有，还有优化的地方哦
                    else if (keys[j] == "BuffDestroy") {
                        this.BuffDestroytype = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "BuffRestart") {
                        this.BuffRestartindex = Convert.ToInt32(lineData[j]);
                    }
                    else if(keys[j] == "BuffAoe"){ //这个buff是否为AOE范畴
                        BuffAoe = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "BuffIndex1") {
                        BuffIndex1 = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "BuffIndex2"){
                        BuffIndex2 = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "BuffIndex3"){                        
                        BuffIndex3 = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "BuffIndex4"){                       
                         BuffIndex4 = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "BuffIndex5"){                        
                        BuffIndex5 = Convert.ToInt32(lineData[j]);
                    }      
                    else if (keys[j] == "BuffIndex6"){                        
                        BuffIndex6 = Convert.ToInt32(lineData[j]);
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
    参数6：对于行动力的百分比加成，这个考虑后续添加
                    */
    public void castDirect(){
        if(UnityEngine.Random.Range(0, 100)<=BuffIndex1){
            int Damage = character.Damage * BuffIndex3/100 + character.HpNow * BuffIndex4/100 + character.ShieldNow * BuffIndex5/100;
            int Pointed = levelsystem.PointedEnemy;

            // Debug.Log(Damage);
            // Debug.Log(BuffIndex2);
            if(BuffAoe == 0){ //不为AOE攻击
                switch (BuffIndex2){ //BuffIndex2是判断对目标的什么属性生效
                    case (int)DirectBuffType.Hp:
                    levelsystem.battleInitial.Enemyem[Pointed].hpDecrease(Damage);
                    break;

                    case (int)DirectBuffType.Shield:
                    levelsystem.battleInitial.Enemyem[Pointed].shieldDecrease(Damage);
                    break;

                    case (int)DirectBuffType.Action:
                    levelsystem.battleInitial.Enemyem[Pointed].actionDecrease(Damage);
                    break;

                    default:
                    break;
                }
            }
            else if(BuffAoe == 1){
                 switch (BuffIndex2){ //BuffIndex2是判断对目标的什么属性生效
                    case (int)DirectBuffType.Hp:
                    for(int i = 0 ; i < levelsystem.battleInitial.Enemyem.Length; i++){
                    levelsystem.battleInitial.Enemyem[i].hpDecrease(Damage);
                    }
                    break;

                    case (int)DirectBuffType.Shield:
                    for(int i = 0 ; i < levelsystem.battleInitial.Enemyem.Length; i++){
                    levelsystem.battleInitial.Enemyem[i].hpDecrease(Damage);
                    }
                    break;

                    case (int)DirectBuffType.Action:
                    for(int i = 0 ; i < levelsystem.battleInitial.Enemyem.Length; i++){
                    levelsystem.battleInitial.Enemyem[i].hpDecrease(Damage);
                    }
                    break;

                    default:
                    break;
                }
            }

        }
    }
    /* 同时叠加多个buff类技能
    参数1：发动几率
    参数2-6 ： buffID
    */
    public void castGiving(){
        int i2 = BuffIndex2;
        int i3 = BuffIndex3;
        int i4 = BuffIndex4;
        int i5 = BuffIndex5;
        int i6 = BuffIndex6;
        if(UnityEngine.Random.Range(0, 100)<=BuffIndex1){
            this.cast(i2);
            Debug.Log(BuffIndex3);
            this.cast(i3);
            this.cast(i4);
            this.cast(i5);
            this.cast(i6);

        }
    }

    /* 回合开始时发动作用的技能
    参数1：回合开始时的发动概率
    参数2-6：派发的buffID
    */

    public void castStart(){
        Character.Buff newbuff = new Character.Buff();
        newbuff.BuffType = 3;
        newbuff.BuffDestroyType = BuffDestroytype;
        newbuff.BuffRefreshType = BuffRestartindex;
        newbuff.BuffStartIndex = BuffStartIndex;
        newbuff.BuffNowIndex = BuffStartIndex;
        newbuff.Param[0] = BuffIndex3;
        newbuff.Param[1] = BuffIndex4;
        newbuff.Param[2] = BuffIndex5;

        int Pointed = levelsystem.PointedEnemy;
        if(UnityEngine.Random.Range(0, 100) <= BuffIndex1){
            if( BuffAoe == 1 ){
                for(int i = 0 ; i < levelsystem.battleInitial.Enemyem.Length; i++){
                    levelsystem.battleInitial.Enemyem[i].BuffList.Add(newbuff);
                }
            }
            else if( BuffAoe == 0 ){
                levelsystem.battleInitial.Enemyem[Pointed].BuffList.Add(newbuff);
            }
        }
    }

    /* 受到攻击时进行结算的buff
    参数1：buff的发动概率
    参数2：对目标作用的属性 1、伤害值 2、行动力值 3、其它 如果为3则参数4-6有意义
    参数3：减免/增加的百分比值
    参数4：增加/减免的固定值
    参数4：受击对自身释放的buffid
    参数5：受击对敌方释放的buffid
    */
    public void castCalculate(){
        Character.Buff newbuff = new Character.Buff();
        newbuff.Param = new int[4];
        newbuff.BuffType = 3;
        newbuff.BuffDestroyType = BuffDestroytype;
        newbuff.BuffRefreshType = BuffRestartindex;
        newbuff.BuffStartIndex = BuffStartIndex;
        newbuff.BuffNowIndex = BuffStartIndex;
        newbuff.Param[0] = BuffIndex3;
        newbuff.Param[1] = BuffIndex4;
        newbuff.Param[2] = BuffIndex5;
        newbuff.Param[3] = BuffIndex6;
        //Debug.Log(newbuff);

        int Pointed = levelsystem.PointedEnemy;
        if(UnityEngine.Random.Range(0, 100) <= BuffIndex1){
            if( BuffAoe == 1 ){
                for(int i = 0 ; i < levelsystem.battleInitial.Enemyem.Length; i++){
                    levelsystem.battleInitial.Enemyem[i].BuffList.Add(newbuff);
                }
            }
            else if( BuffAoe == 0 ){
                levelsystem.battleInitial.Enemyem[Pointed].BuffList.Add(newbuff);
            }
        }
    }

    void Awake(){
        character = this.GetComponent<CharacterProperties>();
    }

    void Start(){
        levelsystem = GetComponentInParent<LevelSystem>();

    }
}