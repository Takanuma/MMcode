using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSystem : MonoBehaviour
{
    GameObject canvas;
    [SerializeField] AudioClip startSound;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape)&& canvas!=true){
            canvas = Instantiate( Resources.Load("Prefabs/Menu",
            typeof(GameObject) ) ) as GameObject;
        }
    }
}
