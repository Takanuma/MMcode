using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    [SerializeField]GameObject EnemyControl;
    [SerializeField]GameObject PlayerControl;
    [SerializeField]GameObject CardTable;
    GameObject[] cards;
    PlayerControl pc;
    EnemyControl ec;
    GameObject firstCard = null;
    GameObject obj;
    GameObject canvas;
    GameObject sceneChanger;

    // 効果音,BGM用変数
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    SpriteRenderer sr;
    CardScript CardSC;
    List<int> faceIndexes = new List<int> {0, 1, 2, 3, 0, 1, 2, 3};

    public int turn = 0;
    public int gameDifficulty = 1;
    public bool checkTouching = false;
    [SerializeField]public int defeats = 0;

    private static System.Random rnd = new System.Random();
    private int shuffleNum = 0;
    private int clearCount = 0;
    private int matchCombo = 0;
    private int originalCnt;
    private int[] visibleFaces = {-1, -2};

    // カードを一元管理するリスト
    public List<GameObject> list_card = new List<GameObject>();

    
    // スタート
    void Start(){
        canvas = GameObject.Find("GameCanvas");
        obj = Instantiate( Resources.Load("Prefabs/Title",
        typeof(GameObject) ) ,canvas.transform) as GameObject;
        audioSource = GetComponent<AudioSource>();
        ec = EnemyControl.GetComponent<EnemyControl>();
        pc = PlayerControl.GetComponent<PlayerControl>();
    }

    // 最初にゲームを始める処理
    public void StartGame(){
        SetGame();
    }

    // ゲームの変数を初期化する処理
    public void ResetGame(){
        turn = 0;
        matchCombo = 0;
    }

    // タイトルに戻る処理
    public void ToTitle(){
        cards = GameObject.FindGameObjectsWithTag("card");
        foreach(GameObject clonecard in cards){
            Destroy(clonecard);
        }
        obj = Instantiate( Resources.Load("Prefabs/Title",
        typeof(GameObject) ) ,canvas.transform) as GameObject;
        //canvas = GameObject.Find("GameCanvas");
        clearCount = 0;
        turn = 0;
        rnd = new System.Random();
        faceIndexes = new List<int> {0, 1, 2, 3, 0, 1, 2, 3};
        ec.currentHP = ec.maxHP;
        pc.currentHP = pc.maxHP;
    }

    // カード設置用関数
    public void SetGame(){
        StartCoroutine(SetCards());
    }
    //カードを設置
    IEnumerator SetCards(){
        //合計時間1.4秒
        yield return new WaitForSeconds(0.3f);
        clearCount = 0;
        rnd = new System.Random(); 
        list_card.Clear();
        Debug.Log(gameDifficulty);
        yield return new WaitForSeconds(0.5f);
        Debug.Log("リセット");
        cards = GameObject.FindGameObjectsWithTag("card");
        foreach(GameObject clonecard in cards){
            Destroy(clonecard);
        }
        yield return new WaitForSeconds(0.1f);
        // 難易度による枚数変更
        if(gameDifficulty == 1){
            faceIndexes = new List<int> {0, 1, 2, 3, 0, 1, 2, 3};
        }else if(gameDifficulty == 2){
            faceIndexes = new List<int> {0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5};
        }else if(gameDifficulty == 3){
            faceIndexes = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7};
        }

        originalCnt = faceIndexes.Count;
        ec.cardsCnt = originalCnt;
        yield return new WaitForSeconds(0.5f);
        
        for (int i = 0; i < originalCnt; i++)
        {
            shuffleNum = rnd.Next(0, (faceIndexes.Count));
            var temp = Instantiate( Resources.Load("Prefabs/Card")) as GameObject;
            temp.transform.SetParent(CardTable.transform);
            // z軸に位置を手前にずらす
            temp.GetComponent<RectTransform> ().localPosition = new Vector3(0, 0, 0);
            temp.GetComponent<RectTransform> ().localScale = new Vector3(1, 1, 1);
            //Debug.Log(temp.GetComponent<RectTransform> ().localPosition.z);
            list_card.Add(temp);

            temp.GetComponent<CardScript>().faceIndex = faceIndexes[shuffleNum];
            faceIndexes.Remove(faceIndexes[shuffleNum]);
        }
        visibleFaces[0] = -1;
        visibleFaces[1] = -2;
    }


    // 2枚めくられているか判定
    public bool TwoCardsUp(){
        bool cardsUp = false;
        if (visibleFaces[0] >= 0 && visibleFaces[1] >= 0)
        {
            cardsUp = true;
        }
        return cardsUp;
    }
    // めくられたカードの情報を保持
    public void AddVisibleFace(int index, GameObject thisCard){
        if(visibleFaces[0] == -1)
        {
            firstCard = thisCard;
            visibleFaces[0] = index;
        }
        else if (visibleFaces[1] == -2)
        {
            visibleFaces[1] = index;
        }
        
    }
    // めくられたカードの情報を削除
    public void RemoveVisibleFace(){
        visibleFaces[0] = -1;
        visibleFaces[1] = -2;
    }

    // カードが合っているか確認
    public bool CheckMatch(int index, GameObject thisCard){
        bool success = false;
        if(visibleFaces[0] == -1 || visibleFaces[1] == -2){
            // 2枚めくられていない場合
        }else if (visibleFaces[0] == visibleFaces[1])
        {   
            // 2枚とも一致する場合
            StartCoroutine(MatchCards(thisCard));
            success = true;
        }else{
            // 不正解
            StartCoroutine(GappingCards(thisCard));
        }
        return success;
    }

    // 敵を倒した時の処理
    public IEnumerator GameClear(){
        //int tmp = PlayerPrefs.GetInt("Defeats");
        defeats += 1;
        yield return new WaitForSeconds(0.1f);  //0.5秒待つ    
        //if(Defeats > tmp)PlayerPrefs.SetInt("Defeats",Defeats);
        //PlayerPrefs.Save();
        //audioSource.PlayOneShot(sound1);
        //obj = Instantiate( Resources.Load("ClearText", typeof(GameObject) ) ) as GameObject;
        //Destroy(obj,2.8f);
    }
    // カードが合っていた時の削除処理
    private IEnumerator MatchCards(GameObject thisCard){
        Debug.Log("カードが一致");
        RemoveVisibleFace();
        clearCount += 1;

        checkTouching = true;
        yield return new WaitForSeconds(1.0f);
        VanishCards(thisCard);
        StartCoroutine(Damage());
        yield return new WaitForSeconds(0.7f);
        checkTouching = false;
        if(CheckCardTable() == true){
            yield return new WaitForSeconds(1.5f);
            if(turn == 1)StartCoroutine(ec.SelectCard());
        }else if(turn == 1){
            StartCoroutine(ec.SelectCard());
        }
    }
    // (正解用)カード削除処理
    private void VanishCards(GameObject thisCard){
        audioSource.PlayOneShot(sound2);
        CardSC = thisCard.GetComponent<CardScript>();
        CardSC.VanishCard();
        CardSC = firstCard.GetComponent<CardScript>();
        CardSC.VanishCard();
    }
    //カードの枚数確認
    private bool CheckCardTable(){
        // カードが無くなっていた場合、カードを配る。
        if(pc.currentHP <= 0){
            StartCoroutine(pc.Death());
        }else if(ec.currentHP <= 0){
        }else if(clearCount > 3 && gameDifficulty == 1){
            SetGame();
            return true;
        }else if(clearCount > 5 && gameDifficulty == 2){
            SetGame();
            return true;
        }else if(clearCount > 7 && gameDifficulty == 3){
            SetGame();
            return true;
        }
        return false;
    }
    // カードが違っていた時の削除処理
    private IEnumerator GappingCards(GameObject thisCard){
        Debug.Log("カード一致せず");
        checkTouching = true;
        ChangeTurn();
        yield return new WaitForSeconds(1.0f);
        BackCards(thisCard);
        RemoveVisibleFace();
        yield return new WaitForSeconds(0.7f);
        checkTouching = false;
        matchCombo = 0;

        if(turn==1)StartCoroutine(ec.SelectCard());
    }
    // (間違っていた用)カード裏返し処理
    public void BackCards(GameObject thisCard){
        Debug.Log("裏返し処理中");
        CardSC = thisCard.GetComponent<CardScript>();
        StartCoroutine(CardSC.SetBack());
        CardSC = firstCard.GetComponent<CardScript>();
        StartCoroutine(CardSC.SetBack());
    }
    // 敵と味方のターン変更
    private void ChangeTurn(){
        if(turn == 0){
            turn = 1;
            Debug.Log("敵の"+turn);
        }else if(turn == 1){
            turn = 0;
            Debug.Log("味方の"+turn);
        }
    }

    // ダメージ処理
    private IEnumerator Damage(){
        yield return new WaitForSeconds(0.2f);  //0.5秒待つ
        // ダメージ判定
        if(turn == 0){
            ec.currentHP -= 1f;
            matchCombo += 1;
        }else if(turn == 1){
            pc.currentHP -= 1f;
        }
    }

    private void Heal(){
        pc.currentHP += 1f;
    }
    public void SceneChanger(){
        sceneChanger = Instantiate( Resources.Load("Prefabs/NowLoading",
        typeof(GameObject) ) ,canvas.transform) as GameObject;
        Destroy(sceneChanger, 3.0f);
    }
    public void SetMonster(){
        gameDifficulty = ec.SetMonster(defeats);
        Heal();
    }

}
