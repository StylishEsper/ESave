//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: Save file setup custom editor.
//***************************************************************************************

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Esper.ESave.Editor
{
    [CustomEditor(typeof(SaveFileSetup))]
    public class SaveFileSetupEditor : UnityEditor.Editor
    {
        private SerializedProperty saveFileData;

        private void OnEnable()
        {
            saveFileData = serializedObject.FindProperty("saveFileData");
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(saveFileData);
            EditorGUILayout.Space(5);

            var target = (SaveFileSetup)this.target;

            if (target.saveFileData.encryptionMethod == SaveFileSetupData.EncryptionMethod.AES)
            {
                if (GUILayout.Button("Generate AES Tokens"))
                {
                    target.GenerateAESTokens();
                }
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif