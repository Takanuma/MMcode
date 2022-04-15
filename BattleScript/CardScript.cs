using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CardScript : MonoBehaviour
{
    public RawImage cardImage;
    public Texture[] faces;
    public Texture back;
    public Texture cleared;
    public int faceIndex;
    public bool isMatched = false;
    public bool isChecked = false;
    public bool isFront = false;

    // 効果音
    public AudioClip sound1;
    AudioSource audioSource;

    //外部ソース
    GameObject gameControl;
    GameControl gc;


    void Start () {
        //Componentを取得
        audioSource = GetComponent<AudioSource>();
        gameControl = GameObject.Find("GameControl");
        gc = gameControl.GetComponent<GameControl>();

    }
    
    // カードをクリック時の処理
    public void OnClick(){
        Debug.Log("クリック");
        if(isFront == true){ return; }
        if(gc.turn != 0){ return; }
        if(gc.checkTouching){ return;}
        Debug.Log("クリック条件達成");
        CheckCards();
    }

    // カードの状態を確認
    public void CheckCards(){
        StartCoroutine(SetFront());
        gameControl.GetComponent<GameControl>().AddVisibleFace(faceIndex,this.gameObject);
        gameControl.GetComponent<GameControl>().CheckMatch(faceIndex,this.gameObject);
    }

    // カードを表にセットする関数
    private IEnumerator SetFront(){
        isFront = true;
        isChecked = true;
        //ローカル軸に対して回転
        audioSource.PlayOneShot(sound1);
        transform.DORotate(new Vector3(0,90,0), 0.2f, RotateMode.LocalAxisAdd);  
        transform.DORotate(new Vector3(0,270,0), 0.3f, RotateMode.LocalAxisAdd).SetDelay(0.2f).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.2f);
        cardImage.texture = faces[faceIndex];
    }
    // カードを裏にセットする関数
    public IEnumerator SetBack(){
        transform.DORotate(new Vector3(0,360,0), 0.3f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.1f);
        isFront = false;
        cardImage.texture = back;
    }
    // カードが揃った時に削除する処理関数
    public void VanishCard(){
        isMatched = true;
        transform.DORotate(new Vector3(0,90,0), 0.2f, RotateMode.LocalAxisAdd).OnComplete(() =>
        {
            cardImage.texture = cleared;
        });
    }

}