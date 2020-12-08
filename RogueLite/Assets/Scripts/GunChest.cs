using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunChest : MonoBehaviour
{
    [SerializeField] GunPickup[] potentialGuns;
    [SerializeField] SpriteRenderer sr;
    [SerializeField] Sprite chestOpen;
    [SerializeField] GameObject notification;
    [SerializeField] Transform spawnPoint;
    [SerializeField] float scaleSpeed=2f;
    private bool canOpen = false;
    private bool opened = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(canOpen && Input.GetKeyDown(KeyCode.E) && !opened ){
            opened = true;
            sr.sprite = chestOpen;
            int gunSelect = Random.Range(0, potentialGuns.Length);
            Instantiate(potentialGuns[gunSelect], spawnPoint.position, spawnPoint.rotation);
            transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
        }
        if (opened)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one,Time.deltaTime*scaleSpeed);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !opened)
        {
            notification.SetActive(true);
            canOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            notification.SetActive(false);
            canOpen = false;
        }
    }
}
