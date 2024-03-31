#if UNITY_EDITOR
using GameMain;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EllipsisText))]
public class EllipsisTextEditor : Editor
{
    public override void OnInspectorGUI()
    {
        EllipsisText script = (EllipsisText)target;

        script.autoLimitLength = EditorGUILayout.Toggle("Auto Limit Length", script.autoLimitLength);

        // 仅在autoLimitLength == true 的时候显示 limitLength
        if (!script.autoLimitLength)
        {
            script.limitLength = EditorGUILayout.IntField("Limit Length", script.limitLength);
        }

        script.tipsTransform = (RectTransform)EditorGUILayout.ObjectField("Tips Transform", script.tipsTransform, typeof(RectTransform), true);
        
        if(GUI.changed)
        {
            EditorUtility.SetDirty(script);
        }
    }
}
#endif