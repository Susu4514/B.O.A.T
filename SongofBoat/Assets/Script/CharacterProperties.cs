using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterProperties : MonoBehaviour {
    // Start is called before the first frame update

    //人物基础属性，所有生物都有这些属性
    private int Hp;
    private int Shield;
    private int Damage;
    private int Activity;
    private int Critical;
    private int Agile;

    private Dictionary<int, CsvDemo> csvDataDic;
    static private string filepath = Application.streamingAssetsPath + "/CharacterConfig.csv";
    string[] fileData = File.ReadAllLines(filepath);

    public class CsvDemo {
        public int ID { get; set; }
        public string Introdction { get; set; }
        public int Hp { get; set; }
        public int Shield { get; set; }
        public int Damage { get; set; }
        public int Activity { get; set; }
        public int Critical { get; set; }
        public int Agile { get; set; }
    }
    void Start()
    {
        //keys是说明字段
        string[] keys = fileData[0].Split(',');

        csvDataDic = new Dictionary<int, CsvDemo>();

        for (int i = 2; i < fileData.Length; i++) {
            /* 每一行的内容都是逗号分隔，读取每一列的值 */
            string[] lineData = fileData[i].Split(',');

            CsvDemo csvDemo = new CsvDemo();

            //csv类对应
            for (int j = 0; j < lineData.Length; j++) {
                if (keys[j] == "ID") {
                    csvDemo.ID = Convert.ToInt32(lineData[j]);
                }
                else if (keys[j] == "Introdction") {
                    csvDemo.Introdction = lineData[j];
                }
                else if (keys[j] == "Hp") {
                    //   Debug.Log(lineData[j]);
                    csvDemo.Hp = Convert.ToInt32(lineData[j]);
                }
                else if (keys[j] == "Shield") {
                    csvDemo.Shield = Convert.ToInt32(lineData[j]);
                }
                else if (keys[j] == "Damage") {
                    csvDemo.Damage = Convert.ToInt32(lineData[j]);
                }
                else if (keys[j] == "Activity") {
                    csvDemo.Activity = Convert.ToInt32(lineData[j]);
                }
                else if (keys[j] == "Critical") {
                    csvDemo.Critical = Convert.ToInt32(lineData[j]);
                }
                else if (keys[j] == "Agile") {
                    csvDemo.Agile = Convert.ToInt32(lineData[j]);
                }
            }
          //  Debug.Log(csvDemo.ID);
            if (!csvDataDic.ContainsKey(csvDemo.ID)){
                csvDataDic.Add(csvDemo.ID, csvDemo);
            }
        }

        foreach (KeyValuePair<int,CsvDemo> pair in csvDataDic) {
           Debug.Log(pair.Key+"  " + pair.Value);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
