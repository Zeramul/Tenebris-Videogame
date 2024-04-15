using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutHUD : MonoBehaviour
{
    public Animator animator;

    public float fadeTemp;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        //animator.enabled = false;

        fadeTemp = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (fadeTemp <= 4)
        {
            fadeTemp += Time.deltaTime;

            if (fadeTemp <= 0.5f)
            {
                animator.enabled = false;
            }
            else if (fadeTemp > 0.5 && fadeTemp < 4)
            {
                animator.enabled = true;
            }
            else
            {
                animator.enabled = false;
                this.gameObject.SetActive(false);
            }
        }
    }
}
