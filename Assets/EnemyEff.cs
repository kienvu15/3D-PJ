using UnityEngine;
using UnityEngine.AI;

public class EnemyFollow : MonoBehaviour
{
    public NavMeshAgent enemy;
    public PlayerStats playerStats; 
    public Transform Player;
    public Animator animator;

    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    private bool playerInRange = false;
    public PlayerController playerController;
    void Update()
    {
        
            enemy.SetDestination(Player.position);
            float speed = enemy.velocity.magnitude;

        if (speed < 0.1f)
        {
            animator.SetBool("Run", false);
        }
        else
        {
            animator.SetBool("Run", true);
        }


        float distance = Vector3.Distance(transform.position, Player.position);
        if (distance <= 1f)
        {
            playerInRange = true;
            
            
        }

        if(playerInRange == true)
        {
            animator.SetTrigger("Attack");
            
            animator.SetBool("Run", false);
        }

    }


    //private void OnTriggerEnter(Collider other)
    //{

    //    if (other.CompareTag("Player"))
    //    {
           
    //        playerStats.TakeDamage(1);
    //        Debug.Log("Player hit by enemy! Health: " + playerController.health);

            
    //        Rigidbody playerRb = other.GetComponent<Rigidbody>();
    //        if (playerRb != null)
    //        {
                
    //            Vector3 pushDir = (other.transform.position - transform.position).normalized;
    //            float pushForce = 5f;
    //            playerRb.AddForce(pushDir * pushForce, ForceMode.Impulse);
    //        }

            
    //    }

    //}



}
