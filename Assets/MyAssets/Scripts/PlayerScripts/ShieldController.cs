using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shieldObject;
    public float shieldDuration = 1f; // Duración del escudo activo
    public float shieldCooldown = 5f; // Tiempo de enfriamiento entre usos del escudo

    public static bool shieldActive = false;
    private float shieldTimer = 0f;
    private bool isOnCooldown = false;
    private float cooldownTimer = 0f;

    void Update()
    {
        // Verificar si el escudo está en enfriamiento
        if (isOnCooldown)
        {
            cooldownTimer += Time.deltaTime;

            // Si el enfriamiento ha terminado, reiniciar el estado
            if (cooldownTimer >= shieldCooldown)
            {
                isOnCooldown = false;
                cooldownTimer = 0f;
            }
        }
        else
        {
            // Verificar si se ha hecho clic en el botón derecho del mouse y el juego no está en pausa ni el escudo está activo
            if (Input.GetMouseButtonDown(1) && GameManager.Instance.currentGameStatus != Enumerators.GameStatus.Pause && !shieldActive)
            {
                // Activar el escudo
                ActivateShield();
            }
        }

        // Si el escudo está activo, incrementar el temporizador
        if (shieldActive)
        {
            shieldTimer += Time.deltaTime;

            // Si ha pasado el tiempo de duración, desactivar el escudo
            if (shieldTimer >= shieldDuration)
            {
                DeactivateShield();
            }
        }
    }

    void ActivateShield()
    {
        if (!isOnCooldown && shieldObject != null)
        {
            shieldObject.SetActive(true);
            shieldActive = true;
            shieldTimer = 0f; // Reiniciar el temporizador
            isOnCooldown = true; // Iniciar el enfriamiento
        }
        else
        {
            Debug.LogWarning("No se puede activar el escudo: el escudo esta recargandose o su referencia es 'null'");
        }
    }

    void DeactivateShield()
    {
        if (shieldObject != null)
        {
            shieldObject.SetActive(false);
            shieldActive = false;
        }
        else
        {
            Debug.LogWarning("La referencia del objeto escudo es 'null'");
        }
    }
}
