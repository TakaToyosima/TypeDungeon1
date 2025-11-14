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
    public float startX = -8f;
    public float startZ = 0f;
    public float xSpacing = 2f;
    public float zSpacing = 2f;

    [Header("InvisibleWall (距離判定用)")]
    public Transform invisibleWallTransform;

    private List<AlphabetWall> allWalls = new List<AlphabetWall>();

    void Start()
    {
        GenerateWalls();
    }

    // ←★ 新規メソッド：Enter押下時に呼ばれる
    public void DestroyWallByLetter(char letter)
    {
        AlphabetWall closestWall = null;
        float minZDiff = float.MaxValue;

        foreach (var wall in allWalls)
        {
            if (wall.AssignedLetter != letter) continue;

            // InvisibleWall より奥だけ破壊対象
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
