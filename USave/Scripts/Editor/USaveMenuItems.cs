//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: Custom create menu options for USave.
//***************************************************************************************

using Esper.USave.Encryption;
using UnityEditor;
using UnityEngine;

namespace Esper.USave.Editor
{
    public static class USaveMenuItems
    {
        [MenuItem("GameObject/USave/Save Storage")]
        private static void CreateSaveStorage()
        {
            GameObject storage = ObjectFactory.CreateGameObject("SaveStorage", typeof(SaveStorage));

            var saveFile = storage.GetComponent<SaveFileSetup>();
            saveFile.saveFileData = new SaveFileSetupData("GameName_SavePaths", 
                SaveFileSetupData.SaveLocation.DataPath, "GameName_Saves", SaveFileSetupData.FileType.Json, 
                SaveFileSetupData.EncryptionMethod.None, USaveEncryption.GenerateRandomToken(16), 
                USaveEncryption.GenerateRandomToken(16), false);

            if (Selection.activeGameObject)
            {
                storage.transform.SetParent(Selection.activeGameObject.transform);
            }

            Selection.activeGameObject = storage;
        }
    }
}

