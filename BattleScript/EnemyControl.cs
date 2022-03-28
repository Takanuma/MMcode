using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyControl : MonoBehaviour
{
    [SerializeField]GameObject EnemyHpber;
    [SerializeField]GameObject gameControl;
    [SerializeField]GameObject Header;
    [SerializeField]GameObject textObj;
    [SerializeField]Image MonsterImage;
    GameObject tempObj;
    GameObject tempObj2;
    GameControl gc;
    
    MainToken mt;
    MasterMonsterModel mData;
    public int cardsCnt;
    int WaitingTime = 0;

    [SerializeField]public int maxHP;
    [SerializeField]public float currentHP;
    


    // Start is called before the first frame update
    void Start()
    {
        mData = Resources.Load<MasterMonsterModel> ("Monsters/Slime");
        MonsterImage.sprite = mData.thumbnail;
        maxHP = mData.hp;
        currentHP = maxHP;
        gc = gameControl.GetComponent<GameControl>();
    }

    // Update is called once per frame
    void Update()
    {
        textObj.GetComponent<Text>().text = ((int)currentHP).ToString();
        EnemyHpber.GetComponent<HpBarSystem>().HPDown(currentHP, maxHP);
        if(gc.waitToTouch==1)return;
        if(WaitingTime == 0 && gc.turn == 1){
            StartCoroutine(SelectCard());
            WaitingTime = 1;
        }
        if(WaitingTime == 0 && currentHP <= 0){
            StartCoroutine(Death());
            WaitingTime = 1;
        }
    }

    private IEnumerator SelectCard(){
        int temp=0;
        yield return new WaitForSeconds(0.5f);
        // カードリストの中身を整理する処理
        for(int i = 0; i < cardsCnt; i++){
            tempObj = gc.list_card[i];
            if(tempObj.GetComponent<MainToken>().IsCleared()==true){
                Debug.Log("カードけす"+gc.list_card[i]);
                gc.list_card.RemoveAt(i);
                i = -1;
                cardsCnt = gc.list_card.Count;
            }

        }
        //Debug.Log("整理後中身確認");
        for(int i = 0; i < cardsCnt; i++){
            //Debug.Log(gc.list_card[i]);
        }
        
        // AI判断
        if(gc.gameDifficulty!=1){
            StartCoroutine(SelectCardByAI());
            yield break;
        }

        // 以下弱い敵用
        // カードを選択する処理
        while(true){
            Debug.Log("1枚目のカードの選択");
            temp = Random.Range (0, cardsCnt);
            if(gc.list_card[temp]==true)break;
            
        }
        tempObj = gc.list_card[temp];
        tempObj.GetComponent<MainToken>().CheckCards();

        while(true){
            Debug.Log("2枚目のカードの選択");
            temp = Random.Range (0, cardsCnt);
            tempObj2 = gc.list_card[temp];
            if(gc.list_card[temp]==true&&tempObj!=tempObj2)break;
            
        }
        yield return new WaitForSeconds(0.5f);
        tempObj2 = gc.list_card[temp];
        tempObj2.GetComponent<MainToken>().CheckCards();
        yield return new WaitForSeconds(1.0f);
        WaitingTime=0;
    }

    private IEnumerator SelectCardByAI(){
        
        int temp=0;
        yield return new WaitForSeconds(0.5f);
        // カードリストの中身を整理する処理
        for(int i = 0; i < cardsCnt; i++){
            if(gc.list_card[i]!=true ||gc.list_card[i]==null ){
                gc.list_card.RemoveAt(i);
                i = -1;
                cardsCnt = gc.list_card.Count;
            }

        }
        Debug.Log("整理後中身確認");
        for(int i = 0; i < cardsCnt; i++){
            Debug.Log(gc.list_card[i]);
        }

        cardsCnt = gc.list_card.Count;
        Debug.Log("把握しているカードの数");
        Debug.Log(cardsCnt);

        // 見たことがあるカードを選ぶ処理
        for(int i = 0; i < cardsCnt-1; i++){
            tempObj = gc.list_card[i];
            if(tempObj.GetComponent<MainToken>().isChecked==true){
                for(int j = i+1; j < cardsCnt; j++){
                    tempObj2 = gc.list_card[j];
                    if(tempObj2.GetComponent<MainToken>().isChecked==true&&tempObj.GetComponent<MainToken>().faceIndex==tempObj2.GetComponent<MainToken>().faceIndex){
                        Debug.Log("AI発動");
                        tempObj.GetComponent<MainToken>().CheckCards();
                        yield return new WaitForSeconds(0.5f);
                        tempObj2.GetComponent<MainToken>().CheckCards();
                        yield return new WaitForSeconds(1.0f);
                        WaitingTime=0;
                        yield break;
                    }
                }
            }
        } 

        // カードを選択する処理
        while(true){
            Debug.Log("最初のカード");
            temp = Random.Range (0, cardsCnt);
            if(gc.list_card[temp]==true)break;
            
        }
        tempObj = gc.list_card[temp];
        Debug.Log(tempObj.GetComponent<MainToken>().isChecked);
        tempObj.GetComponent<MainToken>().CheckCards();
        // 見たことがあったらカードを選ぶ処理
        Debug.Log("見たことある次のカード");
        for(int i = 0; i < cardsCnt-1; i++){
            tempObj2 = gc.list_card[i];
            if(tempObj2.GetComponent<MainToken>().isChecked==true&&tempObj.GetComponent<MainToken>().faceIndex==tempObj2.GetComponent<MainToken>().faceIndex&&tempObj!=tempObj2){
                yield return new WaitForSeconds(0.5f);
                tempObj2 = gc.list_card[i];
                tempObj2.GetComponent<MainToken>().CheckCards();
                yield return new WaitForSeconds(1.0f);
                WaitingTime=0;
                yield break;
                    
            }
        }

        while(true){
            Debug.Log("次のカード");
            temp = Random.Range (0, cardsCnt);
            tempObj2 = gc.list_card[temp];
            if(gc.list_card[temp]==true&&tempObj!=tempObj2)break;
            
        }
        yield return new WaitForSeconds(0.5f);
        tempObj2 = gc.list_card[temp];
        tempObj2.GetComponent<MainToken>().CheckCards();
        yield return new WaitForSeconds(1.0f);
        WaitingTime=0;
    }

    public int SetMonster(int defeats){
        defeats+=1;
        mData = Resources.Load<MasterMonsterModel> ("Monsters/Slime");
        if(defeats == 5 && defeats != 0)mData = Resources.Load<MasterMonsterModel> ("Monsters/Banana");
        if(defeats == 15 && defeats != 0)mData = Resources.Load<MasterMonsterModel> ("Monsters/Banana2");
        if(defeats == 10 && defeats != 0)mData = Resources.Load<MasterMonsterModel> ("Monsters/Dragon");
        if(defeats >= 20 && defeats != 0)mData = Resources.Load<MasterMonsterModel> ("Monsters/Dragon2");

        //本番用データ
        /*
        //雑魚敵
        if(defeats == 1)mData = Resources.Load<MasterMonsterModel>("Monsters/FirstEnemy");
        else if(defeats > 80)mData = Resources.Load<MasterMonsterModel>("Monsters/Normal5");
        else if(defeats > 60)mData = Resources.Load<MasterMonsterModel>("Monsters/Normal4");
        else if(defeats > 40)mData = Resources.Load<MasterMonsterModel>("Monsters/Normal3");
        else if(defeats > 20)mData = Resources.Load<MasterMonsterModel>("Monsters/Normal2");
        else if(defeats > 1)mData = Resources.Load<MasterMonsterModel>("Monsters/Normal1");

        //中ボス
        if(defeats == 95)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss9");
        else if(defeats == 85)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss8");
        else if(defeats == 75)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss7");
        else if(defeats == 65)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss6");
        else if(defeats == 55)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss5");
        else if(defeats == 45)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss4");
        else if(defeats == 35)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss3");
        else if(defeats == 25)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss2");
        else if(defeats == 15)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss2");
        else if(defeats ==  5)mData = Resources.Load<MasterMonsterModel>("Monsters/MiniBoss1");

        //ボス
        if(defeats == 100)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss10");
        else if(defeats == 90)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss9");
        else if(defeats == 80)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss8");
        else if(defeats == 70)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss7");
        else if(defeats == 60)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss6");
        else if(defeats == 50)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss5");
        else if(defeats == 40)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss4");
        else if(defeats == 30)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss3");
        else if(defeats == 20)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss2");
        else if(defeats == 10)mData = Resources.Load<MasterMonsterModel>("Monsters/Boss1");

        //エンドコンテンツ
        if(defeats > 100)mData = Resources.Load<MasterMonsterModel>("Monsters/Mugen");
        */

        MonsterImage.sprite = mData.thumbnail;
        maxHP = mData.hp;
        currentHP = maxHP;
        return mData.strength;
    }

    private IEnumerator Death(){
        // 合計0.5秒
        GameObject obj = Instantiate( Resources.Load("Prefabs/ClearText", typeof(GameObject) ) ) as GameObject;
        Destroy(obj,2.8f);
        
        obj = Instantiate( Resources.Load("Prefabs/NowLoading",
        typeof(GameObject) ) ,Header.transform) as GameObject;
        Destroy(obj,2.0f);


        yield return new WaitForSeconds(0.1f);        
        StartCoroutine(gc.GameClear());
        yield return new WaitForSeconds(0.1f);
        gc.SetMonster();
        gc.ResetGame();
        yield return new WaitForSeconds(0.5f);
        WaitingTime = 0;
    }

    

}
