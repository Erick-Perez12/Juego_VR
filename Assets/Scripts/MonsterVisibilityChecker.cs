//using UnityEngine;

//public class MonsterVisibilityChecker : MonoBehaviour
//{
//    public static MonsterVisibilityChecker Instance;

//    [Header("Referencias VR")]
//    [Tooltip("Arrastra la Main Camera (CenterEyeAnchor) del Player VR")]
//    public Transform camaraVR;

//    [Header("Configuración de Visión")]
//    [Tooltip("Distancia máxima a la que el jugador puede detectar/mirar al monstruo")]
//    public float distanciaMaxima = 15f;
//    [Tooltip("Ángulo máximo (en grados) desde el centro de la mirada para considerar que lo está viendo")]
//    public float anguloVisionMaximo = 30f;
//    [Tooltip("Capa (Layer) en la que se encuentra el monstruo para Raycast")]
//    public LayerMask capaMonstruo;

//    void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    void Start()
//    {
//        // Si no se asignó la cámara manualmente, buscar la Main Camera del proyecto
//        if (camaraVR == null && Camera.main != null)
//        {
//            camaraVR = Camera.main.transform;
//        }
//    }

//    /// <summary>
//    /// Devuelve 'true' si la cámara del jugador está apuntando directamente al objetivo
//    /// </summary>
//    public bool JugadorEstaMirandoA(Transform objetivo)
//    {
//        if (camaraVR == null || objetivo == null)
//            return false;

//        // 1. Comprobar dirección y ángulo entre la mirada de la cámara y el monstruo
//        Vector3 direccionHaciaMonstruo = (objetivo.position - camaraVR.position).normalized;
//        float angulo = Vector3.Angle(camaraVR.forward, direccionHaciaMonstruo);

//        if (angulo > anguloVisionMaximo)
//        {
//            return false; // El monstruo está fuera del cono de visión central
//        }

//        // 2. Comprobar mediante Raycast que no haya paredes u obstáculos bloqueando la vista
//        RaycastHit hit;
//        if (Physics.Raycast(camaraVR.position, direccionHaciaMonstruo, out hit, distanciaMaxima))
//        {
//            // Verifica si lo que impactó el Raycast es el monstruo o un hijo de este
//            if (hit.transform == objetivo || hit.transform.IsChildOf(objetivo))
//            {
//                return true;
//            }
//        }

//        return false;
//    }
//}

using UnityEngine;

public class MonsterVisibilityChecker : MonoBehaviour
{
    public static MonsterVisibilityChecker Instance;

    [Header("Referencias VR")]
    [Tooltip("Arrastra aquí el CenterEyeAnchor de tu Player VR")]
    public Transform camaraVR;

    [Header("Configuración de Visión")]
    [Tooltip("Distancia máxima a la que se considera visible")]
    public float distanciaMaxima = 12f;
    [Tooltip("Ángulo de tolerancia del cono de visión (45° es ideal para VR)")]
    public float anguloVisionMaximo = 45f;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Buscar automáticamente la Main Camera si no fue asignada
        if (camaraVR == null && Camera.main != null)
        {
            camaraVR = Camera.main.transform;
        }
    }

    /// <summary>
    /// Verifica si el jugador está mirando en dirección al monstruo.
    /// </summary>
    public bool JugadorEstaMirandoA(Transform objetivo)
    {
        if (camaraVR == null || objetivo == null)
        {
            Debug.LogWarning("MonsterVisibilityChecker: Falta asignar la cámara VR o el objetivo.");
            return false;
        }

        // 1. Determinar el punto central a evaluar (si tiene Collider, usa el centro del Collider)
        Vector3 puntoObjetivo = objetivo.position;
        Collider col = objetivo.GetComponentInChildren<Collider>();
        if (col != null)
        {
            puntoObjetivo = col.bounds.center; // Toma el centro físico del cuerpo/pecho del monstruo
        }

        // 2. Verificar distancia
        float distancia = Vector3.Distance(camaraVR.position, puntoObjetivo);
        if (distancia > distanciaMaxima)
        {
            Debug.Log("Visión: El monstruo está demasiado lejos (" + distancia + "m)");
            return false;
        }

        // 3. Verificar si el objetivo está dentro del campo de visión (cono frontal de la cabeza/visor)
        Vector3 direccionHaciaObjetivo = (puntoObjetivo - camaraVR.position).normalized;
        float angulo = Vector3.Angle(camaraVR.forward, direccionHaciaObjetivo);

        if (angulo <= anguloVisionMaximo)
        {
            // El jugador tiene al monstruo enfrente dentro de su campo visual
            return true;
        }

        Debug.Log("Visión: Ángulo fuera de rango (" + angulo + "° / Max: " + anguloVisionMaximo + "°)");
        return false;
    }
}