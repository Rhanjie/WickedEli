using UnityEngine;

//https://www.evozon.com/two-unity-tricks-isometric-games/

public class IsometricObject : MonoBehaviour
{
    private const int IsometricRangePerYUnit = 100;
    
    [SerializeField]
    protected Transform attachedTo;

    [SerializeField]
    protected int offset = 0;
    
    protected Renderer[] Renderers;

    private void Awake()
    {
        Renderers = GetComponentsInChildren<Renderer>();
    }

    protected virtual void Update()
    {
        if (attachedTo == null)
            attachedTo = transform;

        foreach (var rendererComponent in Renderers)
        { 
            rendererComponent.sortingOrder = -(int)(attachedTo.position.y * IsometricRangePerYUnit) + offset;
        }
    }
}
