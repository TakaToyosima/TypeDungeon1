using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickRetry : MonoBehaviour
{
    [Header("ジャンプ先の設定")]
    public string targetSceneName = "Stage1"; // インスペクターでシーン名を入力

    void Update()
    {
        // Rキーが押された瞬間
        if (Input.GetKeyDown(KeyCode.R))
        {
            // 指定したシーンを読み込む
            SceneManager.LoadScene(targetSceneName);
        }
    }
}