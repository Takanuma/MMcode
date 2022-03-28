using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]GameObject gameControl;
    [SerializeField]GameObject textObj;
    [SerializeField]GameObject textObj2;
    GameObject tempObj;
    GameControl gc;
    MainToken mt;
    

    [SerializeField]public int maxHP;
    [SerializeField]public float currentHP;
    


    // Start is called before the first frame update
    void Start(){
        maxHP = 7;
        currentHP = maxHP;
        gc = gameControl.GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update(){
        textObj.GetComponent<Text>().text = ((int)currentHP).ToString();
        textObj2.GetComponent<Text>().text = ((int)gc.defeats).ToString();
    }

    // 死亡処理
    public IEnumerator Death(){
        // 合計0.5秒
        gc.turn = 0;
        GameObject obj = Instantiate( Resources.Load("Prefabs/GameOverText", typeof(GameObject) ) ) as GameObject;
        yield return new WaitForSeconds(1.0f);
        Destroy(obj,5.0f);
        currentHP = maxHP;
        gc.defeats = 0;
        gc.SceneChanger();
        yield return new WaitForSeconds(1.5f);
        gc.ToTitle();
    }

    

}
