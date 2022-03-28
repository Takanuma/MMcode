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
    GameObject token;
    GameObject firsttoken = null;
    GameObject obj;
    GameObject canvas;
    GameObject sceneChanger;

    // 効果音,BGM用変数
    public AudioClip sound1;
    public AudioClip sound2;
    AudioSource audioSource;

    SpriteRenderer sr;
    MainToken TokenScript;
    List<int> faceIndexes = new List<int> {0, 1, 2, 3, 0, 1, 2, 3};

    public static System.Random rnd = new System.Random();
    public int shuffleNum = 0;
    public int WaitToTouch = 0;
    public int clearCount = 0;
    public int originalCnt;
    int[] visibleFaces = {-1, -2};
    public int turn = 0;
    public int gameDifficulty = 1;
    [SerializeField]public int defeats = 0;

    // カードを一元管理するリスト
    public List<GameObject> list_card = new List<GameObject>();

    
    // スタート
    void Start(){
        //StartCoroutine(SetGame());
        canvas = Instantiate( Resources.Load("Prefabs/TitleCanvas",
        typeof(GameObject) ) ) as GameObject;
        canvas = GameObject.Find("GameCanvas");
        audioSource = GetComponent<AudioSource>();
        ec = EnemyControl.GetComponent<EnemyControl>();
        pc = PlayerControl.GetComponent<PlayerControl>();
    }

    // 最初にゲームを始める処理
    public void StartGame(){
        SetGame();
    }

    // ゲームをリセットする処理
    public void ResetGame(){
        StartCoroutine(ResetGames());
    }

    // タイトルに戻る処理
    public void ToTitle(){
        cards = GameObject.FindGameObjectsWithTag("card");
        foreach(GameObject clonecard in cards){
            Destroy(clonecard);
        }
        canvas = Instantiate( Resources.Load("Prefabs/TitleCanvas",
        typeof(GameObject) ) ) as GameObject;
        //canvas = GameObject.Find("GameCanvas");
        clearCount = 0;
        turn = 0;
        rnd = new System.Random();
        faceIndexes = new List<int> {0, 1, 2, 3, 0, 1, 2, 3};
        ec.currentHP = ec.maxHP;
        pc.currentHP = pc.maxHP;
    }

    // ゲームをリセットする処理
    IEnumerator ResetGames(){
        Debug.Log("リセット");
        turn = 0;
        cards = GameObject.FindGameObjectsWithTag("card");
        foreach(GameObject clonecard in cards){
            Destroy(clonecard);
        }

        yield return new WaitForSeconds(0.1f);
        SetGame();
        
    }
    
    // カード設置
    public void SetGame(){
        clearCount = 0;
        rnd = new System.Random(); 
        list_card.Clear();
        Debug.Log(gameDifficulty);
        StartCoroutine(SetCards());

    }

    IEnumerator SetCards(){
        yield return new WaitForSeconds(0.5f);
        float xPosition = -0.3f;
        if(gameDifficulty == 1){
            faceIndexes = new List<int> {0, 1, 2, 3, 0, 1, 2, 3};
            xPosition = -3.0f;
        }else if(gameDifficulty == 2){
            faceIndexes = new List<int> {0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5};
            xPosition = -5.0f;
        }else if(gameDifficulty == 3){
            faceIndexes = new List<int> {0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3, 4, 5, 6, 7};
            xPosition = -7.0f;
        }

        originalCnt = faceIndexes.Count;
        ec.cardsCnt = originalCnt;
        float yPosition = 1.0f;
        
        yield return new WaitForSeconds(0.5f);  //10秒待つ
        
        for (int i = 0; i < originalCnt; i++)
        {
            shuffleNum = rnd.Next(0, (faceIndexes.Count));
            // Set card face index
            var temp = Instantiate( Resources.Load("Prefabs/Token")) as GameObject;
            /*var temp = Instantiate( Resources.Load("Prefabs/Token",
                typeof(GameObject) ),new Vector3(xPosition, yPosition, 0), 
                Quaternion.identity) as GameObject;*/
            //temp.name = token.name;
            temp.transform.SetParent(CardTable.transform);
            temp.GetComponent<RectTransform> ().localPosition = new Vector3(0, 0, -10);
            //Debug.Log(temp.GetComponent<RectTransform> ().localPosition.z);
            list_card.Add(temp);

            temp.GetComponent<MainToken>().faceIndex = faceIndexes[shuffleNum];
            faceIndexes.Remove(faceIndexes[shuffleNum]);
            xPosition = xPosition + 2;
            if(i+1 == (originalCnt)/2)
            {
                if(gameDifficulty == 1){
                    xPosition = -3.0f;
                }else if(gameDifficulty == 2){
                    xPosition = -5.0f;
                }else if(gameDifficulty == 3){
                    xPosition = -7.0f;
                }
                yPosition = -1.0f;
            }
        }
        //token.GetComponent<MainToken>().faceIndex = faceIndexes[0];
        visibleFaces[0] = -1;
        visibleFaces[1] = -2;
    }
        // 元ソース
    IEnumerator SetGameEasy(){
        faceIndexes = new List<int> {0, 1, 2, 3, 0, 1, 2, 3};
        int originalCnt = faceIndexes.Count;
        float yPosition = 1.0f;
        float xPosition = -1.0f;
        yield return new WaitForSeconds(0.1f);  //10秒待つ
        token = Instantiate( Resources.Load("Prefabs/Token",
        typeof(GameObject) ) ) as GameObject;
        token.transform.parent = CardTable.transform;
        list_card.Add(token);
        //token.transform.parent = canvas.transform;
        
        for (int i = 0; i < 7; i++)
        {
            shuffleNum = rnd.Next(0, (faceIndexes.Count));
            // Set card face index

            var temp = Instantiate( Resources.Load("Prefabs/Token",
                typeof(GameObject) ),new Vector3(xPosition, yPosition, 0), 
                Quaternion.identity ) as GameObject;
            temp.name = token.name;
            temp.transform.parent = CardTable.transform;
            list_card.Add(temp);
            //temp.transform.parent = canvas.transform;

            temp.GetComponent<MainToken>().faceIndex = faceIndexes[shuffleNum];
            faceIndexes.Remove(faceIndexes[shuffleNum]);
            xPosition = xPosition + 2;
            if(i == (originalCnt/2 - 2))
            {
                yPosition = -1.0f;
                xPosition = -3.0f;
            }
        }
        token.GetComponent<MainToken>().faceIndex = faceIndexes[0];
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
    public void AddVisibleFace(int index, GameObject thistoken){
        if(visibleFaces[0] == -1)
        {
            firsttoken = thistoken;
            visibleFaces[0] = index;
        }
        else if (visibleFaces[1] == -2)
        {
            visibleFaces[1] = index;
        }
        
    }
    // めくられたカードの情報を削除
    public void RemoveVisibleFace(int index){
        if (visibleFaces[0] == index)
        {
            visibleFaces[0] = -1;
        }
        else if(visibleFaces[1] == index)
        {
            visibleFaces[1] = -2;
        }
    }

    // カードが合っているか確認
    public bool CheckMatch(int index, GameObject thistoken){
        bool success = false;
        if (visibleFaces[0] == visibleFaces[1])
        {   
            // 正解
            StartCoroutine(VanishCards(thistoken));

            success = true;
   
        }else if(visibleFaces[0] == -1 || visibleFaces[1] == -2){
            // 2枚めくれていない場合
        }else{
            // 不正解
            StartCoroutine(BackCards(thistoken));
        }
        
        //盤面クリア判定
        if(clearCount > 3&&gameDifficulty==1){
            SetGame();
        }else if(clearCount > 5&&gameDifficulty==2){
            SetGame();
        }else if(clearCount > 7&&gameDifficulty==3){
            SetGame();
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
    
    // カードが正解だった時の削除処理
    public IEnumerator VanishCards(GameObject thistoken){
        Debug.Log("カード削除開始");
        clearCount += 1;
        Destroy(thistoken, 1.0f);
        Destroy(firsttoken, 1.0f);
        WaitToTouch = 1;

        yield return new WaitForSeconds(0.7f);  //0.7秒待つ
        audioSource.PlayOneShot(sound2);
        TokenScript = thistoken.GetComponent<MainToken>();
        TokenScript.VanishCard();
        TokenScript = firsttoken.GetComponent<MainToken>();
        TokenScript.VanishCard();
        StartCoroutine(Damage());

        visibleFaces[0] = -1;
        visibleFaces[1] = -2;
        WaitToTouch = 0;
    }

    // ダメージ処理
    private IEnumerator Damage(){
        yield return new WaitForSeconds(0.2f);  //0.5秒待つ
        // ダメージ判定
        if(turn == 0){
            ec.currentHP -= 1f;
        }else if(turn == 1){
            pc.currentHP -= 1f;
        }

        if(pc.currentHP <= 0)StartCoroutine(pc.Death());
    }

    void Heal(){
        pc.currentHP += 1f;
    }

    // カードの裏返し処理
    public IEnumerator BackCards(GameObject thistoken){
        Debug.Log("カード裏返し開始");
        WaitToTouch = 1;
        yield return new WaitForSeconds(0.7f);  //0.5秒待つ
        TokenScript = thistoken.GetComponent<MainToken>();
        TokenScript.BackCard();
        TokenScript = firsttoken.GetComponent<MainToken>();
        TokenScript.BackCard();
        WaitToTouch = 0;
        if(turn == 0){
            turn = 1;
        }else if(turn == 1){
            turn = 0;
        }
    }

    public void SceneChanger(){
        sceneChanger = Instantiate( Resources.Load("Prefabs/TransitionCanvas",
        typeof(GameObject) ) ) as GameObject;
    }

    public void SetMonster(){
        gameDifficulty = ec.SetMonster(defeats);
        Heal();
    }

    void Awake(){
        token = GameObject.Find("Token");
    }
}
