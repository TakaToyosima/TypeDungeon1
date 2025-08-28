using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 5f;
    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        CheckCollision();
    }

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.UpArrow)) moveZ = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) moveZ = -1f;
        if (Input.GetKey(KeyCode.LeftArrow)) moveX = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) moveX = 1f;

        Vector3 move = new Vector3(moveX, 0f, moveZ).normalized;
        controller.Move(move * moveSpeed * Time.deltaTime);

        CheckCollision();
    }

    void CheckCollision()
    {
        Vector3 center = controller.bounds.center;
        Vector3 halfExtents = controller.bounds.extents;

        Collider[] hits = Physics.OverlapBox(center, halfExtents, Quaternion.identity);
        foreach (var hit in hits)
        {
            if (hit.CompareTag("AlphaWall"))
            {
                SceneManager.LoadScene("GameOver");
                return;
            }
        }
    }
}
