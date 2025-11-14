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
    // あ行
    {"A","あ"}, {"I","い"}, {"U","う"}, {"E","え"}, {"O","お"},
    {"LA","ぁ"}, {"LI","ぃ"}, {"LU","ぅ"}, {"LE","ぇ"}, {"LO","ぉ"},
    {"XA","ぁ"}, {"XI","ぃ"}, {"XU","ぅ"}, {"XE","ぇ"}, {"XO","ぉ"},
    {"YE","いぇ"},
    {"WHA","うぁ"}, {"WHI","うぃ"}, {"WHE","うぇ"}, {"WHO","うぉ"},

    // か行
    {"KA","か"}, {"KI","き"}, {"KU","く"}, {"KE","け"}, {"KO","こ"},
    {"KYA","きゃ"}, {"KYI","きぃ"}, {"KYU","きゅ"}, {"KYE","きぇ"}, {"KYO","きょ"},
    {"CA","か"}, {"CU","く"}, {"CO","こ"},
    {"QU","く"}, {"QA","か"}, {"QI","き"}, {"QE","け"}, {"QO","こ"},
    {"QYA","くゃ"}, {"QYU","くゅ"}, {"QYO","くょ"},
    {"QWA","くぁ"}, {"QWI","くぃ"}, {"QWU","くぅ"}, {"QWE","くぇ"}, {"QWO","くぉ"},

    // が行
    {"GA","が"}, {"GI","ぎ"}, {"GU","ぐ"}, {"GE","げ"}, {"GO","ご"},
    {"GYA","ぎゃ"}, {"GYI","ぎぃ"}, {"GYU","ぎゅ"}, {"GYE","ぎぇ"}, {"GYO","ぎょ"},
    {"GWA","ぐぁ"}, {"GWI","ぐぃ"}, {"GWU","ぐぅ"}, {"GWE","ぐぇ"}, {"GWO","ぐぉ"},

    // さ行
    {"SA","さ"}, {"SI","し"}, {"SU","す"}, {"SE","せ"}, {"SO","そ"},
    {"SYA","しゃ"}, {"SYI","しぃ"}, {"SYU","しゅ"}, {"SYE","しぇ"}, {"SYO","しょ"},
    {"SHI","し"}, {"SHA","しゃ"}, {"SHU","しゅ"}, {"SHE","しぇ"}, {"SHO","しょ"},

    // ざ行
    {"ZA","ざ"}, {"ZI","じ"}, {"ZU","ず"}, {"ZE","ぜ"}, {"ZO","ぞ"},
    {"JA","じゃ"}, {"JI","じ"}, {"JU","じゅ"}, {"JE","じぇ"}, {"JO","じょ"},
    {"ZYA","じゃ"}, {"ZYI","じぃ"}, {"ZYU","じゅ"}, {"ZYE","じぇ"}, {"ZYO","じょ"},

    // た行
    {"TA","た"}, {"TI","ち"}, {"TU","つ"}, {"TE","て"}, {"TO","と"},
    {"CHA","ちゃ"}, {"CHI","ち"}, {"CHU","ちゅ"}, {"CHE","ちぇ"}, {"CHO","ちょ"},
    {"TYA","ちゃ"}, {"TYI","ちぃ"}, {"TYU","ちゅ"}, {"TYE","ちぇ"}, {"TYO","ちょ"},
    {"TSA","つぁ"}, {"TSI","つぃ"}, {"TSU","つ"}, {"TSE","つぇ"}, {"TSO","つぉ"},
    {"THA","てゃ"}, {"THI","てぃ"}, {"THU","てゅ"}, {"THE","てぇ"}, {"THO","てょ"},
    {"TWA","とぁ"}, {"TWI","とぃ"}, {"TWU","とぅ"}, {"TWE","とぇ"}, {"TWO","とぉ"},

    // だ行
    {"DA","だ"}, {"DI","ぢ"}, {"DU","づ"}, {"DE","で"}, {"DO","ど"},
    {"DYA","ぢゃ"}, {"DYI","ぢぃ"}, {"DYU","ぢゅ"}, {"DYE","ぢぇ"}, {"DYO","ぢょ"},
    {"DHA","でゃ"}, {"DHI","でぃ"}, {"DHU","でゅ"}, {"DHE","でぇ"}, {"DHO","でょ"},
    {"DWA","どぁ"}, {"DWI","どぃ"}, {"DWU","どぅ"}, {"DWE","どぇ"}, {"DWO","どぉ"},

    // な行
    {"NA","な"}, {"NI","に"}, {"NU","ぬ"}, {"NE","ね"}, {"NO","の"},
    {"NYA","にゃ"}, {"NYI","にぃ"}, {"NYU","にゅ"}, {"NYE","にぇ"}, {"NYO","にょ"},

    // は行
    {"HA","は"}, {"HI","ひ"}, {"HU","ふ"}, {"HE","へ"}, {"HO","ほ"},
    {"HYA","ひゃ"}, {"HYI","ひぃ"}, {"HYU","ひゅ"}, {"HYE","ひぇ"}, {"HYO","ひょ"},
    {"FYA","ふゃ"}, {"FYU","ふゅ"}, {"FYO","ふょ"},
    {"FWA","ふぁ"}, {"FWI","ふぃ"}, {"FWU","ふぅ"}, {"FWE","ふぇ"}, {"FWO","ふぉ"},
    {"FA","ふぁ"}, {"FI","ふぃ"}, {"FE","ふぇ"}, {"FO","ふぉ"},
    
    // ば行
    {"BA","ば"}, {"BI","び"}, {"BU","ぶ"}, {"BE","べ"}, {"BO","ぼ"},
    {"BYA","びゃ"}, {"BYI","びぃ"}, {"BYU","びゅ"}, {"BYE","びぇ"}, {"BYO","びょ"},

    // ぱ行
    {"PA","ぱ"}, {"PI","ぴ"}, {"PU","ぷ"}, {"PE","ぺ"}, {"PO","ぽ"},
    {"PYA","ぴゃ"}, {"PYI","ぴぃ"}, {"PYU","ぴゅ"}, {"PYE","ぴぇ"}, {"PYO","ぴょ"},

    // ま行
    {"MA","ま"}, {"MI","み"}, {"MU","む"}, {"ME","め"}, {"MO","も"},
    {"MYA","みゃ"}, {"MYI","みぃ"}, {"MYU","みゅ"}, {"MYE","みぇ"}, {"MYO","みょ"},

    // や行
    {"YA","や"}, {"YU","ゆ"}, {"YO","よ"},
    {"LYA","ゃ"}, {"LYU","ゅ"}, {"LYO","ょ"},
    {"XYA","ゃ"}, {"XYU","ゅ"}, {"XYO","ょ"},

    // ら行
    {"RA","ら"}, {"RI","り"}, {"RU","る"}, {"RE","れ"}, {"RO","ろ"},
    {"RYA","りゃ"}, {"RYI","りぃ"}, {"RYU","りゅ"}, {"RYE","りぇ"}, {"RYO","りょ"},

    // わ行
    {"WA","わ"}, {"WI","うぃ"}, {"WE","うぇ"}, {"WO","を"},
    {"NN","ん"}, {"XN","ん"},

    // ヴ行
    {"VA","ゔぁ"}, {"VI","ゔぃ"}, {"VU","ゔ"}, {"VE","ゔぇ"}, {"VO","ゔぉ"},
    {"VYA","ゔゃ"}, {"VYI","ゔぃ"}, {"VYU","ゔゅ"}, {"VYE","ゔぇ"}, {"VYO","ゔょ"},

    // 促音（っ）— 子音2回
    // → 実際の処理はコード側でやるので辞書には入れない

    // 伸ばし棒
    {"-","ー"},
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

