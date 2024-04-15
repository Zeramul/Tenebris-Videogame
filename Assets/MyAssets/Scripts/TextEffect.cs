using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextEffect : MonoBehaviour
{
    //Un efecto para un string que permite que los caracteres aparezcan poco a poco

    public TMP_Text textComponent;
    public string fullText;
    public float delayBetweenLetters;

    private string currentText = "";
    private float timer = 0f;
    private int currentIndex = 0;

    private void Start()
    {
        StartTextEffect();
    }

    private void Update()
    {
        // Si aún no hemos mostrado todo el texto y ha pasado suficiente tiempo,
        // añadir la siguiente letra al texto actual
        if (currentIndex < fullText.Length && timer >= delayBetweenLetters)
        {
            currentText += fullText[currentIndex];
            textComponent.text = currentText;
            currentIndex++;
            timer = 0f;
        }

        // Incrementar el temporizador
        timer += Time.deltaTime;
    }

    public void StartTextEffect()
    {
        currentText = "";
        currentIndex = 0;
        textComponent.text = currentText;
    }
}
