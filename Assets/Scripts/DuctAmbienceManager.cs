using UnityEngine;
using System.Collections;

public class DuctAmbienceManager : MonoBehaviour
{
    public static DuctAmbienceManager Instance;

    [Header("Componentes de Audio")]
    [Tooltip("AudioSource ubicado en la zona del ducto")]
    public AudioSource audioDucto;
    [Tooltip("Clip con el sonido del ducto")]
    public AudioClip sonidoDucto;

    [Header("Configuración de Tiempos Aleatorios (Segundos)")]
    [Tooltip("Tiempo mínimo de espera para reproducir el sonido")]
    public float tiempoMinimoEspera = 10f;
    [Tooltip("Tiempo máximo de espera para reproducir el sonido")]
    public float tiempoMaximoEspera = 25f;

    private bool puedeSonar = true;
    private Coroutine rutinaSonidoAleatorio;

    private void Awake()
    {
        // Singleton sencillo para acceder desde WinTrigger o MonsterController
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (audioDucto != null && sonidoDucto != null)
        {
            audioDucto.clip = sonidoDucto;
            audioDucto.loop = false; // Queremos que suene por lapsos aleatorios
        }

        // Iniciar el ciclo de reproducción aleatoria
        IniciarCicloAleatorio();
    }

    public void IniciarCicloAleatorio()
    {
        puedeSonar = true;
        if (rutinaSonidoAleatorio != null)
        {
            StopCoroutine(rutinaSonidoAleatorio);
        }
        rutinaSonidoAleatorio = StartCoroutine(RutinaReproducirAleatorio());
    }

    private IEnumerator RutinaReproducirAleatorio()
    {
        while (puedeSonar)
        {
            // 1. Esperar un tiempo aleatorio entre el rango definido
            float tiempoEspera = Random.Range(tiempoMinimoEspera, tiempoMaximoEspera);
            yield return new WaitForSeconds(tiempoEspera);

            // 2. Si sigue permitido sonar, reproducir el clip
            if (puedeSonar && audioDucto != null && sonidoDucto != null)
            {
                audioDucto.Play();

                // Esperar a que termine de sonar el audio antes de contar el siguiente lapso
                yield return new WaitForSeconds(sonidoDucto.length);
            }
        }
    }

    /// <summary>
    /// Detiene el sonido actual e impide que vuelva a sonar (usar en GameOver, Victoria o cuando aparece el Monstruo).
    /// </summary>
    public void DetenerSonidoDucto()
    {
        puedeSonar = false;

        if (rutinaSonidoAleatorio != null)
        {
            StopCoroutine(rutinaSonidoAleatorio);
        }

        if (audioDucto != null && audioDucto.isPlaying)
        {
            audioDucto.Stop();
        }

        Debug.Log("Sonido de ducto detenido por estado del juego.");
    }
}