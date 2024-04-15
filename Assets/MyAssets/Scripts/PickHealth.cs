using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickHealth : MonoBehaviour
{
    public int healthReg;

    private void Awake()
    {
        if (gameObject.CompareTag("PickableSmall"))
        {
            healthReg = 5;
        }
        else if (gameObject.CompareTag("PickableBig"))
        {
            healthReg = 20;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDataManager.Instance.HealthRecovery(healthReg);
            Destroy(gameObject);
        }
    }
}
