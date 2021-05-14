using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update

    public Image healthNowimage;
    public Image healthEffect;
    public Text  startText;
    private Character enemy;
    void Start()
    {
        enemy = GetComponentInParent<Character>();
    }

    // Update is called once per frame
    // 下面这个地方要改成携程
    void Update()
    {
        healthNowimage.fillAmount = (float)enemy.HpNow/(float)enemy.Hp;
        //Debug.Log(enemy.HpNow/enemy.Hp);
        if(healthEffect.fillAmount>healthNowimage.fillAmount){
            healthEffect.fillAmount -= 0.003f;
        }
        else{
           healthEffect.fillAmount = healthNowimage.fillAmount;
        }
    }
}
