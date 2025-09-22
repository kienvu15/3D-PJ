using EazyCamera.Legacy;
using UnityEngine;

public class Boost : MonoBehaviour
{
    public float statetime = 0f;
    public float duration = 2f; 

    public PlayerController playerController;
    public bool isBoosted = false;
    public bool hasBoosted = false; 


    void Start()
    {
        
    }

    void Update()
    {
        if(isBoosted == true)
        {
            if(hasBoosted == false)
            {
                playerController.walkSpeed = playerController.walkSpeed * 2;
                hasBoosted = true; 
            }
            statetime += Time.deltaTime;
            if (statetime >= duration)
            {
                playerController.walkSpeed = playerController.walkSpeed / 2;
                statetime = 0f; 
                isBoosted = false;
                Destroy(gameObject); 
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                isBoosted = true; 
               
            }
        }
    }
}
