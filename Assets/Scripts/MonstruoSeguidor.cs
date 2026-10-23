using UnityEngine;
using UnityEngine.AI;


public class MonstruoSeguidor : MonoBehaviour
{
    public Transform jugador;
    public float distanciaParaMatar = 1.5f;

    private NavMeshAgent agente;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();

        jugador = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (jugador == null)
            return;

        // Seguir al jugador
        agente.SetDestination(jugador.position);

        // Si está suficientemente cerca, mata al jugador
        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaParaMatar)
        {
            MatarJugador();
        }
    }

    void MatarJugador()
    {
        Debug.Log("¡EL MONSTRUO TE ATRAPÓ!");

        // Aquí después podemos poner:
        // animación de muerte
        // sonido
        // pantalla de Game Over
        // reiniciar escena
    }
}
