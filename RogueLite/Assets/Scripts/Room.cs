using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] public bool closeWhenEntered;
    [SerializeField] GameObject[] doors;
    
    [HideInInspector]
     public bool roomActive=false;

    void Update()
    {


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

    public void OpenDoors()
    {
        foreach(GameObject door in doors){
            door.SetActive(false);
        }
        closeWhenEntered = false;
    }
}
