using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MugenScript : MonoBehaviour
{
    [SerializeField]int myX;
    [SerializeField]int myY;
    [SerializeField]int myTime;
    [SerializeField]int matteyo;
    // Start is called before the first frame update
    void Start()
    {

        this.transform.DOLocalMove(new Vector3(myX, myY, 0), myTime).SetEase(Ease.Linear).SetLoops(-1,LoopType.Restart).SetDelay(matteyo);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
