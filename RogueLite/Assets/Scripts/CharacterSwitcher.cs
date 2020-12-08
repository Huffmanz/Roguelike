using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitcher : MonoBehaviour
{
    [SerializeField] GameObject message;
    [SerializeField] public PlayerController playerToSpawn;
    [SerializeField] bool shouldUnlock;
    private bool canSelect;

    private void Start()
    {
        if (!shouldUnlock) return;
        if (PlayerPrefs.HasKey(playerToSpawn.name))
        {
            if(PlayerPrefs.GetInt(playerToSpawn.name) == 1)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    private void Update()
    {
        if (canSelect && Input.GetKeyDown(KeyCode.E))
        {
            Vector3 playerPosition = PlayerController.instance.gameObject.transform.position;
            
            Destroy(PlayerController.instance.gameObject);
            PlayerController newPlayer = Instantiate(playerToSpawn, playerPosition, playerToSpawn.transform.rotation);
            PlayerController.instance = newPlayer;
            gameObject.SetActive(false);
            CameraController.instance.target = newPlayer.transform;
            CharacterSelectManager.instance.activePlayer = newPlayer;
            CharacterSelectManager.instance.activeCharacterSelecter.gameObject.SetActive(true);
            CharacterSelectManager.instance.activeCharacterSelecter = this;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canSelect = true;
            message.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canSelect = false;
            message.SetActive(false);
        }
    }
}
