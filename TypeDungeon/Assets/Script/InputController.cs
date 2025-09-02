using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class InputController : MonoBehaviour
{
    public TextMeshProUGUI displayText; // �\������UI�i�C���X�y�N�^�[�Ŋ��蓖�āj

    private string currentInput = "";   // ���͒��̕�����i�܂��ϊ�����Ă��Ȃ������j
    private string convertedText = "";  // �m�肵���Ђ炪�ȕ���

    // ���[�}�����Ђ炪�ȑΉ��\
    private Dictionary<string, string> romaToHira = new Dictionary<string, string>()
    {
        // �ꉹ
        {"a","��"},{"i","��"},{"u","��"},{"e","��"},{"o","��"},

        // �s
        {"ka","��"},{"ki","��"},{"ku","��"},{"ke","��"},{"ko","��"},
        {"sa","��"},{"shi","��"},{"su","��"},{"se","��"},{"so","��"},
        {"ta","��"},{"chi","��"},{"tsu","��"},{"te","��"},{"to","��"},
        {"na","��"},{"ni","��"},{"nu","��"},{"ne","��"},{"no","��"},
        {"ha","��"},{"hi","��"},{"fu","��"},{"he","��"},{"ho","��"},
        {"ma","��"},{"mi","��"},{"mu","��"},{"me","��"},{"mo","��"},
        {"ya","��"},{"yu","��"},{"yo","��"},
        {"ra","��"},{"ri","��"},{"ru","��"},{"re","��"},{"ro","��"},
        {"wa","��"},{"wo","��"},{"n","��"},

        // ����
        {"ga","��"},{"gi","��"},{"gu","��"},{"ge","��"},{"go","��"},
        {"za","��"},{"ji","��"},{"zu","��"},{"ze","��"},{"zo","��"},
        {"da","��"},{"de","��"},{"do","��"},
        {"ba","��"},{"bi","��"},{"bu","��"},{"be","��"},{"bo","��"},

        // ������
        {"pa","��"},{"pi","��"},{"pu","��"},{"pe","��"},{"po","��"},

        // �X��
        {"kya","����"},{"kyu","����"},{"kyo","����"},
        {"sha","����"},{"shu","����"},{"sho","����"},
        {"cha","����"},{"chu","����"},{"cho","����"},
        {"nya","�ɂ�"},{"nyu","�ɂ�"},{"nyo","�ɂ�"},
        {"hya","�Ђ�"},{"hyu","�Ђ�"},{"hyo","�Ђ�"},
        {"mya","�݂�"},{"myu","�݂�"},{"myo","�݂�"},
        {"rya","���"},{"ryu","���"},{"ryo","���"},
        {"gya","����"},{"gyu","����"},{"gyo","����"},
        {"ja","����"},{"ju","����"},{"jo","����"},
        {"bya","�т�"},{"byu","�т�"},{"byo","�т�"},
        {"pya","�҂�"},{"pyu","�҂�"},{"pyo","�҂�"},

        // ����������
        {"xa","��"},{"xi","��"},{"xu","��"},{"xe","��"},{"xo","��"},
        {"xya","��"},{"xyu","��"},{"xyo","��"},{"xtu","��"}
    };

    void Update()
    {
        foreach (char c in Input.inputString)
        {
            if (c == '\b') // �o�b�N�X�y�[�X
            {
                if (currentInput.Length > 0)
                    currentInput = currentInput.Substring(0, currentInput.Length - 1);
                else if (convertedText.Length > 0)
                    convertedText = convertedText.Substring(0, convertedText.Length - 1);
            }
            else
            {
                currentInput += c; // ���͒ǉ�
            }

            ConvertInput();
        }

        displayText.text = convertedText + currentInput; // �\���X�V
    }

    void ConvertInput()
    {
        bool matched = true;

        while (matched && currentInput.Length > 0)
        {
            matched = false;

            // �����L�[����D��I�Ƀ`�F�b�N
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

            // �����i��: "kk" �� "��k"�j
            if (!matched && currentInput.Length >= 2 && currentInput[0] == currentInput[1])
            {
                if ("bcdfghjklmnpqrstvwxyz".Contains(currentInput[0].ToString()))
                {
                    convertedText += "��";
                    currentInput = currentInput.Substring(1); // �擪�������Ďc��͕ێ�
                    matched = true;
                }
            }
        }
    }
}
