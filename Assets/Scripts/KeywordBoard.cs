
using UnityEngine;
using TMPro;
using System;

public class KeywordBoard : MonoBehaviour
{
    public TMP_Text textoPizarra;

    public string[] palabras =
    {
        "HUYE",
        "SILENCIO",
        "SOMBRA",
        "VETE"
    };

    public float intervaloCambio = 60f;

    private int indicePalabra = 0;
    private int cambiosRealizados = 0;

    public string PalabraActual
    {
        get { return palabras[indicePalabra]; }
    }

    public event Action<string> PalabraCambiada;

    void Start()
    {
        MostrarPalabra();

        // Cambia la palabra cada 60 segundos.
        InvokeRepeating(
            nameof(CambiarPalabra),
            intervaloCambio,
            intervaloCambio
        );
    }

    void CambiarPalabra()
    {
        // Solo permitir 3 cambios durante la partida.
        if (cambiosRealizados >= 3)
        {
            CancelInvoke(nameof(CambiarPalabra));
            return;
        }

        indicePalabra++;
        cambiosRealizados++;

        MostrarPalabra();

        Debug.Log("Nueva palabra clave: " + PalabraActual);
    }

    void MostrarPalabra()
    {
        if (textoPizarra != null)
        {
            textoPizarra.text = PalabraActual;
        }

        PalabraCambiada?.Invoke(PalabraActual);
    }
}