using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InputController : MonoBehaviour
{
    public TextMeshProUGUI displayText; // 表示するUI（インスペクターで割り当て）

    private string currentInput = "";   // 入力中の文字列（まだ変換されていない部分）
    private string convertedText = "";  // 確定したひらがな部分

    // ローマ字→ひらがな対応表
    private Dictionary<string, string> romaToHira = new Dictionary<string, string>()
    {
        // 母音
        {"a","あ"},{"i","い"},{"u","う"},{"e","え"},{"o","お"},

        // 行
        {"ka","か"},{"ki","き"},{"ku","く"},{"ke","け"},{"ko","こ"},
        {"sa","さ"},{"shi","し"},{"su","す"},{"se","せ"},{"so","そ"},
        {"ta","た"},{"chi","ち"},{"tsu","つ"},{"te","て"},{"to","と"},
        {"na","な"},{"ni","に"},{"nu","ぬ"},{"ne","ね"},{"no","の"},
        {"ha","は"},{"hi","ひ"},{"fu","ふ"},{"he","へ"},{"ho","ほ"},
        {"ma","ま"},{"mi","み"},{"mu","む"},{"me","め"},{"mo","も"},
        {"ya","や"},{"yu","ゆ"},{"yo","よ"},
        {"ra","ら"},{"ri","り"},{"ru","る"},{"re","れ"},{"ro","ろ"},
        {"wa","わ"},{"wo","を"},{"n","ん"},

        // 濁音
        {"ga","が"},{"gi","ぎ"},{"gu","ぐ"},{"ge","げ"},{"go","ご"},
        {"za","ざ"},{"ji","じ"},{"zu","ず"},{"ze","ぜ"},{"zo","ぞ"},
        {"da","だ"},{"de","で"},{"do","ど"},
        {"ba","ば"},{"bi","び"},{"bu","ぶ"},{"be","べ"},{"bo","ぼ"},

        // 半濁音
        {"pa","ぱ"},{"pi","ぴ"},{"pu","ぷ"},{"pe","ぺ"},{"po","ぽ"},

        // 拗音
        {"kya","きゃ"},{"kyu","きゅ"},{"kyo","きょ"},
        {"sha","しゃ"},{"shu","しゅ"},{"sho","しょ"},
        {"cha","ちゃ"},{"chu","ちゅ"},{"cho","ちょ"},
        {"nya","にゃ"},{"nyu","にゅ"},{"nyo","にょ"},
        {"hya","ひゃ"},{"hyu","ひゅ"},{"hyo","ひょ"},
        {"mya","みゃ"},{"myu","みゅ"},{"myo","みょ"},
        {"rya","りゃ"},{"ryu","りゅ"},{"ryo","りょ"},
        {"gya","ぎゃ"},{"gyu","ぎゅ"},{"gyo","ぎょ"},
        {"ja","じゃ"},{"ju","じゅ"},{"jo","じょ"},
        {"bya","びゃ"},{"byu","びゅ"},{"byo","びょ"},
        {"pya","ぴゃ"},{"pyu","ぴゅ"},{"pyo","ぴょ"},

        // 小さい文字
        {"xa","ぁ"},{"xi","ぃ"},{"xu","ぅ"},{"xe","ぇ"},{"xo","ぉ"},
        {"xya","ゃ"},{"xyu","ゅ"},{"xyo","ょ"},{"xtu","っ"}
    };

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // バックスペース
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                else if (convertedText.Length > 0)
                    convertedText = convertedText.Substring(0, convertedText.Length - 1);
            }
            else
            {
                currentInput += c; // 入力追加
            }

            ConvertInput();
        }

        displayText.text = convertedText + currentInput; // 表示更新
    }

    void ConvertInput()
    {
        bool matched = true;

        while (matched && currentInput.Length > 0)
        {
            matched = false;

            // 長いキーから優先的にチェック
            foreach (var pair in romaToHira)
            {
                if (currentInput.StartsWith(pair.Key))
                {
                    convertedText += pair.Value;
                    currentInput = currentInput.Substring(pair.Key.Length);
                    matched = true;
                    break;
                }
            }

            // 促音（例: "kk" → "っk"）
            if (!matched && currentInput.Length >= 2 && currentInput[0] == currentInput[1])
            {
                if ("bcdfghjklmnpqrstvwxyz".Contains(currentInput[0].ToString()))
                {
                    convertedText += "っ";
                    currentInput = currentInput.Substring(1); // 先頭を消して残りは保持
                    matched = true;
                }
            }
        }
    }
}
