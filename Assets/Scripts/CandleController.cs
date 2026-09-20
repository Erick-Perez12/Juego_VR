using UnityEngine;

public class CandleController : MonoBehaviour
{
    public GameObject llamaLuz;
    public bool encendida = false;

    void Start()
    {
        // Al iniciar el juego, nos aseguramos de que la vela empiece apagada
        encendida = false;

        if (llamaLuz != null)
        {
            llamaLuz.SetActive(false);
        }
    }

    public void Encender()
    {
        if (encendida)
            return;

        encendida = true;

        if (llamaLuz != null)
        {
            llamaLuz.SetActive(true);
        }

        Debug.Log("¡VELA ENCENDIDA!");
    }

    public void Apagar()
    {
        if (!encendida)
            return;

        encendida = false;

        if (llamaLuz != null)
        {
            llamaLuz.SetActive(false);
        }

        Debug.Log("VELA APAGADA");
    }
}