//using UnityEngine;

//public class CandleController : MonoBehaviour
//{
//    public GameObject llamaLuz;
//    public bool encendida = false;

//    void Start()
//    {
//        // Al iniciar el juego, nos aseguramos de que la vela empiece apagada
//        encendida = false;

//        if (llamaLuz != null)
//        {
//            llamaLuz.SetActive(false);
//        }
//    }

//    public void Encender()
//    {
//        if (encendida)
//            return;

//        encendida = true;

//        if (llamaLuz != null)
//        {
//            llamaLuz.SetActive(true);
//        }

//        Debug.Log("¡VELA ENCENDIDA!");
//    }

//    public void Apagar()
//    {
//        if (!encendida)
//            return;

//        encendida = false;

//        if (llamaLuz != null)
//        {
//            llamaLuz.SetActive(false);
//        }

//        Debug.Log("VELA APAGADA");
//    }
//}

using UnityEngine;

public class CandleController : MonoBehaviour
{
    public GameObject llamaLuz;
    public bool encendida = false;

    void Start()
    {
        encendida = false;
        ApagarEfectos();
    }

    public void Encender()
    {
        if (encendida)
            return;

        encendida = true;

        if (llamaLuz != null)
        {
            // 1. Activa el GameObject contenedor
            llamaLuz.SetActive(true);

            // 2. Enciende y reproduce todas las partículas que contenga
            ParticleSystem[] particulas = llamaLuz.GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem ps in particulas)
            {
                ps.Play(true);
            }

            // 3. Enciende todas las luces (Point Light) dentro del objeto
            Light[] luces = llamaLuz.GetComponentsInChildren<Light>(true);
            foreach (Light l in luces)
            {
                l.enabled = true;
            }
        }

        Debug.Log("¡VELA ENCENDIDA!");
    }

    public void Apagar()
    {
        if (!encendida)
            return;

        encendida = false;
        ApagarEfectos();

        Debug.Log("VELA APAGADA");
    }

    private void ApagarEfectos()
    {
        if (llamaLuz != null)
        {
            // Detiene y limpia las partículas inmediatamente
            ParticleSystem[] particulas = llamaLuz.GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem ps in particulas)
            {
                ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }

            // Apaga las luces
            Light[] luces = llamaLuz.GetComponentsInChildren<Light>(true);
            foreach (Light l in luces)
            {
                l.enabled = false;
            }

            // Desactiva el objeto padre
            llamaLuz.SetActive(false);
        }
    }
}