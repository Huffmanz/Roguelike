using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ShopItem : MonoBehaviour
{
    [SerializeField] GameObject buyMessage;
    [SerializeField] int itemCost;
    [SerializeField] bool isHealthRestore;
    [SerializeField] bool isHealthUpgrade;
    [SerializeField] int healthUpgradeAmount;
    [SerializeField] bool isGunUpgrade;
    [SerializeField] Gun[] potentialGuns;
    [SerializeField] SpriteRenderer gunSprite;
    [SerializeField] Text infoText; 


    private Gun theGun;
    private bool inBuyZone = false;

    private void Start()
    {
        if (isGunUpgrade)
        {
            int selectedGun = Random.Range(0, potentialGuns.Length);
            theGun = potentialGuns[selectedGun];
            gunSprite.sprite = theGun.shopSprite;
            infoText.text = theGun.weaponName + "\n -" + theGun.price + " Gold";
        }
    }

    private void Update() {
        if(inBuyZone && Input.GetKeyDown(KeyCode.E)){
            if(LevelManager.instance.currentCoins >= itemCost){
                LevelManager.instance.SpendCoin(itemCost);
                if(isHealthRestore){
                    PlayerHealthController.instance.HealPlayer(PlayerHealthController.instance.maxHealth);
                }
                if(isHealthUpgrade){
                    PlayerHealthController.instance.IncreaseMaxHealth(healthUpgradeAmount);
                }
                if (isGunUpgrade)
                {
                    Gun newGun = Instantiate(theGun);
                    newGun.transform.parent = PlayerController.instance.gunArm;
                    newGun.transform.position = PlayerController.instance.gunArm.position;
                    newGun.transform.localRotation = Quaternion.Euler(Vector3.zero);
                    newGun.transform.localScale = Vector3.one;
                    PlayerController.instance.availableGuns.Add(newGun);
                    PlayerController.instance.currentGun = PlayerController.instance.availableGuns.Count - 1;
                    PlayerController.instance.SwitchGun();
                }
                gameObject.SetActive(false);
                inBuyZone = false;
                AudioManager.instance.playSfx(18);

            }
            else{
                AudioManager.instance.playSfx(19);
            }

        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player"){
            buyMessage.SetActive(true);
            inBuyZone = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player"){
            buyMessage.SetActive(false);
            inBuyZone = false;
        }
    }
}
