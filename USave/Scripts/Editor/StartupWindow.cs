//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: May 2024
// Description: Startup window and Newtonsoft JSON installer.
//***************************************************************************************

using UnityEditor;
using UnityEngine;
using UnityEditor.PackageManager.Requests;
using UnityEditor.PackageManager;

namespace Esper.USave.Editor
{
    [InitializeOnLoad]
    public class StartupWindow : EditorWindow
    {
        private const string alreadyShownKey = "com.stylishesper.usave.startup_window_shown";

        private static StartupWindow window;
        private static AddRequest request;

        static StartupWindow()
        {
            EditorApplication.delayCall += ShowOnStartup;
        }

        private static void ShowOnStartup()
        {
            var isAlreadyShown = SessionState.GetBool(alreadyShownKey, false);
            if (!isAlreadyShown)
            {
                ShowWelcomeWindow();
                SessionState.SetBool(alreadyShownKey, true);
            }

            EditorApplication.delayCall -= ShowOnStartup;
        }

        public static void ShowWelcomeWindow()
        {
#if !INSTALLED_NEWTONSOFTJSON
            window = GetWindow<StartupWindow>();
            window.maxSize = new Vector2(500f, 100f);
            window.minSize = window.maxSize;
            window.titleContent = new GUIContent("Newtonsoft JSON Installer");
#endif
        }

        private void OnGUI()
        {
            GUILayout.Label("Newtonsoft JSON is required for USave.");

            if (GUILayout.Button("Install Newtonsoft JSON"))
            {
                InstallNewtonsoftJSON();
            }
        }

        [MenuItem("Window/USave/Install Newtonsoft JSON")]
        private static void InstallNewtonsoftJSON()
        {
            request = Client.Add("com.unity.nuget.newtonsoft-json");
            EditorApplication.update += Progress;
            window.Close();
        }

        private static void Progress()
        {
            if (request.IsCompleted)
            {
                if (request.Status == StatusCode.Success)
                {
                    Debug.Log("Installed: " + request.Result.packageId);
                }
                else if (request.Status >= StatusCode.Failure)
                {
                    Debug.Log(request.Error.message);
                }

                EditorApplication.update -= Progress;
            }
        }
    }
}