using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public void LateUpdate()
    {
        Vector3 dir = transform.position - Camera.main.transform.position;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
