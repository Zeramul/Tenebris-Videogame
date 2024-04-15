using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource audioSource;

    public AudioClip[] sceneMusicClips; //Array con pistas de audio

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
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Reproduce la pista musical en funcion del numero de escena
    /// </summary>
    /// <param name="sceneIndex"></param>

    //Este metodo se puede utilizar sin necesidad de usar el SceneIndex y haciendo uso simplemente de un int
    public void SetMusicTrack(int sceneIndex)
    {
        // Verificar si el �ndice de la escena est� dentro del rango de clips de m�sica
        if (sceneIndex >= 0 && sceneIndex < sceneMusicClips.Length)
        {
            AudioClip _clip = sceneMusicClips[sceneIndex];
            audioSource.clip = _clip;
            audioSource.Play();
        }
        else
        {
            Debug.LogError("No se encontr� una pista de m�sica para la escena con �ndice: " + sceneIndex);
        }
    }
}
