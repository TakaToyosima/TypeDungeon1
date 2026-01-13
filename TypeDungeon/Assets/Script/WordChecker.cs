using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

public class WordChecker : MonoBehaviour
{
    [Header("UI・コントローラー参照")]
    public TMP_Text sourceText;           // HiraInput（ひらがな）をアタッチ
    public InputController inputController;
    public WallController wallController;

    [Header("辞書設定")]
    public TextAsset dictionaryFile;

    [Header("イベント設定")]
    public UnityEvent<string> onValid;
    public UnityEvent<string> onInvalid;

    private HashSet<string> dictionary = new HashSet<string>();

    void Awake()
    {
        LoadDictionary();
    }

    private void LoadDictionary()
    {
        if (dictionaryFile == null)
        {
            Debug.LogError("WordChecker: 辞書ファイル(TextAsset)がセットされていません。");
            return;
        }

        string[] lines = dictionaryFile.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        dictionary.Clear();
        foreach (string line in lines)
        {
            string word = line.Trim();
            if (!string.IsNullOrEmpty(word)) dictionary.Add(word);
        }

        // 辞書読み込み結果のログを追加
        Debug.Log($"<color=cyan>【辞書読み込み】 {dictionary.Count} 単語の登録が完了しました。</color>");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            string word = sourceText.text.Trim();

            if (word.Length == 0)
            {
                inputController.ClearInput();
                return;
            }

            // 辞書に存在するか判定
            if (dictionary.Contains(word))
            {
                // 成功時のデバッグログを追加
                Debug.Log($"<color=green>【判定成功】 辞書に一致しました: {word}</color>");

                onValid?.Invoke(word);

                // 入力されたアルファベット（ローマ字）を取得して壁を破壊
                string targetLetters = inputController.alphabetInputField.text;
                foreach (char c in targetLetters)
                {
                    if (char.IsLetter(c))
                    {
                        char letter = char.ToUpper(c);
                        wallController.DestroyWallByLetter(letter);
                    }
                }
            }
            else
            {
                // 失敗時のデバッグログ
                Debug.Log($"<color=red>【判定失敗】 辞書に存在しません: {word}</color>");
                onInvalid?.Invoke(word);
            }

            // 入力リセット
            inputController.ClearInput();
        }
    }
}