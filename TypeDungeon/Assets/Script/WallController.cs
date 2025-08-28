using UnityEngine;
using System.Collections.Generic;

public class WallController : MonoBehaviour
{
    [Header("�ǃv���n�u")]
    public GameObject wallPrefab;

    [Header("������")]
    public int rows = 5;
    public int columns = 9;

    [Header("�����ʒu�ƊԊu")]
    public float startX = -8f;        // �������
    public float startZ = 0f;         // Z������iInvisibleWall ����O�ɐݒ�j
    public float xSpacing = 2f;       // ����Ԋu
    public float zSpacing = 2f;       // �c��Ԋu

    [Header("InvisibleWall (��������p)")]
    public Transform invisibleWallTransform; // Inspector��1�ݒ�

    private List<AlphabetWall> allWalls = new List<AlphabetWall>();

    void Start()
    {
        if (invisibleWallTransform == null)
            Debug.LogWarning("InvisibleWall ���ݒ肳��Ă��܂���I");

        GenerateWalls();
    }

    void Update()
    {
        // A-Z�L�[���͂��m���Ɏ擾
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

            // InvisibleWall ����O�iZ < InvisibleWall.Z�j�̕ǂ̂ݑΏ�
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
                // InvisibleWall �̎�O�ɐ����iZ���W���������ɔz�u�j
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
