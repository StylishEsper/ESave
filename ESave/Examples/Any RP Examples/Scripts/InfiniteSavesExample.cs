//***************************************************************************************
// Writer: Stylish Esper
// Last Updated: May 2024
// Description: ESave infinite saves example.
//***************************************************************************************

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Esper.ESave.SaveFileSetupData;

namespace Esper.ESave.Example
{
    public class InfiniteSavesExample : MonoBehaviour
    {
        private const string timeElapsedKey = "TimeElapsed";

        [SerializeField]
        private Button saveSlotPrefab;
        [SerializeField]
        private Button saveSlotCreator;
        [SerializeField]
        private Transform content;
        [SerializeField]
        private Text timeElapsedText;
        [SerializeField]
        private Toggle modeToggle;

        private List<Button> slots = new();

        private float timeElapsed;

        private bool loadMode { get => modeToggle.isOn; }

        private void Start()
        {
            // Instantiate slots for existing saves
            foreach (var save in SaveStorage.instance.saves.Values) 
            {
                CreateNewSaveSlot(save);
            }

            // Save slot creator on-click event
            saveSlotCreator.onClick.AddListener(CreateNewSave);
        }

        private void Update()
        {
            // Increment time per frame
            timeElapsed += Time.deltaTime;
            timeElapsedText.text = $"Time Elapsed: {timeElapsed}";
        }

        /// <summary>
        /// Creates a save slot for a save file.
        /// </summary>
        /// <param name="saveFile">The save file.</param>
        public void CreateNewSaveSlot(SaveFile saveFile)
        {
            // Instantiate the save slot
            var slot = Instantiate(saveSlotPrefab, content);
            var slotText = slot.transform.GetChild(0).GetComponent<Text>();
            slotText.text = $"Save Slot {slots.Count}";

            // Move save creator to the bottom
            saveSlotCreator.transform.SetAsLastSibling();

            // Add on-click event for loading
            slot.onClick.AddListener(() => LoadOrOverwriteSave(saveFile));

            slots.Add(slot);
        }

        /// <summary>
        /// Creates a new save.
        /// </summary>
        public void CreateNewSave()
        {
            // Create the save file data
            SaveFileSetupData saveFileSetupData = new()
            {
                fileName = $"InfiniteExampleSave{SaveStorage.instance.saveCount}",
                saveLocation = SaveLocation.DataPath,
                filePath = "StylishEsper/ESave/Examples/Any RP Examples",
                fileType = FileType.Json,
                encryptionMethod = EncryptionMethod.None,
                addToStorage = true
            };

            SaveFile saveFile = new SaveFile(saveFileSetupData);

            // Save the time elapsed
            // Techincally, nothing is being overwitten at this stage since it is an empty save file
            OverwriteSave(saveFile);

            // Create ths save slot for this data
            CreateNewSaveSlot(saveFile);
        }

        /// <summary>
        /// Loads a save.
        /// </summary>
        /// <param name="saveFile">The save file.</param>
        public void LoadSave(SaveFile saveFile)
        {
            timeElapsed = saveFile.GetData<float>(timeElapsedKey);
        }

        /// <summary>
        /// Overwrites a save.
        /// </summary>
        /// <param name="saveFile">The save file.</param>
        public void OverwriteSave(SaveFile saveFile)
        {
            // Save the time elapsed
            saveFile.AddOrUpdateData(timeElapsedKey, timeElapsed);
            saveFile.Save();
        }

        /// <summary>
        /// Loads or overwrites the save based on the active mode.
        /// </summary>
        /// <param name="saveFile">The save file.</param>
        public void LoadOrOverwriteSave(SaveFile saveFile)
        {
            if (loadMode)
            {
                LoadSave(saveFile);
            }
            else
            {
                OverwriteSave(saveFile);
            }
        }
    }
}