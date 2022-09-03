using UnityEngine;

public class DetachFromParent : MonoBehaviour
{
    private void OnEnable()
    {
        if (transform.parent != null)
        {
            transform.parent = null;
        }
    }
}
