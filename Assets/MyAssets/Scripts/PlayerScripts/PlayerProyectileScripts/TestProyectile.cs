using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProyectile : MonoBehaviour
{
    public int damage;
    public int manaCost;
    public float destroyDelay = 5f; // Tiempo en segundos antes de destruir el proyectil

    void Awake()
    {
        damage = 10;
        manaCost = 1;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyDataManager enemyDataManager = collision.gameObject.GetComponent<EnemyDataManager>();
            if (enemyDataManager != null)
            {
                // Restar el daño al EnemyHealth del enemigo
                enemyDataManager.EnemyData.EnemyHealth -= damage;
            }
            else
            {
                Debug.Log("El enemigo no tiene componente enemyDataManager");
            }
        }
        // Destruir el proyectil cuando colisiona con otro objeto
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        // Destruir el proyectil después de un cierto tiempo
        Invoke("DestroyProjectile", destroyDelay);

        // Ignorar colisiones con el jugador u otros proyectiles del jugador
        //Physics.IgnoreLayerCollision(LayerMask.NameToLayer("PlayerProyectile"), LayerMask.NameToLayer("Player"), true);
    }

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}