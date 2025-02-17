using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Material activeMaterial;
    public Material inactiveMaterial;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        if (meshRenderer == null)
        {
            Debug.LogError($"{gameObject.name} doesn't have MeshRenderer.");
        }
    }

    private void Start()
    {
        SetInactive();
    }

    public void SetActive()
    {
        if (meshRenderer != null && activeMaterial != null)
        {
            meshRenderer.material = activeMaterial;
        }
    }

    public void SetInactive()
    {
        if (meshRenderer != null && inactiveMaterial != null)
        {
            meshRenderer.material = inactiveMaterial;
        }
    }
}
