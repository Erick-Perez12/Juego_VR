//using UnityEngine;

//public class MonsterVoiceVosk : MonoBehaviour
//{
//    public VoskSpeechToText voskComponent;
//    public MonsterVoiceHandler voiceHandler;
//    public MonsterController monstruo;

//    void Start()
//    {
//        if (voskComponent == null)
//        {
//            voskComponent = FindFirstObjectByType<VoskSpeechToText>();
//        }

//        // Se suscribe al evento Action<string> usando +=
//        if (voskComponent != null)
//        {
//            voskComponent.OnTranscriptionResult += ProcesarTextoJson;
//            Debug.Log("Suscrito a VoskSpeechToText correctamente.");
//        }
//        else
//        {
//            Debug.LogError("No se encontró VoskSpeechToText en la escena.");
//        }
//    }

//    void OnDestroy()
//    {
//        // Cancela la suscripción al destruir el objeto para evitar memory leaks
//        if (voskComponent != null)
//        {
//            voskComponent.OnTranscriptionResult -= ProcesarTextoJson;
//        }
//    }

//    public void ProcesarTextoJson(string jsonResult)
//    {
//        if (string.IsNullOrEmpty(jsonResult)) return;

//        string textoLimpio = jsonResult.ToLower();
//        Debug.Log("<color=yellow>Vosk transcribió: " + textoLimpio + "</color>");

//        // Envía la palabra procesada a la pizarra
//        if (voiceHandler != null)
//        {
//            voiceHandler.ProcesarPalabraReconocida(textoLimpio);
//        }

//        // O la compara directamente
//        if (textoLimpio.Contains("huye") || textoLimpio.Contains("silencio") || textoLimpio.Contains("vete"))
//        {
//            if (monstruo != null)
//            {
//                monstruo.Ahuyentar();
//                Debug.Log("¡AHUYENTANDO MONSTRUO POR VOZ!");
//            }
//        }
//    }
//}

using UnityEngine;

public class MonsterVoiceVosk : MonoBehaviour
{
    public VoskSpeechToText voskComponent;
    public MonsterVoiceHandler voiceHandler;
    public MonsterController monstruo;

    [System.Serializable] private class VoskAlt { public string text; public float confidence; }
    [System.Serializable] private class VoskResult { public VoskAlt[] alternatives; public string text; }

    void Start()
    {
        if (voskComponent == null)
            voskComponent = FindFirstObjectByType<VoskSpeechToText>();

        if (voskComponent != null)
            voskComponent.OnTranscriptionResult += ProcesarTextoJson;
        else
            Debug.LogError("No se encontró VoskSpeechToText en la escena.");
    }

    void OnDestroy()
    {
        if (voskComponent != null)
            voskComponent.OnTranscriptionResult -= ProcesarTextoJson;
    }

    public void ProcesarTextoJson(string jsonResult)
    {
        if (string.IsNullOrEmpty(jsonResult)) return;

        // Extraer solo el texto real del JSON
        string texto = "";
        VoskResult r = JsonUtility.FromJson<VoskResult>(jsonResult);
        if (r != null)
        {
            if (r.alternatives != null)
                foreach (var alt in r.alternatives) texto += " " + alt.text;
            else if (!string.IsNullOrEmpty(r.text))
                texto = r.text;
        }
        texto = texto.ToLower().Trim();

        bool activo = monstruo != null && monstruo.EstaActivo();
        Debug.Log($"[Voz] t={Time.time:F2} | texto='{texto}' | monstruoActivo={activo}");

        if (string.IsNullOrEmpty(texto)) return; // ignorar resultados vacíos

        if (!activo)
        {
            Debug.LogWarning("[Voz] Llegó tarde: el monstruo ya no estaba activo.");
            return;
        }

        if (voiceHandler != null)
            voiceHandler.ProcesarPalabraReconocida(texto);

        if (texto.Contains("huye") || texto.Contains("silencio") || texto.Contains("vete"))
        {
            monstruo.Ahuyentar();
            Debug.Log("¡AHUYENTANDO MONSTRUO POR VOZ!");
        }
    }
}