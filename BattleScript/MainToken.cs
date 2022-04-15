using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MainToken : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    GameObject gameControl;
    GameControl gc;

    public Sprite[] faces;
    public Sprite back;
    public Sprite cleared;
    public int faceIndex;
    public bool matched = false;
    public bool isChecked = false;
    private int stopTouch=0;

    public AudioClip sound1;
    AudioSource audioSource;

    void Start () {
    //Componentを取得
        audioSource = GetComponent<AudioSource>();
        gc = gameControl.GetComponent<GameControl>();

    }

    void Update(){
        // scale の大きさ変更
        Vector3 kero = this.gameObject.transform.localScale;
        kero.x = 60 * 0.5f * Screen.height / Screen.width;
        kero.y = 60 * 0.5f * Screen.height / Screen.width;
        this.gameObject.transform.localScale = kero;
    }
    
    void OnMouseEnter() {
        //if (spriteRenderer.sprite == back && gc.turn == 0) this.spriteRenderer.DOColor(Color.red, 0);
        //transform.DOScale(new Vector3(30,30,30), 0.2f);

    }
    void OnMouseExit() {
        //this.spriteRenderer.DOColor(Color.white, 0);
        //transform.DOScale(new Vector3(27,27,27), 0.2f);

    }
    
    public void OnMouseDown()
    {  
        if(EventSystem.current.currentSelectedGameObject != null){ return; }
        if(EventSystem.current.IsPointerOverGameObject()){ return; }
        //this.spriteRenderer.DOColor(Color.white, 0);
        if(stopTouch == 0 && gc.turn == 0)CheckCards();
    }

    public bool IsCleared(){
        if(spriteRenderer.sprite == cleared)return true;
        return false;
    }

    public void CheckCards(){
        float tmp = 0;
        tmp = 0.52f;

        StartCoroutine(stopTouchMe(tmp));

        if (matched != false &&  gameControl.GetComponent<GameControl>().checkTouching == false) return;
        if (spriteRenderer.sprite != back) return;
        if (gameControl.GetComponent<GameControl>().TwoCardsUp() == false){   
            audioSource.PlayOneShot(sound1);
            StartCoroutine(CheckFaces());
            gameControl.GetComponent<GameControl>().AddVisibleFace(faceIndex,this.gameObject);
            matched = gameControl.GetComponent<GameControl>().CheckMatch(faceIndex,this.gameObject);
            isChecked = true;
        }

    }

    public void BackCard(){
        float tmp = 0.42f;
        
        StartCoroutine(SetBackCard());
        StartCoroutine(stopTouchMe(tmp));
    }

    public void VanishCard(){
        // 合計0.2秒
        float tmp = 0.42f;
        StartCoroutine(stopTouchMe(tmp));
        transform.DORotate(new Vector3(0,90,0), 0.2f, RotateMode.LocalAxisAdd).OnComplete(() =>
        {
            spriteRenderer.sprite = cleared;
        });
    }

    private IEnumerator SetBackCard(){
        // 合計0.2秒
        transform.DORotate(new Vector3(0,360,0), 0.3f, RotateMode.LocalAxisAdd).SetEase(Ease.OutSine);
        yield return new WaitForSeconds(0.1f);
        //消した//gameControl.GetComponent<GameControl>().RemoveVisibleFace(this.faceIndex);
        spriteRenderer.sprite = back;
    }

    private IEnumerator stopTouchMe(float tmp){
        stopTouch += 1;
        yield return new WaitForSeconds(tmp);
        stopTouch -= 1;
    }

    void Awake(){
        gameControl = GameObject.Find("GameControl");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator CheckFaces(){
        // 合計0.5秒
        transform.DORotate(new Vector3(0,90,0), 0.2f, RotateMode.LocalAxisAdd);  //ローカル軸に対して
        transform.DORotate(new Vector3(0,270,0), 0.3f, RotateMode.LocalAxisAdd).SetDelay(0.2f).SetEase(Ease.OutSine);  //ローカル軸に対して
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.sprite = faces[faceIndex];
    }


}
