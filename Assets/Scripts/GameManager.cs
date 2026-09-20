
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TimerManager timer;

    public CandleController[] velas;

    // Los acertijos empiezan todos sin resolver.
    private bool[] acertijosResueltos = new bool[4];

    public int numero1 = 7;
    public int numero2 = 3;
    public int numero3 = 9;
    public int numero4 = 1;

    void Start()
    {
        timer.IniciarTemporizador();
        Debug.Log("JUEGO INICIADO");
    }

    public bool EstaResuelto(int indice)
    {
        if (indice < 0 || indice >= acertijosResueltos.Length)
            return false;

        return acertijosResueltos[indice];
    }

    public void ObjetoCorrecto(int numeroAcertijo, int numero)
    {
        int indice = numeroAcertijo - 1;

        if (indice < 0 || indice >= acertijosResueltos.Length)
            return;

        // Evita resolver el mismo acertijo dos veces.
        if (acertijosResueltos[indice])
        {
            Debug.Log("Este acertijo ya fue resuelto.");
            return;
        }

        acertijosResueltos[indice] = true;

        Debug.Log("¡ACERTIJO " + numeroAcertijo + " RESUELTO!");
        Debug.Log("Número encontrado: " + numero);

        // Enciende la vela que corresponde a ese acertijo.
        if (velas != null && indice < velas.Length
            && velas[indice] != null)
        {
            velas[indice].Encender();
        }

        // Comprueba si ya se resolvieron los cuatro.
        ComprobarVictoria();
    }

    public void ObjetoIncorrecto()
    {
        Debug.Log("OBJETO INCORRECTO");
        timer.RestarTiempo(30f);
    }

    void ComprobarVictoria()
    {
        for (int i = 0; i < acertijosResueltos.Length; i++)
        {
            if (!acertijosResueltos[i])
                return;
        }

        Debug.Log("¡TODOS LOS ACERTIJOS COMPLETADOS!");
        Debug.Log("Código final: " +
            numero1 + numero2 + numero3 + numero4);
    }

    public bool TodosLosAcertijosResueltos()
    {
        for (int i = 0; i < acertijosResueltos.Length; i++)
        {
            if (!acertijosResueltos[i])
                return false;
        }

        return true;
    }

    public void QuitarObjeto(int indice)
    {
        if (indice < 0 || indice >= acertijosResueltos.Length)
            return;

        // Marcamos el acertijo como NO resuelto
        acertijosResueltos[indice] = false;

        // Apagamos la vela correspondiente
        if (velas != null && indice < velas.Length && velas[indice] != null)
        {
            velas[indice].Apagar(); // Asegúrate de que CandleController tenga el método Apagar()
        }

        Debug.Log("Acertijo " + (indice + 1) + " desmarcado.");
    }
}