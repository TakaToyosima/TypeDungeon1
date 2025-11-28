using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("レーンの位置（9個）")]
    public Transform[] lanePositions;

    [Header("移動速度")]
    public float moveSpeed = 10f; // レーン移動の速さ

    private int currentLane = 4; // 0～8の 9レーン。中央は4

    void Update()
    {
        HandleLaneInput();
        MoveToLane();
    }

    /// <summary>
    /// 左右キー入力でレーン番号を変更
    /// </summary>
    private void HandleLaneInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            currentLane = Mathf.Max(currentLane - 1, 0);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            currentLane = Mathf.Min(currentLane + 1, lanePositions.Length - 1);
        }
    }

    /// <summary>
    /// 対象レーンの位置へスムーズに移動（Xのみ）
    /// </summary>
    private void MoveToLane()
    {
        if (lanePositions == null || lanePositions.Length == 0) return;

        Vector3 targetPos = new Vector3(
            lanePositions[currentLane].position.x,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPos,
            moveSpeed * Time.deltaTime
        );
    }

    /// <summary>
    /// 壁衝突でゲームオーバー
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Wall"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
        }
    }
}
