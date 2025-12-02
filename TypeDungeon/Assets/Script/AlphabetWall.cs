using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class AlphabetWall : MonoBehaviour
{
    public float moveSpeed = 5f;
    private AlphabetUI alphabetUI;

    public char letter; // この壁が担当している文字


    private MeshRenderer mr;


    [Header("色設定")]
    public Color normalColor = Color.black;
    public Color highlightColor = Color.red;

    public char AssignedLetter { get; private set; }




    public void SetLetter(char c)
    {
        AssignedLetter = c;
        if (alphabetUI != null) alphabetUI.SetLetter(c);
    }


    void Awake()
    {


        alphabetUI = GetComponentInChildren<AlphabetUI>();


        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc != null && !bc.isTrigger)
            bc.isTrigger = true;

        mr = GetComponentInChildren<MeshRenderer>();

        if (mr != null)
        {
            // マテリアルのインスタンス化
            mr.material = new Material(mr.material);

            // 初期色設定
            mr.material.color = normalColor;
        }
    }


    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }

    public void Highlight(bool enable)
    {
        if (mr != null)
        {
            mr.material.color = enable ? highlightColor : normalColor;
        }
    }

}
