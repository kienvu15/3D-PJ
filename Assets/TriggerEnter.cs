using UnityEngine;

public class TriggerEnter : MonoBehaviour
{
    public GameObject volum;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the trigger area.");
            // You can add more logic here, such as applying effects or triggering events
            volum.SetActive(true);
        }
    }
}
