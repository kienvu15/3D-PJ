using UnityEngine;

public class MouseLookToggle : MonoBehaviour
{
    public float sensitivity = 5f;
    public Transform playerBody;

    private float xRotation = 0f;
    private bool isLooking = false;

    void Update()
    {
        // Ấn chuột phải để bắt đầu điều khiển camera
        if (Input.GetMouseButtonDown(1))
        {
            LockCursor();
        }

        // Ấn ESC để dừng điều khiển camera
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnlockCursor();
        }

        // Nếu đang điều khiển camera thì xoay theo chuột
        if (isLooking)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isLooking = true;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isLooking = false;
    }
}
