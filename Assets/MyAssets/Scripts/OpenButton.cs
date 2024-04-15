using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenButton : MonoBehaviour
{
    //Todo este script controla las animaciones que funcionan con un elemnto "boton" y un objeto con el que interactuara

    public Animator anim;

    public PlayerController playerController;

    public bool isPressed;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
        isPressed = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerController = null;
        }
    }
    private void Update()
    {
        if (playerController != null && Input.GetKeyDown(KeyCode.E) && !isPressed)
        {
            if (anim != null)
            {
                anim.enabled = true;
                isPressed = true;
            }
        }
    }
}
