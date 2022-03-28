using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class ButtonScript : MonoBehaviour
{
    Image thisImage;
    [SerializeField]RectTransform rectTran;
    public Sprite normal;
    public Sprite pressed;
    // Start is called before the first frame update
    void Start()
    {
        thisImage=this.gameObject.GetComponent<Image>();
    }

    public void Pressed(){
        //rectTran.DOLocalMoveY(0,0);
        thisImage.sprite = pressed;
        
    }

    public void Normalized(){
        thisImage.sprite = normal;
        //rectTran.DOLocalMoveY(50,0);
    }    
}