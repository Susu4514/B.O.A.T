using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuffBase:MonoBehaviour
{
    public enum BuffType{
    DirectBuff = 1,
    GivingBuff = 2,
    CalculateBuff = 3,
    StartBuff = 4,
    HittedBuff = 5,
    }

    public enum BuffDestroyType{
    HiteedBuff = 1,
    StartBuff = 2,
    }

    public enum BuffRestartIndex{
    refreshBuff = 1,
    addBuff = 2,
    }

    private int buffID;
    private BuffType Bufftype;
    private string BuffDescription;
    private int BuffStartIndex;
    private string BuffIcon;
    private BuffDestroyType BuffDestroytype;
    private BuffRestartIndex BuffRestartindex;
    private int BuffIndex1;
    private int BuffIndex2;
    private int BuffIndex3;
    private int BuffIndex4;
    private int BuffIndex5;



     // public void LevelInitial(int index, GameObject enemy,string path){
    //     string[] EnemyLine = File.ReadAllLines(path);
    //     //然后把变量行提取出来
    //     string[] keys = EnemyLine[1].Split(',');
    //     //建立字典映射，这里关卡可以没有
    //     for (int i = 3; i < EnemyLine.Length; i++) {
    //         /* 每一行的内容都是逗号分隔，读取每一列的值 */

    //         string[] lineData = EnemyLine[i].Split(',');
            
    //         if(Convert.ToInt32(lineData[0]) == index){ //如果关卡ID对上了那么继续，如果对不上就不存
    //         //csv类对应
            
    //             for (int j = 0; j < lineData.Length; j++) {
    //                 if (keys[j] == "LevelID") {
    //                     battleInitial.LevelID = Convert.ToInt32(lineData[j]);
    //                 }
    //                 else if (keys[j] == "levelBgd") {
    //                     battleInitial.levelBgd = lineData[j];
    //                 }
    //                 else if (keys[j] == "EnemyAmount") {
    //                     //   Debug.Log(lineData[j]);
    //                     battleInitial.EnemyAmount = Convert.ToInt32(lineData[j]);
    //                 }
    //                 //TODO:这个地方填表一定要填-1才没有，还有优化的地方哦
    //                 else if (keys[j] == "EnemyID1") {
    //                         battleInitial.EnemyIDs[0] = Convert.ToInt32(lineData[j]);
    //                 }
    //                 else if (keys[j] == "EnemyID2") {
    //                         battleInitial.EnemyIDs[1] = Convert.ToInt32(lineData[j]);
    //                 }
    //                 else if (keys[j] == "EnemyID3") {
    //                         battleInitial.EnemyIDs[2] = Convert.ToInt32(lineData[j]);
    //                 }
    //             }
    //         }
    //     }
    // }
}