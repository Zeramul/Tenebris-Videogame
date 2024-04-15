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
            // Aquí puedes acceder al script de manejo de daño del jugador y aplicarle el daño
            PlayerDataManager playerData = other.GetComponent<PlayerDataManager>();
            if (playerData != null)
            {
                playerData.SetDamage(damageAmount);
            }
        }
        else if (other.CompareTag("Enemy")) // Cambia "Enemy" al tag adecuado si es necesario
        {
            // Aquí puedes acceder al script de manejo de daño del enemigo y aplicarle el daño
            EnemyDataManager enemyData = other.GetComponent<EnemyDataManager>();
            if (enemyData != null)
            {
                enemyData.SetHealth(damageAmount);
            }
        }
    }
}
