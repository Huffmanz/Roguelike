using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] float waitToLoad = 4f;
    [SerializeField] string nextLevel;
    private void Awake() {
        instance = this;
    } 

    public IEnumerator LevelEnd(){
        PlayerController.instance.canMove = false;
        UIController.instance.StartFadeOut();
        AudioManager.instance.playLevelWin();
        yield return new WaitForSeconds(waitToLoad);
        SceneManager.LoadScene(nextLevel);

    }
}
