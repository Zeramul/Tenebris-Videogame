using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretWallOpening : MonoBehaviour
{
    public OpenButton linkedButton;

    public Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (linkedButton.isPressed)
        {
            if (anim != null)
            {
                anim.enabled = true;
            }
        }
    }
}
