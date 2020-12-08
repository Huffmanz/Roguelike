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

    public Text coinText;
    [SerializeField] public GameObject deathScreen;
    [SerializeField] Image fadeScreen;
    [SerializeField] float fadeSpeed=2f;
    [SerializeField] string newGameScene;
    [SerializeField] string mainMenuScene;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public GameObject mapDisplay;
    [SerializeField] public GameObject mapText;
    [SerializeField] public Image currentGun;
    [SerializeField] public Text currentGunName;
    [SerializeField] public Slider bossSlider;
    [SerializeField] public GameObject bossHealthDisplay;
    private bool fadeIn;
    private bool fadeOut;

    
    
    private void Awake() {
        instance=this;
    }
    void Start() 
    {
        fadeOut = false;
        fadeIn = true;
        currentGun.sprite = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].gunUI;
        currentGunName.text = PlayerController.instance.availableGuns[PlayerController.instance.currentGun].weaponName;
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
        Destroy(PlayerController.instance.gameObject);
        Time.timeScale = 1;
        SceneManager.LoadScene(newGameScene);
    }
    public void ReturnToMainMenu(){
        Time.timeScale = 1;
        SceneManager.LoadScene(mainMenuScene);
        Destroy(PlayerController.instance.gameObject);
    }

    public void Resume(){
        LevelManager.instance.PauseUnpause();
    }



}
