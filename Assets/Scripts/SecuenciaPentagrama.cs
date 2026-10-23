

//using System.Collections;
//using UnityEngine;

//public class SecuenciaPentagrama : MonoBehaviour
//{
//    [Header("Modelos del Gato")]
//    public GameObject gatoU;               // Gato estático
//    public GameObject gatoV;               // Gato que gira / baila
//    public Transform pentagramaTransform; // Quad del pentagrama
//    public AudioSource audioSource;        // AudioSource de la canción

//    [Header("Renderers y Materiales")]
//    public Renderer rendererGatoU;
//    public Renderer rendererGatoV;
//    public Material materialNeonRosa;      // Asigna aquí tu 'GatoNeonMat'

//    private Material matOriginalU;
//    private Material matOriginalV;

//    [Header("Luz Neón Opcional")]
//    public Light luzRosaGatoU; // Point Light hija de gatoU
//    public Light luzRosaGatoV; // Point Light hija de gatoV           // Opcional: Point Light hija del gato

//    [Header("Ajustes de Rotación (Gato V)")]
//    public float vueltasPorSegundo = 1.0f;
//    public Vector3 ejeRotacion = Vector3.up;

//    [Header("Ajustes de Temblor Inicial")]
//    public float duracionTemblor = 5.0f;
//    public float intensidadTemblor = 0.05f;

//    [Header("Ajustes de Elevación")]
//    public float alturaElevadaY = 0.8f;
//    public float duracionElevacion = 2.0f;

//    [Header("Ajustes de Movimiento Errático / Rápido (18.52s - 30.16s)")]
//    public Vector3 rangoMovimiento = new Vector3(0.8f, 0.4f, 0.8f);
//    public float intervaloCambioErratico = 0.03f;

//    [Header("Intervalos de Baile / Giro de la Canción")]
//    public float tiempoFinCancion = 44.0f;

//    [System.Serializable]
//    public struct IntervaloBaile
//    {
//        public float tiempoInicio;
//        public float tiempoFin;
//    }

//    public IntervaloBaile[] intervalosBaile = new IntervaloBaile[]
//    {
//        new IntervaloBaile { tiempoInicio = 0.28f, tiempoFin = 1.88f },
//        new IntervaloBaile { tiempoInicio = 3.12f, tiempoFin = 5.20f },
//        new IntervaloBaile { tiempoInicio = 6.08f, tiempoFin = 30.16f },
//        new IntervaloBaile { tiempoInicio = 31.96f, tiempoFin = 34.56f },
//        new IntervaloBaile { tiempoInicio = 34.80f, tiempoFin = 40.96f },
//        new IntervaloBaile { tiempoInicio = 41.20f, tiempoFin = 44.00f }
//    };

//    private bool seActivo = false;
//    private bool estaEnFaseDeBaile = false;

//    // Control de posiciones y rotaciones
//    private Vector3 posicionOriginalPentagrama;
//    private Vector3 posInicialGatos;
//    private Vector3 posElevadaGatos;
//    private Quaternion rotacionInicialGatoU;

//    // Control del movimiento errático
//    private float temporizadorErratico = 0f;
//    private Vector3 posOffsetErratico = Vector3.zero;

//    private bool efectoNeonActivo = false;

//    //void Start()
//    //{
//    //    if (pentagramaTransform != null)
//    //        posicionOriginalPentagrama = pentagramaTransform.localPosition;

//    //    if (gatoU != null)
//    //    {
//    //        rotacionInicialGatoU = gatoU.transform.rotation;
//    //        posInicialGatos = gatoU.transform.localPosition;
//    //        posElevadaGatos = new Vector3(posInicialGatos.x, alturaElevadaY, posInicialGatos.z);
//    //    }

//    //    GuardarMaterialesOriginales();
//    //    OcultarAmbosGatos();

//    //    if (luzRosaGato != null) luzRosaGato.enabled = false;

//    //    if (audioSource != null)
//    //    {
//    //        audioSource.playOnAwake = false;
//    //        audioSource.Stop();
//    //    }
//    //}
//    void Start()
//    {
//        if (pentagramaTransform != null)
//            posicionOriginalPentagrama = pentagramaTransform.localPosition;

//        if (gatoU != null)
//        {
//            rotacionInicialGatoU = gatoU.transform.rotation;
//            posInicialGatos = gatoU.transform.localPosition;
//            posElevadaGatos = new Vector3(posInicialGatos.x, alturaElevadaY, posInicialGatos.z);
//        }

//        GuardarMaterialesOriginales();
//        OcultarAmbosGatos();

//        // Apagamos ambas luces al arrancar la escena
//        if (luzRosaGatoU != null) luzRosaGatoU.enabled = false;
//        if (luzRosaGatoV != null) luzRosaGatoV.enabled = false;

//        if (audioSource != null)
//        {
//            audioSource.playOnAwake = false;
//            audioSource.Stop();
//        }
//    }
//    private void GuardarMaterialesOriginales()
//    {
//        if (rendererGatoU != null) matOriginalU = rendererGatoU.material;
//        if (rendererGatoV != null) matOriginalV = rendererGatoV.material;
//    }

//    private void OnTriggerEnter(Collider other)
//    {
//        if (seActivo) return;

//        seActivo = true;
//        StartCoroutine(RutinaInvocacion());
//    }

//    IEnumerator RutinaInvocacion()
//    {
//        // 1. Temblor del pentagrama (5 segundos)
//        float tiempoTranscurrido = 0f;
//        while (tiempoTranscurrido < duracionTemblor)
//        {
//            if (pentagramaTransform != null)
//            {
//                Vector3 desplazamiento = Random.insideUnitSphere * intensidadTemblor;
//                desplazamiento.y = 0;
//                pentagramaTransform.localPosition = posicionOriginalPentagrama + desplazamiento;
//            }
//            tiempoTranscurrido += Time.deltaTime;
//            yield return null;
//        }

//        if (pentagramaTransform != null)
//            pentagramaTransform.localPosition = posicionOriginalPentagrama;

//        // 2. Aparece GatoU en el suelo durante 5 segundos
//        MostrarGatoU();
//        yield return new WaitForSeconds(5.0f);

//        // 3. Iniciar música y fase de baile
//        if (audioSource != null)
//            audioSource.Play();

//        estaEnFaseDeBaile = true;
//    }

//    void Update()
//    {
//        if (!estaEnFaseDeBaile || audioSource == null) return;

//        float tiempoAudio = audioSource.time;

//        // Al finalizar la canción
//        if (tiempoAudio >= tiempoFinCancion || !audioSource.isPlaying)
//        {
//            audioSource.Stop();
//            RestaurarMateriales();
//            OcultarAmbosGatos();
//            estaEnFaseDeBaile = false;
//            return;
//        }

//        // --- 1. ELEVACIÓN Y MOVIMIENTO ERRÁTICO (6.08s en adelante) ---
//        if (tiempoAudio >= 6.08f)
//        {
//            float tElevacion = Mathf.Clamp01((tiempoAudio - 6.08f) / duracionElevacion);
//            Vector3 posBaseActual = Vector3.Lerp(posInicialGatos, posElevadaGatos, tElevacion);

//            if (tiempoAudio >= 18.52f && tiempoAudio <= 30.16f)
//            {
//                temporizadorErratico += Time.deltaTime;
//                if (temporizadorErratico >= intervaloCambioErratico)
//                {
//                    temporizadorErratico = 0f;
//                    posOffsetErratico = new Vector3(
//                        Random.Range(-rangoMovimiento.x, rangoMovimiento.x),
//                        Random.Range(-rangoMovimiento.y, rangoMovimiento.y),
//                        Random.Range(-rangoMovimiento.z, rangoMovimiento.z)
//                    );
//                }

//                Vector3 posFinalErratica = posElevadaGatos + posOffsetErratico;
//                if (gatoU != null) gatoU.transform.localPosition = posFinalErratica;
//                if (gatoV != null) gatoV.transform.localPosition = posFinalErratica;
//            }
//            else
//            {
//                if (gatoU != null) gatoU.transform.localPosition = posBaseActual;
//                if (gatoV != null) gatoV.transform.localPosition = posBaseActual;
//                posOffsetErratico = Vector3.zero;
//            }
//        }

//        // --- 2. ACTIVACIÓN DEL MATERIAL NEÓN (18.52s - 31.94s) ---
//        if (tiempoAudio >= 18.52f && tiempoAudio <= 31.94f)
//        {
//            if (!efectoNeonActivo)
//            {
//                AplicarMaterialNeon();
//                efectoNeonActivo = true;
//            }
//        }
//        else
//        {
//            if (efectoNeonActivo)
//            {
//                RestaurarMateriales();
//                efectoNeonActivo = false;
//            }
//        }

//        // --- 3. ROTACIÓN Y VISIBILIDAD ---
//        bool debeGirar = false;
//        foreach (var intervalo in intervalosBaile)
//        {
//            if (tiempoAudio >= intervalo.tiempoInicio && tiempoAudio <= intervalo.tiempoFin)
//            {
//                debeGirar = true;
//                break;
//            }
//        }

//        if (debeGirar)
//        {
//            MostrarGatoV();

//            float velocidadGrados = vueltasPorSegundo * 360.0f;
//            if (gatoV != null)
//            {
//                gatoV.transform.Rotate(ejeRotacion, velocidadGrados * Time.deltaTime, Space.Self);
//            }
//        }
//        else
//        {
//            MostrarGatoU();
//        }
//    }

//    private void AplicarMaterialNeon()
//    {
//        if (materialNeonRosa == null) return;

//        if (rendererGatoU != null) rendererGatoU.material = materialNeonRosa;
//        if (rendererGatoV != null) rendererGatoV.material = materialNeonRosa;
//        // Encender la luz correspondiente según cuál esté activo
//        if (luzRosaGatoU != null) luzRosaGatoU.enabled = (gatoU != null && gatoU.activeSelf);
//        if (luzRosaGatoV != null) luzRosaGatoV.enabled = (gatoV != null && gatoV.activeSelf);
//    }

//    private void RestaurarMateriales()
//    {
//        if (rendererGatoU != null && matOriginalU != null) rendererGatoU.material = matOriginalU;
//        if (rendererGatoV != null && matOriginalV != null) rendererGatoV.material = matOriginalV;
//        if (luzRosaGatoU != null) luzRosaGatoU.enabled = false;
//        if (luzRosaGatoV != null) luzRosaGatoV.enabled = false;
//    }

//    private void MostrarGatoU()
//    {
//        if (gatoU != null)
//        {
//            gatoU.transform.rotation = rotacionInicialGatoU;
//            gatoU.SetActive(true);
//        }
//        if (gatoV != null) gatoV.SetActive(false);
//        if (efectoNeonActivo)
//        {
//            if (luzRosaGatoU != null) luzRosaGatoU.enabled = true;
//            if (luzRosaGatoV != null) luzRosaGatoV.enabled = false;
//        }
//    }

//    private void MostrarGatoV()
//    {
//        if (gatoU != null) gatoU.SetActive(false);
//        if (gatoV != null) gatoV.SetActive(true);
//        if (efectoNeonActivo)
//        {
//            if (luzRosaGatoU != null) luzRosaGatoU.enabled = false;
//            if (luzRosaGatoV != null) luzRosaGatoV.enabled = true;
//        }
//    }

//    private void OcultarAmbosGatos()
//    {
//        if (gatoU != null) gatoU.SetActive(false);
//        if (gatoV != null) gatoV.SetActive(false);
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecuenciaPentagrama : MonoBehaviour
{
    [Header("Modelos del Gato")]
    public GameObject gatoU;               // Gato estático
    public GameObject gatoV;               // Gato que gira / baila
    public Transform pentagramaTransform; // Quad del pentagrama
    public AudioSource audioSource;        // AudioSource de la canción

    [Header("Audio de Aparecer Gato")]
    public AudioSource audioSourceSFX; // AudioSource dedicado para efectos de sonido
    public AudioClip sonidoAparecerGato; // Clip de audio (ej. maullido, aparición mágica, etc.)

    [Header("Renderers y Materiales")]
    public Renderer rendererGatoU;
    public Renderer rendererGatoV;
    public Material materialNeonRosa;      // Asigna aquí tu 'GatoNeonMat'

    private Material matOriginalU;
    private Material matOriginalV;

    [Header("Luz Neón Opcional")]
    public Light luzRosaGatoU; // Point Light hija de gatoU
    public Light luzRosaGatoV; // Point Light hija de gatoV

    [Header("Pentagrama, Luz y Golpes de Música")]
    public Renderer rendererPentagrama;     // Mesh/Quad Renderer del pentagrama
    public Light luzPentagrama;             // Point Light del pentagrama
    public float intensidadBaseLuz = 2.0f;  // Intensidad normal
    public float intensidadFlashLuz = 20.0f;// Intensidad del destello
    public float velocidadSuavizado = 8.0f; // Velocidad para regresar a la normalidad
    public float factorEscalaGolpe = 1.25f; // Cuánto se agranda el pentagrama (25% más grande)

    public Color[] coloresImpacto = new Color[]
    {
        Color.red,                          // 12.52s
        Color.cyan,                         // 14.08s
        Color.green,                        // 15.68s
        new Color(1f, 0f, 1f)               // 17.36s (Magenta)
    };

    private int indiceColorActual = -1;
    private Color colorOriginalPentagrama;
    private Vector3 escalaOriginalPentagrama;

    [Header("Ajustes de Rotación (Gato V)")]
    public float vueltasPorSegundo = 1.0f;
    public Vector3 ejeRotacion = Vector3.up;

    [Header("Ajustes de Temblor Inicial")]
    public float duracionTemblor = 5.0f;
    public float intensidadTemblor = 0.05f;

    [Header("Ajustes de Elevación")]
    public float alturaElevadaY = 0.8f;
    public float duracionElevacion = 2.0f;

    [Header("Ajustes de Movimiento Errático / Rápido (18.52s - 30.16s)")]
    public Vector3 rangoMovimiento = new Vector3(0.8f, 0.4f, 0.8f);
    public float intervaloCambioErratico = 0.03f;

    [Header("Ajustes de Multiplicación de Clones (30.16s - 31.96s)")]
    public int totalClones = 8;
    public float radioClones = 1.2f; // Radio en círculo alrededor del gato principal
    private List<GameObject> listaClones = new List<GameObject>();
    private bool seMultiplico = false;
    private Coroutine corrutinaClones;

    [Header("Intervalos de Baile / Giro de la Canción")]
    public float tiempoFinCancion = 44.0f;

    [System.Serializable]
    public struct IntervaloBaile
    {
        public float tiempoInicio;
        public float tiempoFin;
    }

    public IntervaloBaile[] intervalosBaile = new IntervaloBaile[]
    {
        new IntervaloBaile { tiempoInicio = 0.28f, tiempoFin = 1.88f },
        new IntervaloBaile { tiempoInicio = 3.12f, tiempoFin = 5.20f },
        new IntervaloBaile { tiempoInicio = 6.08f, tiempoFin = 30.16f },
        new IntervaloBaile { tiempoInicio = 31.96f, tiempoFin = 34.56f },
        new IntervaloBaile { tiempoInicio = 34.80f, tiempoFin = 40.96f },
        new IntervaloBaile { tiempoInicio = 41.20f, tiempoFin = 44.00f }
    };

    private bool seActivo = false;
    private bool estaEnFaseDeBaile = false;

    // Control de posiciones y rotaciones
    private Vector3 posicionOriginalPentagrama;
    private Vector3 posInicialGatos;
    private Vector3 posElevadaGatos;
    private Quaternion rotacionInicialGatoU;

    // Control del movimiento errático
    private float temporizadorErratico = 0f;
    private Vector3 posOffsetErratico = Vector3.zero;

    private bool efectoNeonActivo = false;

    void Start()
    {
        if (pentagramaTransform != null)
        {
            posicionOriginalPentagrama = pentagramaTransform.localPosition;
            escalaOriginalPentagrama = pentagramaTransform.localScale;
        }
        if (gatoU != null)
        {
            rotacionInicialGatoU = gatoU.transform.rotation;
            posInicialGatos = gatoU.transform.localPosition;
            posElevadaGatos = new Vector3(posInicialGatos.x, alturaElevadaY, posInicialGatos.z);
        }
        if (rendererPentagrama != null && rendererPentagrama.material.HasProperty("_BaseColor"))
        {
            colorOriginalPentagrama = rendererPentagrama.material.GetColor("_BaseColor");
        }
        GuardarMaterialesOriginales();
        OcultarAmbosGatos();

        // Apagamos ambas luces al arrancar la escena
        if (luzRosaGatoU != null) luzRosaGatoU.enabled = false;
        if (luzRosaGatoV != null) luzRosaGatoV.enabled = false;

        if (audioSource != null)
        {
            audioSource.playOnAwake = false;
            audioSource.Stop();
        }
    }

    private void GuardarMaterialesOriginales()
    {
        if (rendererGatoU != null) matOriginalU = rendererGatoU.material;
        if (rendererGatoV != null) matOriginalV = rendererGatoV.material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (seActivo) return;

        seActivo = true;
        StartCoroutine(RutinaInvocacion());
    }

    IEnumerator RutinaInvocacion()
    {
        // 1. Temblor del pentagrama (5 segundos)
        float tiempoTranscurrido = 0f;
        while (tiempoTranscurrido < duracionTemblor)
        {
            if (pentagramaTransform != null)
            {
                Vector3 desplazamiento = Random.insideUnitSphere * intensidadTemblor;
                desplazamiento.y = 0;
                pentagramaTransform.localPosition = posicionOriginalPentagrama + desplazamiento;
            }
            tiempoTranscurrido += Time.deltaTime;
            yield return null;
        }

        if (pentagramaTransform != null)
            pentagramaTransform.localPosition = posicionOriginalPentagrama;

        // 2. Aparece GatoU en el suelo durante 5 segundos
        MostrarGatoU();
        if (audioSourceSFX != null && sonidoAparecerGato != null)
        {
            audioSourceSFX.PlayOneShot(sonidoAparecerGato);
        }
        yield return new WaitForSeconds(5.0f);

        // 3. Iniciar música y fase de baile
        if (audioSource != null)
            audioSource.Play();

        estaEnFaseDeBaile = true;
    }

    void Update()
    {
        if (!estaEnFaseDeBaile || audioSource == null) return;

        float tiempoAudio = audioSource.time;

        // Al finalizar la canción
        if (tiempoAudio >= tiempoFinCancion || !audioSource.isPlaying)
        {
            audioSource.Stop();
            RestaurarMateriales();
            RestaurarColorPentagrama();
            DestruirClones();
            OcultarAmbosGatos();
            estaEnFaseDeBaile = false;
            return;
        }
        if (tiempoAudio >= 12.52f && tiempoAudio < 18.52f)
        {
            int nuevoIndice = -1;

            if (tiempoAudio >= 17.36f) nuevoIndice = 3;
            else if (tiempoAudio >= 15.68f) nuevoIndice = 2;
            else if (tiempoAudio >= 14.08f) nuevoIndice = 1;
            else if (tiempoAudio >= 12.52f) nuevoIndice = 0;

            if (nuevoIndice != -1 && nuevoIndice != indiceColorActual)
            {
                indiceColorActual = nuevoIndice;
                DispararEfectoImpacto(coloresImpacto[indiceColorActual]);
            }
        }
        else if (tiempoAudio >= 18.52f && indiceColorActual != -1)
        {
            RestaurarColorPentagrama();
            indiceColorActual = -1;
        }

        // --- SUAVIZADO DE LUZ Y ESCALA (Regreso progresivo a la normalidad) ---
        ActualizarSuavizadosPentagrama();
        // --- 1. ELEVACIÓN Y MOVIMIENTO ERRÁTICO (6.08s en adelante) ---
        if (tiempoAudio >= 6.08f)
        {
            float tElevacion = Mathf.Clamp01((tiempoAudio - 6.08f) / duracionElevacion);
            Vector3 posBaseActual = Vector3.Lerp(posInicialGatos, posElevadaGatos, tElevacion);

            if (tiempoAudio >= 18.52f && tiempoAudio <= 30.16f)
            {
                temporizadorErratico += Time.deltaTime;
                if (temporizadorErratico >= intervaloCambioErratico)
                {
                    temporizadorErratico = 0f;
                    posOffsetErratico = new Vector3(
                        Random.Range(-rangoMovimiento.x, rangoMovimiento.x),
                        Random.Range(-rangoMovimiento.y, rangoMovimiento.y),
                        Random.Range(-rangoMovimiento.z, rangoMovimiento.z)
                    );
                }

                Vector3 posFinalErratica = posElevadaGatos + posOffsetErratico;
                if (gatoU != null) gatoU.transform.localPosition = posFinalErratica;
                if (gatoV != null) gatoV.transform.localPosition = posFinalErratica;
            }
            else
            {
                if (gatoU != null) gatoU.transform.localPosition = posBaseActual;
                if (gatoV != null) gatoV.transform.localPosition = posBaseActual;
                posOffsetErratico = Vector3.zero;
            }
        }

        // --- 2. ACTIVACIÓN DEL MATERIAL NEÓN (18.52s - 31.94s) ---
        if (tiempoAudio >= 18.52f && tiempoAudio <= 31.94f)
        {
            if (!efectoNeonActivo)
            {
                AplicarMaterialNeon();
                efectoNeonActivo = true;
            }
        }
        else
        {
            if (efectoNeonActivo)
            {
                RestaurarMateriales();
                efectoNeonActivo = false;
            }
        }

        // --- 3. MULTIPLICACIÓN DE CLONES (30.16s - 31.96s) ---
        if (tiempoAudio >= 30.16f && tiempoAudio < 31.96f)
        {
            if (!seMultiplico)
            {
                seMultiplico = true;
                corrutinaClones = StartCoroutine(GenerarClonesProgresivamente(31.96f - 30.16f));
            }
        }
        else
        {
            if (seMultiplico)
            {
                seMultiplico = false;
                if (corrutinaClones != null) StopCoroutine(corrutinaClones);
                DestruirClones();
            }
        }

        // --- 4. ROTACIÓN Y VISIBILIDAD ---
        bool debeGirar = false;
        foreach (var intervalo in intervalosBaile)
        {
            if (tiempoAudio >= intervalo.tiempoInicio && tiempoAudio <= intervalo.tiempoFin)
            {
                debeGirar = true;
                break;
            }
        }

        if (debeGirar)
        {
            MostrarGatoV();

            float velocidadGrados = vueltasPorSegundo * 360.0f;
            if (gatoV != null)
            {
                gatoV.transform.Rotate(ejeRotacion, velocidadGrados * Time.deltaTime, Space.Self);
            }
        }
        else
        {
            MostrarGatoU();
        }
    }
    private void DispararEfectoImpacto(Color nuevoColor)
    {
        // 1. Cambiar color del material y emisión
        if (rendererPentagrama != null)
        {
            if (rendererPentagrama.material.HasProperty("_BaseColor"))
                rendererPentagrama.material.SetColor("_BaseColor", nuevoColor);
            else
                rendererPentagrama.material.color = nuevoColor;

            if (rendererPentagrama.material.HasProperty("_EmissionColor"))
                rendererPentagrama.material.SetColor("_EmissionColor", nuevoColor * 2.5f);
        }

        // 2. Destello de Luz (Flash Instantáneo)
        if (luzPentagrama != null)
        {
            luzPentagrama.color = nuevoColor;
            luzPentagrama.intensity = intensidadFlashLuz; // Sube de golpe a 20
            luzPentagrama.enabled = true;
        }

        // 3. Golpe de Escala (Pulsación Instantánea)
        if (pentagramaTransform != null)
        {
            pentagramaTransform.localScale = escalaOriginalPentagrama * factorEscalaGolpe;
        }
    }

    private void ActualizarSuavizadosPentagrama()
    {
        // Disminuir suavemente la intensidad de la luz de regreso al valor base
        if (luzPentagrama != null && luzPentagrama.enabled)
        {
            luzPentagrama.intensity = Mathf.Lerp(luzPentagrama.intensity, intensidadBaseLuz, Time.deltaTime * velocidadSuavizado);
        }

        // Encoger suavemente la escala del pentagrama de regreso al tamaño normal
        if (pentagramaTransform != null)
        {
            pentagramaTransform.localScale = Vector3.Lerp(pentagramaTransform.localScale, escalaOriginalPentagrama, Time.deltaTime * velocidadSuavizado);
        }
    }

    private void RestaurarColorPentagrama()
    {
        if (rendererPentagrama != null)
        {
            if (rendererPentagrama.material.HasProperty("_BaseColor"))
                rendererPentagrama.material.SetColor("_BaseColor", colorOriginalPentagrama);
            else
                rendererPentagrama.material.color = colorOriginalPentagrama;
        }

        if (pentagramaTransform != null)
            pentagramaTransform.localScale = escalaOriginalPentagrama;

        if (luzPentagrama != null)
            luzPentagrama.intensity = intensidadBaseLuz;
    }
    //private IEnumerator GenerarClonesProgresivamente(float duracionVentana)
    //{
    //    float intervaloAparicion = duracionVentana / totalClones;

    //    for (int i = 0; i < totalClones; i++)
    //    {
    //        float angulo = i * (360f / totalClones) * Mathf.Deg2Rad;
    //        Vector3 offsetCirculo = new Vector3(Mathf.Cos(angulo) * radioClones, 0, Mathf.Sin(angulo) * radioClones);
    //        Vector3 posClon = gatoU.transform.position + offsetCirculo;

    //        GameObject clon = Instantiate(gatoU, posClon, gatoU.transform.rotation, transform);

    //        // Si el prefab original incluye luces hijas, se apagan en el clon
    //        Light[] lucesClon = clon.GetComponentsInChildren<Light>();
    //        foreach (Light l in lucesClon) l.enabled = false;

    //        // Restaurar material original al clon en caso de haber heredado el material Neón
    //        Renderer rendClon = clon.GetComponentInChildren<Renderer>();
    //        if (rendClon != null && matOriginalU != null)
    //        {
    //            rendClon.material = matOriginalU;
    //        }

    //        listaClones.Add(clon);
    //        yield return new WaitForSeconds(intervaloAparicion);
    //    }
    //}
    private IEnumerator GenerarClonesProgresivamente(float duracionVentana)
    {
        float intervaloAparicion = duracionVentana / totalClones;

        for (int i = 0; i < totalClones; i++)
        {
            // Genera un desplazamiento aleatorio en 3D (arriba, abajo, lados, profundidad)
            Vector3 offset3D = Random.insideUnitSphere * radioClones;

            // Si quieres evitar que aparezcan demasiado abajo del piso/pentagrama, 
            // puedes ajustar la altura mínima:
            // offset3D.y = Mathf.Max(offset3D.y, -0.2f);

            Vector3 posClon = gatoU.transform.position + offset3D;

            GameObject clon = Instantiate(gatoU, posClon, gatoU.transform.rotation, transform);

            // Desactivar luces clonadas para no saturar la escena
            Light[] lucesClon = clon.GetComponentsInChildren<Light>();
            foreach (Light l in lucesClon) l.enabled = false;

            // Asegurar que el clon tenga el material original (sin brillo neón)
            Renderer rendClon = clon.GetComponentInChildren<Renderer>();
            if (rendClon != null && matOriginalU != null)
            {
                rendClon.material = matOriginalU;
            }

            listaClones.Add(clon);
            yield return new WaitForSeconds(intervaloAparicion);
        }
    }
    private void DestruirClones()
    {
        foreach (GameObject clon in listaClones)
        {
            if (clon != null) Destroy(clon);
        }
        listaClones.Clear();
    }

    private void AplicarMaterialNeon()
    {
        if (materialNeonRosa == null) return;

        if (rendererGatoU != null) rendererGatoU.material = materialNeonRosa;
        if (rendererGatoV != null) rendererGatoV.material = materialNeonRosa;

        if (luzRosaGatoU != null) luzRosaGatoU.enabled = (gatoU != null && gatoU.activeSelf);
        if (luzRosaGatoV != null) luzRosaGatoV.enabled = (gatoV != null && gatoV.activeSelf);
    }

    private void RestaurarMateriales()
    {
        if (rendererGatoU != null && matOriginalU != null) rendererGatoU.material = matOriginalU;
        if (rendererGatoV != null && matOriginalV != null) rendererGatoV.material = matOriginalV;

        if (luzRosaGatoU != null) luzRosaGatoU.enabled = false;
        if (luzRosaGatoV != null) luzRosaGatoV.enabled = false;
    }

    private void MostrarGatoU()
    {
        if (gatoU != null)
        {
            gatoU.transform.rotation = rotacionInicialGatoU;
            gatoU.SetActive(true);
        }
        if (gatoV != null) gatoV.SetActive(false);

        if (efectoNeonActivo)
        {
            if (luzRosaGatoU != null) luzRosaGatoU.enabled = true;
            if (luzRosaGatoV != null) luzRosaGatoV.enabled = false;
        }
    }

    private void MostrarGatoV()
    {
        if (gatoU != null) gatoU.SetActive(false);
        if (gatoV != null) gatoV.SetActive(true);

        if (efectoNeonActivo)
        {
            if (luzRosaGatoU != null) luzRosaGatoU.enabled = false;
            if (luzRosaGatoV != null) luzRosaGatoV.enabled = true;
        }
    }

    private void OcultarAmbosGatos()
    {
        if (gatoU != null) gatoU.SetActive(false);
        if (gatoV != null) gatoV.SetActive(false);
    }
}