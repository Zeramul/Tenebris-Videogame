using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnding : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("El player finaliza el juego");
                GameManager.Instance.SetStatus(Enumerators.GameStatus.EndLevel);
            }
        }
    }
}
