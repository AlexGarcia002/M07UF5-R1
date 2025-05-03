using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenuManager : MonoBehaviour
{
    public Transform head;
    [SerializeField] float spawnDistance = 1f;
    public GameObject menu;
    public InputActionProperty showButton;

    void Start()
    {
        
    }

    void Update()
    {
        if (showButton.action.WasPressedThisFrame()) {
            menu.SetActive(!menu.activeSelf);

            menu.transform.position =   head.position + 
                                        new Vector3(head.position.x, 0f, head.forward.z).normalized * spawnDistance;
        }

        menu.transform.position = head.position +
                                        new Vector3(head.position.x, 0f, head.forward.z).normalized * spawnDistance;

        menu.transform.LookAt(new Vector3(head.position.x, menu.transform.position.y, head.position.z));
        menu.transform.forward *= -1f;
    }
}
