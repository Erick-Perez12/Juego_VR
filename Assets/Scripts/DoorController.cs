
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Transform puerta;
    public float anguloApertura = 90f;
    public float velocidad = 2f;

    private bool abierta = false;
    private Quaternion rotacionCerrada;
    private Quaternion rotacionAbierta;

    void Start()
    {
        rotacionCerrada = puerta.rotation;
        rotacionAbierta = puerta.rotation *
            Quaternion.Euler(0, anguloApertura, 0);
    }

    public void AbrirPuerta()
    {
        abierta = true;
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