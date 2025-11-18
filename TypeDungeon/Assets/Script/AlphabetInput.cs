using UnityEngine;
using TMPro;

public class AlphabetInput : MonoBehaviour
{
    [Header("入力表示用テキスト (TextMeshPro)")]
    public TextMeshProUGUI displayText;

    [Header("壁制御")]
    public WallController wallController;   // ★追加：WallController への参照

    private string currentInput = "";   // 入力中のアルファベット

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            // バックスペース
            if (c == '\b')
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
            }
            // エンター
            else if (c == '\n' || c == '\r')
            {
                if (!string.IsNullOrEmpty(currentInput))
                {
                    // ★ 入力されている全文字を WallController へ渡す
                    wallController.DestroyByString(currentInput);
                }

                currentInput = ""; // 入力消去
            }
            // アルファベットのみ受け付け（大文字化）
            else if (char.IsLetter(c))
            {
                currentInput += char.ToUpper(c);
            }
        }

        if (displayText != null)
            displayText.text = currentInput;
    }

    // ★ 外部が現在の入力文字列を取得したい時用
    public string GetCurrentInput()
    {
        return currentInput;
    }
}
