using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSystem : Character {
    // Start is called before the first frame update

    private int MissionLevel = 1001;
    static private string BattleInitialfilepath = Application.streamingAssetsPath + "/BattleInitialConfig.csv";
    static private string EnemyModelfilepath = Application.streamingAssetsPath + "/CharModel.csv";

    public GameObject Enemy;

    public class BattleInitialSystem {
        public int LevelID { get; set; }

        public int EnemyAmount { get; set; }

        public int[] EnemyIDs {get; set;}

        public String levelBgd {get;set;}

        public string[] EnemySprite{get;set;}
        public string[] EnemyTexture{get;set;}
        public List<GameObject> EnemyGroup;
    }

    private BattleInitialSystem battleInitial; //关卡表的实例变量

    void Start()
    {
        //先按照行划分
        battleInitial = new BattleInitialSystem();
        battleInitial.EnemyIDs = new int[3];
        battleInitial.EnemySprite = new string[3];
        battleInitial.EnemyTexture = new string[3];
        //读了关卡基本表
        readBattleSystem(BattleInitialfilepath);
        //读了关卡怪物纹理
        readEnemyModel(battleInitial.EnemyIDs, battleInitial.EnemyAmount, EnemyModelfilepath);

        LevelEnemyInitialize();


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void readBattleSystem(String path){
        string[] BattleLine = File.ReadAllLines(path);
        //然后把变量行提取出来
        string[] keys = BattleLine[1].Split(',');
        //建立字典映射，这里关卡可以没有
        for (int i = 3; i < BattleLine.Length; i++) {
            /* 每一行的内容都是逗号分隔，读取每一列的值 */

            string[] lineData = BattleLine[i].Split(',');

            if(Convert.ToInt32(lineData[0]) == MissionLevel){ //如果关卡ID对上了那么继续，如果对不上就不存
            //csv类对应
                for (int j = 0; j < lineData.Length; j++) {
                    if (keys[j] == "LevelID") {
                        battleInitial.LevelID = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "levelBgd") {
                        battleInitial.levelBgd = lineData[j];
                    }
                    else if (keys[j] == "EnemyAmount") {
                        //   Debug.Log(lineData[j]);
                        battleInitial.EnemyAmount = Convert.ToInt32(lineData[j]);
                    }
                    //TODO:这个地方填表一定要填-1才没有，还有优化的地方哦
                    else if (keys[j] == "EnemyID1") {
                            battleInitial.EnemyIDs[0] = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "EnemyID2") {
                            battleInitial.EnemyIDs[1] = Convert.ToInt32(lineData[j]);
                    }
                    else if (keys[j] == "EnemyID3") {
                            battleInitial.EnemyIDs[2] = Convert.ToInt32(lineData[j]);
                    }
                }
            }
        }
    }

    void readEnemyModel(int [] EnemyIDs,int EnemyAmount,String path){  
        string[] ModelLine = File.ReadAllLines(path);
        //然后把变量行提取出来
        string[] keys = ModelLine[1].Split(','); 
        for(int index = 0 ; index < EnemyAmount ; index++){
            for (int i = 3; i < ModelLine.Length; i++) {
            /* 每一行的内容都是逗号分隔，读取每一列的值 */
                string[] lineData = ModelLine[i].Split(',');
                if(Convert.ToInt32(lineData[0]) == EnemyIDs[index]){ //
                    //TODO：找到了，需要读路径和怪物动作
                    for (int j = 0; j < lineData.Length; j++) {
                        if (keys[j] == "EnemySprite") {
                            battleInitial.EnemySprite[index] = lineData[j];
                        }
                        if (keys[j] == "EnemyTexture") {
                            battleInitial.EnemyTexture[index] = lineData[j];
                        }
                        
                    }
                }
            }   
        }
    }

    void LevelEnemyInitialize(){
        //已经存储好路径，要新建一个path然后读取assetbundle，然后配一个prefeb
        for(int i = 0 ; i < battleInitial.EnemyAmount ; i++){
            
        }
        StartCoroutine(readSpriteFromFile(battleInitial.EnemySprite[0],battleInitial.EnemyTexture[0]));
    }

    IEnumerator readSpriteFromFile(string spriteID,string textureID){
        string path = Application.dataPath+ "/AssetBundle/" + spriteID;
        AssetBundle spriteasset = AssetBundle.LoadFromFile(path);
        Sprite Enemysprite = spriteasset.LoadAsset<Sprite>(textureID);
        GameObject enemy1 = GameObject.Instantiate(Enemy);
        Debug.Log(spriteasset);
        enemy1.GetComponentInChildren<SpriteRenderer>().sprite = Enemysprite;
        yield return null ;
    }
}