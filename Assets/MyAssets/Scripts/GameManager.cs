using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject HUD;
    public GameObject HUDTransitionPanel;
    public GameObject pauseMenu;
    public GameObject endLevel;
    public GameObject deathScreen;

    public Animator animator;

    public Enumerators.GameStatus lastGameStatus;
    public Enumerators.GameStatus currentGameStatus;

    public Enumerators.Levels currentLevel;

    private bool isPaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            Destroy(gameObject);
        }

        animator = HUDTransitionPanel.GetComponent<Animator>();

        // Suscribirse al evento SceneManager.sceneLoaded
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        //SetStatusByScene();
    }

    // Método que se llama cada vez que se carga una escena, ayuda a manejar de manera comoda, ciertas cosas que requieren de
    // un estado por defecto
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetStatusByScene();
        SetLevelByScene();

        if (SceneManager.GetActiveScene().buildIndex != 0)
        {
            animator.SetBool("FadeIn", false);
            animator.SetBool("FadeOut", true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) &&
            (currentGameStatus != Enumerators.GameStatus.MainMenu && currentGameStatus != Enumerators.GameStatus.GameOver))
        {
            TogglePause(); // Pausar o reanudar el juego al presionar Escape
        }

        /*if (Input.GetKeyDown(KeyCode.H))
        {
            SetStatus(Enumerators.GameStatus.EndLevel);
        }*/

        //Bloquea el cursor al centro de la pantalla solo cuando el estado del juego sea el gamepaly.
        if (currentGameStatus == Enumerators.GameStatus.Running)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }

    /// <summary>
    /// Cambia el estado del juego al del parametro especificado
    /// </summary>
    /// <param name="newStatus"></param>
    public void SetStatus(Enumerators.GameStatus newStatus)
    {
        lastGameStatus = currentGameStatus;
        currentGameStatus = newStatus;

        switch (currentGameStatus)
        {
            //------------------------------------------------
            case Enumerators.GameStatus.None:
                break;
            //------------------------------------------------
            case Enumerators.GameStatus.MainMenu:
                Time.timeScale = 1f;
                HUD.SetActive(false);
                endLevel.SetActive(false);
                deathScreen.SetActive(false);
                break;
            //------------------------------------------------
            case Enumerators.GameStatus.Running:
                Time.timeScale = 1f;
                Cursor.visible = false;
                HUD.SetActive(true);
                pauseMenu.SetActive(false);
                if (lastGameStatus == Enumerators.GameStatus.MainMenu)
                {
                    SceneManager.LoadScene(1);
                }

                break;
            //------------------------------------------------
            case Enumerators.GameStatus.Pause:
                Time.timeScale = 0f;
                Cursor.visible = true; // Mostrar cursor cuando el juego está en pausa
                HUD.SetActive(true);
                pauseMenu.SetActive(true);
                break;
            //------------------------------------------------
            case Enumerators.GameStatus.EndLevel:
                Time.timeScale = 1f;
                Cursor.visible = true;
                endLevel.SetActive(true);
                break;
            //------------------------------------------------
            case Enumerators.GameStatus.GameOver:
                Time.timeScale = 1f;
                deathScreen.SetActive(true);
                Cursor.visible = true;
                break;
        }
    }

    public void SetStatusByScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 0)
        {
            SetStatus(Enumerators.GameStatus.MainMenu);
        }
        else
        {
            SetStatus(Enumerators.GameStatus.Running);
        }

        SetMusicTrackByScene();
    }
    /// <summary>
    /// Activa o desactiva el estado de pausa dependiendo del estado actual
    /// </summary>
    public void TogglePause()
    {
        isPaused = !isPaused; // Cambiar el estado de pausa

        if (currentGameStatus != Enumerators.GameStatus.EndLevel || currentGameStatus != Enumerators.GameStatus.GameOver)
        {
            if (isPaused)
            {
                SetStatus(Enumerators.GameStatus.Pause); // Pausar el juego si estaba corriendo
            }
            else
            {
                SetStatus(Enumerators.GameStatus.Running); // Reanudar el juego si estaba pausado
            }
        }
    }

    public void SetLevel(Enumerators.Levels level)
    {
        currentLevel = level;
    }
    /// <summary>
    /// Metodo por si fuese necesario manejar los niveles por separado
    /// </summary>
    public void SetLevelByScene()
    {
        /*
        int _sceneIndex = SceneManager.GetActiveScene().buildIndex;

        switch (_sceneIndex)
        {
            case 0:
                SetLevel(Enumerators.Levels.Menu);
                break;
            case 1:
                SetLevel(Enumerators.Levels.Level1);
                break;
            case 2:
                SetLevel(Enumerators.Levels.Level2);
                break;
        }
        */
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SetLevel((Enumerators.Levels)sceneIndex);
    }
    /// <summary>
    /// Asigna una pista musical a escenas concretas haciendo uso de su numero de escena
    /// </summary>
    public void SetMusicTrackByScene()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (SoundManager.Instance != null)
        {
            // Detener la música actual antes de cambiarla
            if (SoundManager.Instance.audioSource.isPlaying)
            {
                SoundManager.Instance.audioSource.Stop();
            }

            // Reproducir la nueva pista de música
            SoundManager.Instance.SetMusicTrack(sceneIndex);
        }
    }

    // Importante: desuscribirse del evento al destruir el objeto
    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void BackToMenu()
    {
        Debug.Log("Volviendo al menu");

        animator.SetBool("FadeOut", false);
        animator.SetBool("FadeIn", true);

        SceneManager.LoadScene(0);
    }
}