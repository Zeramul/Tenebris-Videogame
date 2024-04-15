using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimatorAvd : MonoBehaviour
{
    public ArcherEnemyBehabiour enemyBehaviour;

    private Animator anim;
    [Range(0f, 1f)]
    public float weight;

    private void Awake()
    {
        enemyBehaviour = transform.GetComponentInParent<ArcherEnemyBehabiour>();
        weight = 1f;
        anim = GetComponent<Animator>();
    }
    //Ajusta la cabeza de los NPC para que miren hacia su eje Z positivo
    private void OnAnimatorIK(int layerIndex)
    {
        if (enemyBehaviour.status != Enumerators.EnemyStatus.Dead)
        {
            anim.SetLookAtPosition(transform.position + transform.forward * 2 + Vector3.up * 1.5f);
            anim.SetLookAtWeight(weight);
        }
        else
        {
            anim.SetLookAtPosition(transform.position + transform.forward * 2 + Vector3.up * 1.5f);
            anim.SetLookAtWeight(0f);
        }
    }
}
