
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
    private bool pausadoPorApagon = false;
    private bool yaAtrapo = false;

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
        if (monstruo == null || monstruoActivo || pausadoPorApagon)
            return;

        if (puntoAparicion != null)
        {
            monstruo.transform.position = puntoAparicion.position;
            monstruo.transform.rotation = puntoAparicion.rotation;
        }

        monstruo.SetActive(true);
        monstruoActivo = true;

        MonsterAnimationManager animManager = monstruo.GetComponent<MonsterAnimationManager>();
        if (animManager != null)
        {
            animManager.AparecerYRugir();
        }
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

    public void PausarPorApagon()
    {
        pausadoPorApagon = true;

        // Detener la corrutina de parpadeo de luz para que no la vuelva a encender
        if (rutinaParpadeo != null)
        {
            StopCoroutine(rutinaParpadeo);
            rutinaParpadeo = null;
        }

        // Apagar la luz del techo explícitamente
        if (luzTecho != null)
        {
            luzTecho.enabled = false;
        }

        // Ocultar al monstruo si estaba visible
        if (monstruo != null)
        {
            monstruo.SetActive(false);
        }

        if (audioMonstruo != null && audioMonstruo.isPlaying)
        {
            audioMonstruo.Pause();
        }

        Debug.Log("MonsterController: Pausado temporalmente por Apagón del Keypad.");
    }

    public void ReanudarTrasApagon()
    {
        pausadoPorApagon = false;

        // Si el monstruo sigue estando en su ciclo activo, lo volvemos a mostrar y restauramos el parpadeo
        if (monstruoActivo)
        {
            if (monstruo != null)
            {
                monstruo.SetActive(true);
            }

            if (audioMonstruo != null)
            {
                audioMonstruo.UnPause();
            }

            if (luzTecho != null)
            {
                rutinaParpadeo = StartCoroutine(EfectoParpadeo());
            }
        }
        else
        {
            // Si no había monstruo activo, solo devolvemos la luz a su estado normal
            RestaurarLuzNormal();
        }

        Debug.Log("MonsterController: Reanudado tras el Apagón.");
    }
    //public void Ahuyentar()
    //{
    //    if (!monstruoActivo)
    //        return;

    //    //  NUEVA VALIDACIÓN: Verificar si el jugador está mirando al monstruo
    //    if (MonsterVisibilityChecker.Instance != null && monstruo != null)
    //    {
    //        bool estaMirando = MonsterVisibilityChecker.Instance.JugadorEstaMirandoA(monstruo.transform);

    //        if (!estaMirando)
    //        {
    //            Debug.Log("Dijo la palabra clave, ¡pero NO está mirando al monstruo! El monstruo permanece.");
    //            return; // Se ignora la orden porque no lo está mirando
    //        }
    //    }

    //    // Si lo está mirando y dijo la palabra clave, se retira
    //    Retirarse();

    //    Debug.Log("¡PALABRA CLAVE CORRECTA Y MONSTRUO EN VISTA! Monstruo ahuyentado.");
    //}

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

    //private void JugadorPierde()
    //{
    //    Debug.Log("¡GAME OVER! El monstruo te atrapó.");

    //    Retirarse();
    //    DesactivarMonstruoCompletamente();
    //    // Pausa el reloj si está referenciado
    //    if (timerManager != null)
    //    {
    //        timerManager.PausarTemporizador();
    //    }
    //    // Detener el ambiente del ducto al perder 
    //    //if (DuctAmbienceManager.Instance != null)
    //    //{
    //    //    DuctAmbienceManager.Instance.DetenerSonidoDucto();
    //    //}
    //    // Cambiar mensaje en la pizarra
    //    if (GameOverManager.Instance != null)
    //    {
    //        GameOverManager.Instance.MostrarMensajePorMonstruo();
    //    }
    //    // Llama directamente a la utilidad de teletransporte VR
    //    if (playerRig != null && gameOverSpawnPoint != null)
    //    {
    //        VRTeleport.TeleportTo(playerRig, gameOverSpawnPoint);
    //    }
    //    else
    //    {
    //        Debug.LogWarning("MonsterController: Falta asignar 'playerRig' o 'gameOverSpawnPoint' en el Inspector.");
    //    }
    //}
    private void JugadorPierde()
    {
        if (yaAtrapo) return; // Evita ejecuciones dobles
        yaAtrapo = true;
        Debug.Log("¡GAME OVER! El monstruo te atrapó.");

        UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }
        // Pausa el temporizador si aplica
        if (timerManager != null)
        {
            timerManager.PausarTemporizador();
        }

        Transform camaraVR = Camera.main != null ? Camera.main.transform : null;

        if (camaraVR != null)
        {
            // Coloca al monstruo a 0.8 metros al frente de los ojos del jugador
            Vector3 posicionCara = camaraVR.position + (camaraVR.forward * 0.8f);

            // Ajustar la altura (opcional, por si el modelo tiene el pivote en los pies)
            posicionCara.y = camaraVR.position.y - 0.5f;

            transform.position = posicionCara;

            // Hace que el monstruo mire directamente hacia la cámara
            Vector3 direccionMirar = camaraVR.position - transform.position;
            direccionMirar.y = 0; // Para que no se incline de forma extraña si la cámara está alta/baja
            if (direccionMirar != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(-direccionMirar);
            }
        }

        // Ejecutar la animación y sonido del Screamer antes de teletransportar
        MonsterAnimationManager animManager = monstruo.GetComponent<MonsterAnimationManager>();
        if (animManager != null)
        {
            // Pasa el punto en la cámara frente al jugador si lo tienes o la posición del jugador
            Transform puntoCamara = playerRig != null ? playerRig : transform;
            animManager.EjecutarScreamer(puntoCamara);
        }

        // Iniciar corrutina para esperar a que termine el screamer antes de teletransportar
        StartCoroutine(RutinaGameOverScreamer());
    }

    private IEnumerator RutinaGameOverScreamer()
    {
        // Tiempo de duración del screamer antes del teletransporte (ej. 1.5 segundos)
        yield return new WaitForSeconds(4f);

        DesactivarMonstruoCompletamente();

        if (GameOverManager.Instance != null)
        {
            GameOverManager.Instance.MostrarMensajePorMonstruo();
        }

        if (playerRig != null && gameOverSpawnPoint != null)
        {
            VRTeleport.TeleportTo(playerRig, gameOverSpawnPoint);
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