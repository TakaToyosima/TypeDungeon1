using UnityEngine;
using TMPro;


public class AlphabetInput : MonoBehaviour
{
    [Header("入力表示用テキスト (TextMeshPro)")]
    public TextMeshProUGUI displayText;


    [Header("壁制御")]
    public WallController wallController;


    [Header("AreaWall（入力許可の基準位置）")]
    public Transform areaWallTransform;


    private string currentInput = "";


    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b')
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput[..^1];
            }
            else if (c == '\n' || c == '\r')
            {
                wallController.DestroyByString(currentInput);
                currentInput = "";
            }
            else if (char.IsLetter(c))
            {
                char up = char.ToUpper(c);
                if (IsLetterAllowed(up)) currentInput += up;
            }
        }


        if (displayText != null) displayText.text = currentInput;
    }


    bool IsLetterAllowed(char letter)
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("AlphaWall");


        foreach (var w in walls)
        {
            AlphabetWall aw = w.GetComponent<AlphabetWall>();
            if (aw == null) continue;
            if (char.ToUpper(aw.AssignedLetter) != letter) continue;


            if (aw.transform.position.z < areaWallTransform.position.z)
                return true;
        }
        return false;
    }


    public string GetCurrentInput() => currentInput;
}