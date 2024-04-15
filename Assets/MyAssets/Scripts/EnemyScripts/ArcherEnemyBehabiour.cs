using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ArcherEnemyBehabiour : MonoBehaviour
{
    public EnemyDataManager data; 

    public Enumerators.EnemyStatus status;
    private Transform playerTransform;
    private bool playerDetected;

    public float fireRate;
    private float shootTemp;
    //private float firstShootDelay;

    public GameObject projectilePrefab; // Prefab del proyectil disparado
    public Transform eyesTransform; // La posición de los ojos del enemigo
    public Animator animator;

    public Transform[] stops;
    public int actualStop;

    NavMeshAgent agent;

    //Variables para controlar la ruta de patrulla del enemigo
    public bool waiting; //Una variable para controlar facilmente cuando el NPC se encuentra esperando en un punto
    public float waitingTime; //El tiempo que el NPC permanecera en la parada
    public float actualWaitingTime; //El tiempo que el NPC lleva parado

    //TODO: Seria genial que en vez de usar animaciones de muerte se aplicase un sistema de ragdoll
    public int deathAnimationIndex; //Una variable para almacenar aleatoriamente cada vez una muerte distinta

    //TODO: Cuando un proyectil impacte deberia forzar una animacion de daño para tener un mejor feedback para el jugador
    //de que efectivamente sus proyectiles estan dando en el blanco y haciendo daño
    /*
    public bool isHit;
    public float hitDuration;
    private float hitTimer;
    */
    void Awake()
    {
        data = gameObject.GetComponent<EnemyDataManager>();
        playerDetected = false;
        status = Enumerators.EnemyStatus.Idle;
        animator = transform.GetChild(3).GetComponent<Animator>();
        //Por testear
        agent = GetComponent<NavMeshAgent>();
        waitingTime = Random.Range(5f, 7f);
        //firstShootDelay = waitingTime;
        deathAnimationIndex = RandomizeDeath();
    }

    void Start()
    {
        // Obtener la transformada del jugador al iniciar
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        SetActualStopPosition();
    }

    void Update()
    {
        //TODO: Hay un error muy evidente, cuando el NPC se está moviendo, si detecta al jugador entrara en la animacion de apuntar y atacar
        //pero no dejara de moverse. Hay que reestructurar el codigo para evitar esto.

        //TODO: Se deberia poder ejecutar el resto del codigo aunque la variable "stops" este vacia

        //TODO: La deteccion del jugador podria ampliarse, los NPC podrian reaccionar tambien cuando el jugador les ataca

        //TODO: Al morir los enemigos podria soltar algún objeto de mana, salud o armadura (Se tendra que comprobar si afecta al balance del juego)

        if (data.GetHealth() > 0)
        {

            // Verificar si el estado es "PlayerDetected"
            if (status == Enumerators.EnemyStatus.PlayerDetected)
            {
                EndAllMovement();
                AimingStance();
                // Rotar el enemigo para que mire hacia el jugador
                Vector3 direction = playerTransform.position - transform.position;
                direction.y = 0f; // Mantener la rotación en el plano horizontal
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            }
            else if (status == Enumerators.EnemyStatus.Idle)
            {
                if (stops != null)
                {
                    Vector3 _pointA = stops[actualStop].position;
                    Vector3 _pointB = transform.position;

                    _pointA.y = 0f;
                    _pointB.y = 0f;

                    float _distance = Vector3.Distance(_pointA, _pointB);
                    if (_distance < 0.05f)
                    {
                        waiting = true;
                    }

                    if (waiting)
                    {
                        animator.SetBool("Walking", false);
                        animator.SetBool("Idle", true);

                        Quaternion targetRotation = Quaternion.LookRotation(stops[actualStop].forward, Vector3.up);
                        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 1f);

                        if (actualWaitingTime < waitingTime)
                        {
                            actualWaitingTime += Time.deltaTime;
                        }
                        else
                        {
                            NextStop();
                        }
                    }
                    else if (!waiting)
                    {
                        animator.SetBool("Walking", true);
                        animator.SetBool("Idle", false);
                        SetActualStopPosition();
                    }
                }
                else
                {
                    animator.SetBool("Walking", false);
                }
            }

            // Generar el rayo solo cuando el jugador esté detectado
            if (playerDetected)
            {
                EndAllMovement();
                AimingStance();
                RaycastHit hit;
                Vector3 rayOrigin = eyesTransform.position; // Origen del rayo desde los ojos del enemigo
                Vector3 rayDirection = (playerTransform.position - rayOrigin).normalized; // Dirección hacia el jugador
                float maxDistance = Vector3.Distance(rayOrigin, playerTransform.position); // Distancia máxima del rayo

                // Configurar el layerMask para ignorar la capa del enemigo
                int layerMask = ~(1 << gameObject.layer);

                // Dibujar el rayo en el editor
                Debug.DrawRay(rayOrigin, rayDirection * maxDistance, Color.red);

                if (Physics.Raycast(rayOrigin, rayDirection, out hit, maxDistance, layerMask))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        // Cambiar el estado a "PlayerDetected" si el rayo alcanza al jugador directamente
                        status = Enumerators.EnemyStatus.PlayerDetected;
                        if (Time.time >= shootTemp && playerDetected)
                        {
                            status = Enumerators.EnemyStatus.Shooting;
                            ShootingStance();
                            FireProjectile();
                            Debug.Log("Proyectil enemigo disparado");
                            shootTemp = Time.time + fireRate; // Establecer el próximo tiempo de disparo
                        }
                    }
                }
            }
        }

        else
        {
            status = Enumerators.EnemyStatus.Dead;
            ChangeAnimStateToDeath();
            agent.enabled = false;
            StartCoroutine(StartDeathAnim(deathAnimationIndex));
        }
    }
    private void FireProjectile()
    {
        // Calcular un punto más alto que la posición del jugador
        Vector3 targetPosition = playerTransform.position + Vector3.up * 1.5f; // Ajusta el valor "1.5f" según sea necesario

        // Disparar el prefab del proyectil
        GameObject projectile = Instantiate(projectilePrefab, eyesTransform.position, eyesTransform.rotation);
        Rigidbody projectileRigidbody = projectile.GetComponent<Rigidbody>();
        if (projectileRigidbody != null)
        {
            // Aplicar una fuerza al proyectil en la dirección del disparo hacia el punto más alto
            Vector3 shootDirection = (targetPosition - eyesTransform.position).normalized;
            projectileRigidbody.AddForce(shootDirection * 20f, ForceMode.VelocityChange);
        }
        else
        {
            Debug.LogWarning("El prefab del proyectil no tiene un componente Rigidbody.");
        }
    }

    private void SetActualStopPosition()
    {
        agent.SetDestination(stops[actualStop].position);
        agent.speed = 2f;
    }

    private void NextStop()
    {
        waiting = false;

        actualWaitingTime = 0f;

        actualStop++;

        if (actualStop > stops.Length - 1)
        {
            actualStop = 0;

            SetActualStopPosition();
        }
    }

    #region Metodos relacionados con las variables del componente Animator del personaje
    private void AimingStance()
    {
        animator.SetBool("AimingPlayer", true);
        animator.SetBool("Shooting", false);
    }

    private void ShootingStance()
    {
        animator.SetBool("AimingPlayer", false);
        animator.SetBool("Shooting", true);
    }

    private void EndAllMovement()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walking", false);
    }
    private void ChangeAnimStateToDeath()
    {
        animator.SetBool("Idle", false);
        animator.SetBool("Walking", false);
        animator.SetBool("AimingPlayer", false);
        animator.SetBool("Shooting", false);

        animator.SetBool("Dead", true);
    }
    private int RandomizeDeath()
    {
        float _randomNumber = Random.Range(1f, 2f);
        int _roundedNumber = Mathf.RoundToInt(_randomNumber);
        return _roundedNumber;
    }

    private IEnumerator StartDeathAnim(int num)
    {
        // Reproducir la animación de muerte
        animator.SetInteger("DeadAnim", num);
        animator.SetBool("Dead", true);

        // Esperar un pequeño tiempo para que la transición a la animación de muerte se complete
        yield return new WaitForSeconds(0.1f);

        // Esperar hasta que la animación de muerte haya avanzado al menos un frame
        yield return new WaitForEndOfFrame();

        // Esperar hasta que la animación de muerte haya terminado
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f || animator.IsInTransition(0))
        {
            yield return null;
        }

        // Desactivar el Animator cuando la animación haya terminado
        animator.enabled = false;
    }
    #endregion

    void OnTriggerEnter(Collider other)
    {

        // Si el collider detectado es el del jugador, activar la detección del jugador
        if (other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si el collider que sale de la zona es el del jugador, desactivar la detección del jugador
        if (other.CompareTag("Player"))
        {
            playerDetected = false;
        }
    }
}