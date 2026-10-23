//using UnityEngine;

//public class MonsterAnimationManager : MonoBehaviour
//{
//    public Animator animator;
//    public AudioSource audioSource;
//    public AudioClip sonidoRugido;
//    public AudioClip sonidoScreamer;

//    //void Start()
//    //{
//    //    if (animator == null)
//    //        animator = GetComponent<Animator>();
//    //}
//    void Awake() // Usa Awake en lugar de Start
//    {
//        if (animator == null)
//            animator = GetComponent<Animator>();
//    }
//    // 1. Método para cuando el monstruo aparece
//    public void AparecerYRugir()
//    {
//        gameObject.SetActive(true);

//        if (animator != null)
//        {
//            animator.SetTrigger("Rugir");
//        }

//        if (audioSource != null && sonidoRugido != null)
//        {
//            audioSource.PlayOneShot(sonidoRugido);
//        }
//    }

//    // 2. Método para el Screamer/Jumpscare cuando pierde por tiempo
//    public void EjecutarScreamer(Transform puntoFrenteCamara)
//    {
//        gameObject.SetActive(true);

//        // Mover al monstruo directamente frente a la cara del jugador VR
//        if (puntoFrenteCamara != null)
//        {
//            transform.position = puntoFrenteCamara.position;
//            transform.rotation = puntoFrenteCamara.rotation;
//        }

//        if (animator != null)
//        {
//            animator.SetTrigger("Screamer");
//        }

//        if (audioSource != null && sonidoScreamer != null)
//        {
//            audioSource.PlayOneShot(sonidoScreamer);
//        }
//    }
//}

using UnityEngine;

public class MonsterAnimationManager : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip sonidoRugido;
    public AudioClip sonidoScreamer;

    //void Start()
    //{
    //    if (animator == null)
    //        animator = GetComponent<Animator>();
    //}
    void Awake() // Usa Awake en lugar de Start
    {
        if (animator == null)
            animator = GetComponent<Animator>();
    }
    // 1. Método para cuando el monstruo aparece
    public void AparecerYRugir()
    {
        gameObject.SetActive(true);

        if (animator != null)
        {
            animator.SetTrigger("Rugir");
        }

        if (audioSource != null && sonidoRugido != null)
        {
            audioSource.PlayOneShot(sonidoRugido);
        }
    }

    // 2. Método para el Screamer/Jumpscare cuando pierde por tiempo
    //public void EjecutarScreamer(Transform puntoFrenteCamara)
    //{
    //    gameObject.SetActive(true);

    //    // Mover al monstruo directamente frente a la cara del jugador VR
    //    if (puntoFrenteCamara != null)
    //    {
    //        transform.position = puntoFrenteCamara.position;
    //        transform.rotation = puntoFrenteCamara.rotation;
    //    }

    //    if (animator != null)
    //    {
    //        animator.SetTrigger("Screamer");
    //    }

    //    if (audioSource != null && sonidoScreamer != null)
    //    {
    //        audioSource.PlayOneShot(sonidoScreamer);
    //    }
    //}
    public void EjecutarScreamer(Transform puntoFrenteCamara)
    {
        if (puntoFrenteCamara != null)
        {
            // Posicionar a 0.7 - 1 metro justo delante del visor VR
            Vector3 posicionObjetivo = puntoFrenteCamara.position + (puntoFrenteCamara.forward * 0.75f);
            posicionObjetivo.y = puntoFrenteCamara.position.y - 0.4f; // Ajustar la altura según el modelo

            transform.position = posicionObjetivo;

            // Orientar cara a cara
            Vector3 mirarHacia = puntoFrenteCamara.position - transform.position;
            mirarHacia.y = 0;
            if (mirarHacia != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(mirarHacia);
            }
        }

        // Activar animación
        if (animator != null)
        {
            animator.SetTrigger("Screamer");
        }

        // Reproducir audio
        if (audioSource != null && sonidoScreamer != null)
        {
            audioSource.PlayOneShot(sonidoScreamer);
        }
    }
}
