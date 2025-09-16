using UnityEngine;
using TMPro;

public class AlphabetInput : MonoBehaviour
{
    [Header("入力表示用テキスト (TextMeshPro)")]
    public TextMeshProUGUI displayText;

    private string currentInput = "";   // 入力中のアルファベット

    void Update()
    {
        // 文字入力処理
        foreach (char c in Input.inputString)
        {
            // バックスペース
            if (c == '\b')
            {
                if (currentInput.Length > 0)
                {
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                }
            }
            // エンター / リターン
            else if (c == '\n' || c == '\r')
            {
                currentInput = ""; // 全消去
            }
            // アルファベットのみ受け付け
            else if (char.IsLetter(c))
            {
                currentInput += c;
            }
        }

        // 表示を更新
        if (displayText != null)
        {
            displayText.text = currentInput;
        }
    }
}
