using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 
public class PlayerMove : MonoBehaviour
{
    public float turnSpeed = 20f;
 
    public VariableJoystick variableJoystick;
 
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity; //初始化旋转角度
    Animator m_Animator; 
 
    float speedIndex = 6f;
    Rigidbody m_Rigidbody; 
    AudioSource m_AudioSource;

    private Vector3 leftV3 = new Vector3(30, 30, 0);
    private Vector3 rightV3 = new Vector3(-30, 210, 0);

    //private float horizontalyet = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_Animator = GetComponentInChildren<Animator>();//获得动画组件
        m_Rigidbody = GetComponent<Rigidbody>();
        m_AudioSource = GetComponent<AudioSource>();//获得刚体组件

        Debug.Log(m_Animator);
    }
 
 
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

 
        //判断如果没有输入再获取摇杆的值
        horizontal = horizontal == 0 ? variableJoystick.Horizontal : horizontal;
        vertical = vertical == 0 ? variableJoystick.Vertical : vertical;
 
        m_Movement.Set(horizontal, 0f, vertical);
        m_Movement.Normalize();
        m_Movement.x /= speedIndex;
        m_Movement.y /= speedIndex;
        m_Movement.z /= speedIndex;


        //方法接受两个 float 参数，并返回布尔值；
        //如果两个 float 数值大致相等，则返回 true，
        //否则返回 false。
        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalINput = !Mathf.Approximately(vertical, 0f);
 
        //根据水平和垂直数值，如果有一个移动就代表着行走
        bool isWalking = hasHorizontalInput || hasVerticalINput;

        bool isMirror = (horizontal >= 0 && vertical != 0) ? true : false;

        //Debug.Log(isWalking);
        if(isWalking){
            m_Animator.SetInteger("AnimState", 1);
            if (isMirror){
                transform.Find("Sprite").transform.rotation = Quaternion.Euler(rightV3);
                
            }
            else{
                transform.Find("Sprite").transform.rotation = Quaternion.Euler(leftV3);
            }
        }
        else{
            m_Animator.SetInteger("AnimState",0);
        }
        // if (isWalking)
        // {
        //     if (!m_AudioSource.isPlaying)
        //     {
        //         m_AudioSource.Play();
        //     }
        // }
        // else
        // {
        //     m_AudioSource.Stop();
        // }
 
 
        //RotateTowards 接受四个参数：前两个是 Vector3，分别是旋转时背离和朝向的矢量。
        //接下来的两个参数是起始矢量和目标矢量之间的变化量：首先是角度变化（以弧度为单位），然后是大小变化。
        Vector3 desiredForward = Vector3.RotateTowards(transform.forward,
            m_Movement, turnSpeed * Time.deltaTime, 0f);
 
        m_Rotation = Quaternion.LookRotation(desiredForward);

        m_Rigidbody.MovePosition(m_Rigidbody.position + m_Movement /* m_Animator.deltaPosition.magnitude*/);
 
 
        //m_Rigidbody.MoveRotation(m_Rotation);
    }
 
 
 
 
    // void OnAnimatorMove()
    // {
    //     //Animator 的 deltaPosition 是由于可以应用于此帧的根运动而导致的位置变化


    // }
}
