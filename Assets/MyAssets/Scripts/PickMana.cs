using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickMana : MonoBehaviour
{
    public int manaReg;

    private void Awake()
    {
        if (gameObject.CompareTag("PickableSmall"))
        {
            manaReg = 5;
        }
        else if (gameObject.CompareTag("PickableBig"))
        {
            manaReg = 20;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDataManager.Instance.GiveMana(manaReg);
            Destroy(gameObject);
        }
    }
}
