using UnityEngine;

public class MonsterVoiceVosk : MonoBehaviour
{
    public VoskSpeechToText voskComponent;
    public MonsterVoiceHandler voiceHandler;
    public MonsterController monstruo;

    void Start()
    {
        if (voskComponent == null)
        {
            voskComponent = FindFirstObjectByType<VoskSpeechToText>();
        }

        // Se suscribe al evento Action<string> usando +=
        if (voskComponent != null)
        {
            voskComponent.OnTranscriptionResult += ProcesarTextoJson;
            Debug.Log("Suscrito a VoskSpeechToText correctamente.");
        }
        else
        {
            Debug.LogError("No se encontró VoskSpeechToText en la escena.");
        }
    }

    void OnDestroy()
    {
        // Cancela la suscripción al destruir el objeto para evitar memory leaks
        if (voskComponent != null)
        {
            voskComponent.OnTranscriptionResult -= ProcesarTextoJson;
        }
    }

    public void ProcesarTextoJson(string jsonResult)
    {
        if (string.IsNullOrEmpty(jsonResult)) return;

        string textoLimpio = jsonResult.ToLower();
        Debug.Log("<color=yellow>Vosk transcribió: " + textoLimpio + "</color>");

        // Envía la palabra procesada a la pizarra
        if (voiceHandler != null)
        {
            voiceHandler.ProcesarPalabraReconocida(textoLimpio);
        }

        // O la compara directamente
        if (textoLimpio.Contains("huye") || textoLimpio.Contains("silencio") || textoLimpio.Contains("vete"))
        {
            if (monstruo != null)
            {
                monstruo.Ahuyentar();
                Debug.Log("¡AHUYENTANDO MONSTRUO POR VOZ!");
            }
        }
    }
}