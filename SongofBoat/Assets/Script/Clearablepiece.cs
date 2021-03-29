using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clearablepiece : MonoBehaviour
{
    // Start is called before the first frame update
    public AnimationClip clearAnimation;

    private bool isCleared = false;

    public bool IsCleared {
        get{ return isCleared; }
    }

    protected gamePiece piece;

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

    public void clear() {
        isCleared = true;
        StartCoroutine(ClearCoroutine());

    }
    private IEnumerator ClearCoroutine() {
        Animator animator = GetComponent<Animator>();
        if (animator) {
            animator.Play(clearAnimation.name);

            yield return new WaitForSeconds(clearAnimation.length);

            Destroy(gameObject);
        }
    }
}
