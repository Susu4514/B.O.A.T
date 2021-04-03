using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterProperties : Character {
    // Start is called before the first frame update

    private Dictionary<int, BattleInitialSystem> battleInitial;

    private int MissionLevel = 1001;
    static private string BattleInitialfilepath = Application.streamingAssetsPath + "/BattleInitialConfig.csv";
    public class BattleInitialSystem {
        public int LevelID { get; set; }

        public int EnemyAmount { get; set; }

        public int EnemyID1 { get; set; }
       
        public int EnemyID2 { get; set; }      

        public int EnemyID3 { get; set; }

        public String levelBgd {get;set;}
    }

    void Start()
    {
        //先按照行划分
        string[] BattleLine = File.ReadAllLines(BattleInitialfilepath);
        //然后把变量行提取出来
        string[] keys = BattleLine[1].Split(',');
        //建立字典映射，这里关卡可以没有
        battleInitial = new Dictionary<int, BattleInitialSystem>();
        
        for (int i = 3; i < BattleLine.Length; i++) {
            /* 每一行的内容都是逗号分隔，读取每一列的值 */

            string[] lineData = BattleLine[i].Split(',');

            if(Convert.ToInt32(lineData[0]) == MissionLevel){ //如果关卡ID对上了那么继续，如果对不上就不存
                BattleInitialSystem BattleInitial = new BattleInitialSystem();
            //csv类对应
                for (int j = 0; j < lineData.Length; j++) {
                    if (keys[j] == "LevelID") {
                        BattleInitial.LevelID = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "levelBgd") {
                        BattleInitial.levelBgd = lineData[j];
                    }
                    else if (keys[j] == "EnemyAmount") {
                        //   Debug.Log(lineData[j]);
                        BattleInitial.EnemyAmount = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "EnemyID1") {
                        BattleInitial.EnemyID1 = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "EnemyID2") {
                        BattleInitial.EnemyID2 = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "EnemyID3") {
                        BattleInitial.EnemyID3 = Convert.ToInt32(lineData[j]);
                    }
                }
                if (!battleInitial.ContainsKey(BattleInitial.LevelID)){
                    battleInitial.Add(BattleInitial.LevelID, BattleInitial);
                }
            }
        }
        foreach (KeyValuePair<int,BattleInitialSystem> pair in battleInitial) {
           Debug.Log(pair.Key+"  " + pair.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
