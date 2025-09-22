using UnityEngine;

public class triggerZone : MonoBehaviour
{

    public EnemyFollow EnemyFloo;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnemyFloo.enabled = false; // Đảm bảo EnemyFollow không hoạt động ngay từ đầu
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Gọi hàm StartEnemyFloo từ EnemyFollow
            EnemyFloo.enabled = true; // Bật EnemyFollow khi Player vào vùng trigger
        }
    }
}
