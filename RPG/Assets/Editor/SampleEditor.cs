using UnityEngine;
using UnityEditor;
using CharacterController = RPG.Controllers.CharacterController;

#if UNITY_EDITOR
[CustomEditor(typeof(Sample))]
public class SampleEditor : Editor
{
    Sample sample;
    CharacterController controller;

    GUILayoutOption[] options = new GUILayoutOption[]
    {
        GUILayout.Height(40.0f),
        GUILayout.ExpandWidth(true)
    };

    GUILayoutOption[] controllerSpecLayoutOptions = new GUILayoutOption[]
    {
        GUILayout.Height(250.0f),
        GUILayout.ExpandWidth(true),
    };

    private void OnEnable()
    {
        sample = (Sample)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        GUIStyle tmpStyle = new GUIStyle(EditorStyles.label);

        
        EditorGUILayout.BeginVertical();
        
            EditorGUILayout.BeginHorizontal(tmpStyle, options);

            if (GUILayout.Button("Set Random Number"))
            {
               sample.Num = Random.Range(0, 100);
            }
            EditorGUILayout.IntField("Number", sample.Num);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal(tmpStyle, options);
            sample.state = (States)EditorGUILayout.EnumPopup("Current State", sample.state);
            EditorGUILayout.EndHorizontal();

        controller = (CharacterController)EditorGUILayout.ObjectField(controller, typeof(CharacterController), controller);

        if (controller != null)
        {
            EditorGUILayout.BeginVertical(EditorStyles.label, controllerSpecLayoutOptions);
            GUILayout.Box(default(Texture));
            EditorGUILayout.EnumPopup(controller.state);
            EditorGUILayout.Vector3Field("Velocity", controller.velocity);

            EditorGUILayout.EndVertical();
        }

        sample.dataVisualization = EditorGUILayout.Toggle(sample.dataVisualization);
        if (sample.dataVisualization)
        {
            EditorGUILayout.LabelField("Data");
            Sample.Data tmpData;
            tmpData.key = EditorGUILayout.TextField(sample.data.key);
            tmpData.value = EditorGUILayout.TextField(sample.data.value);
            sample.data = tmpData;
        }

        EditorGUILayout.EndVertical();

        

        if (EditorGUI.EndChangeCheck())
            serializedObject.ApplyModifiedProperties();
    }
}
#endif