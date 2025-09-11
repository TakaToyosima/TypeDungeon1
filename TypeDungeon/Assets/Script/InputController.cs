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
        {"sa","さ"},{"shi","し"},{"si","し"},{"su","す"},{"se","せ"},{"so","そ"},
        {"ta","た"},{"chi","ち"},{"ti","ち"},{"tsu","つ"},{"tu","つ"},{"te","て"},{"to","と"},
        {"na","な"},{"ni","に"},{"nu","ぬ"},{"ne","ね"},{"no","の"},
        {"ha","は"},{"hi","ひ"},{"fu","ふ"},{"hu","ふ"},{"he","へ"},{"ho","ほ"},
        {"ma","ま"},{"mi","み"},{"mu","む"},{"me","め"},{"mo","も"},
        {"ya","や"},{"yu","ゆ"},{"yo","よ"},
        {"ra","ら"},{"ri","り"},{"ru","る"},{"re","れ"},{"ro","ろ"},
        {"wa","わ"},{"wo","を"},

        // n
        {"nn","ん"},

        // 濁音
        {"ga","が"},{"gi","ぎ"},{"gu","ぐ"},{"ge","げ"},{"go","ご"},
        {"za","ざ"},{"ji","じ"},{"zi","じ"},{"zu","ず"},{"ze","ぜ"},{"zo","ぞ"},
        {"da","だ"},{"di","ぢ"},{"du","づ"},{"de","で"},{"do","ど"},
        {"ba","ば"},{"bi","び"},{"bu","ぶ"},{"be","べ"},{"bo","ぼ"},

        // 半濁音
        {"pa","ぱ"},{"pi","ぴ"},{"pu","ぷ"},{"pe","ぺ"},{"po","ぽ"},

        // 拗音
        {"kya","きゃ"},{"kyu","きゅ"},{"kyo","きょ"},
        {"sha","しゃ"},{"shu","しゅ"},{"sho","しょ"},{"sya","しゃ"},{"syu","しゅ"},{"syo","しょ"},
        {"cha","ちゃ"},{"chu","ちゅ"},{"cho","ちょ"},{"tya","ちゃ"},{"tyu","ちゅ"},{"tyo","ちょ"},
        {"nya","にゃ"},{"nyu","にゅ"},{"nyo","にょ"},
        {"hya","ひゃ"},{"hyu","ひゅ"},{"hyo","ひょ"},
        {"mya","みゃ"},{"myu","みゅ"},{"myo","みょ"},
        {"rya","りゃ"},{"ryu","りゅ"},{"ryo","りょ"},
        {"gya","ぎゃ"},{"gyu","ぎゅ"},{"gyo","ぎょ"},
        {"ja","じゃ"},{"ju","じゅ"},{"jo","じょ"},{"jya","じゃ"},{"jyu","じゅ"},{"jyo","じょ"},
        {"bya","びゃ"},{"byu","びゅ"},{"byo","びょ"},
        {"pya","ぴゃ"},{"pyu","ぴゅ"},{"pyo","ぴょ"},

        // 小さい文字（x系）
        {"xa","ぁ"},{"xi","ぃ"},{"xu","ぅ"},{"xe","ぇ"},{"xo","ぉ"},
        {"xya","ゃ"},{"xyu","ゅ"},{"xyo","ょ"},{"xtu","っ"},{"xwa","ゎ"},

        // 小文字（l系）
        {"la","ぁ"},{"li","ぃ"},{"lu","ぅ"},{"le","ぇ"},{"lo","ぉ"},
        {"lya","ゃ"},{"lyu","ゅ"},{"lyo","ょ"},{"ltu","っ"},{"lwa","ゎ"},

        // v行 / f行（外来語用）
        {"va","ゔぁ"},{"vi","ゔぃ"},{"vu","ゔ"},{"ve","ゔぇ"},{"vo","ゔぉ"},
        {"fa","ふぁ"},{"fi","ふぃ"},{"fe","ふぇ"},{"fo","ふぉ"},{"fyu","ふゅ"},

        //伸ばし棒
        {"-","ー"}
    };

    void Update()
    {
        // Enterキーで消去
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            currentInput = "";
            convertedText = "";
            displayText.text = "";
            return; // 処理終了
        }

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
                    currentInput = currentInput.Substring(1);
                    matched = true;
                    continue;
                }
            }

            // n→んの処理（nn または n + 非母音）
            if (!matched && currentInput[0] == 'n')
            {
                if (currentInput.Length >= 2)
                {
                    char next = currentInput[1];
                    if (next == 'n')
                    {
                        convertedText += "ん";
                        currentInput = currentInput.Substring(2);
                        matched = true;
                    }
                    else if (!"aiueoy".Contains(next.ToString()))
                    {
                        convertedText += "ん";
                        currentInput = currentInput.Substring(1);
                        matched = true;
                    }
                }
            }
        }
    }
}
