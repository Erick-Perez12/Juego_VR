//using UnityEngine;

//public class TimerManager : MonoBehaviour
//{
//    public float tiempoInicial = 300f;

//    private float tiempoRestante;
//    private bool juegoActivo = false;

//    void Start()
//    {
//        tiempoRestante = tiempoInicial;
//    }

//    void Update()
//    {
//        if (!juegoActivo)
//            return;

//        tiempoRestante -= Time.deltaTime;

//        if (tiempoRestante <= 0)
//        {
//            tiempoRestante = 0;
//            juegoActivo = false;

//            Debug.Log("TIEMPO AGOTADO");
//        }
//    }

//    public void IniciarTemporizador()
//    {
//        juegoActivo = true;
//    }

//    public void RestarTiempo(float segundos)
//    {
//        tiempoRestante -= segundos;

//        if (tiempoRestante < 0)
//            tiempoRestante = 0;

//        Debug.Log("Penalización: -" + segundos + " segundos");
//    }

//    public float ObtenerTiempo()
//    {
//        return tiempoRestante;
//    }
//}
//using UnityEngine;
//using TMPro; // Esencial para controlar TextMeshPro

//public class TimerManager : MonoBehaviour
//{
//    public float tiempoInicial = 300f; // 5 minutos = 300 segundos
//    public TMP_Text textoTiempo; // Arrastra tu componente TextMeshPro aquí

//    private float tiempoRestante;
//    private bool juegoActivo = false;

//    void Start()
//    {
//        tiempoRestante = tiempoInicial;
//        ActualizarTextoReloj();
//    }

//    void Update()
//    {
//        if (!juegoActivo)
//            return;

//        tiempoRestante -= Time.deltaTime;

//        if (tiempoRestante <= 0)
//        {
//            tiempoRestante = 0;
//            juegoActivo = false;
//            Debug.Log("TIEMPO AGOTADO");
//        }

//        ActualizarTextoReloj();
//    }

//    public void IniciarTemporizador()
//    {
//        juegoActivo = true;
//    }

//    public void RestarTiempo(float segundos)
//    {
//        tiempoRestante -= segundos;

//        if (tiempoRestante < 0)
//            tiempoRestante = 0;

//        Debug.Log("Penalización: -" + segundos + " segundos");
//        ActualizarTextoReloj();
//    }

//    public float ObtenerTiempo()
//    {
//        return tiempoRestante;
//    }

//    private void ActualizarTextoReloj()
//    {
//        if (textoTiempo != null)
//        {
//            int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
//            int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

//            // Formato MM:SS (ejemplo 05:00, 04:59)
//            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
//        }
//    }
//}

using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar la escena
using TMPro;
using System.Collections;

public class TimerManager : MonoBehaviour
{
    public float tiempoInicial = 300f; // 5 minutos = 300 segundos
    public TMP_Text textoTiempo; // Arrastra tu componente TextMeshPro aquí

    [Header("Configuración de GameOver")]
    public Transform jugadorVR;             // Arrastra tu OVRCameraRig / Player aquí
    public Transform puntoTeleportGameOver; // Arrastra el 'GameOverSpawnPoint' aquí

    [Header("Audio SFX Temporizador")]
    [Tooltip("AudioSource para el tictac en bucle")]
    public AudioSource audioRelojFondo;
    [Tooltip("Clip del sonido del reloj (Tic-Tac)")]
    public AudioClip sonidoRelojFondo;

    [Tooltip("AudioSource para los bips (o usará audioRelojFondo si se deja vacío)")]
    public AudioSource audioBip;
    [Tooltip("Clip del sonido Bip al restar tiempo")]
    public AudioClip sonidoBipPenalizacion;
    [Header("Ajuste de Duración del Beep")]
    [Tooltip("Duración máxima en segundos para cortar el beep (ejemplo: 0.5s)")]
    public float duracionBeep = 0.5f;
    private float tiempoRestante;
    private bool juegoActivo = false;
    private Coroutine rutinaDetenerBeep;

    void Start()
    {
        tiempoRestante = tiempoInicial;
        ActualizarTextoReloj();

        // Configurar el audio de fondo si está asignado
        if (audioRelojFondo != null && sonidoRelojFondo != null)
        {
            audioRelojFondo.clip = sonidoRelojFondo;
            audioRelojFondo.loop = true; // Que suene en bucle continuo
        }
        IniciarTemporizador();
    }

    void Update()
    {
        if (!juegoActivo)
            return;

        tiempoRestante -= Time.deltaTime;

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            juegoActivo = false;
            DetenerSonidoFondo();
            // Detener el ambiente del ducto al perder por tiempo
            if (DuctAmbienceManager.Instance != null)
            {
                DuctAmbienceManager.Instance.DetenerSonidoDucto();
            }
            Debug.Log("TIEMPO AGOTADO");

            // Desactivar al monstruo si existe en la escena
            MonsterController monstruo = FindFirstObjectByType<MonsterController>();
            if (monstruo != null)
            {
                monstruo.DesactivarMonstruoCompletamente();
            }

            // Cambiar mensaje y teletransportar
            if (GameOverManager.Instance != null)
            {
                GameOverManager.Instance.MostrarMensajePorTiempo();
            }

            TeletransportarAGameOver();
        }

        ActualizarTextoReloj();
    }

    public void IniciarTemporizador()
    {
        juegoActivo = true;
        // Iniciar el sonido constante del reloj
        if (audioRelojFondo != null && sonidoRelojFondo != null && !audioRelojFondo.isPlaying)
        {
            audioRelojFondo.Play();
        }
    }
    public void PausarTemporizador()
    {
        juegoActivo = false;

        DetenerSonidoFondo();
    }

    public void DetenerYGanar()
    {
        juegoActivo = false;
        DetenerSonidoFondo();
        TeletransportarAGameOver();
        Debug.Log("¡JUEGO GANADO!");
    }
    public void RestarTiempo(float segundos)
    {
        tiempoRestante -= segundos;

        if (tiempoRestante < 0)
            tiempoRestante = 0;

        // Reproducir sonido Bip reiniciando el anterior para que no se sobrepongan
        if (audioBip != null && sonidoBipPenalizacion != null)
        {
            // Detener la corrutina previa si estaba acortando un beep anterior
            if (rutinaDetenerBeep != null)
            {
                StopCoroutine(rutinaDetenerBeep);
            }

            audioBip.Stop(); // Detener el beep anterior inmediatamente
            audioBip.clip = sonidoBipPenalizacion;
            audioBip.time = 0f; // Reiniciar al inicio del audio
            audioBip.Play();

            // Iniciar corrutina para cortar el beep tras la duración establecida
            rutinaDetenerBeep = StartCoroutine(RutinaCortarBeep(duracionBeep));
        }
        Debug.Log("Penalización: -" + segundos + " segundos");
        ActualizarTextoReloj();
    }

    private IEnumerator RutinaCortarBeep(float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if (audioBip != null && audioBip.isPlaying)
        {
            audioBip.Stop();
        }
    }
    public float ObtenerTiempo()
    {
        return tiempoRestante;
    }

    private void ActualizarTextoReloj()
    {
        if (textoTiempo != null)
        {
            int minutos = Mathf.FloorToInt(tiempoRestante / 60f);
            int segundos = Mathf.FloorToInt(tiempoRestante % 60f);

            // Formato MM:SS (ejemplo 05:00, 04:59)
            textoTiempo.text = string.Format("{0:00}:{1:00}", minutos, segundos);
        }
    }
    private void DetenerSonidoFondo()
    {
        if (audioRelojFondo != null && audioRelojFondo.isPlaying)
        {
            audioRelojFondo.Stop();
        }
    }
    private void TeletransportarAGameOver()
    {
        VRTeleport.TeleportTo(jugadorVR, puntoTeleportGameOver);
        Debug.Log("Jugador teletransportado a GameOver con VRTeleport.");
    }
}