//using UnityEngine;

//public class SocketReceiver : MonoBehaviour
//{
//    [Header("Referencias")]
//    public GameManager gameManager;
//    [Tooltip("Número de la vela/acertijo correspondiente (1, 2, 3 o 4)")]
//    public int numeroAcertijoEsperado = 1;

//    [Tooltip("Transform hijo donde encajará el objeto")]
//    public Transform socketSnapPoint;

//    private bool resuelto = false;

//    private void OnTriggerEnter(Collider other)
//    {
//        // Si la base ya está resuelta, ignorar cualquier otro objeto
//        if (resuelto) return;

//        PuzzleObject objeto = other.GetComponent<PuzzleObject>();
//        if (objeto == null) return;

//        // Comprobar si es el objeto correcto para esta base
//        if (objeto.numeroAcertijo == numeroAcertijoEsperado)
//        {
//            resuelto = true;

//            // 1. Acomodar posición y rotación exacta en el SnapPoint
//            if (socketSnapPoint != null)
//            {
//                other.transform.position = socketSnapPoint.position;
//                other.transform.rotation = socketSnapPoint.rotation;
//            }

//            // 2. Desactivar scripts de interacción de agarre de forma genérica
//            MonoBehaviour[] scripts = other.GetComponents<MonoBehaviour>();
//            foreach (MonoBehaviour script in scripts)
//            {
//                if (script != null && (script.GetType().Name.Contains("Grabbable") || script.GetType().Name.Contains("HandGrab")))
//                {
//                    script.enabled = false;
//                }
//            }

//            // 3. Detener física y congelar el objeto
//            Rigidbody rb = other.GetComponent<Rigidbody>();
//            if (rb != null)
//            {
//                rb.linearVelocity = Vector3.zero;
//                rb.angularVelocity = Vector3.zero;
//                rb.isKinematic = true;
//            }

//            // 4. Encender la vela correspondiente
//            if (gameManager != null)
//            {
//                gameManager.ObjetoCorrecto(objeto.numeroAcertijo, objeto.numeroGrabado);
//            }

//            Debug.Log("¡Objeto encajado y BLOQUEADO en la base " + numeroAcertijoEsperado + "!");
//        }
//        else
//        {
//            Debug.Log("Objeto incorrecto en este socket.");
//            if (gameManager != null)
//            {
//                gameManager.ObjetoIncorrecto();
//            }
//        }
//    }
//}

//using UnityEngine;

//public class SocketReceiver : MonoBehaviour
//{
//    [Header("Referencias")]
//    public GameManager gameManager;
//    [Tooltip("Número de la vela/acertijo correspondiente (1, 2 o 3)")]
//    public int numeroAcertijoEsperado = 1;

//    [Tooltip("Transform hijo donde encajará el objeto")]
//    public Transform socketSnapPoint;

//    private bool resuelto = false;
//    private float tiempoUltimoError = 0f;
//    private const float COOLDOWN_ERROR = 2.0f; // Tiempo mínimo antes de volver a restar tiempo en este socket

//    private void OnTriggerEnter(Collider other)
//    {
//        if (resuelto) return;

//        PuzzleObject objeto = other.GetComponent<PuzzleObject>();
//        if (objeto == null) return;

//        // Comprobar si es el objeto correcto para esta base
//        if (objeto.numeroAcertijo == numeroAcertijoEsperado)
//        {
//            resuelto = true;

//            // 1. Acomodar posición y rotación exacta en el SnapPoint
//            if (socketSnapPoint != null)
//            {
//                other.transform.position = socketSnapPoint.position;
//                other.transform.rotation = socketSnapPoint.rotation;
//            }

//            // 2. Desactivar scripts de agarre VR/Interacción
//            MonoBehaviour[] scripts = other.GetComponents<MonoBehaviour>();
//            foreach (MonoBehaviour script in scripts)
//            {
//                if (script != null && (script.GetType().Name.Contains("Grabbable") || script.GetType().Name.Contains("HandGrab")))
//                {
//                    script.enabled = false;
//                }
//            }

//            // 3. Congelar física
//            Rigidbody rb = other.GetComponent<Rigidbody>();
//            if (rb != null)
//            {
//                rb.linearVelocity = Vector3.zero;
//                rb.angularVelocity = Vector3.zero;
//                rb.isKinematic = true;
//            }

//            // 4. Notificar al GameManager
//            if (gameManager != null)
//            {
//                gameManager.ObjetoCorrecto(objeto.numeroAcertijo, objeto.numeroGrabado);
//            }

//            Debug.Log("¡Objeto encajado y BLOQUEADO en la base " + numeroAcertijoEsperado + "!");
//        }
//        else
//        {
//            // Solo restamos tiempo si pasaron más de 2 segundos desde el último intento
//            if (Time.time - tiempoUltimoError > COOLDOWN_ERROR)
//            {
//                tiempoUltimoError = Time.time;
//                Debug.Log("Objeto incorrecto en socket " + numeroAcertijoEsperado);
//                if (gameManager != null)
//                {
//                    gameManager.ObjetoIncorrecto();
//                }
//            }
//        }
//    }
//}

using UnityEngine;

public class SocketReceiver : MonoBehaviour
{
    [Header("Referencias")]
    public GameManager gameManager;
    [Tooltip("Número de la vela/acertijo correspondiente (1, 2 o 3)")]
    public int numeroAcertijoEsperado = 1;

    [Tooltip("Transform hijo donde encajará el objeto")]
    public Transform socketSnapPoint;

    private bool resuelto = false;
    private GameObject objetoEncajado;
    private float tiempoUltimoError = 0f;
    private const float COOLDOWN_ERROR = 2.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (resuelto) return;

        PuzzleObject objeto = other.GetComponent<PuzzleObject>();
        if (objeto == null) return;

        if (objeto.numeroAcertijo == numeroAcertijoEsperado)
        {
            resuelto = true;
            objetoEncajado = other.gameObject;

            // 1. Acomodar posición y rotación exacta en el SnapPoint
            if (socketSnapPoint != null)
            {
                other.transform.position = socketSnapPoint.position;
                other.transform.rotation = socketSnapPoint.rotation;
            }

            // 2. Desactivar scripts de agarre VR
            MonoBehaviour[] scripts = other.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script != null && (script.GetType().Name.Contains("Grabbable") || script.GetType().Name.Contains("HandGrab")))
                {
                    script.enabled = false;
                }
            }

            // 3. Congelar física
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // 4. Notificar al GameManager
            if (gameManager != null)
            {
                gameManager.ObjetoCorrecto(objeto.numeroAcertijo, objeto.numeroGrabado);
            }

            Debug.Log("¡Objeto encajado y BLOQUEADO en la base " + numeroAcertijoEsperado + "!");
        }
        else
        {
            if (Time.time - tiempoUltimoError > COOLDOWN_ERROR)
            {
                tiempoUltimoError = Time.time;
                if (gameManager != null)
                {
                    gameManager.ObjetoIncorrecto();
                }
            }
        }
    }

    // Método para resetear el socket cuando los objetos sean desordenados
    public void LiberarSocket()
    {
        if (!resuelto) return;

        resuelto = false;

        if (objetoEncajado != null)
        {
            // Reactivar física
            Rigidbody rb = objetoEncajado.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = false;
            }

            // Reactivar agarre VR
            MonoBehaviour[] scripts = objetoEncajado.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script != null && (script.GetType().Name.Contains("Grabbable") || script.GetType().Name.Contains("HandGrab")))
                {
                    script.enabled = true;
                }
            }

            objetoEncajado = null;
        }

        // Apagar la vela y resetear estado en el GameManager
        if (gameManager != null)
        {
            gameManager.QuitarObjeto(numeroAcertijoEsperado - 1);
        }
    }
}