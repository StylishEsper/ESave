//***************************************************************************************
// Writer: Stylish Esper
//***************************************************************************************

using Esper.ESave.SavableObjects;

#if INSTALLED_NEWTONSOFTJSON
using Newtonsoft.Json;
#endif

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Esper.ESave.SaveFileSetupData;
using Esper.ESave.Encryption;
using Esper.ESave.Threading;

namespace Esper.ESave
{
    /// <summary>
    /// A save file contains savable data.
    /// </summary>
    public class SaveFile
    {
        private Dictionary<string, SavableObject> saveData = new();
        private SaveFileSetupData saveFileSetupData;

        /// <summary>
        /// The save location.
        /// </summary>
        public SaveLocation saveLocation { get => saveFileSetupData.saveLocation; }

        /// <summary>
        /// The data type.
        /// </summary>
        public FileType dataType { get => saveFileSetupData.fileType; }

        /// <summary>
        /// The encryption method.
        /// </summary>
        public EncryptionMethod encryptionMethod { get => saveFileSetupData.encryptionMethod; }

        /// <summary>
        /// The active save file operation.
        /// </summary>
        public SaveFileOperation operation { get => saveOperation != null ? saveOperation : loadOperation; }

        private SaveFileOperation saveOperation;

        private SaveFileOperation loadOperation;

        /// <summary>
        /// The file name.
        /// </summary>
        public string fileName { get => saveFileSetupData.fileName; }

        /// <summary>
        /// The starting file path (either Application.dataPath or Application.persistentDataPath).
        /// </summary>
        public string startPath { get; private set; }

        /// <summary>
        /// The path of the file after start path.
        /// </summary>
        public string filePath { get => saveFileSetupData.filePath; }

        /// <summary>
        /// The file extension.
        /// </summary>
        public string fileExtension { get; private set; }

        /// <summary>
        /// The directory of the file.
        /// </summary>
        public string directory { get => $"{startPath}/{(filePath == string.Empty ? string.Empty : filePath + "/")}"; }

        /// <summary>
        /// The full path to the file.
        /// </summary>
        public string fullPath { get => $"{directory}{fileName}.{fileExtension}"; }

        /// <summary>
        /// If the data should be saved and loaded in the background.
        /// </summary>
        public bool backgroundTask { get => saveFileSetupData.backgroundTask; set => saveFileSetupData.backgroundTask = value; }

        /// <summary>
        /// If the data is encoded in json format.
        /// </summary>
        public bool isJson { get => dataType == FileType.Json; }

        /// <summary>
        /// This will return true if a saving or loading operation is currently ongoing. Otherwise, it will return false.
        /// </summary>
        public bool isOperationOngoing { get => operation != null && operation.state == SaveFileOperation.OperationState.Ongoing; }

        /// <summary>
        /// If the save file has any data.
        /// </summary>
        public bool HasAnyData { get => saveData != null && saveData.Count > 0; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="saveFileSetupData">The save file data.</param>
        /// <param name="shouldExist">If this save file should already exist in the user's system. Default: true</param>
        public SaveFile(SaveFileSetupData saveFileSetupData, bool shouldExist = false)
        {
            this.saveFileSetupData = saveFileSetupData;
            SetupFile(shouldExist);
        }

        /// <summary>
        /// Sets up the file.
        /// </summary>
        /// <param name="shouldExist">If this save file should already exist in the user's system. Default: true</param>
        /// <returns>The load operation or null if the file should exist but it doesn't anymore.</returns>
        private SaveFileOperation SetupFile(bool shouldExist = true)
        {
            switch (saveLocation)
            {
                case SaveLocation.PersistentDataPath:
                     startPath = Application.persistentDataPath;
                     break;
                case SaveLocation.DataPath:
                     startPath = Application.dataPath;
                     break;
            }

            switch (dataType)
            {
                case FileType.Json:
                     fileExtension = "json";
                     break;
            }

            if (saveFileSetupData.addToStorage && SaveStorage.instance && !SaveStorage.instance.ContainsKey(fileName))
            {
                // Ensure the file still exists and if not, remove the path from storage
                if (!File.Exists(fullPath) && shouldExist)
                {
                    SaveStorage.instance.RemoveFromSavedPaths(this);
                    return null;
                }
                else
                {
                    SaveStorage.instance.AddSave(this);
                }
            }

            return Load();
        }

        /// <summary>
        /// Returns the save file data.
        /// </summary>
        /// <returns>The save file data or null if it doesn't exist.</returns>
        public SaveFileSetupData GetSetupData()
        {
            if (saveFileSetupData != null)
            {
                return saveFileSetupData;
            }

            return null;
        }

        /// <summary>
        /// Sets the setup data.
        /// </summary>
        /// <param name="saveFileSetupData">The save file setup data.</param>
        public void SetSetupData(SaveFileSetupData saveFileSetupData)
        {
            this.saveFileSetupData = saveFileSetupData;
            SetupFile();
        }

        /// <summary>
        /// Saves this file in the user's system. The file will be saved in the background
        /// if backgroundTask is true.
        /// </summary>
        /// <param name="ignoreExistingOperation">If this is true, an existing operation will not prevent this
        /// one from running. Default: false.</param>
        /// <returns>The save operation or null if there is an ongoing operation.</returns>
        public SaveFileOperation Save(bool ignoreExistingOperation = false)
        {
#if INSTALLED_NEWTONSOFTJSON
            if (!ignoreExistingOperation && isOperationOngoing)
            {
                return null;
            }

            saveOperation = new SaveFileOperation(SaveData, backgroundTask);
            saveOperation.Start();
            return saveOperation;
#else
            return null;
#endif
        }

        /// <summary>
        /// Loads data from the appropriate file in the user's system. The file will be loaded
        /// in the background if backgroundTask is true.
        /// </summary>
        /// <param name="ignoreExistingOperation">If this is true, an existing operation will not prevent this
        /// one from running. Default: false.</param>
        /// <returns>The load operation or null if there is an ongoing operation.</returns>
        public SaveFileOperation Load(bool ignoreExistingOperation = false)
        {
#if INSTALLED_NEWTONSOFTJSON
            if (!ignoreExistingOperation && isOperationOngoing)
            {
                return null;
            }

            loadOperation = new SaveFileOperation(LoadData, backgroundTask);
            loadOperation.Start();
            return loadOperation;
#else
            return null;
#endif
        }

        /// <summary>
        /// Saves this file in the user's system.
        /// </summary>
        private void SaveData()
        {
#if INSTALLED_NEWTONSOFTJSON
            if (isJson)
            {
                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                };

                string json = JsonConvert.SerializeObject(saveData, Formatting.Indented, jsonSerializerSettings);

                // Encryption method
                switch (encryptionMethod)
                {
                    case EncryptionMethod.None:
                        File.WriteAllText(fullPath, json);
                        break;

                    case EncryptionMethod.AES:
                        File.WriteAllBytes(fullPath, json.AESEncrypt(saveFileSetupData.aesKey.ToAesBytes(), saveFileSetupData.aesIV.ToAesBytes()));
                        break;

                    default:
                        File.WriteAllText(fullPath, json);
                        break;
                }
            }

            Debug.Log($"Save Path: {fullPath}");
#endif
        }

        /// <summary>
        /// Loads data from the appropriate file in the user's system.
        /// </summary>
        private void LoadData()
        {
#if INSTALLED_NEWTONSOFTJSON
            // Create new file if one doesn't exist
            if (!File.Exists(fullPath))
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                Save(true);
            }

            if (isJson)
            {
                string json;

                // Decrypt
                switch (encryptionMethod)
                {
                    case EncryptionMethod.None:
                        json = File.ReadAllText(fullPath);
                        break;

                    case EncryptionMethod.AES:
                        var cipher = File.ReadAllBytes(fullPath);
                        json = cipher.AESDecrypt(saveFileSetupData.aesKey.ToAesBytes(), saveFileSetupData.aesIV.ToAesBytes());
                        break;

                    default:
                        json = File.ReadAllText(fullPath);
                        break;
                }

                var jsonSerializerSettings = new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    TypeNameAssemblyFormatHandling = TypeNameAssemblyFormatHandling.Full
                };

                saveData = JsonConvert.DeserializeObject<Dictionary<string, SavableObject>>(json, jsonSerializerSettings);

                if (saveData == null)
                {
                    saveData = new();
                }
            }
#endif
        }

        /// <summary>
        /// Permanently deletes the save file from the user's system. Use with caution, as this cannot be undone.
        /// </summary>
        public void DeleteFile()
        {
            // Check if the file exists in the path
            if (File.Exists(fullPath))
            {
                // Remove from the save storage if it exists in it
                if (SaveStorage.instance && SaveStorage.instance.ContainsFile(this))
                {
                    SaveStorage.instance.RemoveSave(this);
                }

                saveData.Clear();

                File.Delete(fullPath);
            }
        }

        /// <summary>
        /// Completes removes all data from the file. Use with caution, as this cannot be undone.
        /// </summary>
        public void EmptyFile()
        {
            saveData.Clear();
            Save(true);
        }

        /// <summary>
        /// Removes data by ID.
        /// </summary>
        /// <param name="id">The savable object ID.</param>
        public void DeleteData(string id)
        {
            if (!HasData(id))
            {
                Debug.LogWarning($"Save File: data with the ID {id} does not exist.");
                return;
            }

            saveData.Remove(id);
        }

        /// <summary>
        /// Adds to or updates save data by ID.
        /// </summary>
        /// <typeparam name="T">The data type.</typeparam>
        /// <param name="id">The ID of the data.</param>
        /// <param name="value">The value of the data.</param>
        public void AddOrUpdateData<T>(string id, T value)
        {
            if (HasData(id))
            {
                saveData[id].SetValue(value);
            }
            else
            {
                saveData.Add(id, new SavableData<T>(id, value));
            }
        }

        /// <summary>
        /// Adds to or updates save data by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        /// <param name="value">The value of the data.</param>
        public void AddOrUpdateData(string id, Vector4 value)
        {
            AddOrUpdateData(id, value.ToFloat4());
        }

        /// <summary>
        /// Adds to or updates save data by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        /// <param name="value">The value of the data.</param>
        public void AddOrUpdateData(string id, Vector3 value)
        {
            AddOrUpdateData(id, value.ToFloat3());
        }

        /// <summary>
        /// Adds to or updates save data by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        /// <param name="value">The value of the data.</param>
        public void AddOrUpdateData(string id, Vector2 value)
        {
            AddOrUpdateData(id, value.ToFloat2());
        }

        /// <summary>
        /// Adds to or updates save data by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        /// <param name="value">The value of the data.</param>
        public void AddOrUpdateData(string id, Quaternion value)
        {
            AddOrUpdateData(id, value.ToFloat4());
        }

        /// <summary>
        /// Adds to or updates save data by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        /// <param name="value">The value of the data.</param>
        public void AddOrUpdateData(string id, Color value)
        {
            AddOrUpdateData(id, value.ToFloat4());
        }

        /// <summary>
        /// Adds to or updates save data by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        /// <param name="value">The value of the data.</param>
        public void AddOrUpdateData(string id, Transform value)
        {
            AddOrUpdateData(id, new SavableTransform(value));
        }

        /// <summary>
        /// Gets data by ID.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="id">The ID of the data.</param>
        /// <param name="defaultValue">A default value to fallback to in case the data doesn't exist.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public T GetData<T>(string id, T defaultValue = default)
        {
            if (!HasData(id))
            {
                Debug.LogWarning($"Save File: data with the ID {id} does not exist.");
                return defaultValue;
            }

            return saveData[id].GetValue<T>();
        }

        /// <summary>
        /// Gets Vector3 by ID.
        /// <param name="id">The ID of the data.</param>
        /// <param name="defaultValue">A default value to fallback to in case the data doesn't exist.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Vector3 GetVector4(string id, Vector4 defaultValue = default)
        {
            var result = GetData<float[]>(id);

            if (result == null || result.Length == 0)
            {
                return defaultValue;
            }

            return result.ToVector4();
        }

        /// <summary>
        /// Gets Vector3 by ID.
        /// <param name="id">The ID of the data.</param>
        /// <param name="defaultValue">A default value to fallback to in case the data doesn't exist.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Vector3 GetVector3(string id, Vector3 defaultValue = default)
        {
            var result = GetData<float[]>(id);

            if (result == null || result.Length == 0)
            {
                return defaultValue;
            }

            return result.ToVector3();
        }

        /// <summary>
        /// Gets Vector2 by ID.
        /// <param name="id">The ID of the data.</param>
        /// <param name="defaultValue">A default value to fallback to in case the data doesn't exist.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Vector2 GetVector2(string id, Vector2 defaultValue = default)
        {
            var result = GetData<float[]>(id);

            if (result == null || result.Length == 0)
            {
                return defaultValue;
            }

            return result.ToVector2();
        }

        /// <summary>
        /// Gets Quaternion by ID.
        /// <param name="id">The ID of the data.</param>
        /// <param name="defaultValue">A default value to fallback to in case the data doesn't exist.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Quaternion GetQuaternion(string id, Quaternion defaultValue = default)
        {
            var result = GetData<float[]>(id);

            if (result == null || result.Length == 0)
            {
                return defaultValue;
            }

            return result.ToQuaternion();
        }

        /// <summary>
        /// Gets color by ID.
        /// <param name="id">The ID of the data.</param>
        /// <param name="defaultValue">A default value to fallback to in case the data doesn't exist.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Color GetColor(string id, Color defaultValue = default)
        {
            var result = GetData<float[]>(id);

            if (result == null || result.Length == 0)
            {
                return defaultValue;
            }

            return result.ToColor();
        }

        /// <summary>
        /// Gets a savable transform by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        /// <param name="defaultValue">A default value to fallback to in case the data doesn't exist.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public SavableTransform GetTransform(string id, SavableTransform defaultValue = default)
        {
            var result = GetData<SavableTransform>(id);

            if (result == null)
            {
                return defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Gets a list of data of the same type by IDs.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <param name="ids">List of data IDs.</param>
        /// <returns>A list of relevant data. The list may be empty if no data has a matching ID.</returns>
        public List<T> GetData<T>(params string[] ids)
        {
            var datas = new List<T>();

            foreach (var id in ids)
            {
                try
                {
                    var value = saveData[id].GetValue<T>();

                    if (value != null)
                    {
                        datas.Add(value);
                    }
                }
                catch
                {
                    Debug.Log("Save File: Data mismatch. This can be ignored.");
                }
            }

            return datas;
        }

        /// <summary>
        /// Gets a list of all data of the same type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>A list of relevant data. The list may be empty if no data has a matching type.</returns>
        public List<T> GetAllDataOfType<T>()
        {
            var datas = new List<T>();

            foreach (var data in saveData.Values)
            {
                try 
                {
                    var value = data.GetValue<T>();

                    if (value != null)
                    {
                        datas.Add(value);
                    }
                }
                catch 
                {
                    Debug.Log("Save File: Data mismatch. This can be ignored.");
                }
            }

            return datas;
        }

        /// <summary>
        /// Gets the first data of a type.
        /// </summary>
        /// <typeparam name="T">The type.</typeparam>
        /// <returns>The first data of type T if it exists. Otherwise, null.</returns>
        public T GetFirstDataByType<T>()
        {
            foreach (var data in saveData.Values)
            {
                try
                {
                    return data.GetValue<T>();
                }
                catch
                {
                    Debug.Log("Save File: Data mismatch. This can be ignored.");
                }
            }

            return default;
        }

        /// <summary>
        /// Checks if this file contains a savable object with an ID.
        /// </summary>
        /// <param name="id">The savable object ID.</param>
        /// <returns>True if data with an ID exists. Otherwise, false.</returns>
        public bool HasData(string id)
        {
            return saveData.ContainsKey(id);
        }
    }
}