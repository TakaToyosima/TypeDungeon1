using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class InvisibleWall : MonoBehaviour
{
    private void Awake()
    {
        BoxCollider bc = GetComponent<BoxCollider>();
        if (!bc.isTrigger)
            bc.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        AlphabetWall wall = other.GetComponent<AlphabetWall>();
        if (wall != null)
        {
            Destroy(wall.gameObject);
        }
    }
}
