using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonReiniciarVR : MonoBehaviour
{
    private bool reiniciando = false;

    private void OnTriggerEnter(Collider other)
    {
        if (reiniciando) return;

        if (other.name.Contains("Hand") || other.name.Contains("Controller") ||
            other.CompareTag("Player") || other.GetComponentInParent<OVRCameraRig>() != null)
        {
            reiniciando = true;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}