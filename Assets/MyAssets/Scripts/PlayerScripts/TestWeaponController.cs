using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWeaponController : MonoBehaviour
{
    PlayerController playerController;
    public GameObject projectilePrefab;
    Transform origin;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        origin = transform.GetChild(0);
    }

    void Update()
    {
        if (playerController != null)
        {
            if (ShieldController.shieldActive == false && GameManager.Instance.currentGameStatus != Enumerators.GameStatus.Pause)
            {
                // Disparar si se detecta el clic del mouse
                if (Input.GetMouseButtonDown(0))
                {
                    Vector3 shootDirection;

                    // Si el rayo del jugador detecta una colisión, dispara hacia el punto de impacto del rayo del jugador
                    if (playerController.hitPoint.transform != null)
                    {
                        shootDirection = (playerController.hitPoint.point - origin.position).normalized;
                    }
                    // Si no se detecta una colisión, dispara hacia adelante
                    else
                    {
                        shootDirection = transform.forward;
                    }

                    // Dispara el prefab del proyectil
                    GameObject projectile = Instantiate(projectilePrefab, origin.position, Quaternion.identity);
                    Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
                    if (projectileRigidbody != null)
                    {
                        // Aplica una fuerza al proyectil en la dirección del disparo
                        projectileRigidbody.AddForce(shootDirection * 50f, ForceMode.VelocityChange);
                    }
                    else
                    {
                        Debug.LogWarning("El prefab del proyectil no tiene un componente Rigidbody.");
                    }
                }
            }
        }
    }
}

