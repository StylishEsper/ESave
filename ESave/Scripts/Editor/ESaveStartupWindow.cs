//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: May 2024
// Description: Startup window and Newtonsoft JSON installer.
//***************************************************************************************

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;

namespace Esper.ESave.Editor
{
    [InitializeOnLoad]
    public class ESaveStartupWindow : EditorWindow
    {
        private const string alreadyShownKey = "com.stylishesper.esave.startup_window_shown";

        private static ESaveStartupWindow window;
        private static AddRequest newtonsoftJsonRequest;

        static ESaveStartupWindow()
        {
            EditorApplication.delayCall += ShowOnStartup;
        }

        private static void ShowOnStartup()
        {
            var isAlreadyShown = SessionState.GetBool(alreadyShownKey, false);
            if (!isAlreadyShown)
            {
                ShowInstallerWindow();
                SessionState.SetBool(alreadyShownKey, true);
            }

            EditorApplication.delayCall -= ShowOnStartup;
        }

        public static void ShowInstallerWindow()
        {
#if !INSTALLED_NEWTONSOFTJSON
            window = GetWindow<ESaveStartupWindow>();
            window.maxSize = new Vector2(500f, 100f);
            window.minSize = window.maxSize;
            window.titleContent = new GUIContent("Newtonsoft JSON Installer");
#endif
        }

        [MenuItem("Window/ESave/Install Newtonsoft JSON")]
        public static void ShowInstallerWindowMenu()
        {
            window = GetWindow<ESaveStartupWindow>();
            window.maxSize = new Vector2(500f, 100f);
            window.minSize = window.maxSize;
            window.titleContent = new GUIContent("Newtonsoft JSON Installer");
        }

        private void OnGUI()
        {
            GUILayout.Label("Newtonsoft JSON is required for ESave.");

            if (GUILayout.Button("Install Newtonsoft JSON"))
            {
                InstallNewtonsoftJSON();
            }
        }

        private static void InstallNewtonsoftJSON()
        {
            newtonsoftJsonRequest = Client.Add("com.unity.nuget.newtonsoft-json");
            EditorApplication.update += Progress;
            window.Close();
        }

        private static void Progress()
        {
            if (newtonsoftJsonRequest.IsCompleted)
            {
                if (newtonsoftJsonRequest.Status == StatusCode.Success)
                {
                    Debug.Log("Installed: " + newtonsoftJsonRequest.Result.packageId);
                }
                else if (newtonsoftJsonRequest.Status >= StatusCode.Failure)
                {
                    Debug.Log(newtonsoftJsonRequest.Error.message);
                }

                EditorApplication.update -= Progress;
            }
        }
    }
}
#endif