using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CustomButton))]
    public class CustomButtonEditor : ButtonEditor
    {
        public override void OnInspectorGUI()
        {
            CustomButton customButton = (CustomButton)target;

            customButton.clickClip = (AudioClip)EditorGUILayout.ObjectField("Click Clip", customButton.clickClip, typeof(AudioClip), true);

            base.OnInspectorGUI();
        }
    }
}