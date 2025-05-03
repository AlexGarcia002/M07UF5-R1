using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class BulletTime : MonoBehaviour
{
    [SerializeField] InputActionProperty bulletTimeActivator;
    public bool bulletTimeActive;

    void Start()
    {
        bulletTimeActive = false;
    }

    void Update()
    {
        if(bulletTimeActivator.action.WasReleasedThisFrame()){
            bulletTimeActive = !bulletTimeActive;
        }        
    }
}
