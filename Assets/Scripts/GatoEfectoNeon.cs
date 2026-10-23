using UnityEngine;

public class GatoEfectoNeon : MonoBehaviour
{
    [Header("Referencias")]
    public SkinnedMeshRenderer gatoMeshRenderer; // O MeshRenderer según tu modelo
    public Material materialOriginal;
    public Material materialNeonRosa;

    private void Start()
    {
        // Guardar el material inicial si no está asignado
        if (gatoMeshRenderer != null && materialOriginal == null)
        {
            materialOriginal = gatoMeshRenderer.material;
        }
    }

    // Llamar al entrar al área / activar estado
    public void ActivarEfectoNeon()
    {
        if (gatoMeshRenderer != null && materialNeonRosa != null)
        {
            gatoMeshRenderer.material = materialNeonRosa;
        }
    }

    // Llamar al salir del área / restaurar estado
    public void DesactivarEfectoNeon()
    {
        if (gatoMeshRenderer != null && materialOriginal != null)
        {
            gatoMeshRenderer.material = materialOriginal;
        }
    }
}