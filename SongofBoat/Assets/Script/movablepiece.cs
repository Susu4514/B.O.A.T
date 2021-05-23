using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//可以移动的方块或者是能量球
//会被挂上这个组件
//虽然现在没有涉及到不可移动的物体，但是为了拓展，将这个功能作为一个组件来处理。
public class movablepiece : MonoBehaviour
{
    private gamePiece piece;
    private IEnumerator moveCoroutine;
    // Start is called before the first frame update
    private void Awake() {
        piece = GetComponent<gamePiece>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(float newx , float newy, float time) {
        /*piece.X = newx;
        piece.Y = newy;
        piece.transform.localPosition = piece.GridRef.GetWorldPositon(newx, newy);*/
        if (moveCoroutine != null) {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = MoveCoroutine(newx,newy,time);
        StartCoroutine(moveCoroutine);
    }

    private IEnumerator MoveCoroutine(float newx, float newy, float time) {
        piece.X = (int)newx;
        piece.Y = (int)newy;

        Vector3 startPos = transform.position;
        Vector3 endPos = piece.GridRef.GetWorldPositon(newx, newy);

        for(float t = 0; t <= 1 * time; t += Time.deltaTime) {
            piece.transform.position = Vector3.Lerp(startPos, endPos, t / time);
            yield return 0;
        }

        piece.transform.position = endPos;
    }
}
