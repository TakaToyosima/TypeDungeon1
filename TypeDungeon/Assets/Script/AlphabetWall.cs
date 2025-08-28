using UnityEngine;

[RequireComponent(typeof(BoxCollider))] // Collider•K{
public class AlphabetWall : MonoBehaviour
{
    public float moveSpeed = 5f;
    private AlphabetUI alphabetUI;

    public char AssignedLetter { get; private set; }

    void Start()
    {
        int ascii = Random.Range(65, 91); // A-Z
        AssignedLetter = (char)ascii;

        alphabetUI = GetComponentInChildren<AlphabetUI>();
        if (alphabetUI != null)
            alphabetUI.SetLetter(AssignedLetter);

        // Collider‚ªTrigger‚Å‚È‚¢ê‡‚Íİ’è
        BoxCollider bc = GetComponent<BoxCollider>();
        if (bc != null && !bc.isTrigger)
            bc.isTrigger = true;
    }

    void Update()
    {
        transform.Translate(Vector3.back * moveSpeed * Time.deltaTime, Space.World);
    }
}
