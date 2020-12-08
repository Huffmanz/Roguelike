using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string levelToLoad;
    [SerializeField] GameObject deletePanel;
    public CharacterSwitcher[] charactersToDelete;
    

    public void StartGame(){
        SceneManager.LoadScene(levelToLoad);
    }

    public void ExitGame(){
        Application.Quit();
    }

    public void DeleteSave()
    {
        deletePanel.SetActive(true);
        foreach(CharacterSwitcher character in charactersToDelete)
        {
            PlayerPrefs.SetInt(character.playerToSpawn.name, 0);
        }
    }

    public void ConfirmDelete()
    {
        deletePanel.SetActive(false);
    }

    public void CancelDelete()
    {
        deletePanel.SetActive(false);
    }
}
