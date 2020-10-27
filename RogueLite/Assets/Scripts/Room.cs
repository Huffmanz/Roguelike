using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] bool closeWhenEntered;
    [SerializeField] GameObject[] doors;
    [SerializeField] bool openWhenEnemiesCleared;
    [SerializeField] List<GameObject> enemiesInRoom = new List<GameObject>();

    private bool roomActive=false;

    void Update()
    {
        if(enemiesInRoom.Count > 0 && roomActive && openWhenEnemiesCleared)
        {
            for(int i = 0; i < enemiesInRoom.Count; i++)
            {
                if(enemiesInRoom[i] == null)
                {
                    enemiesInRoom.RemoveAt(i);
                    i--;
                }
            }
        }
        if (enemiesInRoom.Count == 0 && roomActive && openWhenEnemiesCleared)
        {
            OpenDoors();
            closeWhenEntered = false;
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            roomActive = true;
            CameraController.instance.ChangeTarget(transform);
            if (closeWhenEntered) {
                foreach (GameObject door in doors)
                {
                    door.SetActive(true);
                }
            }
        }
    }

    void OnTriggerExit2D(Collider2D other){
        if (other.tag == "Player")
        {
            roomActive = false;
        }
    }

    void OpenDoors()
    {
        foreach(GameObject door in doors){
            door.SetActive(false);
        }
    }
}
