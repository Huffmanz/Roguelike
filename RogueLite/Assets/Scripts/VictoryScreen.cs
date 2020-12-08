using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] float waitTime=2f;
    [SerializeField] GameObject anyText;
    [SerializeField] string mainMenu;

    private float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(PlayerController.instance.gameObject);
        Time.timeScale = 1;
        anyText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        waitTime-= Time.deltaTime;
        if(waitTime <= 0f){
            anyText.SetActive (true);
            if(Input.anyKeyDown){
                SceneManager.LoadScene(mainMenu);
            }
        }
    }
}
