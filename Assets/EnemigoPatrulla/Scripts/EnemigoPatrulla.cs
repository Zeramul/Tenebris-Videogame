using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
///
/// DESCRIPCION:
///
/// </summary>

public class EnemigoPatrulla : MonoBehaviour
{

    // -----------------------------------------------------------------
    #region 1) Definicion de Variables
    public Transform[] paradas;
    public int paradaActual;

    NavMeshAgent agente;

    public bool esperando;
    float tiempoEsperando;
    float tiempoDeEspera;
#endregion
// -----------------------------------------------------------------
#region 2) Funciones Predeterminadas de Unity 
void Awake (){
        agente = GetComponent<NavMeshAgent>();
        tiempoDeEspera = Random.Range(0.5f, 1.25f);
    }

    // Start is called before the first frame update
    void Start()
    {
        EstablecerParadaActual();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _puntoA = paradas[paradaActual].position;
        Vector3 _puntoB = transform.position;

        _puntoA.y = 0f;
        _puntoB.y = 0f;

        float distancia = Vector3.Distance(_puntoA, _puntoB);
        if (distancia < 0.05f) esperando = true;

        if (esperando)
        {
            if (tiempoEsperando < 1f) tiempoEsperando += Time.deltaTime;
            else SiguienteParada();
        }
    }

    private void OnDrawGizmos()
    {
        if (paradas == null) return;

        Vector3 dirHaciaSiguienteParada = Vector3.zero;
        int ultimoElementoArray = paradas.Length - 1;

        for (int i = 0; i < paradas.Length; i++)
        {
            if (i >= 0 && i < ultimoElementoArray)
            {
                dirHaciaSiguienteParada = paradas[i + 1].position - paradas[i].position;
                Debug.DrawRay(paradas[i].position, dirHaciaSiguienteParada, Color.red);
            }
        }


        dirHaciaSiguienteParada = paradas[0].position - paradas[ultimoElementoArray].position;
        Debug.DrawRay(paradas[ultimoElementoArray].position, dirHaciaSiguienteParada, Color.red);
    }
    #endregion
    // -----------------------------------------------------------------
    #region 3) Metodos Originales
    void EstablecerParadaActual()
    {
        agente.SetDestination(paradas[paradaActual].position);
    }

    void SiguienteParada()
    {
        tiempoDeEspera = Random.Range(0.5f, 1.25f);

        esperando = false;
        tiempoEsperando = 0f;

        paradaActual++;
        if (paradaActual > paradas.Length - 1) paradaActual = 0;

        EstablecerParadaActual();
    }
    #endregion
    // -----------------------------------------------------------------

}
