using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUnlock : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] SpriteRenderer cagedSpriteRenderer;
    public CharacterSwitcher[] characterSwitcher;
    private CharacterSwitcher playerToUnlock;
    private bool canUnlock;
    void Start()
    {
        playerToUnlock = characterSwitcher[Random.Range(0, characterSwitcher.Length)];
        cagedSpriteRenderer.sprite = playerToUnlock.playerToSpawn.bodysr.sprite;
        
    }

    // Update is called once per frame
    void Update()
    {
      if(canUnlock && Input.GetKeyDown(KeyCode.E))
        {
            PlayerPrefs.SetInt(playerToUnlock.playerToSpawn.name, 1);
            Instantiate(playerToUnlock, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canUnlock = true;
            message.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canUnlock = false;
            message.SetActive(false);
        }
    }
}
