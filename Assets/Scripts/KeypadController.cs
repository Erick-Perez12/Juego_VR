
//using UnityEngine;
//using TMPro;

//public class KeypadController : MonoBehaviour
//{
//    public TMP_Text pantalla;
//    public GameManager gameManager;
//    public DoorController puerta;

//    public string codigoCorrecto = "7391";

//    private string codigoIngresado = "";
//    private const int longitudCodigo = 4;

//    void Start()
//    {
//        ActualizarPantalla();
//    }

//    public void PresionarNumero(int numero)
//    {
//        if (codigoIngresado.Length >= longitudCodigo)
//            return;

//        codigoIngresado += numero.ToString();
//        ActualizarPantalla();
//    }

//    public void PresionarBorrar()
//    {
//        codigoIngresado = "";
//        ActualizarPantalla();
//    }

//    public void PresionarEnter()
//    {
//        if (!gameManager.TodosLosAcertijosResueltos())
//        {
//            Debug.Log("Primero debes resolver los cuatro acertijos.");
//            return;
//        }

//        if (codigoIngresado == codigoCorrecto)
//        {
//            Debug.Log("¡CÓDIGO CORRECTO!");
//            puerta.AbrirPuerta();
//        }
//        else
//        {
//            Debug.Log("Código incorrecto.");
//            codigoIngresado = "";
//            ActualizarPantalla();
//        }
//    }

//    void ActualizarPantalla()
//    {
//        if (pantalla != null)
//        {
//            pantalla.text = codigoIngresado.PadRight(
//                longitudCodigo, '_'
//            );
//        }
//    }
//}

using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadController : MonoBehaviour
{
    [Header("UI / Malla Pantalla")]
    public TMP_Text pantalla;
    [Tooltip("Arrastra aquí tu objeto 3D Cube 'fondo'")]
    public MeshRenderer fondoCubo3D;

    [Header("Audio SFX Keypad")]
    [Tooltip("Componente AudioSource ubicado en el Keypad")]
    public AudioSource audioSource;
    [Tooltip("Sonido de acceso concedido (Correcto)")]
    public AudioClip sonidoCorrecto;
    [Tooltip("Sonido de denegado / error (Incorrecto)")]
    public AudioClip sonidoIncorrecto;

    [Header("Evento Distractor / Sustos")]
    [Tooltip("Luces principales de la habitación que se apagarán")]
    public Light[] lucesHabitacion;
    [Tooltip("Sonido de susto / falla eléctrica que sonará durante el apagón")]
    public AudioClip sonidoApagon;
    [Tooltip("Tiempo en segundos que durará el apagón")]
    public float duracionApagon = 2.5f;

    [Header("Ceguera al Cambio (Desordenar Objetos)")]
    [Tooltip("Los objetos interactivos de la mesa (ej. tren, pájaro, etc.)")]
    public Transform[] objetosADesordenar;
    [Tooltip("Puntos a donde se moverán los objetos cuando ocurra el apagón (fuera de sus sockets)")]
    public Transform[] posicionesNuevas;
    [Tooltip("Sockets/Bases de la mesa donde encajaban los objetos")]
    public SocketReceiver[] socketsMesa;

    [Header("Colores de Fondo")]
    public Color colorFondoNormal = new Color(0.2f, 0.8f, 0.2f); // Verde claro
    public Color colorTextoNormal = Color.black;
    public Color colorFondoError = new Color(0.8f, 0.1f, 0.1f);  // Rojo
    public Color colorTextoError = Color.white;
    public Color colorFondoExito = new Color(0.1f, 0.9f, 0.2f);  // Verde vivo
    public Color colorTextoExito = Color.black;

    [Header("Referencias del Juego")]
    public GameManager gameManager;
    public DoorController puerta;

    [Header("Configuración del Código")]
    public string codigoCorrecto = "739";

    private string codigoIngresado = "";
    private const int longitudCodigo = 3;
    private bool estaBloqueado = false;
    private bool yaOcurrioApagon = false;
    private Coroutine rutinaFeedback;

    void Start()
    {
        EstablecerEstadoNormal();
    }

    public void PresionarNumero(int numero)
    {
        if (estaBloqueado) return;

        // Si ya resolvió todo y es la primera vez que toca el teclado, dispara el evento
        if (gameManager != null && gameManager.TodosLosAcertijosResueltos() && !yaOcurrioApagon)
        {
            StartCoroutine(RutinaEventoApagon());
            return;
        }

        if (codigoIngresado.Length >= longitudCodigo)
            return;

        codigoIngresado += numero.ToString();
        ActualizarPantalla();
    }

    public void PresionarBorrar()
    {
        if (estaBloqueado) return;

        codigoIngresado = "";
        ActualizarPantalla();
    }

    public void PresionarEnter()
    {
        if (estaBloqueado) return;

        if (gameManager != null && !gameManager.TodosLosAcertijosResueltos())
        {
            ReproducirSonido(sonidoIncorrecto);
            MostrarFeedbackTemporal("LOCKED", colorFondoError, colorTextoError, 2.0f);
            Debug.Log("Primero debes resolver los tres acertijos.");
            return;
        }

        // Si intenta presionar Enter y no ha ocurrido el apagón
        if (!yaOcurrioApagon)
        {
            StartCoroutine(RutinaEventoApagon());
            return;
        }

        if (codigoIngresado == codigoCorrecto)
        {
            Debug.Log("¡CÓDIGO CORRECTO!");

            estaBloqueado = true;
            ReproducirSonido(sonidoCorrecto);
            if (pantalla != null) pantalla.text = "ACCEPTED";
            AplicarColores(colorFondoExito, colorTextoExito);

            if (puerta != null)
            {
                puerta.AbrirPuerta();
            }
        }
        else
        {
            Debug.Log("Código incorrecto.");
            ReproducirSonido(sonidoIncorrecto);
            MostrarFeedbackTemporal("ERROR", colorFondoError, colorTextoError, 1.5f);
        }
    }

    IEnumerator RutinaEventoApagon()
    {
        yaOcurrioApagon = true;
        estaBloqueado = true;

        // 1. Sonido de error y susto
        ReproducirSonido(sonidoApagon != null ? sonidoApagon : sonidoIncorrecto);
        MostrarFeedbackTemporal("ERROR", colorFondoError, colorTextoError, duracionApagon);

        // 2. Apagar luces del cuarto
        if (lucesHabitacion != null)
        {
            foreach (Light l in lucesHabitacion)
            {
                if (l != null) l.enabled = false;
            }
        }
        if (socketsMesa != null)
        {
            foreach (SocketReceiver socket in socketsMesa)
            {
                if (socket != null) socket.LiberarSocket();
            }
        }
        Debug.Log("¡EVENTO: APAGÓN DE DISTRACCIÓN!");
        MoverObjetosFueraDeSitio();
        // 3. Esperar la duración del apagón
        yield return new WaitForSeconds(duracionApagon);

        // 4. Encender las luces de nuevo
        if (lucesHabitacion != null)
        {
            foreach (Light l in lucesHabitacion)
            {
                if (l != null) l.enabled = true;
            }
        }

        // 5. Resetear pantalla del Keypad
        EstablecerEstadoNormal();
    }
    private void MoverObjetosFueraDeSitio()
    {
        if (objetosADesordenar == null || posicionesNuevas == null) return;

        for (int i = 0; i < objetosADesordenar.Length; i++)
        {
            if (objetosADesordenar[i] != null && i < posicionesNuevas.Length && posicionesNuevas[i] != null)
            {
                // Si el objeto tiene Rigidbody, reseteamos sus velocidades para moverlo de forma limpia
                Rigidbody rb = objetosADesordenar[i].GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                // Movemos el objeto a la nueva posición fuera del socket
                objetosADesordenar[i].position = posicionesNuevas[i].position;
                objetosADesordenar[i].rotation = posicionesNuevas[i].rotation;
            }
        }
    }

    void ActualizarPantalla()
    {
        if (pantalla != null)
        {
            pantalla.text = codigoIngresado.PadRight(longitudCodigo, '_');
        }
    }

    private void EstablecerEstadoNormal()
    {
        codigoIngresado = "";
        estaBloqueado = false;
        AplicarColores(colorFondoNormal, colorTextoNormal);
        ActualizarPantalla();
    }

    private void AplicarColores(Color fondo, Color texto)
    {
        // Cambiar el color del material del Cubo 3D
        if (fondoCubo3D != null && fondoCubo3D.material != null)
        {
            fondoCubo3D.material.color = fondo;
        }

        if (pantalla != null)
        {
            pantalla.color = texto;
        }
    }
    private void ReproducirSonido(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    private void MostrarFeedbackTemporal(string mensaje, Color fondo, Color texto, float duracion)
    {
        if (rutinaFeedback != null) StopCoroutine(rutinaFeedback);
        rutinaFeedback = StartCoroutine(RutinaFeedbackTemporal(mensaje, fondo, texto, duracion));
    }

    IEnumerator RutinaFeedbackTemporal(string mensaje, Color fondo, Color texto, float duracion)
    {
        estaBloqueado = true;

        if (pantalla != null) pantalla.text = mensaje;
        AplicarColores(fondo, texto);

        yield return new WaitForSeconds(duracion);

        EstablecerEstadoNormal();
    }
}