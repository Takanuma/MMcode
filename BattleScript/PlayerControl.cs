using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]GameObject gameControl;
    [SerializeField]GameObject textObj;
    [SerializeField]GameObject textObj2;
    GameControl gc;
    
    [SerializeField]public int maxHP;
    [SerializeField]public float currentHP;
    

    void Start(){
        maxHP = 7;
        currentHP = maxHP;
        gc = gameControl.GetComponent<GameControl>();
    }
    void Update(){
        int level = (int)gc.defeats + 1;
        // 体力表示
        textObj.GetComponent<Text>().text = ("体力 " + (int)currentHP).ToString();
        // 階層表示
        textObj2.GetComponent<Text>().text = ("階層" + (int)level + "階").ToString();
    }

    // 死亡処理
    public IEnumerator Death(){
        // 合計1.5秒
        gc.turn = 0;
        GameObject obj = Instantiate( Resources.Load("Prefabs/GameOverText", typeof(GameObject) ) ) as GameObject;
        yield return new WaitForSeconds(1.0f);
        Destroy(obj,5.0f);
        currentHP = maxHP;
        gc.defeats = 0;
        yield return new WaitForSeconds(0.5f);
        
        gc.ToTitle();
        gc.SceneChanger();
    }
}
