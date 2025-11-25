using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(WeightTable))]
public class WeightTableDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 28;   // 26行+ラベル
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // ラベル表示
        position.height = EditorGUIUtility.singleLineHeight;
        EditorGUI.LabelField(position, label);

        // 配列
        SerializedProperty weights = property.FindPropertyRelative("weights");

        // 各文字(A〜Z)の表示
        for (int i = 0; i < 26; i++)
        {
            position.y += EditorGUIUtility.singleLineHeight;

            char letter = (char)('A' + i);
            EditorGUI.PropertyField(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                weights.GetArrayElementAtIndex(i),
                new GUIContent(letter.ToString()) // ★ A〜Z のラベル
            );
        }

        EditorGUI.EndProperty();
    }
}
