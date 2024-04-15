using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public int damage;
    public float destroyDelay = 5f; // Tiempo en segundos antes de destruir el proyectil

    private Rigidbody rb;

    void Awake()
    {
        damage = 10;
        rb = GetComponent<Rigidbody>(); // Obtener el componente Rigidbody
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && ShieldController.shieldActive == false)
        {
            PlayerDataManager.Instance.SetDamage(damage);
            Debug.Log("Colision con el jugador");
        }
        else if (collision.gameObject.CompareTag("Player") && ShieldController.shieldActive == true)
        {
            Destroy(gameObject);
            Debug.Log("Colision con el escudo");
        }
        // Destruir el proyectil cuando colisiona con otro objeto
        Destroy(gameObject);
    }

    /*private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            PlayerDataManager.Instance.setDamage(damage);
            Debug.Log("Colision con el jugador");
        }
        else if (other.gameObject.CompareTag("PlayerShield"))
        {
            Destroy(gameObject);
            Debug.Log("Colision con el escudo");
        }
        // Destruir el proyectil cuando colisiona con otro objeto
        Destroy(gameObject);
    }*/

    // Start is called before the first frame update
    void Start()
    {
        // Destruir el proyectil después de un cierto tiempo
        Invoke("DestroyProjectile", destroyDelay);
        Debug.Log("Proyectil enemigo lanzado");
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

    void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
