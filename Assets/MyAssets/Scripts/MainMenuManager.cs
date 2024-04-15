using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    Transform panel;
    public float transitionDelay = 4f;
    private bool isTransitioning = false;

    void Awake()
    {
        Instance = this;
        panel = transform.GetChild(1);
    }

    /// <summary>
    /// Evento para ejecutar una transicion y pasar al gameplay
    /// </summary>
    public void PlayGame()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionAndPlayGame());
        }
    }

    /// <summary>
    /// Evento para cerrar la aplicacion
    /// </summary>
    public void QuitGame()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionAndQuitGame());
        }
    }

    //Corrutinas para controlar de manera precisa los tiempos de transicion y evitar que el juego se ejecute o acabe antes de tiempo
    IEnumerator TransitionAndPlayGame()
    {
        isTransitioning = true;

        // Esperar el tiempo de retraso antes de mostrar la transición
        //yield return new WaitForSeconds(transitionDelay);

        // Mostrar la transición (activar el panel)
        panel.gameObject.SetActive(true);

        // Esperar a que termine la animación de transición (ajusta el tiempo según la duración de tu animación)
        yield return new WaitForSeconds(transitionDelay);

        // Realizar la acción correspondiente (en este caso, iniciar el juego)
        GameManager.Instance.SetStatus(Enumerators.GameStatus.Running);

        isTransitioning = false;
    }

    IEnumerator TransitionAndQuitGame()
    {
        isTransitioning = true;

        // Mostrar la transición (activar el panel)
        panel.gameObject.SetActive(true);

        // Esperar a que termine la animación de transición (ajusta el tiempo según la duración de tu animación)
        yield return new WaitForSeconds(transitionDelay);

        // Realizar la acción correspondiente (en este caso, salir del juego)
        Application.Quit();

        isTransitioning = false;
    }
}
