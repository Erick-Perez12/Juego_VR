//using UnityEngine;

//public class WinTrigger : MonoBehaviour
//{
//    [Header("Referencias del Jugador")]
//    [Tooltip("Objeto raíz de tu VR (ej. OVRCameraRig)")]
//    public Transform playerRig;

//    [Header("Destino de Victoria")]
//    [Tooltip("Objeto vacío en la habitación negra donde aparecerá el jugador")]
//    public Transform blackRoomTarget;

//    [Header("Gestor de Tiempo")]
//    [Tooltip("Arrastra aquí el objeto que tiene el script TimeManager")]
//    public TimerManager timeManager;

//    private void OnTriggerEnter(Collider other)
//    {
//        // Detecta si lo que tocó el cubo fue una mano o controlador
//        if (other.CompareTag("Hand") || other.name.Contains("Hand") || other.name.Contains("Controller") || other.name.Contains("Poke"))
//        {
//            // 1. Detener el reloj si asignaste el TimeManager
//            if (timeManager != null)
//            {
//                timeManager.PausarTemporizador();
//            }

//            // 2. Teletransportar al jugador
//            TeleportToBlackRoom();
//        }
//    }

//    public void TeleportToBlackRoom()
//    {
//        VRTeleport.TeleportTo(playerRig, blackRoomTarget);
//        Debug.Log("¡Victoria! Jugador teletransportado sin desfase.");
//    }
//}
using UnityEngine;

public class WinTrigger : MonoBehaviour
{
    [Header("Referencias del Jugador")]
    [Tooltip("Objeto raíz de tu VR (ej. OVRCameraRig o PlayerController)")]
    public Transform playerRig;

    [Header("Destino de Victoria")]
    [Tooltip("Objeto vacío en la habitación negra donde aparecerá el jugador")]
    public Transform blackRoomTarget;

    [Header("Gestor de Tiempo")]
    [Tooltip("Arrastra aquí el objeto que tiene el script TimerManager")]
    public TimerManager timeManager;

    private bool activado = false;

    private void OnTriggerEnter(Collider other)
    {
        if (activado) return;

        // Verificar si la colisión proviene de una mano, controlador, interactor de Meta XR o cuerpo del jugador
        if (EsManoOJugador(other))
        {
            activado = true;

            // 1. Detener el temporizador
            if (timeManager != null)
            {
               timeManager.PausarTemporizador();
           }
            // Cambiar el mensaje en la pizarra
            if (GameOverManager.Instance != null)
            {
                GameOverManager.Instance.MostrarMensajeVictoria();
            }
            // Desactivar al monstruo para que no interrumpa en el cuarto negro
            MonsterController monstruo = FindFirstObjectByType<MonsterController>();
            if (monstruo != null)
            {
                monstruo.DesactivarMonstruoCompletamente();
            }

            if (DuctAmbienceManager.Instance != null)
            {
                DuctAmbienceManager.Instance.DetenerSonidoDucto();
            }
            // 2. Teletransportar al jugador sin desfase
            TeleportToBlackRoom();
        }
    }

    private bool EsManoOJugador(Collider other)
    {
        string nombre = other.name.ToLower();
        string tag = other.tag.ToLower();

        // 1. Detección por nombre o tag
        if (nombre.Contains("hand") || nombre.Contains("controller") || nombre.Contains("poke") ||
            nombre.Contains("finger") || nombre.Contains("interactor") || nombre.Contains("player") ||
            tag == "hand" || tag == "player")
        {
            return true;
        }

        // 2. Detección si el objeto o sus padres pertenecen a OVRCameraRig o al jugador
        if (other.GetComponentInParent<OVRCameraRig>() != null || other.GetComponentInParent<CharacterController>() != null)
        {
            return true;
        }

        return false;
    }

    public void TeleportToBlackRoom()
    {
        if (playerRig != null && blackRoomTarget != null)
        {
            VRTeleport.TeleportTo(playerRig, blackRoomTarget);
            Debug.Log("¡Victoria! Jugador teletransportado sin desfase.");
        }
        else
        {
            Debug.LogWarning("WinTrigger: Falta asignar 'playerRig' o 'blackRoomTarget' en el Inspector.");
        }
    }

    // Método para permitir reactivar si se reinicia la escena o prueba
    public void ResetearTrigger()
    {
        activado = false;
    }
}