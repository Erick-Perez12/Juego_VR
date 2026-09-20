using System.Collections;
using UnityEngine;

public class SpawnInicialVR : MonoBehaviour
{
    [Tooltip("Objeto raíz del jugador (OVRCameraRig o el padre con CharacterController)")]
    public Transform playerRig;

    [Tooltip("Objeto vacío en 0.14, 1.78, 2.851 con la rotación inicial deseada")]
    public Transform spawnPoint;

    private IEnumerator Start()
    {
        // Esperar a que exista la cámara y el tracking del casco esté activo
        while (Camera.main == null) yield return null;
        yield return new WaitForSeconds(0.5f);

        VRTeleport.TeleportTo(playerRig, spawnPoint, 0f);
    }
}