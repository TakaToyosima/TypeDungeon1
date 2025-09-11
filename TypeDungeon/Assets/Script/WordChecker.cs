using UnityEngine;
using UnityEngine.Events;
using TMPro;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Enter�L�[�ŕ\���e�L�X�g�i�܂��͍Ō�̒P��j�����o���A�P�ꃊ�X�g�ɂ��邩�`�F�b�N����R���|�[�l���g�B
/// �P�ꃊ�X�g�� TextAsset�i1�s1�P��j�� Inspector �̔z��ŗ^������B
/// ���茋�ʂ� UnityEvent<string> �Ŏ󂯎���i�����̓`�F�b�N�����P��j�B
/// </summary>
public class WordChecker : MonoBehaviour
{
    public enum CheckMode
    {
        WholeText,  // sourceText.text �S�̂�P��Ƃ��ă`�F�b�N
        LastWord    // sourceText.text �̍Ō�̋󔒋�؂�g�[�N���̂݃`�F�b�N
    }

    [Header("���̓\�[�X")]
    [Tooltip("����Ώۂ̕\���e�L�X�g�i��: InputController ���\�����Ă��� TextMeshProUGUI�j")]
    public TMP_Text sourceText;

    [Header("�P�ꃊ�X�g")]
    [Tooltip("�s��؂�ŒP�����ꂽ�e�L�X�g�t�@�C���i�D�悵�ēǂݍ��܂��j")]
    public TextAsset wordListFile;

    [Tooltip("���K�͂ȒP�ꃊ�X�g�𒼐ړ��ꂽ�����͂�����iwordListFile ���Ȃ��ꍇ�Ɏg���܂��j")]
    public string[] wordList;

    [Header("����ݒ�")]
    public CheckMode mode = CheckMode.LastWord;
    [Tooltip("�啶���������𖳎����Ĕ�r����ꍇ�� ON")]
    public bool ignoreCase = true;
    [Tooltip("��r���ɑO��̋󔒂��g��������")]
    public bool trimWhitespace = true;

    [Header("����C�x���g")]
    public UnityEvent<string> onValid;   // �P��Ƃ��Đ������Ă���
    public UnityEvent<string> onInvalid; // �P��Ƃ��Đ������Ă��Ȃ�

    // �����Z�b�g�i���������p�j
    private HashSet<string> dictionary = new HashSet<string>();

    void Awake()
    {
        LoadDictionary();
    }

    void Update()
    {
        // Enter �L�[�iReturn / KeypadEnter�j�̂ǂ��炩�������ꂽ�画��
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string target = ExtractTargetString();
            if (!string.IsNullOrEmpty(target))
            {
                bool ok = CheckWord(target);
                // ���O�i�f�o�b�O�p�j
                Debug.Log($"WordChecker: \"{target}\" => {(ok ? "Valid" : "Invalid")}");
            }
            else
            {
                Debug.Log("WordChecker: ����Ώۂ���ł��isourceText �����ݒ肩��̉\���j");
            }
        }
    }

    /// <summary>
    /// ������ǂݍ��ށBTextAsset ������΂����D��A�Ȃ���Δz�� wordList ���g���B
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
    /// sourceText ���画��Ώە���������o���imode �Ɉˑ��j�B
    /// </summary>
    private string ExtractTargetString()
    {
        if (sourceText == null)
        {
            Debug.LogWarning("WordChecker: sourceText �����ݒ�ł��B");
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
            // �󔒂ŋ�؂��čŌ�̃g�[�N�������
            var tokens = text.Split((char[])null, System.StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length == 0) return null;
            return Normalize(tokens[tokens.Length - 1]);
        }
    }

    /// <summary>
    /// ������𐳋K���itrim/case���j�B�K�v�Ȃ炱���ɑS�p�����p�ϊ��Ȃǂ��ǉ��B
    /// </summary>
    private string Normalize(string s)
    {
        if (string.IsNullOrEmpty(s)) return s;
        if (trimWhitespace) s = s.Trim();
        if (ignoreCase) s = s.ToLowerInvariant();
        return s;
    }

    /// <summary>
    /// ���J���胁�\�b�h�B�w�肵���P�ꂪ�����ɂ���� true ��Ԃ��AonValid ���ĂԁB�Ȃ���� onInvalid ���ĂԁB
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
