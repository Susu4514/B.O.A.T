using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatPointBase : MonoBehaviour
{
    // Start is called before the first frame update
    public float Destroytime;
    void Start()
    {
        Destroy(gameObject, Destroytime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
