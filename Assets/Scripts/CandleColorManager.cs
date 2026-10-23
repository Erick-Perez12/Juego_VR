using UnityEngine;

public class CandleColorManager : MonoBehaviour
{
    [Header("Referencias de Luces de Velas")]
    public Light luzVela1; // Vela correspondencia 1
    public Light luzVela2; // Vela correspondencia 2
    public Light luzVela3; // Vela correspondencia 3

    [Header("Colores Iniciales (Orden Original)")]
    public Color colorInicialVela1 = Color.red;    // Ej: Rojo
    public Color colorInicialVela2 = Color.green;  // Ej: Verde
    public Color colorInicialVela3 = Color.blue;   // Ej: Azul

    [Header("Colores Tras el Apagón (Orden Mezclado)")]
    public Color colorNuevoVela1 = Color.blue;     // Cambia a Azul
    public Color colorNuevoVela2 = Color.red;      // Cambia a Rojo
    public Color colorNuevoVela3 = Color.green;    // Cambia a Verde

    void Start()
    {
        // Establecer los colores por defecto al iniciar la escena
        EstablecerColoresIniciales();
    }

    public void EstablecerColoresIniciales()
    {
        if (luzVela1 != null) luzVela1.color = colorInicialVela1;
        if (luzVela2 != null) luzVela2.color = colorInicialVela2;
        if (luzVela3 != null) luzVela3.color = colorInicialVela3;
    }

    public void MeclarColoresApagon()
    {
        // Asignamos la nueva combinación de colores durante la oscuridad
        if (luzVela1 != null) luzVela1.color = colorNuevoVela1;
        if (luzVela2 != null) luzVela2.color = colorNuevoVela2;
        if (luzVela3 != null) luzVela3.color = colorNuevoVela3;

        Debug.Log("¡Los colores de iluminación de las velas han sido reordenados!");
    }
}