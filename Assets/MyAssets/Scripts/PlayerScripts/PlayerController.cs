using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(0f, 12f)]
    public float velocidad; //Velocidad del jugador
    public Vector2 ejesVirtuales; //Un vector donde guardamos las entradas del jugador en ejes verticales y horizontales
    public Vector2 ejesMouse; //Un vector donde guardar la entrada del raton, para facilitar el control de la camara
    Vector3 dirMovimiento; //Guardamos la direccion a la que queremos que vaya el jugador
    Vector3 dirRotacion; //Guardamos la rotacion a la que queremos que rote el jugador
    Quaternion rot; //Rotacion actual del jugador
    public float smoothRotationSpeed;

    Transform cam; //Representacion de la camara principal
    Transform cmFollow; //Un objeto presente en la escena para anclar la camara
    Transform hombro; //Un objeto para controlar la rotacion del brazo

    Rigidbody rb; //Referencia al componente RigidBody del personaje

    public Enumerators.PlayerStatus status;

    public bool isGrounded;
    [Range(0f, 10f)]
    public float jumpForce;
    [Range(0f, 3f)]
    public float rayCastLong;

    public RaycastHit hitPoint; // Guarda el punto de impacto del rayo del jugador

    public LayerMask camRaylm;

    public AudioSource footStepSource;

    private void Awake()
    {
        //Instanciacion de las variables que requieren de valores iniciales
        cam = Camera.main.transform;
        cmFollow = transform.GetChild(0);
        hombro = transform.GetChild(2);
        velocidad = 6f;
        rb = GetComponent<Rigidbody>();
        status = Enumerators.PlayerStatus.Idle;
        footStepSource = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Obtenemos las entradas del jugador para el movimiento
        ejesVirtuales.x = Input.GetAxisRaw("Horizontal");
        ejesVirtuales.y = Input.GetAxisRaw("Vertical");

        //Calculo de la direccion a la que queremos mover el personaje segun la direccion de la camara
        dirMovimiento = cam.right * ejesVirtuales.x + cam.forward * ejesVirtuales.y;
        dirMovimiento.y = 0f;
        dirMovimiento.Normalize();

        //El mismo proceso anterior aplicado a la rotacion, usando solo del eje z
        dirRotacion = cam.forward;
        dirRotacion.y = 0f;
        dirRotacion.Normalize();

        //Se aplica la rotacion deseada a la rotacion actual y ajustamos la posicion del jugador
        rot = Quaternion.LookRotation(dirRotacion);
        if (dirRotacion.magnitude != 0f)
        {
            transform.rotation = rot;
            cmFollow.forward = cam.forward;
            hombro.forward = cam.forward;
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            footStepSource.enabled = true;
        }
        else
        {
            footStepSource.enabled = false;
        }

        //Control de estados
        if (dirMovimiento.magnitude > 0 && status != Enumerators.PlayerStatus.Dead)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                SetStatus(Enumerators.PlayerStatus.Running);
            }
            else
            {
                SetStatus(Enumerators.PlayerStatus.Moving);
            }
        }
        else if (PlayerDataManager.Instance.GetHealth() <= 0)
        {
            SetStatus(Enumerators.PlayerStatus.Dead);
        }
        else
        {
            SetStatus(Enumerators.PlayerStatus.Idle);
        }
    }


    private void FixedUpdate()
    {
        //Movemos el jugador haciendo uso de las fisicas, que es la manera correcta para evitar que el movimiento dependa del framerate
        rb.velocity = dirMovimiento * velocidad + Vector3.up * rb.velocity.y;

        //Verificar si el jugador esta en el suelo
        isGrounded = CheckGround();

        if (status != Enumerators.PlayerStatus.Dead && Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        Ray ray = new Ray(cam.position, cam.forward);

        bool result = Physics.Raycast(ray, out hitPoint, 50f, camRaylm);
        if (result)
        {
            Debug.DrawRay(ray.origin, ray.direction * hitPoint.distance, Color.red);
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * 50f, Color.blue);
        }
    }

    private void SetStatus(Enumerators.PlayerStatus status)
    {
        this.status = status;

        switch (status)
        {
            case Enumerators.PlayerStatus.Idle:
                break;
            case Enumerators.PlayerStatus.Moving:
                velocidad = 6f;
                break;
            case Enumerators.PlayerStatus.Running:
                velocidad = 12f;
                break;
            case Enumerators.PlayerStatus.Dead:
                break;
        }
    }

    private bool CheckGround()
    {
        // Origen del raycast desde la posición del cmFollow
        Vector3 rayOrigin = cmFollow.position;

        // Dirección del raycast hacia abajo
        Vector3 rayDirection = Vector3.down;

        Ray ray = new Ray(rayOrigin, rayDirection);

        bool result = Physics.Raycast(ray, out RaycastHit hit, rayCastLong);

        // Realizar el raycast
        if (result)
        {
            Debug.DrawRay(ray.origin, ray.direction * rayCastLong, Color.yellow);
            // Si el raycast golpea algo en la capa del suelo, el jugador está en el suelo
            return true;
        }
        else
        {
            Debug.DrawRay(ray.origin, ray.direction * rayCastLong, Color.green);
            // Si no golpea nada, el jugador no está en el suelo
            return false;
        }
    }

    private void Jump()
    {
        // Aplicar fuerza vertical para simular el salto
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        // El jugador está en el aire después de saltar
        isGrounded = false;
    }
}