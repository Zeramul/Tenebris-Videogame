using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceProjectile : MonoBehaviour
{
    public int damage;
    public float destroyDelay = 2f; // Tiempo en segundos antes de destruir el proyectil
    public float fadeDuration = 1.5f; // Duración de la atenuación gradual

    public AudioSource audioSource;

    private Rigidbody rb;

    void Awake()
    {
        damage = 6;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>(); // Obtener el componente Rigidbody
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
        StartCoroutine(FadeOutAndDestroy());
    }

    // Start is called before the first frame update
    void Start()
    {
        // Destruir el proyectil después de un cierto tiempo
        Invoke("DestroyProjectile", destroyDelay);
    }

    void FixedUpdate()
    {
        // Obtener la dirección del movimiento del proyectil
        Vector3 moveDirection = rb.velocity.normalized;

        // Orientar el proyectil hacia la dirección del movimiento
        if (moveDirection != Vector3.zero)
        {
            transform.LookAt(transform.position + moveDirection);
        }
    }
    IEnumerator FadeOutAndDestroy()
    {
        // Desactivar geometría del objeto y destruirlo
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }

        if (audioSource != null)
        {
            float startVolume = audioSource.volume;
            float currentTime = 0f;

            while (currentTime < fadeDuration)
            {
                currentTime += Time.deltaTime * 3f;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / fadeDuration);
                yield return null;
            }
        }

        Destroy(gameObject);
    }

    void DestroyProjectile()
    {
        // Si el proyectil colisiona o pasa el tiempo de destrucción, reproducir la corrutina de atenuación y destrucción
        StartCoroutine(FadeOutAndDestroy());
    }
}