using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Referencia al Texto")]
    [Tooltip("Arrastra aquí el objeto TextoPizarra que contiene el TextMeshPro")]
    public TMP_Text textoPizarra;

    public static GameOverManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void MostrarMensajeVictoria()
    {
        if (textoPizarra != null)
        {
            textoPizarra.text = "¡FELICIDADES!\n¿PUDISTE ESCAPAR O NO?\n\n<size=70%>Mete la mano para reiniciar</size>";
        }
    }

    public void MostrarMensajePorTiempo()
    {
        if (textoPizarra != null)
        {
            textoPizarra.text = "GAME OVER\nCreo que las adivinanzas fueron difíciles...\n\n<size=70%>Mete la mano para reiniciar</size>";
        }
    }

    public void MostrarMensajePorMonstruo()
    {
        if (textoPizarra != null)
        {
            textoPizarra.text = "GAME OVER\nDi la palabra correcta en su presencia.\n\n<size=70%>Mete la mano para reiniciar</size>";
        }
    }
}
