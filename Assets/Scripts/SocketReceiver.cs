using UnityEngine;

public class SocketReceiver : MonoBehaviour
{
    [Header("Referencias")]
    public GameManager gameManager;
    [Tooltip("Número de la vela/acertijo correspondiente (1, 2, 3 o 4)")]
    public int numeroAcertijoEsperado = 1;

    [Tooltip("Transform hijo donde encajará el objeto")]
    public Transform socketSnapPoint;

    private bool resuelto = false;

    private void OnTriggerEnter(Collider other)
    {
        // Si la base ya está resuelta, ignorar cualquier otro objeto
        if (resuelto) return;

        PuzzleObject objeto = other.GetComponent<PuzzleObject>();
        if (objeto == null) return;

        // Comprobar si es el objeto correcto para esta base
        if (objeto.numeroAcertijo == numeroAcertijoEsperado)
        {
            resuelto = true;

            // 1. Acomodar posición y rotación exacta en el SnapPoint
            if (socketSnapPoint != null)
            {
                other.transform.position = socketSnapPoint.position;
                other.transform.rotation = socketSnapPoint.rotation;
            }

            // 2. Desactivar scripts de interacción de agarre de forma genérica
            MonoBehaviour[] scripts = other.GetComponents<MonoBehaviour>();
            foreach (MonoBehaviour script in scripts)
            {
                if (script != null && (script.GetType().Name.Contains("Grabbable") || script.GetType().Name.Contains("HandGrab")))
                {
                    script.enabled = false;
                }
            }

            // 3. Detener física y congelar el objeto
            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.isKinematic = true;
            }

            // 4. Encender la vela correspondiente
            if (gameManager != null)
            {
                gameManager.ObjetoCorrecto(objeto.numeroAcertijo, objeto.numeroGrabado);
            }

            Debug.Log("¡Objeto encajado y BLOQUEADO en la base " + numeroAcertijoEsperado + "!");
        }
        else
        {
            Debug.Log("Objeto incorrecto en este socket.");
            if (gameManager != null)
            {
                gameManager.ObjetoIncorrecto();
            }
        }
    }
}