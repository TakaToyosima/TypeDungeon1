using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class AlphabetWall : MonoBehaviour
{
    public float moveSpeed = 5f;
    private AlphabetUI alphabetUI;

    public char letter; // Ç±ÇÃï«Ç™íSìñÇµÇƒÇ¢ÇÈï∂éö


    private MeshRenderer mr;


    [Header("êFê›íË")]
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

        mr = GetComponent<MeshRenderer>();
        if (mr != null && mr.material != null)
        {
            mr.material.color = normalColor;
        }
    }


    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }

    public void Highlight(bool enable)
    {
        if (mr != null && mr.material != null)
        {
            mr.material.color = enable ? highlightColor : normalColor;
        }
    }

}
