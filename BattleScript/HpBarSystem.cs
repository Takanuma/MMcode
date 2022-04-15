using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HpBarSystem : MonoBehaviour {

    GameObject image;
    [SerializeField]
    Image hpImageFront;
    [SerializeField]
    Image hpImageBack;
    
	void Start () {
        //ImageをGameObjectとして取得
        image = GameObject.Find("Image");
	}
	
    //()の中身は引数、他のところから数値を得て{}の中で使う
	public void HPDown (float current, int max) {
        //ImageというコンポーネントのfillAmountを取得して操作する
            HPDownSlow(current,max);
            hpImageFront.DOFillAmount(current / max,0.7f);
    }
    private void HPDownSlow(float current, int max){
        hpImageBack.DOFillAmount(current / max,2.0f);
    }
}