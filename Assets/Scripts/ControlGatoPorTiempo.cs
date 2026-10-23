using UnityEngine;

public class ControlGatoPorTiempo : MonoBehaviour
{
    public Animator animatorGato;
    public AudioSource audioSource;

    // Estructura para definir un rango de tiempo donde el gato debe bailar
    [System.Serializable]
    public struct IntervaloBaile
    {
        public float tiempoInicio; // Segundo en que empieza el canto
        public float tiempoFin;    // Segundo en que termina
    }

    [Header("Intervalos donde canta/se mueve el gato")]
    public IntervaloBaile[] intervalos = new IntervaloBaile[]
    {
        new IntervaloBaile { tiempoInicio = 0.0f,  tiempoFin = 5.5f },  // 00:00 - 00:05
        new IntervaloBaile { tiempoInicio = 19.0f, tiempoFin = 29.5f }, // 00:19 - 00:29
        new IntervaloBaile { tiempoInicio = 33.0f, tiempoFin = 43.0f }  // 00:33 - 00:43
    };

    void Start()
    {
        if (animatorGato == null) animatorGato = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (audioSource == null || animatorGato == null) return;

        float tiempoActual = audioSource.time;

        // Si sobrepasa el segundo 43, detener audio y animación
        if (tiempoActual >= 43.0f)
        {
            animatorGato.speed = 0f;
            if (audioSource.isPlaying) audioSource.Stop();
            return;
        }

        // Evaluar si el segundo actual está dentro de algún intervalo de canto
        bool debeBailar = false;
        foreach (var intervalo in intervalos)
        {
            if (tiempoActual >= intervalo.tiempoInicio && tiempoActual <= intervalo.tiempoFin)
            {
                debeBailar = true;
                break;
            }
        }

        // Si está en el rango, la animación se reproduce (velocidad 1), si no, se detiene (velocidad 0)
        animatorGato.speed = debeBailar ? 1.0f : 0.0f;
    }
}