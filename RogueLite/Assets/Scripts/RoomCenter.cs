using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomCenter : MonoBehaviour
{

        [SerializeField] List<GameObject> enemiesInRoom = new List<GameObject>();
        [SerializeField] bool openWhenEnemiesCleared;
        [HideInInspector] public Room theRoom;
    void Start()
    {
        if(openWhenEnemiesCleared){
            theRoom.closeWhenEntered = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(enemiesInRoom.Count > 0 && theRoom.roomActive && openWhenEnemiesCleared)
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
        if (enemiesInRoom.Count == 0 && theRoom.roomActive && openWhenEnemiesCleared)
        {
            theRoom.OpenDoors();
            
        }
    }
}
