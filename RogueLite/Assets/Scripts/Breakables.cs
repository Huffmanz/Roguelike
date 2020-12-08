using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breakables : MonoBehaviour
{
    [SerializeField] GameObject[] brokenPieces;
    [SerializeField] int maxPieces = 5;
    [SerializeField] bool dropItem=false;
    [SerializeField] GameObject[] itemsToDrop;
    [Range(0,100)]
    [SerializeField] float itemDropPercent;
    private void OnTriggerEnter2D(Collider2D other) {
        if((other.tag=="Player" && (PlayerController.instance.dashCounter > 0 ))|| other.tag=="PlayerBullet"){
            int peicesToDrop = Random.Range(1, maxPieces);
            for(int i=0; i<peicesToDrop;i++){
                int randomPiece= Random.Range(0, brokenPieces.Length);
                Instantiate(brokenPieces[randomPiece], transform.position, transform.rotation);

            }
            //Drop items
            if(dropItem){
                float dropChance = Random.Range(0f,100f);
                if(itemDropPercent > dropChance){
                    GameObject itemToDrop = itemsToDrop[Random.Range(0,itemsToDrop.Length)];
                    Instantiate(itemToDrop, transform.position, transform.rotation);
                }
            }
            AudioManager.instance.playSfx(0);
            Destroy(gameObject);
        }
    }


}
