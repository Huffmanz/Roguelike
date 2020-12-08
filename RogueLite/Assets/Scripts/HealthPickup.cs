using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] int healAmount;
    [SerializeField] float waitToCollect = .5f;
    [SerializeField] int pickupSound;

    private void Update() {
        if(waitToCollect > 0f){
            waitToCollect-=Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && waitToCollect <= 0){
            PlayerHealthController.instance.HealPlayer(healAmount);
            AudioManager.instance.playSfx(pickupSound);
            Destroy(gameObject);
        }
    }
}
