using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    public PlayerController playerController;
    public AudioClip sound;
    public AudioSource audioSource;

    public GameObject canvas;
    public GameObject player;
    public bool isGameOver = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isGameOver == true)
        {

            isGameOver = true;

        }

        if (isGameOver == true)
        {
            StopSound();
            canvas.SetActive(true);
            player.SetActive(false);
        }

    }

    void PlaySound()
    {
        audioSource.Play();
    }


    void StopSound()
    {
        audioSource.Stop();
    }
}
