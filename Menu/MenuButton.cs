using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuButton : MonoBehaviour
{
    [SerializeField] GameObject Canvas;
    [SerializeField] GameObject Title;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DeleteInventory(){
        Destroy(Canvas);
    }
    public void ExitGame(){
        Application.Quit();
    }
    public void ToTitle(){
        SceneManager.LoadScene ("MemoryBattleMobile");
    }
}
