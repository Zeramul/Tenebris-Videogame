using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceWeaponController : MonoBehaviour
{
    PlayerController playerController;
    public GameObject projectilePrefab;
    public int manaCost;
    Transform origin;

    [Range(0f, 45f)]
    public float projectileAngle;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        origin = transform.GetChild(0);
        manaCost = 3;
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
                    if (PlayerDataManager.Instance.EnoughMana(manaCost))
                    {
                        // Vector de direcci�n del primer proyectil
                        Vector3 shootDirection = Vector3.zero;

                        // Si el rayo del jugador detecta una colisi�n, dispara hacia el punto de impacto del rayo del jugador
                        if (playerController.hitPoint.transform != null)
                        {
                            shootDirection = (playerController.hitPoint.point - origin.position).normalized;
                        }
                        // Si no se detecta una colisi�n, dispara hacia adelante
                        else
                        {
                            shootDirection = transform.forward;
                        }

                        // Disparar el primer proyectil
                        ShootProjectile(origin.position, shootDirection);

                        // Disparar los proyectiles restantes
                        for (int i = 1; i <= 5; i++)
                        {
                            // Calcular la direcci�n girada para los proyectiles adicionales
                            Vector3 rotatedDirection = Quaternion.AngleAxis(projectileAngle * (i - 3), Vector3.up) * shootDirection;

                            // Disparar el proyectil en la direcci�n girada
                            ShootProjectile(origin.position, rotatedDirection);
                        }

                        PlayerDataManager.Instance.SetManaCost(manaCost);
                    }
                    else
                    {
                        Debug.Log("No tienes suficiente mana");
                    }
                }
            }
        }
    }

    // M�todo para disparar un proyectil en una direcci�n espec�fica
    void ShootProjectile(Vector3 position, Vector3 direction)
    {
        // Disparar el prefab del proyectil
        GameObject projectile = Instantiate(projectilePrefab, position, Quaternion.LookRotation(direction));
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            // Aplica una fuerza al proyectil en la direcci�n del disparo
            projectileRigidbody.AddForce(direction * 50f, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogWarning("El prefab del proyectil no tiene un componente Rigidbody.");
        }
    }
}
