using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(LetterWeightDrawerAttribute))]
public class LetterWeightDrawer : PropertyDrawer
{
    private readonly string[] labels = new string[26]
    {
        "A","B","C","D","E","F","G","H","I","J","K","L",
        "M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"
    };

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight * 27;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.LabelField(new Rect(position.x, position.y, position.width, 20), "出現確率（A〜Z）");

        if (property.propertyType == SerializedPropertyType.Generic && property.isArray)
        {
            EditorGUI.indentLevel++;

            for (int i = 0; i < 26; i++)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(i);

                Rect r = new Rect(
                    position.x,
                    position.y + (i + 1) * EditorGUIUtility.singleLineHeight,
                    position.width,
                    EditorGUIUtility.singleLineHeight
                );

                EditorGUI.PropertyField(r, element, new GUIContent(labels[i]));
            }

            EditorGUI.indentLevel--;
        }
    }
}

public class LetterWeightDrawerAttribute : PropertyAttribute { }
