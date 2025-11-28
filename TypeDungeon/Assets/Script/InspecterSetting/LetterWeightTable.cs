using UnityEngine;

[CreateAssetMenu(fileName = "LetterWeightTable", menuName = "Game/Letter Weight Table")]
public class LetterWeightTable : ScriptableObject
{
    // AÅ`Z ÇÃ 26 óvëf
    public float[] weights = new float[26];

    public float GetTotalWeight()
    {
        float t = 0f;
        foreach (var w in weights) t += w;
        return t;
    }
}
