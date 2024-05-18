using UnityEngine;

//https://www.evozon.com/two-unity-tricks-isometric-games/

[ExecuteInEditMode]
public class ProjectOnGround : MonoBehaviour
{
    private Quaternion savedRotation;

    private void OnRenderObject()
    {
        transform.rotation = savedRotation;
    }

    private void OnWillRenderObject()
    {
        savedRotation = transform.rotation;
        var eulers = savedRotation.eulerAngles;
        transform.rotation = Quaternion.Euler(45f, eulers.y, eulers.z);
    }
}