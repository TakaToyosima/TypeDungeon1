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


    [Header("出現確率（A〜Z の 26 要素）")]
    public List<float> letterWeights = new List<float>(new float[26]);

    //[Header("出現確率（A〜Z）")]
    //public WeightTable weightTable;


    private List<AlphabetWall> allWalls = new List<AlphabetWall>();


    void Start()
    {
        if (letterWeights.Count != 26)
        {
            letterWeights = new List<float>(new float[26]);
            for (int i = 0; i < 26; i++) letterWeights[i] = 1f;
        }


        GenerateWalls();
    }


    // -----------------------
    // Public API
    // -----------------------
    public void DestroyWallByLetter(char letter)
    {
        DestroyClosestWall(letter);
    }


    public void DestroyByString(string letters)
    {
        if (string.IsNullOrEmpty(letters)) return;
        foreach (char c in letters)
        {
            if (char.IsLetter(c)) DestroyClosestWall(c);
        }
    }


    // -----------------------
    // Weighted Random Letter
    // -----------------------
    char GetWeightedRandomLetter()
    {
        float total = 0f;
        for (int i = 0; i < 26; i++) total += letterWeights[i];
        float r = Random.Range(0f, total);


        for (int i = 0; i < 26; i++)
        {
            if (r < letterWeights[i]) return (char)('A' + i);
            r -= letterWeights[i];
        }
        return 'A';
    }

    // -----------------------
    // DestroyClosestWall
    // -----------------------
    void DestroyClosestWall(char letter)
    {
        char target = char.ToUpper(letter);


        allWalls.RemoveAll(w => w == null);


        AlphabetWall closest = null;
        float minDist = float.MaxValue;


        foreach (var wall in allWalls)
        {
            if (wall == null) continue;
            if (char.ToUpper(wall.AssignedLetter) != target) continue;


            float dist = Mathf.Abs(wall.transform.position.z - invisibleWallTransform.position.z);
            if (dist < minDist)
            {
                minDist = dist;
                closest = wall;
            }
        }


        if (closest != null)
        {
            allWalls.Remove(closest);
            Destroy(closest.gameObject);
        }
    }// -----------------------
     // GenerateWalls
     // -----------------------
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


                GameObject wallObj = Instantiate(wallPrefab, pos, Quaternion.identity);
                AlphabetWall wall = wallObj.GetComponent<AlphabetWall>();


                if (wall != null)
                {
                    char letter = GetWeightedRandomLetter();
                    wall.SetLetter(letter);
                    allWalls.Add(wall);
                }
            }
        }
    }
}