using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [SerializeField] float moveSpeed;
    [SerializeField] public Transform target;
    [SerializeField] public Camera mainCamera;
    [SerializeField] Camera bigMapCamera;
    [SerializeField] bool isBossRoom;
    private bool bigMapActive = false;
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (isBossRoom)
        {
            target = PlayerController.instance.gameObject.transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(target != null){
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(target.position.x, target.position.y, transform.position.z), moveSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.M) && !isBossRoom)
        {
            if (bigMapActive)
            {
                DeactivateBigMap();
            }
            else
            {
                ActivateBigMap();
            }
        }
    }

    public void ChangeTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ActivateBigMap()
    {
        if (LevelManager.instance.isPaused) return;
        bigMapActive = true;
        bigMapCamera.enabled = true;
        mainCamera.enabled = false;
        UIController.instance.mapDisplay.SetActive(false);
        UIController.instance.mapText.SetActive(true);
        PlayerController.instance.canMove = false;
        Time.timeScale = 0;

    }

    public void DeactivateBigMap()
    {
        if (LevelManager.instance.isPaused) return;
        bigMapActive = false;
        bigMapCamera.enabled = false;
        mainCamera.enabled = true;
        UIController.instance.mapText.SetActive(false);
        UIController.instance.mapDisplay.SetActive(true);
        PlayerController.instance.canMove = true;
        Time.timeScale = 1;
    }
}
