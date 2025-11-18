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
        if (invisibleWallTransform == null)
            Debug.LogWarning("InvisibleWall が設定されていません！");

        GenerateWalls();
    }

    // -----------------------
    // 互換用公文 API
    // -----------------------

    /// <summary>
    /// 互換用：1文字分の壁を破壊する public メソッド（呼び出し元がこの名前を期待している場合用）
    /// </summary>
    public void DestroyWallByLetter(char letter)
    {
        DestroyClosestWall(letter);
    }

    /// <summary>
    /// 文字列を受け取って全ての文字について破壊処理を呼ぶ
    /// </summary>
    public void DestroyByString(string letters)
    {
        if (string.IsNullOrEmpty(letters)) return;

        foreach (char c in letters)
        {
            if (!char.IsLetter(c)) continue;
            DestroyClosestWall(c);
        }
    }

    // -----------------------
    // 内部処理
    // -----------------------

    /// <summary>
    /// 指定文字に一致する壁のうち、InvisibleWall に一番近いものを1つ破壊する
    /// </summary>
    void DestroyClosestWall(char letter)
    {
        // 正常化（大文字で比較）
        char target = char.ToUpper(letter);

        // クリーンアップ：null な参照を除去（他所で Destroy されている可能性）
        allWalls.RemoveAll(w => w == null);

        AlphabetWall closestWall = null;
        float minZDiff = float.MaxValue;

        foreach (var wall in allWalls)
        {
            if (wall == null) continue;

            // AssignedLetter が char 型なので大文字比較
            if (char.ToUpper(wall.AssignedLetter) != target) continue;

            // InvisibleWall より手前（手前＝z > invisibleZ の設計だった場合は条件を反転してください）
            if (invisibleWallTransform != null)
            {
                if (wall.transform.position.z <= invisibleWallTransform.position.z) continue;
            }

            float zDiff = invisibleWallTransform != null
                ? Mathf.Abs(wall.transform.position.z - invisibleWallTransform.position.z)
                : Mathf.Abs(wall.transform.position.z); // invisible 指定がなければ単純な絶対値

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
                if (wallScript != null) allWalls.Add(wallScript);
                else Debug.LogWarning("WallController: instantiated prefab has no AlphabetWall component.");
            }
        }
    }
}
