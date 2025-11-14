using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;

public class WordChecker : MonoBehaviour
{
    public TMP_Text sourceText;
    public InputController inputController;
    public WallController wallController;

    public UnityEvent<string> onValid;
    public UnityEvent<string> onInvalid;

    private HashSet<string> dictionary = new HashSet<string>();

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

            string normalized = word.ToLowerInvariant();
            bool valid = dictionary.Contains(normalized);

            if (valid)
            {
                onValid?.Invoke(word);

                // ★ 入力されているアルファベットすべてについて壁を破壊
                foreach (char c in word)
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
                onInvalid?.Invoke(word);
            }

            // 入力リセット
            inputController.ClearInput();
        }
    }
}
