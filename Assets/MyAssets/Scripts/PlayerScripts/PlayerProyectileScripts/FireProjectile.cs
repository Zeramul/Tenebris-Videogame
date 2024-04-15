using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FireProjectile : MonoBehaviour
{
    public int damage;
    public int explosionDamage;
    public float destroyDelay = 1f; // Tiempo en segundos antes de destruir el proyectil
    public float fadeDuration = 1.5f; // Duración de la atenuación gradual

    public AudioSource audioSource;
    public GameObject explosion;
    public GameObject geo;

    private Rigidbody rb;
    public bool collided;

    void Awake()
    {
        damage = 10;
        explosionDamage = 10;
        collided = false;
        rb = GetComponent<Rigidbody>(); // Obtener el componente Rigidbody
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collided) return; // Evitar que se procese la colisión más de una vez para asegurarse de que la explosion ocurra solo en un area
        collided = true;

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

        // Desactivar la geometria del objeto
        if (geo != null)
        {
            // Desactivar el Rigidbody para que el objeto se quede en su posición actual
            rb.isKinematic = true;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            geo.SetActive(false);
        }

        // Activar la explosión
        if (explosion != null)
        {
            explosion.SetActive(true);
        }

        // Esperar antes de destruir el proyectil
        StartCoroutine(DelayedDestroy());
    }

    IEnumerator DelayedDestroy()
    {
        yield return new WaitForSeconds(destroyDelay);

        // Destruir el proyectil
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Cambia "Player" al tag adecuado si es necesario
        {
            // Aquí puedes acceder al script de manejo de daño del jugador y aplicarle el daño
            PlayerDataManager playerData = other.GetComponent<PlayerDataManager>();
            if (playerData != null)
            {
                playerData.SetDamage(explosionDamage);
                Debug.Log("La explosión causa daño");
            }
        }
        else if (other.CompareTag("Enemy")) // Cambia "Enemy" al tag adecuado si es necesario
        {
            // Aquí puedes acceder al script de manejo de daño del enemigo y aplicarle el daño
            EnemyDataManager enemyData = other.GetComponent<EnemyDataManager>();
            if (enemyData != null)
            {
                enemyData.ReduceHealth(explosionDamage);
                Debug.Log("La explosión causa daño");
            }
        }
    }
}