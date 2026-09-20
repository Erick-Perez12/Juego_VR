
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
    public string codigoCorrecto = "7391";

    private string codigoIngresado = "";
    private const int longitudCodigo = 4;
    private bool estaBloqueado = false;
    private Coroutine rutinaFeedback;

    void Start()
    {
        EstablecerEstadoNormal();
    }

    public void PresionarNumero(int numero)
    {
        if (estaBloqueado) return;

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
            Debug.Log("Primero debes resolver los cuatro acertijos.");
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