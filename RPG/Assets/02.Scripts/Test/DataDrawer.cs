using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Sample.Data))]
public class DataDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        base.OnGUI(position, property, label);
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, label);

        var keyRect = new Rect(position.x, position.y, 40, position.height);
        var valueRect = new Rect(position.x + 40, position.y, 40, position.height);

        EditorGUI.PropertyField(keyRect, property.FindPropertyRelative("key"), GUIContent.none);
        EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);

        EditorGUI.EndProperty();
    }
}