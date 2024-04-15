using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionDamage : MonoBehaviour
{
    public int damageAmount = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Cambia "Player" al tag adecuado si es necesario
        {
            // Aqu� puedes acceder al script de manejo de da�o del jugador y aplicarle el da�o
            PlayerDataManager playerData = other.GetComponent<PlayerDataManager>();
            if (playerData != null)
            {
                playerData.SetDamage(damageAmount);
            }
        }
        else if (other.CompareTag("Enemy")) // Cambia "Enemy" al tag adecuado si es necesario
        {
            // Aqu� puedes acceder al script de manejo de da�o del enemigo y aplicarle el da�o
            EnemyDataManager enemyData = other.GetComponent<EnemyDataManager>();
            if (enemyData != null)
            {
                enemyData.SetHealth(damageAmount);
            }
        }
    }
}
