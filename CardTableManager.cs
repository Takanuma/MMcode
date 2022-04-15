using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardTableManager : MonoBehaviour{
    GridLayoutGroup grid;
    [SerializeField]GameControl gc;
    GameObject kore;
    // Start is called before the first frame update
    void Start(){
        grid = GetComponent<GridLayoutGroup>();
        kore =this.gameObject;
    }

    void Update(){
        //Debug.Log("Screen Width : " + Screen.width);
        //Debug.Log("Screen  height: " + Screen.height);
        Debug.Log("大きさw:"+gameObject.GetComponent<RectTransform> ().rect);
        int diff = gc.gameDifficulty;
        float scRatio = (float)Screen.width/(float)Screen.height;
        if(diff==1){
            grid.constraintCount=2;
            grid.cellSize = new Vector2(400, 400);
        }else if(diff==2){
            grid.constraintCount=3;
            if(scRatio<=0.67f){
                grid.cellSize = new Vector2(400, 400);
            }else if(scRatio<=0.8f){
                grid.cellSize = new Vector2(300, 300);
            }else{
                grid.constraintCount=2;
                grid.cellSize = new Vector2(200, 200);
            }
        }else if(diff==3){
            if(scRatio>=0.75f){
                grid.constraintCount=2;
                grid.cellSize = new Vector2(200, 200);
            }else if(scRatio<=0.57f){
                grid.constraintCount=4;
                grid.cellSize = new Vector2(400, 400);
            }else if(scRatio<=0.68f){
                grid.constraintCount=4;
                grid.cellSize = new Vector2(300, 300);
            }else if(scRatio<=0.75f){
                grid.constraintCount=4;
                grid.cellSize = new Vector2(250, 250);
            }
        }
    }
}