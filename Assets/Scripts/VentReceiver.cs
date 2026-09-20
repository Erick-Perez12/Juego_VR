
using UnityEngine;

public class VentReceiver : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        PuzzleObject objeto = other.GetComponent<PuzzleObject>();

        if (objeto == null)
            return;

        int indice = objeto.numeroAcertijo - 1;

        // Si ese acertijo ya fue resuelto, no vuelve a contar.
        if (gameManager.EstaResuelto(indice))
        {
            Debug.Log("Ese acertijo ya está resuelto.");
            return;
        }

        // El objeto corresponde a un acertijo pendiente.
        gameManager.ObjetoCorrecto(
            objeto.numeroAcertijo,
            objeto.numeroGrabado
        );

        objeto.gameObject.SetActive(false);
    }
}