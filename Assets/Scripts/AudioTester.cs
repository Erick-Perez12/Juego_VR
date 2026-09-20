using UnityEngine;

public class AudioTester : MonoBehaviour
{
    private AudioClip micClip;
    private string micDevice;
    private float nivelAudio = 0f;

    void Start()
    {
        if (Microphone.devices.Length > 0)
        {
            // Toma el primer micrófono detectado por el sistema
            micDevice = Microphone.devices[0];
            micClip = Microphone.Start(micDevice, true, 1, 44100);
            Debug.Log("<color=green><b>[AUDIO TEST]</b> Micrófono iniciado: " + micDevice + "</color>");
        }
        else
        {
            Debug.LogError("[AUDIO TEST] No se detectó ningún micrófono conectado al sistema.");
        }
    }

    void Update()
    {
        if (micClip != null && Microphone.IsRecording(micDevice))
        {
            float[] waveData = new float[128];
            int micPosition = Microphone.GetPosition(micDevice) - 128;
            if (micPosition < 0) return;

            micClip.GetData(waveData, micPosition);

            float total = 0;
            for (int i = 0; i < 128; i++)
            {
                total += Mathf.Abs(waveData[i]);
            }
            nivelAudio = total / 128f;

            // Imprime en consola si detecta un pico de volumen
            if (nivelAudio > 0.02f)
            {
                Debug.Log("<color=cyan>¡SONIDO DETECTADO! Nivel: " + nivelAudio.ToString("F4") + "</color>");
            }
        }
    }

    // Dibuja el nivel de audio en la esquina superior izquierda de la pantalla
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 22;
        style.normal.textColor = Color.yellow;

        if (Microphone.devices.Length == 0)
        {
            GUI.Label(new Rect(20, 20, 500, 40), "MICRÓFONO: NO DETECTADO", style);
            return;
        }

        GUI.Label(new Rect(20, 20, 600, 40), "Mic: " + micDevice, style);
        GUI.Label(new Rect(20, 50, 600, 40), "Volumen Audio: " + (nivelAudio * 1000f).ToString("F1"), style);

        if (nivelAudio > 0.02f)
        {
            style.normal.textColor = Color.green;
            GUI.Label(new Rect(20, 80, 600, 40), "¡HABLANDO DETECTADO!", style);
        }
    }
}