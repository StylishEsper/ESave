//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: April 2024
// Description: A save file contains savable data.
//***************************************************************************************

using Esper.USave.SavableObjects;

#if INSTALLED_NEWTONSOFTJSON
using Newtonsoft.Json;
#endif

using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static Esper.USave.SaveFileSetupData;
using Esper.USave.Encryption;

namespace Esper.USave
{
    public class SaveFile
    {
        private Dictionary<string, SavableObject> saveData = new();
        private SaveFileSetupData saveFileData;

        /// <summary>
        /// The file name.
        /// </summary>
        public string fileName { get => saveFileData.fileName; }

        /// <summary>
        /// The starting file path (either Application.dataPath or Application.persistentDataPath).
        /// </summary>
        public string startPath { get; private set; }

        /// <summary>
        /// The path of the file after start path.
        /// </summary>
        public string filePath { get => saveFileData.filePath; }

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
        /// The save location.
        /// </summary>
        public SaveLocation saveLocation { get => saveFileData.saveLocation; }

        /// <summary>
        /// The data type.
        /// </summary>
        public FileType dataType { get => saveFileData.fileType; }

        /// <summary>
        /// The encryption method.
        /// </summary>
        public EncryptionMethod encryptionMethod { get => saveFileData.encryptionMethod; }

        /// <summary>
        /// If the data is encoded in json format.
        /// </summary>
        public bool isJson { get => dataType == FileType.Json; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="saveFileData">The save file data.</param>
        public SaveFile(SaveFileSetupData saveFileData)
        {
            this.saveFileData = saveFileData;
            SetupFile(saveFileData.fileName, saveFileData.saveLocation, saveFileData.fileType, saveFileData.addToStorage);
        }

        /// <summary>
        /// Sets up the file.
        /// </summary>
        /// <param name="fileName">Save file name.</param>
        /// <param name="saveLocation">Save location.</param>
        /// <param name="fileType">File type.</param>
        /// <param name="addToStorage">If this save file should be added to save storage.</param>
        public void SetupFile(string fileName, SaveLocation saveLocation, FileType fileType, bool addToStorage = true)
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

            switch (fileType)
            {
                case FileType.Json:
                     fileExtension = "json";
                     break;
            }

            Load();

            if (addToStorage && !SaveStorage.instance.ContainsKey(fileName))
            {
                SaveStorage.instance.AddSave(this);
            }
        }

        /// <summary>
        /// Returns the save file data.
        /// </summary>
        /// <returns>The save file data or null if it doesn't exist.</returns>
        public SaveFileSetupData GetSetupData()
        {
            if (saveFileData != null)
            {
                return saveFileData;
            }

            return null;
        }

        /// <summary>
        /// Saves this file in the user's system.
        /// </summary>
        public void Save() 
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
                        File.WriteAllBytes(fullPath, json.AESEncrypt(saveFileData.aesKey.ToBytes(), saveFileData.aesIV.ToBytes()));
                        break;

                   default:
                        File.WriteAllText(fullPath, json);
                        break;
                }
            }

            Debug.Log(fullPath);
#endif
        }

        /// <summary>
        /// Loads data from the appropriate file in the user's system.
        /// </summary>
        public void Load()
        {
#if INSTALLED_NEWTONSOFTJSON
            // Create new file if one doesn't exist
            if (!File.Exists(fullPath))
            {
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                Save();
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
                        json = cipher.AESDecrypt(saveFileData.aesKey.ToBytes(), saveFileData.aesIV.ToBytes());
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
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public T GetData<T>(string id)
        {
            if (!HasData(id))
            {
                Debug.LogWarning($"Save File: data with the ID {id} does not exist.");
                return default;
            }

            return saveData[id].GetValue<T>();
        }

        /// <summary>
        /// Gets Vector3 by ID.
        /// <param name="id">The ID of the data.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Vector3 GetVector3(string id)
        {
            return GetData<float[]>(id).ToVector3();
        }

        /// <summary>
        /// Gets Vector2 by ID.
        /// <param name="id">The ID of the data.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Vector2 GetVector2(string id)
        {
            return GetData<float[]>(id).ToVector2();
        }

        /// <summary>
        /// Gets Quaternion by ID.
        /// <param name="id">The ID of the data.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Quaternion GetQuaternion(string id)
        {
            return GetData<float[]>(id).ToQuaternion();
        }

        /// <summary>
        /// Gets color by ID.
        /// <param name="id">The ID of the data.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public Color GetColor(string id)
        {
            return GetData<float[]>(id).ToColor();
        }

        /// <summary>
        /// Gets a savable transform by ID.
        /// </summary>
        /// <param name="id">The ID of the data.</param>
        /// <returns>The data with the ID or null if it doesn't exist.</returns>
        public SavableTransform GetTransform(string id)
        {
            return GetData<SavableTransform>(id);
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