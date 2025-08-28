using UnityEngine;
using UnityEngine.UI;

public class AlphabetUI : MonoBehaviour
{
    private Text text;

    void Awake()
    {
        text = GetComponent<Text>();
    }

    public void SetLetter(char letter)
    {
        if (text != null)
            text.text = letter.ToString();
    }
}
