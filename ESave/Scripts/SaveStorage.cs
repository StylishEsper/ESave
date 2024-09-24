//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: May 2024
// Description: Manages all saves.
//***************************************************************************************

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Esper.ESave
{
    [RequireComponent(typeof(SaveFileSetup)), DefaultExecutionOrder(-1)]
    public class SaveStorage : MonoBehaviour
    {
        public Dictionary<string, SaveFile> saves = new();
        private SaveFile savePathsFile;

        /// <summary>
        /// Save storage instance.
        /// </summary>
        public static SaveStorage instance { get; private set; }

        /// <summary>
        /// The number of saves.
        /// </summary>
        public int saveCount { get => saves.Count; }

        private void Awake()
        {
            // Singleton
            if (!instance)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                var saveFileSetup = GetComponent<SaveFileSetup>();
                savePathsFile = saveFileSetup.GetSaveFile();

                // Get save files
                var allData = savePathsFile.GetAllDataOfType<SaveFileSetupData>();
                foreach (var data in allData)
                {
                    new SaveFile(data, true);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Adds a save to the save storage list.
        /// </summary>
        /// <param name="saveFile">The save file to add.</param>
        public void AddSave(SaveFile saveFile)
        {
            if (saves.ContainsKey(saveFile.fileName))
            {
                Debug.LogWarning($"Save Storage: a save file with the name {saveFile.fileName} seems to " +
                    $"already exists. Please ensure all file names are unique.");
                return;
            }

            saves.Add(saveFile.fileName, saveFile);

            AddToSavedPaths(saveFile);
        }

        /// <summary>
        /// Removes a save from the save storage list.
        /// </summary>
        /// <param name="saveFile">The save file to remove.</param>
        public void RemoveSave(SaveFile saveFile)
        {
            if (!saves.ContainsKey(saveFile.fileName))
            {
                Debug.LogWarning($"Save Storage: a save file with the name {saveFile.fileName} does not exist.");
                return;
            }

            saves.Remove(saveFile.fileName);

            RemoveFromSavedPaths(saveFile);
        }

        /// <summary>
        /// Adds a save file's path to a file containing a list of all save file paths.
        /// </summary>
        /// <param name="saveFile">The save file.</param>
        public void AddToSavedPaths(SaveFile saveFile)
        {
            if (!savePathsFile.HasData(saveFile.fileName))
            {
                savePathsFile.AddOrUpdateData(saveFile.fileName, saveFile.GetSetupData());
                savePathsFile.Save();
            }
        }

        /// <summary>
        /// Removes a save file's path from a file containing a list of all save file paths.
        /// </summary>
        /// <param name="saveFile">The save file.</param>
        public void RemoveFromSavedPaths(SaveFile saveFile)
        {
            if (savePathsFile.HasData(saveFile.fileName))
            {
                savePathsFile.DeleteData(saveFile.fileName);
                savePathsFile.Save();
            }
        }

        /// <summary>
        /// Checks if a save file with a provided key (file name) exists in storage.
        /// </summary>
        /// <param name="key">The key, which is the name of the save file.</param>
        /// <returns>True if the key exists. Otherwise, false.</returns>
        public bool ContainsKey(string key)
        {
            return saves.ContainsKey(key);
        }

        /// <summary>
        /// Checks if a save file exists in storage.
        /// </summary>
        /// <param name="saveFile">The save file.</param>
        /// <returns>True if the file exists in storage. Otherwise, false.</returns>
        public bool ContainsFile(SaveFile saveFile)
        {
            return saves.ContainsValue(saveFile);
        }

        /// <summary>
        /// Gets a save file by name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>The save file or null if none with the provided name exist.</returns>
        public SaveFile GetSaveByFileName(string fileName)
        {
            if (!ContainsKey(fileName))
            {
                Debug.LogWarning($"Save Storage: a save file with the name {fileName} does not exist.");
                return null;
            }

            return saves[fileName];
        }

        /// <summary>
        /// Gets a save file at a specific index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The save file or null if the index is out of range.</returns>
        public SaveFile GetSaveAtIndex(int index)
        {
            if (index < 0 || index >= saves.Count)
            {
                Debug.LogWarning($"Save Storage: index out of range.");
                return null;
            }

            return saves.Values.ElementAt(index);
        }
    }
}