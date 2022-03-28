using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TitleManeger : MonoBehaviour
{
    GameObject obj;
    SceneAnimation sa;
    [SerializeField]AudioClip startSound;
    [SerializeField]RectTransform rectTran;
    GameControl gc;
    AudioSource audioSource;
    public int score_num = 0;
    int oneClick = 0;
    void Start(){
        audioSource = GetComponent<AudioSource>();
        //score_num = PlayerPrefs.GetInt("Defeats");
        //Debug.Log(score_num);
        //textObj.GetComponent<Text>().text = (score_num).ToString();

    }
    public void StartGame(){
        if(oneClick ==0){
            oneClick+=1;
            audioSource.PlayOneShot(startSound);

            obj = GameObject.Find("GameControl");
            gc = obj.GetComponent<GameControl>();
            gc.StartGame();
            gc.SetMonster();
            Destroy(this.gameObject,3.0f);
            
            rectTran.DOLocalMoveY(5000,0.8f).SetEase(Ease.InQuart);
            // obj = Instantiate( Resources.Load("Prefabs/TransitionCanvas",
            // typeof(GameObject) ) ) as GameObject;
            // sa = obj.GetComponentInChildren<SceneAnimation>();
            // sa.StartAnimation(-450);
        }
        
    }
}
