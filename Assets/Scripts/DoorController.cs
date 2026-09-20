
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform puerta;
    public float anguloApertura = 90f;
    public float velocidad = 2f;

    [Header("Audio SFX Puerta")]
    public AudioSource audioSource;
    public AudioClip sonidoAbrir;

    private bool abierta = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        rotacionCerrada = puerta.rotation;
        rotacionAbierta = puerta.rotation *
            Quaternion.Euler(0, anguloApertura, 0);

        // Si no se asignó un AudioSource en el Inspector, intenta obtener el del mismo objeto
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void AbrirPuerta()
    {
        if (abierta) return; // Evita reproducir el sonido varias veces

        abierta = true;

        // Reproduce el sonido de la puerta una sola vez
        if (audioSource != null && sonidoAbrir != null)
        {
            audioSource.PlayOneShot(sonidoAbrir);
        }

        Debug.Log("¡ESCAPASTE!");
    }

    void Update()
    {
        if (abierta)
        {
            puerta.rotation = Quaternion.Slerp(
                puerta.rotation,
                rotacionAbierta,
                Time.deltaTime * velocidad
            );
        }
    }
}