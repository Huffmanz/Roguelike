using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource levelMusic;
    [SerializeField] AudioSource gameOverMusic;
    [SerializeField] AudioSource winMusic;
    [SerializeField] AudioSource[] sfx;
    private void Awake() {
        instance = this;
    }
    public void playGameOver(){
        levelMusic.Stop();
        gameOverMusic.Play();
    }
    public void playLevelWin(){
        levelMusic.Stop();
        winMusic.Play();
    }
    public void playSfx(int sfxToPlay){
        sfx[sfxToPlay].Stop();
        sfx[sfxToPlay].Play();

    }
}
