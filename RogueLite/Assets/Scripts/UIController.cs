using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIController instance;
    [SerializeField] 
    public Slider healthBar;
    [SerializeField] 
    public Text healthText;
    [SerializeField] public GameObject deathScreen;
    [SerializeField] Image fadeScreen;
    [SerializeField] float fadeSpeed=2f;
    [SerializeField] string newGameScene;
    [SerializeField] string mainMenuScene;
    [SerializeField] public GameObject pauseMenu;
    private bool fadeIn;
    private bool fadeOut;

    
    
    private void Awake() {
        instance=this;
    }
    void Start()
    {
        fadeOut = false;
        fadeIn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(fadeOut){
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 1f, fadeSpeed * Time.deltaTime));   
        }
        if(fadeIn){
            fadeScreen.color = new Color(fadeScreen.color.r, fadeScreen.color.g, fadeScreen.color.b, Mathf.MoveTowards(fadeScreen.color.a, 0f, fadeSpeed * Time.deltaTime));   
        }
        if(fadeScreen.color.a == 1) fadeOut = true;
        if(fadeScreen.color.a == 0) fadeIn = false;
    }

    public void StartFadeOut(){
        fadeOut = true;
        fadeIn = false;
    }

    public void StartFadeIn(){
        fadeIn = true;
        fadeOut = false;
    }

    public void NewGame(){
        Time.timeScale = 1;
        SceneManager.LoadScene(newGameScene);
    }
    public void ReturnToMainMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void Resume(){
        LevelManager.instance.PauseUnpause();
    }


}
