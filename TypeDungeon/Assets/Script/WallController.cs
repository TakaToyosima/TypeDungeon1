using UnityEngine;
using System.Collections.Generic;

public class WallController : MonoBehaviour
{
    [Header("壁プレハブ")]
    public GameObject wallPrefab;

    [Header("生成列数")]
    public int rows = 5;
    public int columns = 9;

    [Header("生成位置と間隔")]
    public float startX = -8f;        // 横方向基準
    public float startZ = 0f;         // Z方向基準（InvisibleWall より手前に設定）
    public float xSpacing = 2f;       // 横列間隔
    public float zSpacing = 2f;       // 縦列間隔

    [Header("InvisibleWall (距離判定用)")]
    public Transform invisibleWallTransform; // Inspectorで1つ設定

    private List<AlphabetWall> allWalls = new List<AlphabetWall>();

    void Start()
    {
        if (invisibleWallTransform == null)
            Debug.LogWarning("InvisibleWall が設定されていません！");

        GenerateWalls();
    }

    void Update()
    {
        // A-Zキー入力を確実に取得
        for (KeyCode k = KeyCode.A; k <= KeyCode.Z; k++)
        {
            if (Input.GetKeyDown(k))
            {
                char pressed = k.ToString()[0];
                DestroyClosestWall(pressed);
            }
        }
    }

    void DestroyClosestWall(char letter)
    {
        AlphabetWall closestWall = null;
        float minZDiff = float.MaxValue;

        foreach (var wall in allWalls)
        {
            if (wall.AssignedLetter != letter) continue;

            // InvisibleWall より手前（Z < InvisibleWall.Z）の壁のみ対象
            if (wall.transform.position.z <= invisibleWallTransform.position.z) continue;

            float zDiff = Mathf.Abs(wall.transform.position.z - invisibleWallTransform.position.z);
            if (zDiff < minZDiff)
            {
                minZDiff = zDiff;
                closestWall = wall;
            }
        }

        if (closestWall != null)
        {
            allWalls.Remove(closestWall);
            Destroy(closestWall.gameObject);
        }
    }

    void GenerateWalls()
    {
        for (int z = 0; z < rows; z++)
        {
            for (int x = 0; x < columns; x++)
            {
                // InvisibleWall の手前に生成（Z座標小さい側に配置）
                Vector3 pos = new Vector3(
                    startX + x * xSpacing,
                    0f,
                    startZ + z * zSpacing
                );

                GameObject wall = Instantiate(wallPrefab, pos, Quaternion.identity);
                AlphabetWall wallScript = wall.GetComponent<AlphabetWall>();
                allWalls.Add(wallScript);
            }
        }
    }
}
