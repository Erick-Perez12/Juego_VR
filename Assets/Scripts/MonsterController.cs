
//using System.Collections;
//using UnityEngine;

//public class MonsterController : MonoBehaviour
//{
//    [Header("Referencias")]
//    public GameObject monstruo;
//    public Transform puntoAparicion;
//    public AudioSource audioMonstruo;

//    [Header("Tiempo de aparición")]
//    public float esperaMinima = 20f;
//    public float esperaMaxima = 40f;
//    public float duracionAparicion = 8f;

//    private bool monstruoActivo = false;
//    private Coroutine rutina;

//    void Start()
//    {
//        if (monstruo != null)
//        {
//            monstruo.SetActive(false);
//        }

//        rutina = StartCoroutine(CicloEnemigo());
//    }

//    IEnumerator CicloEnemigo()
//    {
//        while (true)
//        {
//            float espera = Random.Range(esperaMinima, esperaMaxima);
//            yield return new WaitForSeconds(espera);

//            Aparecer();

//            yield return new WaitForSeconds(duracionAparicion);

//            if (monstruoActivo)
//            {
//                Retirarse();
//            }
//        }
//    }

//    public void Aparecer()
//    {
//        if (monstruo == null || monstruoActivo)
//            return;

//        if (puntoAparicion != null)
//        {
//            monstruo.transform.position = puntoAparicion.position;
//            monstruo.transform.rotation = puntoAparicion.rotation;
//        }

//        monstruo.SetActive(true);
//        monstruoActivo = true;

//        if (audioMonstruo != null)
//        {
//            audioMonstruo.Play();
//        }

//        Debug.Log("¡EL MONSTRUO HA APARECIDO!");
//    }

//    public void Retirarse()
//    {
//        if (monstruo == null || !monstruoActivo)
//            return;

//        monstruo.SetActive(false);
//        monstruoActivo = false;

//        if (audioMonstruo != null)
//        {
//            audioMonstruo.Stop();
//        }

//        Debug.Log("El monstruo se ha retirado.");
//    }

//    public void Ahuyentar()
//    {
//        if (!monstruoActivo)
//            return;

//        Retirarse();

//        Debug.Log("¡PALABRA CLAVE CORRECTA! Monstruo ahuyentado.");
//    }
//}

//using System.Collections;
//using UnityEngine;

//public class MonsterController : MonoBehaviour
//{
//    [Header("Referencias")]
//    public GameObject monstruo;
//    public Transform puntoAparicion;
//    public AudioSource audioMonstruo;

//    [Header("Tiempo de aparición")]
//    public float esperaMinima = 20f;
//    public float esperaMaxima = 40f;
//    public float duracionAparicion = 8f;

//    private bool monstruoActivo = false;
//    private Coroutine rutina;

//    void Start()
//    {
//        if (monstruo != null)
//        {
//            monstruo.SetActive(false);
//        }

//        rutina = StartCoroutine(CicloEnemigo());
//    }

//    IEnumerator CicloEnemigo()
//    {
//        while (true)
//        {
//            float espera = Random.Range(esperaMinima, esperaMaxima);
//            yield return new WaitForSeconds(espera);

//            Aparecer();

//            yield return new WaitForSeconds(duracionAparicion);

//            if (monstruoActivo)
//            {
//                Retirarse();
//            }
//        }
//    }

//    public void Aparecer()
//    {
//        if (monstruo == null || monstruoActivo)
//            return;

//        if (puntoAparicion != null)
//        {
//            monstruo.transform.position = puntoAparicion.position;
//            monstruo.transform.rotation = puntoAparicion.rotation;
//        }

//        monstruo.SetActive(true);
//        monstruoActivo = true;

//        if (audioMonstruo != null)
//        {
//            audioMonstruo.Play();
//        }

//        Debug.Log("¡EL MONSTRUO HA APARECIDO!");
//    }

//    public void Retirarse()
//    {
//        if (monstruo == null || !monstruoActivo)
//            return;

//        monstruo.SetActive(false);
//        monstruoActivo = false;

//        if (audioMonstruo != null)
//        {
//            audioMonstruo.Stop();
//        }

//        Debug.Log("El monstruo se ha retirado.");
//    }

//    public void Ahuyentar()
//    {
//        if (!monstruoActivo)
//            return;

//        Retirarse();

//        Debug.Log("¡PALABRA CLAVE CORRECTA! Monstruo ahuyentado.");
//    }

//    // Permite a otros scripts saber si el monstruo está presente
//    public bool EstaActivo()
//    {
//        return monstruoActivo;
//    }
//}

using System.Collections;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [Header("Referencias Monstruo")]
    public GameObject monstruo;
    public Transform puntoAparicion;
    public AudioSource audioMonstruo;

    [Header("Referencias de Teletransporte GameOver")]
    [Tooltip("Objeto raíz del jugador (OVRCameraRig / PlayerController)")]
    public Transform playerRig;
    [Tooltip("Punto de destino en la habitación negra (GameOverSpawnPoint)")]
    public Transform gameOverSpawnPoint;

    [Header("Gestor de Tiempo (Opcional)")]
    [Tooltip("Si lo asignas, pausará el reloj al perder")]
    public TimerManager timerManager;

    [Header("Referencias de Iluminación")]
    [Tooltip("Arrastra aquí el componente Light del foco de tu techo")]
    public Light luzTecho;
    public float velocidadParpadeoMin = 0.05f;
    public float velocidadParpadeoMax = 0.2f;

    [Header("Tiempo de aparición")]
    public float esperaMinima = 20f;
    public float esperaMaxima = 40f;
    public float duracionAparicion = 8f;

    private bool monstruoActivo = false;
    private Coroutine rutinaCiclo;
    private Coroutine rutinaParpadeo;
    private float intensidadOriginalLuz;

    void Start()
    {
        if (monstruo != null)
        {
            monstruo.SetActive(false);
        }

        // Guardar la intensidad inicial de la luz si está asignada
        if (luzTecho != null)
        {
            intensidadOriginalLuz = luzTecho.intensity;
            luzTecho.enabled = true; // Asegurar que empiece encendida
        }

        rutinaCiclo = StartCoroutine(CicloEnemigo());
    }

    IEnumerator CicloEnemigo()
    {
        while (true)
        {
            float espera = Random.Range(esperaMinima, esperaMaxima);
            yield return new WaitForSeconds(espera);

            Aparecer();

            yield return new WaitForSeconds(duracionAparicion);

            if (monstruoActivo)
            {
                //Retirarse();
                JugadorPierde();
                break; // Romper el ciclo de aparición
            }
        }
    }

    public void Aparecer()
    {
        if (monstruo == null || monstruoActivo)
            return;

        if (puntoAparicion != null)
        {
            monstruo.transform.position = puntoAparicion.position;
            monstruo.transform.rotation = puntoAparicion.rotation;
        }

        monstruo.SetActive(true);
        monstruoActivo = true;
        // Detener el ambiente del ducto mientras el monstruo esté presente
        if (DuctAmbienceManager.Instance != null)
        {
            DuctAmbienceManager.Instance.DetenerSonidoDucto();
        }

        if (audioMonstruo != null)
        {
            audioMonstruo.Play();
        }

        // Iniciar el parpadeo de la luz del techo
        if (luzTecho != null)
        {
            if (rutinaParpadeo != null) StopCoroutine(rutinaParpadeo);
            rutinaParpadeo = StartCoroutine(EfectoParpadeo());
        }

        Debug.Log("¡EL MONSTRUO HA APARECIDO!");
    }

    public void Retirarse()
    {
        if (monstruo == null || !monstruoActivo)
            return;

        monstruo.SetActive(false);
        monstruoActivo = false;

        if (audioMonstruo != null)
        {
            audioMonstruo.Stop();
        }
        // Reanudar el ambiente aleatorio del ducto al irse el monstruo
        if (DuctAmbienceManager.Instance != null)
        {
            DuctAmbienceManager.Instance.IniciarCicloAleatorio();
        }
        // Detener el parpadeo y restaurar la luz a su estado normal
        RestaurarLuzNormal();

        Debug.Log("El monstruo se ha retirado.");
    }

    public void Ahuyentar()
    {
        if (!monstruoActivo)
            return;

        Retirarse();

        Debug.Log("¡PALABRA CLAVE CORRECTA! Monstruo ahuyentado.");
    }

    public void DesactivarMonstruoCompletamente()
    {
        // Detener las corrutinas que controlan el ciclo y las luces
        if (rutinaCiclo != null)
        {
            StopCoroutine(rutinaCiclo);
            rutinaCiclo = null;
        }

        if (rutinaParpadeo != null)
        {
            StopCoroutine(rutinaParpadeo);
            rutinaParpadeo = null;
        }

        // Ocultar y silenciar
        if (monstruo != null)
        {
            monstruo.SetActive(false);
        }

        if (audioMonstruo != null)
        {
            audioMonstruo.Stop();
        }

        monstruoActivo = false;

        // Restaurar estado original de la iluminación
        RestaurarLuzNormal();

        Debug.Log("MonsterController: Monstruo desactivado por completo.");
    }

    private void JugadorPierde()
    {
        Debug.Log("¡GAME OVER! El monstruo te atrapó.");

        Retirarse();
        DesactivarMonstruoCompletamente();
        // Pausa el reloj si está referenciado
        if (timerManager != null)
        {
            timerManager.PausarTemporizador();
        }
        // Cambiar mensaje en la pizarra
        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.MostrarMensajePorMonstruo();
        }
        // Llama directamente a la utilidad de teletransporte VR
        if (playerRig != null && gameOverSpawnPoint != null)
        {
            VRTeleport.TeleportTo(playerRig, gameOverSpawnPoint);
        }
        else
        {
            Debug.LogWarning("MonsterController: Falta asignar 'playerRig' o 'gameOverSpawnPoint' en el Inspector.");
        }
    }
    public bool EstaActivo()
    {
        return monstruoActivo;
    }

    // Corrutina que alterna la visibilidad/intensidad de la luz aleatoriamente
    IEnumerator EfectoParpadeo()
    {
        while (monstruoActivo)
        {
            // Alterna entre encendido/apagado o variación rápida
            luzTecho.enabled = !luzTecho.enabled;

            // Espera un tiempo aleatorio corto para simular fallo eléctrico
            float tiempoEspera = Random.Range(velocidadParpadeoMin, velocidadParpadeoMax);
            yield return new WaitForSeconds(tiempoEspera);
        }

        RestaurarLuzNormal();
    }

    private void RestaurarLuzNormal()
    {
        if (rutinaParpadeo != null)
        {
            StopCoroutine(rutinaParpadeo);
            rutinaParpadeo = null;
        }

        if (luzTecho != null)
        {
            luzTecho.enabled = true;
            luzTecho.intensity = intensidadOriginalLuz;
        }
    }
}