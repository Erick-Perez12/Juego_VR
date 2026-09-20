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

public class TimerManager : MonoBehaviour
{
    public float tiempoInicial = 300f; // 5 minutos = 300 segundos
    public TMP_Text textoTiempo; // Arrastra tu componente TextMeshPro aquí

    [Header("Configuración de GameOver")]
    public Transform jugadorVR;             // Arrastra tu OVRCameraRig / Player aquí
    public Transform puntoTeleportGameOver; // Arrastra el 'GameOverSpawnPoint' aquí

    private float tiempoRestante;
    private bool juegoActivo = false;

    void Start()
    {
        tiempoRestante = tiempoInicial;
        ActualizarTextoReloj();
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
    }
    public void PausarTemporizador()
    {
        juegoActivo = false;
    }

    public void DetenerYGanar()
    {
        juegoActivo = false;
        TeletransportarAGameOver();
        Debug.Log("¡JUEGO GANADO!");
    }
    public void RestarTiempo(float segundos)
    {
        tiempoRestante -= segundos;

        if (tiempoRestante < 0)
            tiempoRestante = 0;

        Debug.Log("Penalización: -" + segundos + " segundos");
        ActualizarTextoReloj();
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

    private void TeletransportarAGameOver()
    {
        VRTeleport.TeleportTo(jugadorVR, puntoTeleportGameOver);
        Debug.Log("Jugador teletransportado a GameOver con VRTeleport.");
    }
}