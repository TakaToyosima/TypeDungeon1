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
            if (c == '\b') // Backspace
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput[..^1];
            }
            else if (c == '\n' || c == '\r') // Enter
            {
                // ★修正箇所: ここで wallController.DestroyByString を呼んでいたのを削除しました
                // 壁の破壊は WordChecker 側に一任します
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

    // 特定のエリアにその文字の壁があるかチェックする
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