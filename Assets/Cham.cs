using UnityEngine;
using UnityEngine.Rendering;

public class Cham : MonoBehaviour
{
    public Volume myVolume;

    private void Start()
    {
        //myVolume.weight = 0f;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        myVolume.weight = 1f;
    }
}
