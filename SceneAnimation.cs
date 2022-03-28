using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SceneAnimation : MonoBehaviour
{
    
    [SerializeField]RectTransform rectTran;
    /*void Start(){


    }*/

    public void StartAnimation(int posY){
        int posY2 = 450-posY;
        int top = posY2;
        int posY3 = -(int)((posY2*0.8)-450);

        // アニメーション
        rectTran.offsetMax = new Vector2(0, top);
        rectTran.DOLocalMoveY(posY3,0.25f).SetEase(Ease.InCubic);
        rectTran.DOLocalMoveY(posY,0.5f).SetDelay(0.3f).SetEase(Ease.InCubic);
        rectTran.DOLocalMoveY(900,0.8f).SetDelay(2.0f).SetEase(Ease.InQuart);

        GameObject obj = this.gameObject.transform.parent.gameObject;
        Destroy(obj,5.0f);

        /*
        rectTran.DOLocalMoveY(posY2,0.7f).SetEase(Ease.OutSine);
        rectTran.DOLocalMoveY(posY,0.5f).SetDelay(1.0f);

        rectTran.DOLocalMoveY(posY2,0.4f).SetDelay(3.5f);
        rectTran.DOLocalMoveY(posY3,0.5f).SetDelay(4.0f).SetEase(Ease.Linear);
        //rectTran.DOLocalMoveY(0,1);
        */

    }

}
