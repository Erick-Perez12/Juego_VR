using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    [Header("Referencia al Texto")]
    [Tooltip("Arrastra aquí el objeto TextoPizarra que contiene el TextMeshPro")]
    public TMP_Text textoPizarra;

    [Header("Efectos Visuales de Victoria")]
    [Tooltip("Arrastra aquí el GameObject del Pentagrama para que aparezca al ganar")]
    public GameObject objetoPentagrama;

    public static GameOverManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Asegura que el pentagrama comience oculto al iniciar el juego
        if (objetoPentagrama != null)
        {
            objetoPentagrama.SetActive(false);
        }

    }

    public void MostrarMensajeVictoria()
    {
        if (textoPizarra != null)
        {
            textoPizarra.text = "¡FELICIDADES!\n PUDISTE ESCAPAR O ¿NO?\n\n<size=70%>Mete la mano para reiniciar</size>";
        }
        if (objetoPentagrama != null)
        {
            objetoPentagrama.SetActive(true);
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

    private void OcultarPentagrama()
    {
        if (objetoPentagrama != null)
        {
            objetoPentagrama.SetActive(false);
        }
    }
}
