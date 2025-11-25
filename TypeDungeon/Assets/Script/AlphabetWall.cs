using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class AlphabetWall : MonoBehaviour
{
    public float moveSpeed = 5f;
    private AlphabetUI alphabetUI;


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
    }


    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }
}