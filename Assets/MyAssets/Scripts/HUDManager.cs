using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public TextMeshProUGUI HealthNumber;
    public TextMeshProUGUI ArmorNumber;
    public TextMeshProUGUI CurrentMana;
    public TextMeshProUGUI MaxMana;
    public GameObject hitOverlay;

    public GameObject transition;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void Start()
    {

    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateHealthNumber();
        UpdateCurrentArmor();
        UpdateCurrentMana();

        // Obtener el componente Image del objeto transition
        Image transitionImage = transition.GetComponent<Image>();

        // Obtener el color actual del componente Image
        Color currentColor = transitionImage.color;

        // Establecer la opacidad al máximo (alpha = 1)
        currentColor.a = 1f;

        // Asignar el nuevo color al componente Image
        transitionImage.color = currentColor;
    }
    public void UpdateHealthNumber()
    {
        HealthNumber.text = PlayerDataManager.Instance.GetHealth().ToString();
    }
    public void UpdateCurrentArmor()
    {
        ArmorNumber.text = PlayerDataManager.Instance.GetArmor().ToString();
    }
    public void UpdateCurrentMana()
    {
        CurrentMana.text = PlayerDataManager.Instance.GetCurrentMana().ToString();
    }
    /// <summary>
    /// Control del estado herido para mostrar un overlay que ayude al jugador a identificar que ha sido herido
    /// </summary>
    public void IsHit()
    {
        hitOverlay.SetActive(true);
        StartCoroutine(DisableHitOverlayAfterDelay(1f));
    }
    /// <summary>
    /// Permite que el overlay se muestre derante un tiempo definido antes de volver a desactivarse para volver a ser usado
    /// </summary>
    /// <param name="delay"></param>
    /// <returns></returns>
    IEnumerator DisableHitOverlayAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hitOverlay.SetActive(false); // Desactivar después del retraso
    }
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
