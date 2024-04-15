using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickArmor : MonoBehaviour
{
    public int armorReg;

    private void Awake()
    {
        if (gameObject.CompareTag("PickableSmall"))
        {
            armorReg = 5;
        }
        else if (gameObject.CompareTag("PickableBig"))
        {
            armorReg = 20;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDataManager.Instance.ArmorRecovery(armorReg);
            Destroy(gameObject);
        }
    }
}
