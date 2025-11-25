using UnityEngine;

[System.Serializable]
public class WeightTable
{
    [Range(0f, 1f)]
    public float[] weights = new float[26];
}
