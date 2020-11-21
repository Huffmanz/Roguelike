using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField] GameObject buyMessage;
    [SerializeField] int itemCost;
    [SerializeField] bool isHealthRestore;
    [SerializeField] bool isHealthUpgrade;
    [SerializeField] int healthUpgradeAmount;
    [SerializeField] bool isGunUpgrade;

    private bool inBuyZone = false;

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
