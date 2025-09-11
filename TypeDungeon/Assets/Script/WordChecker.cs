using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Enterキーで表示テキスト（または最後の単語）を取り出し、単語リストにあるかチェックするコンポーネント。
/// 単語リストは TextAsset（1行1単語）か Inspector の配列で与えられる。
/// 判定結果は UnityEvent<string> で受け取れる（引数はチェックした単語）。
/// </summary>
public class WordChecker : MonoBehaviour
{
    public enum CheckMode
    {
        WholeText,  // sourceText.text 全体を単語としてチェック
        LastWord    // sourceText.text の最後の空白区切りトークンのみチェック
    }

    [Header("入力ソース")]
    [Tooltip("判定対象の表示テキスト（例: InputController が表示している TextMeshProUGUI）")]
    public TMP_Text sourceText;

    [Header("単語リスト")]
    [Tooltip("行区切りで単語を入れたテキストファイル（優先して読み込まれる）")]
    public TextAsset wordListFile;

    [Tooltip("小規模な単語リストを直接入れたい時はこちら（wordListFile がない場合に使われます）")]
    public string[] wordList;

    [Header("判定設定")]
    public CheckMode mode = CheckMode.LastWord;
    [Tooltip("大文字小文字を無視して比較する場合は ON")]
    public bool ignoreCase = true;
    [Tooltip("比較時に前後の空白をトリムする")]
    public bool trimWhitespace = true;

    [Header("判定イベント")]
    public UnityEvent<string> onValid;   // 単語として成立している
    public UnityEvent<string> onInvalid; // 単語として成立していない

    // 内部セット（高速検索用）
    private HashSet<string> dictionary = new HashSet<string>();

    void Awake()
    {
        LoadDictionary();
    }

    void Update()
    {
        // Enter キー（Return / KeypadEnter）のどちらかが押されたら判定
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string target = ExtractTargetString();
            if (!string.IsNullOrEmpty(target))
            {
                bool ok = CheckWord(target);
                // ログ（デバッグ用）
                Debug.Log($"WordChecker: \"{target}\" => {(ok ? "Valid" : "Invalid")}");
            }
            else
            {
                Debug.Log("WordChecker: 判定対象が空です（sourceText が未設定か空の可能性）");
            }
        }
    }

    /// <summary>
    /// 辞書を読み込む。TextAsset があればそれを優先、なければ配列 wordList を使う。
    /// </summary>
    public void LoadDictionary()
    {
        dictionary.Clear();

        if (wordListFile != null)
        {
            var lines = wordListFile.text.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var raw in lines)
            {
                var s = raw;
                if (trimWhitespace) s = s.Trim();
                if (ignoreCase) s = s.ToLowerInvariant();
                if (s.Length > 0 && !dictionary.Contains(s)) dictionary.Add(s);
            }
            Debug.Log($"WordChecker: Loaded {dictionary.Count} words from TextAsset '{wordListFile.name}'.");
        }
        else if (wordList != null && wordList.Length > 0)
        {
            foreach (var raw in wordList)
            {
                if (string.IsNullOrEmpty(raw)) continue;
                var s = raw;
                if (trimWhitespace) s = s.Trim();
                if (ignoreCase) s = s.ToLowerInvariant();
                if (s.Length > 0 && !dictionary.Contains(s)) dictionary.Add(s);
            }
            Debug.Log($"WordChecker: Loaded {dictionary.Count} words from inspector array.");
        }
        else
        {
            Debug.LogWarning("WordChecker: No word list provided (wordListFile or wordList). Dictionary is empty.");
        }
    }

    /// <summary>
    /// sourceText から判定対象文字列を取り出す（mode に依存）。
    /// </summary>
    private string ExtractTargetString()
    {
        if (sourceText == null)
        {
            Debug.LogWarning("WordChecker: sourceText が未設定です。");
            return null;
        }

        string text = sourceText.text ?? "";
        if (trimWhitespace) text = text.Trim();

        if (string.IsNullOrEmpty(text)) return null;

        if (mode == CheckMode.WholeText)
        {
            return Normalize(text);
        }
        else // LastWord
        {
            // 空白で区切って最後のトークンを取る
            var tokens = text.Split((char[])null, System.StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return null;
            return Normalize(tokens[tokens.Length - 1]);
        }
    }

    /// <summary>
    /// 文字列を正規化（trim/case等）。必要ならここに全角→半角変換なども追加。
    /// </summary>
    private string Normalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        if (trimWhitespace) s = s.Trim();
        if (ignoreCase) s = s.ToLowerInvariant();
        return s;
    }

    /// <summary>
    /// 公開判定メソッド。指定した単語が辞書にあれば true を返し、onValid を呼ぶ。なければ onInvalid を呼ぶ。
    /// </summary>
    public bool CheckWord(string word)
    {
        if (string.IsNullOrEmpty(word))
        {
            onInvalid?.Invoke(word);
            return false;
        }

        bool exists = dictionary.Contains(Normalize(word));
        if (exists) onValid?.Invoke(word);
        else onInvalid?.Invoke(word);
        return exists;
    }
}
