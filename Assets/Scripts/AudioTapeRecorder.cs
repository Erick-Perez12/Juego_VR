using UnityEngine;

public class AudioTapeRecorder : MonoBehaviour
{
    [Header("Componente de Audio Principal")]
    [SerializeField] private AudioSource audioSource;

    [Header("Pistas de Audio")]
    [Tooltip("Audio para el botón Play (verde)")]
    public AudioClip audioPlayNormal;
    [Tooltip("Audio para el tercer botón (amarillo) que no se puede detener")]
    public AudioClip audioTercerBoton;

    private bool esAudioImparable = false;

    /// <summary>
    /// Botón Verde: Reproduce el audio normal (se puede pausar/detener con el rojo).
    /// </summary>
    public void PlayAudioNormal()
    {
        // Si el audio imparable está sonando, ignorar el botón verde
        if (esAudioImparable && audioSource.isPlaying) return;

        esAudioImparable = false;
        if (audioSource != null && audioPlayNormal != null)
        {
            audioSource.clip = audioPlayNormal;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Botón Amarillo: Reproduce el audio completo y bloquea la detención hasta que termine.
    /// </summary>
    public void PlayAudioIninterrumpido()
    {
        if (audioSource != null && audioTercerBoton != null)
        {
            // Si ya está sonando este mismo audio, no reiniciar
            if (audioSource.isPlaying && esAudioImparable) return;

            esAudioImparable = true;
            audioSource.clip = audioTercerBoton;
            audioSource.Play();
        }
    }

    /// <summary>
    /// Botón Rojo: Detiene o pausa el audio, salvo que sea la pista imparable activa.
    /// </summary>
    public void StopAudio()
    {
        // Si la pista activa es la ininterrumpida y sigue sonando, el botón rojo NO hace nada
        if (esAudioImparable && audioSource.isPlaying)
        {
            Debug.Log("Este audio no se puede detener hasta que termine.");
            return;
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void Update()
    {
        // Liberar el bloqueo automáticamente cuando el audio termine de sonar
        if (esAudioImparable && !audioSource.isPlaying)
        {
            esAudioImparable = false;
        }
    }
}