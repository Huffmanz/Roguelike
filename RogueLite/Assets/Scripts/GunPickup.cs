using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickup : MonoBehaviour
{

    [SerializeField] float waitToCollect = .5f;
    [SerializeField] int pickupSound;
    [SerializeField] Gun theGun;

    private void Update()
    {
        if (waitToCollect > 0f)
        {
            waitToCollect -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && waitToCollect <= 0)
        {
            bool hasGun = false;
            foreach(Gun gun in PlayerController.instance.availableGuns)
            {
                if(gun.weaponName == theGun.weaponName)
                {
                    hasGun = true;
                    break;
                }
            }
            if (!hasGun)
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
            AudioManager.instance.playSfx(pickupSound);
            Destroy(gameObject);
        }
    }
}
