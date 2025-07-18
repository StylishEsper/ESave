//***************************************************************************************
// Writer: Stylish Esper
//***************************************************************************************

using Esper.ESave.Encryption;

namespace Esper.ESave
{
    /// <summary>
    /// Data used to set up a save file.
    /// </summary>
    [System.Serializable]
    public class SaveFileSetupData
    {
        public string fileName = "SaveFileName";
        public SaveLocation saveLocation;
        public string filePath = "Example/Path";
        public FileType fileType;
        public EncryptionMethod encryptionMethod;
        public string aesKey;
        public string aesIV;
        public bool addToStorage = true;
        public bool backgroundTask;

        /// <summary>
        /// Constructor.
        /// </summary>
        public SaveFileSetupData() 
        {
            
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="fileName">Save file name.</param>
        /// <param name="saveLocation">Save location.</param>
        /// <param name="filePath">File path after initial data path.</param>
        /// <param name="fileType">Data type.</param>
        /// <param name="encryptionMethod">Encryption method.</param>
        /// <param name="aesKey">AES key.</param>
        /// <param name="aesIV">AES IV.</param>
        /// <param name="addToStorage">If this save file should be added to save storage.</param>
        /// <param name="backgroundTask">If saving and loading data should occur in the background.</param>
        public SaveFileSetupData(string fileName, SaveLocation saveLocation, string filePath, FileType fileType, 
            EncryptionMethod encryptionMethod, string aesKey, string aesIV,  bool addToStorage, bool backgroundTask)
        {
            this.fileName = fileName;
            this.saveLocation = saveLocation;
            this.filePath = filePath;
            this.fileType = fileType;
            this.encryptionMethod = encryptionMethod;
            this.aesKey = aesKey;
            this.aesIV = aesIV;
            this.addToStorage = addToStorage;
            this.backgroundTask = backgroundTask;
        }

        /// <summary>
        /// Generates random tokens for the AES key and IV.
        /// </summary>
        public void GenerateAESTokens()
        {
            aesKey = ESaveEncryption.GenerateKey().ToAesString();
            aesIV = ESaveEncryption.GenerateIV().ToAesString();
        }

        /// <summary>
        /// Save location types.
        /// </summary>
        public enum SaveLocation
        {
            PersistentDataPath,
            DataPath
        }

        /// <summary>
        /// Supported file types.
        /// </summary>
        public enum FileType
        {
            Json
        }

        /// <summary>
        /// Supported encryption methods.
        /// </summary>
        public enum EncryptionMethod
        {
            None,
            AES
        }
    }
}

