using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace BombermanMultiplayer
{
    /// <summary>
    /// Observer that saves game data when notified by GameState
    /// Implements IObserver pattern
    /// </summary>
    public class SaveGameDataObserver : IObserver
    {
        private GameState subject;
        private SaveGameData observerState;
        private bool autoSaveEnabled = false;
        private const string AutoSavePath = "autosave.bmb";

        /// <summary>
        /// Create a new SaveGameDataObserver
        /// </summary>
        /// <param name="subject">The GameState subject to observe</param>
        public SaveGameDataObserver(GameState subject)
        {
            this.subject = subject;
            if (subject != null)
            {
                subject.Attach(this);
            }
        }

        /// <summary>
        /// Enable or disable auto-saving
        /// </summary>
        /// <param name="enabled">True to enable auto-saving, false to disable</param>
        public void SetAutoSaveEnabled(bool enabled)
        {
            this.autoSaveEnabled = enabled;
        }

        /// <summary>
        /// Check if auto-saving is enabled
        /// </summary>
        /// <returns>True if auto-saving is enabled</returns>
        public bool IsAutoSaveEnabled()
        {
            return autoSaveEnabled;
        }

        /// <summary>
        /// Called when the subject notifies observers of a state change
        /// Saves game data if auto-save is enabled
        /// </summary>
        public void Update()
        {
            if (!autoSaveEnabled)
            {
                return;
            }

            observerState = subject.GetState();

            if (observerState.bombsOnTheMap != null && observerState.MapGrid != null && observerState.players != null)
            {
                try
                {
                    SaveGameDataToFile(observerState, AutoSavePath);
                }
                catch (Exception ex)
                {
                    // Silent failure for auto-save - don't interrupt gameplay
                    System.Diagnostics.Debug.WriteLine($"Auto-save failed: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Save game data to a file
        /// </summary>
        /// <param name="saveData">The SaveGameData to save</param>
        /// <param name="filePath">Path to save the file</param>
        private void SaveGameDataToFile(SaveGameData saveData, string filePath)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (FileStream filestream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                formatter.Serialize(filestream, saveData);
            }
        }

        /// <summary>
        /// Detach this observer from the subject
        /// </summary>
        public void Detach()
        {
            if (subject != null)
            {
                subject.Detach(this);
            }
        }
    }
}

