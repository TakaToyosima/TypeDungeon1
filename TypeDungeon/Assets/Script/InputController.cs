using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InputController : MonoBehaviour
{
    [Header("アルファベット入力欄（TextMeshPro）")]
    public TMP_Text alphabetInputField;   // ← AlphInput

    [Header("ひらがな入力欄（TextMeshPro）")]
    public TMP_Text hiraInputField;       // ← HiraInput

    // 現在のアルファベットの入力（UI から取得）
    public string CurrentWord => alphabetInputField.text;

    // ローマ字変換用バッファ
    private string currentInput = "";     // 未確定ローマ字
    private string convertedText = "";    // 確定したひらがな

    // ローマ字→ひらがな辞書（旧 InputController のまま）
    private Dictionary<string, string> romaToHira = new Dictionary<string, string>()
{
    // あ行
    {"a","あ"}, {"i","い"}, {"u","う"}, {"e","え"}, {"o","お"},
    {"la","ぁ"}, {"li","ぃ"}, {"lu","ぅ"}, {"le","ぇ"}, {"lo","ぉ"},
    {"xa","ぁ"}, {"xi","ぃ"}, {"xu","ぅ"}, {"xe","ぇ"}, {"xo","ぉ"},
    {"ye","いぇ"},
    {"wha","うぁ"}, {"whi","うぃ"}, {"whe","うぇ"}, {"who","うぉ"},

    // か行
    {"ka","か"}, {"ki","き"}, {"ku","く"}, {"ke","け"}, {"ko","こ"},
    {"kya","きゃ"}, {"kyi","きぃ"}, {"kyu","きゅ"}, {"kye","きぇ"}, {"kyo","きょ"},
    {"ca","か"}, {"cu","く"}, {"co","こ"},
    {"qu","く"}, {"qa","か"}, {"qi","き"}, {"qe","け"}, {"qo","こ"},
    {"qya","くゃ"}, {"qyu","くゅ"}, {"qyo","くょ"},
    {"qwa","くぁ"}, {"qwi","くぃ"}, {"qwu","くぅ"}, {"qwe","くぇ"}, {"qwo","くぉ"},

    // が行
    {"ga","が"}, {"gi","ぎ"}, {"gu","ぐ"}, {"ge","げ"}, {"go","ご"},
    {"gya","ぎゃ"}, {"gyi","ぎぃ"}, {"gyu","ぎゅ"}, {"gye","ぎぇ"}, {"gyo","ぎょ"},
    {"gwa","ぐぁ"}, {"gwi","ぐぃ"}, {"gwu","ぐぅ"}, {"gwe","ぐぇ"}, {"gwo","ぐぉ"},

    // さ行
    {"sa","さ"}, {"si","し"}, {"su","す"}, {"se","せ"}, {"so","そ"},
    {"sya","しゃ"}, {"syi","しぃ"}, {"syu","しゅ"}, {"sye","しぇ"}, {"syo","しょ"},
    {"shi","し"}, {"sha","しゃ"}, {"shu","しゅ"}, {"she","しぇ"}, {"sho","しょ"},

    // ざ行
    {"za","ざ"}, {"zi","じ"}, {"zu","ず"}, {"ze","ぜ"}, {"zo","ぞ"},
    {"ja","じゃ"}, {"ji","じ"}, {"ju","じゅ"}, {"je","じぇ"}, {"jo","じょ"},
    {"zya","じゃ"}, {"zyi","じぃ"}, {"zyu","じゅ"}, {"zye","じぇ"}, {"zyo","じょ"},

    // た行
    {"ta","た"}, {"ti","ち"}, {"tu","つ"}, {"te","て"}, {"to","と"},
    {"cha","ちゃ"}, {"chi","ち"}, {"chu","ちゅ"}, {"che","ちぇ"}, {"cho","ちょ"},
    {"tya","ちゃ"}, {"tyi","ちぃ"}, {"tyu","ちゅ"}, {"tye","ちぇ"}, {"tyo","ちょ"},
    {"tsa","つぁ"}, {"tsi","つぃ"}, {"tsu","つ"}, {"tse","つぇ"}, {"tso","つぉ"},
    {"tha","てゃ"}, {"thi","てぃ"}, {"thu","てゅ"}, {"the","てぇ"}, {"tho","てょ"},
    {"twa","とぁ"}, {"twi","とぃ"}, {"twu","とぅ"}, {"twe","とぇ"}, {"two","とぉ"},

    // だ行
    {"da","だ"}, {"di","ぢ"}, {"du","づ"}, {"de","で"}, {"do","ど"},
    {"dya","ぢゃ"}, {"dyi","ぢぃ"}, {"dyu","ぢゅ"}, {"dye","ぢぇ"}, {"dyo","ぢょ"},
    {"dha","でゃ"}, {"dhi","でぃ"}, {"dhu","でゅ"}, {"dhe","でぇ"}, {"dho","でょ"},
    {"dwa","どぁ"}, {"dwi","どぃ"}, {"dwu","どぅ"}, {"dwe","どぇ"}, {"dwo","どぉ"},

    // な行
    {"na","な"}, {"ni","に"}, {"nu","ぬ"}, {"ne","ね"}, {"no","の"},
    {"nya","にゃ"}, {"nyi","にぃ"}, {"nyu","にゅ"}, {"nye","にぇ"}, {"nyo","にょ"},

    // は行
    {"ha","は"}, {"hi","ひ"}, {"hu","ふ"}, {"he","へ"}, {"ho","ほ"},
    {"hya","ひゃ"}, {"hyi","ひぃ"}, {"hyu","ひゅ"}, {"hye","ひぇ"}, {"hyo","ひょ"},
    {"fya","ふゃ"}, {"fyu","ふゅ"}, {"fyo","ふょ"},
    {"fwa","ふぁ"}, {"fwi","ふぃ"}, {"fwu","ふぅ"}, {"fwe","ふぇ"}, {"fwo","ふぉ"},
    {"fa","ふぁ"}, {"fi","ふぃ"}, {"fe","ふぇ"}, {"fo","ふぉ"},

    // ば行
    {"ba","ば"}, {"bi","び"}, {"bu","ぶ"}, {"be","べ"}, {"bo","ぼ"},
    {"bya","びゃ"}, {"byi","びぃ"}, {"byu","びゅ"}, {"bye","びぇ"}, {"byo","びょ"},

    // ぱ行
    {"pa","ぱ"}, {"pi","ぴ"}, {"pu","ぷ"}, {"pe","ぺ"}, {"po","ぽ"},
    {"pya","ぴゃ"}, {"pyi","ぴぃ"}, {"pyu","ぴゅ"}, {"pye","ぴぇ"}, {"pyo","ぴょ"},

    // ま行
    {"ma","ま"}, {"mi","み"}, {"mu","む"}, {"me","め"}, {"mo","も"},
    {"mya","みゃ"}, {"myi","みぃ"}, {"myu","みゅ"}, {"mye","みぇ"}, {"myo","みょ"},

    // や行
    {"ya","や"}, {"yu","ゆ"}, {"yo","よ"},
    {"lya","ゃ"}, {"lyu","ゅ"}, {"lyo","ょ"},
    {"xya","ゃ"}, {"xyu","ゅ"}, {"xyo","ょ"},

    // ら行
    {"ra","ら"}, {"ri","り"}, {"ru","る"}, {"re","れ"}, {"ro","ろ"},
    {"rya","りゃ"}, {"ryi","りぃ"}, {"ryu","りゅ"}, {"rye","りぇ"}, {"ryo","りょ"},

    // わ行
    {"wa","わ"}, {"wi","うぃ"}, {"we","うぇ"}, {"wo","を"},
    {"nn","ん"}, {"xn","ん"},

    // ヴ行
    {"va","ゔぁ"}, {"vi","ゔぃ"}, {"vu","ゔ"}, {"ve","ゔぇ"}, {"vo","ゔぉ"},
    {"vya","ゔゃ"}, {"vyi","ゔぃ"}, {"vyu","ゔゅ"}, {"vye","ゔぇ"}, {"vyo","ゔょ"},

    
    };


    void Update()
    {
        HandleAlphabetInput();   // アルファベット入力処理
        UpdateHiragana();        // ひらがな変換処理

        if (Input.anyKeyDown)
        {
            foreach (char c in Input.inputString)
            {
                if (char.IsLetter(c))
                {
                    WallController.Instance.HighlightWallByChar(c);
                }
            }
        }
    }

    // -------------------------------------------------------
    // アルファベット入力処理
    // -------------------------------------------------------
    private void HandleAlphabetInput()
    {
        // A〜Z 入力
        for (KeyCode k = KeyCode.A; k <= KeyCode.Z; k++)
        {
            if (Input.GetKeyDown(k))
            {
                alphabetInputField.text += k.ToString().ToLower();
            }
        }

        // Backspace
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (hiraToRomaHistory.Count > 0)
            {
                string lastRoma = hiraToRomaHistory[hiraToRomaHistory.Count - 1];
                hiraToRomaHistory.RemoveAt(hiraToRomaHistory.Count - 1);

                // ローマ字削除
                if (alphabetInputField.text.Length >= lastRoma.Length)
                {
                    alphabetInputField.text =
                        alphabetInputField.text.Substring(0, alphabetInputField.text.Length - lastRoma.Length);
                }

                // ひらがな削除
                if (hiraInputField.text.Length > 0)
                {
                    hiraInputField.text =
                        hiraInputField.text.Substring(0, hiraInputField.text.Length - 1);
                }

                return;
            }
        }
    }



    // -------------------------------------------------------
    // ローマ字 → ひらがな変換処理（旧 InputController のまま）
    // -------------------------------------------------------
    private void UpdateHiragana()
    {
        string newInput = CurrentWord.ToLower();  // ひらがな変換用に小文字化

        // 前回の構造を維持するためクリアして入れ直す
        currentInput = newInput;
        convertedText = "";

        ConvertInput();

        if (hiraInputField != null)
            hiraInputField.text = convertedText + currentInput;
    }

    private void ConvertInput()
    {
        bool matched = true;

        while (matched && currentInput.Length > 0)
        {
            matched = false;

            // 長いキーから優先してチェック
            foreach (var pair in romaToHira)
            {
                if (currentInput.StartsWith(pair.Key))
                {
                    convertedText += pair.Value;

                    // ★ここ！！ 追加：ひらがな1文字のために使ったローマ字を記録
                    hiraToRomaHistory.Add(pair.Key);

                    currentInput = currentInput.Substring(pair.Key.Length);
                    matched = true;
                    break;
                }
            }


            // 促音判定（kk → っk）
            if (!matched && currentInput.Length >= 2 &&
                currentInput[0] == currentInput[1] &&
                "bcdfghjklmnpqrstvwxyz".Contains(currentInput[0].ToString()))
            {
                convertedText += "っ";
                currentInput = currentInput.Substring(1);
                matched = true;
                continue;
            }

            // n → ん 処理
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

    /// <summary>
    /// WordChecker などが使えるように入力リセット
    /// </summary>
    public void ClearInput()
    {
        alphabetInputField.text = "";
        hiraInputField.text = "";
        currentInput = "";
        convertedText = "";
    }

    // ひらがなの1文字に対応するローマ字を記録するリスト
    private List<string> hiraToRomaHistory = new List<string>();

}
