
//using UnityEngine;

//public class MonsterVoiceHandler : MonoBehaviour
//{
//    public KeywordBoard pizarra;
//    public MonsterController monstruo;

//    public void ProcesarPalabraReconocida(string palabra)
//    {
//        if (pizarra == null || monstruo == null)
//            return;

//        string palabraLimpia = palabra.Trim().ToUpper();
//        string palabraActual = pizarra.PalabraActual.Trim().ToUpper();

//        Debug.Log("Jugador dijo: " + palabraLimpia);
//        Debug.Log("Palabra esperada: " + palabraActual);

//        if (palabraLimpia.Contains(palabraActual))
//        {
//            monstruo.Ahuyentar();
//            Debug.Log("¡Palabra correcta! El monstruo se retira.");
//        }
//        else
//        {
//            Debug.Log("Palabra incorrecta. El monstruo sigue presente.");
//        }
//    }

//}

using UnityEngine;

public class MonsterVoiceHandler : MonoBehaviour
{
    public KeywordBoard pizarra;
    public MonsterController monstruo;

    public void ProcesarPalabraReconocida(string palabra)
    {
        if (pizarra == null || monstruo == null)
            return;

        // Si el monstruo NO está activo en la escena, ignorar cualquier ruido o palabra
        if (!monstruo.EstaActivo())
            return;

        string palabraLimpia = palabra.Trim().ToUpper();
        string palabraActual = pizarra.PalabraActual.Trim().ToUpper();

        Debug.Log("Jugador dijo: " + palabraLimpia);
        Debug.Log("Palabra esperada: " + palabraActual);

        if (palabraLimpia.Contains(palabraActual))
        {
            monstruo.Ahuyentar();
            Debug.Log("¡Palabra correcta! El monstruo se retira.");
        }
        else
        {
            Debug.Log("Palabra incorrecta. El monstruo sigue presente.");
        }
    }
}