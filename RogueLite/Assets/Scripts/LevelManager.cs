using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] float waitToLoad = 4f;
    [SerializeField] string nextLevel;

    public int currentCoins;

    public bool isPaused = false;
    private void Awake() {
        instance = this;
    } 

    private void Start() {
        UpdateUI();
        Time.timeScale = 1;
    }
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)){
            PauseUnpause();
        }
    }
    
    public void PauseUnpause(){
        if(!isPaused){
            UIController.instance.pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }else{
            UIController.instance.pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
        isPaused = !isPaused;
    }

    public IEnumerator LevelEnd(){
        PlayerController.instance.canMove = false;
        UIController.instance.StartFadeOut();
        AudioManager.instance.playLevelWin();
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(nextLevel);

    }

    public void GetCoins(int amount){
        currentCoins+=amount;
        UpdateUI();
    }

    public void SpendCoin(int amount){
        currentCoins-=amount;
        if(currentCoins < 0) currentCoins = 0;
        UpdateUI();
    }

    private void UpdateUI(){
        UIController.instance.coinText.text = currentCoins.ToString(); 
    }
}
