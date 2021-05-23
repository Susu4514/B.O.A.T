using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSystem : MonoBehaviour {
    // Start is called before the first frame update

    private int MissionLevel = 1001;
    static private string BattleInitialfilepath = Application.streamingAssetsPath + "/BattleInitialConfig.csv";
    static private string EnemyModelfilepath = Application.streamingAssetsPath + "/CharModel.csv";

    //inspector面板中读取
    public GameObject Enemy;
    public GameObject Hero;

    private int LevelID;
    private string LevelBgd;
    private int EnemyAmount;

    public List<EnemyGroups> enemyList;

    public class EnemyGroups {
        public int EnemyID;
        public string EnemySprite;
        public string EnemyTexture;
        public EnemyProperties properties;
        public GameObject enemyObject;
    }

    public int[] EnemyIds;

    public int PointedEnemy;




    void Awake(){
        PointedEnemy = 0;
        this.EnemyIds = new int[3];
        enemyList = new List<EnemyGroups>();
        readBattleSystem(BattleInitialfilepath);
        readEnemyModel(this.EnemyIds, this.EnemyAmount, EnemyModelfilepath);
        ChangeSelect(enemyList[0].properties);    
        LevelHeroInitialize();
    }
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    void LevelHeroInitialize(){
        Hero = GameObject.Instantiate(Hero,new Vector3(-5,1.3f,-0.2f),Quaternion.identity);
        Hero.transform.parent = transform;
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
                        this.LevelID = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "levelBgd") {
                        this.LevelBgd = lineData[j];
                    } else if (keys[j] == "EnemyAmount") {
                        //   Debug.Log(lineData[j]);
                        this.EnemyAmount = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "EnemyID1") {
                        this.EnemyIds[0] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "EnemyID2") {
                        this.EnemyIds[1] = Convert.ToInt32(lineData[j]);
                    } else if (keys[j] == "EnemyID3") {
                        this.EnemyIds[2] = Convert.ToInt32(lineData[j]);
                    }
                }
            }
        }
    }

    void readEnemyModel(int[] EnemyIDs,int EnemyAmount,String path){
        string[] ModelLine = File.ReadAllLines(path);
        //然后把变量行提取出来
        string[] keys = ModelLine[1].Split(','); 
        for(int index = 0 ; index < EnemyAmount ; index++){
            EnemyGroups newEnemy = new EnemyGroups();
            for (int i = 3; i < ModelLine.Length; i++) {
            /* 每一行的内容都是逗号分隔，读取每一列的值 */
                string[] lineData = ModelLine[i].Split(',');
                //Debug.Log(EnemyIDs.Length);
                newEnemy.EnemyID = EnemyIDs[index];
                if (Convert.ToInt32(lineData[0]) == EnemyIDs[index]){ //
                    //TODO：找到了，需要读路径和怪物动作
                    for (int j = 0; j < lineData.Length; j++) {
                        if (keys[j] == "EnemySprite") {
                            newEnemy.EnemySprite = lineData[j];
                        }
                        if (keys[j] == "EnemyTexture") {
                            newEnemy.EnemyTexture = lineData[j];
                        }  
                    }
                }
               // Debug.Log(newEnemy.EnemySprite);
            }
            enemyList.Add(newEnemy);
        }


        LevelEnemyInitialize();
    }

    void LevelEnemyInitialize(){
        //已经存储好路径，要新建一个path然后读取assetbundle，然后配一个prefeb
        //向组内添加纹理

        for(int i = 0 ; i < this.EnemyAmount ; i++){
            //Debug.Log(enemyList[i].EnemySprite);
            string spriteID = enemyList[i].EnemySprite;
            string textureID = enemyList[i].EnemyTexture;
            string path = Application.dataPath + "/AssetBundle/" + spriteID;
            Debug.Log(spriteID);
            AssetBundle spriteasset = AssetBundle.LoadFromFile(path);

            Sprite Enemysprite = spriteasset.LoadAsset<Sprite>(textureID);
            GameObject newEnemy = GameObject.Instantiate(Enemy);
            newEnemy.transform.parent = transform;
            //Debug.Log(spriteasset);
            newEnemy.GetComponentInChildren<SpriteRenderer>().sprite = Enemysprite;

            this.enemyList[i].enemyObject = newEnemy;
            this.enemyList[i].enemyObject.transform.position = GetWorldPositon((i + 1) * 3.2f);
            this.enemyList[i].properties = this.enemyList[i].enemyObject.GetComponent<EnemyProperties>();
            this.enemyList[i].properties.readPropertiesFromFile(this.enemyList[i].EnemyID);
            this.enemyList[i].properties.HealthBarInitial();
        }
        //初始化位置 TODO:读取敌人的属性
        switch (this.EnemyAmount) {
            case (1):
                break;
            case (2):
                break;
            case (3):
                enemyList[1].enemyObject.transform.position += new Vector3(0.3f, 1.3f, 0);
                enemyList[0].enemyObject.transform.position += new Vector3(0, -0.2f, 0);
                break;
            default:
                break;
        }
    }

    public Vector3 GetWorldPositon(float x) {
        // Debug.Log(transform.position.x);
        // Debug.Log(transform.position.y);
        return new Vector3(transform.position.x - x, 1.3f, -0.15f);
    }


    public void ChangeSelect(EnemyProperties enemy){
        for(int i = 0; i < this.EnemyAmount; i ++){
            if(enemyList[i].properties == enemy){
                PointedEnemy = i;
                //Debug.Log("选中了第"+(PointedEnemy+1)+"个敌人");
                enemyList[i].enemyObject.transform.Find("Pointed").GetComponent<SpriteRenderer>().enabled = true;
            }
            else{
                enemyList[i].enemyObject.transform.Find("Pointed").GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }
}