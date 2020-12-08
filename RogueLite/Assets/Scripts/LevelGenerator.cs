using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject layoutRoom;
    [SerializeField] int distanceToEnd;
    [SerializeField] Color startColor;
    [SerializeField] Color endColor;
    [SerializeField] Color shopColor;
    [SerializeField] Color chestColor;
    [SerializeField] Transform generatorPoint;
    [SerializeField] bool includeShop;
    [SerializeField] int minDistanceToShop;
    [SerializeField] int maxDistanceToShop;
    [SerializeField] bool includeChest;
    [SerializeField] int minDistanceToChest;
    [SerializeField] int maxDistinceToChest;
    [SerializeField] float xOffset = 18f;
    [SerializeField] float yOffset = 10f;
    [SerializeField] LayerMask roomLayer;

    [SerializeField] RoomCenter centerStart;
    [SerializeField] RoomCenter centerEnd;
    [SerializeField] RoomCenter centerShop;
    [SerializeField] RoomCenter centerChest;
    [SerializeField] RoomCenter[] centers;
    private GameObject endRoom;
    private GameObject shopRoom;
    private GameObject chestRoom;
    private List<GameObject> layoutRoomObjects = new List<GameObject>();

    private List<GameObject> generatedOutlines = new List<GameObject>();
    public RoomPrefabs rooms;
    public enum Direction{
        up,
        right,
        down,
        left
    };

    public Direction selectedDirection;
    void Start()
    {
        GameObject startRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
        startRoom.GetComponent<SpriteRenderer>().color = startColor;
        selectedDirection = (Direction) Random.Range(0,4);
        MoveGenerationPoint();
        for(int i = 0; i < distanceToEnd; i++){
            GameObject newRoom = Instantiate(layoutRoom, generatorPoint.position, generatorPoint.rotation);
            if(i+1 == distanceToEnd){
                endRoom = newRoom;
                newRoom.GetComponent<SpriteRenderer>().color = endColor;
            }else{
                layoutRoomObjects.Add(newRoom);
            }

            selectedDirection = (Direction) Random.Range(0,4);
            MoveGenerationPoint();
            while(Physics2D.OverlapCircle(generatorPoint.position, .2f,roomLayer)){
                 MoveGenerationPoint();
            }
        }
        if(includeShop){
            int shopSelector = Random.Range(minDistanceToShop, maxDistanceToShop+1);
            shopRoom = layoutRoomObjects[shopSelector];
            layoutRoomObjects.RemoveAt(shopSelector);
            shopRoom.GetComponent<SpriteRenderer>().color = shopColor;
        }
        CreateRoomOutline(Vector3.zero);
        if(includeShop){
            CreateRoomOutline(shopRoom.transform.position);
        }
        if (includeChest)
        {
            int chestSelector = Random.Range(minDistanceToChest, maxDistinceToChest + 1);
            chestRoom = layoutRoomObjects[chestSelector];
            layoutRoomObjects.RemoveAt(chestSelector);
            chestRoom.GetComponent<SpriteRenderer>().color = chestColor;
            CreateRoomOutline(chestRoom.transform.position);
        }
        CreateRoomOutline(endRoom.transform.position);
        foreach(GameObject room in layoutRoomObjects){
            CreateRoomOutline(room.transform.position);
        }

        foreach(GameObject outline in generatedOutlines){
            if(outline.transform.position == Vector3.zero){
                Instantiate(centerStart, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();    
            }
            else if(outline.transform.position == endRoom.transform.position){
                Instantiate(centerEnd, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();    
            }
            else if(includeShop && outline.transform.position == shopRoom.transform.position){
                Instantiate(centerShop, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();    
            }
            else if(includeChest && outline.transform.position == chestRoom.transform.position)
            {
                Instantiate(centerChest, outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
            else{
                int centerSelected = Random.Range(0, centers.Length);
                Instantiate(centers[centerSelected], outline.transform.position, transform.rotation).theRoom = outline.GetComponent<Room>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        if(Input.GetKey(KeyCode.R)){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
#endif

    public void MoveGenerationPoint(){
        switch(selectedDirection){
            case Direction.up:
                generatorPoint.position+=new Vector3(0,yOffset,0);
                break;
            case Direction.down:
                generatorPoint.position+=new Vector3(0,-yOffset,0);
                break;
            case Direction.right:
                generatorPoint.position+=new Vector3(xOffset,0,0);
                break;
            case Direction.left:
                generatorPoint.position+=new Vector3(-xOffset,0,0);
                break;
        }
    }
    public void CreateRoomOutline(Vector3 room){
        bool roomAbove = Physics2D.OverlapCircle(room + new Vector3(0,yOffset,0),.2f,roomLayer);
        bool roomBelow = Physics2D.OverlapCircle(room + new Vector3(0,-yOffset,0),.2f,roomLayer);
        bool roomRight = Physics2D.OverlapCircle(room + new Vector3(xOffset,0,0),.2f,roomLayer);
        bool roomLeft = Physics2D.OverlapCircle(room + new Vector3(-xOffset,0,0),.2f,roomLayer);

        int directionCount = 0;
        if(roomAbove) directionCount++;
        if(roomBelow) directionCount++;
        if(roomRight) directionCount++;
        if(roomLeft) directionCount++;

        switch(directionCount){
            case 0:
                Debug.LogError("No room exists");
                break;
            case 1:
                if(roomAbove) generatedOutlines.Add(Instantiate(rooms.singleUp, room, transform.rotation));
                if(roomBelow) generatedOutlines.Add(Instantiate(rooms.singleDown, room, transform.rotation));
                if(roomRight) generatedOutlines.Add(Instantiate(rooms.singleRight, room, transform.rotation));
                if(roomLeft) generatedOutlines.Add(Instantiate(rooms.singleLeft, room, transform.rotation));
                break;
            case 2:
                if(roomBelow && roomLeft) generatedOutlines.Add(Instantiate(rooms.doubleDownLeft, room, transform.rotation));
                if(roomLeft && roomRight) generatedOutlines.Add(Instantiate(rooms.doubleLeftRight, room, transform.rotation));
                if(roomLeft && roomAbove) generatedOutlines.Add(Instantiate(rooms.doubleLeftUp, room, transform.rotation));
                if(roomRight && roomBelow) generatedOutlines.Add(Instantiate(rooms.doubleRightDown, room, transform.rotation));
                if(roomAbove && roomBelow) generatedOutlines.Add(Instantiate(rooms.doubleUpDown, room, transform.rotation));
                if(roomAbove && roomRight) generatedOutlines.Add(Instantiate(rooms.doubleUpRight, room, transform.rotation));
                break;
            case 3:
                if(roomBelow && roomLeft && roomAbove) generatedOutlines.Add(Instantiate(rooms.tripleDownLeftUp, room, transform.rotation));
                if(roomLeft && roomRight && roomAbove) generatedOutlines.Add(Instantiate(rooms.tripleLeftUpRight, room, transform.rotation));
                if(roomRight && roomBelow && roomLeft) generatedOutlines.Add(Instantiate(rooms.tripleRightDownLeft, room, transform.rotation));
                if(roomAbove && roomRight && roomBelow) generatedOutlines.Add(Instantiate(rooms.tripleUpRightDown, room, transform.rotation));

                break;
            case 4:
                generatedOutlines.Add(Instantiate(rooms.fourway, room, transform.rotation));

                break;
        }
    }
}

[System.Serializable]
public class RoomPrefabs{
    public GameObject singleUp, singleDown, singleLeft, singleRight,    
    doubleUpDown, doubleLeftRight, doubleUpRight, doubleRightDown, doubleDownLeft, doubleLeftUp,
    tripleUpRightDown, tripleRightDownLeft, tripleDownLeftUp, tripleLeftUpRight,
    fourway;

}
