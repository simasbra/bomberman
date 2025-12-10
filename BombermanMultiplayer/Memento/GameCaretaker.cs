using System;
using System.Collections.Generic;

namespace BombermanMultiplayer.Memento
{
    /// <summary>
    /// Caretaker - Manages memento history for undo/redo operations.
    /// Does NOT have access to memento internals, only stores opaque references.
    /// </summary>
    public class GameCaretaker
    {
        private readonly Stack<GameMemento> _undoStack = new Stack<GameMemento>();
        private readonly Stack<GameMemento> _redoStack = new Stack<GameMemento>();
        private const int MaxHistorySize = 10; // Limit memory usage

        /// <summary>
        /// Save a new checkpoint. Clears redo history.
        /// </summary>
        public void SaveCheckpoint(GameMemento memento)
        {
            if (memento == null)
                throw new ArgumentNullException(nameof(memento));

            _undoStack.Push(memento);
            _redoStack.Clear(); // New action invalidates redo history

            // Limit history size to prevent memory issues
            if (_undoStack.Count > MaxHistorySize)
            {
                var temp = new Stack<GameMemento>();
                for (int i = 0; i < MaxHistorySize; i++)
                {
                    temp.Push(_undoStack.Pop());
                }
                _undoStack.Clear();
                while (temp.Count > 0)
                {
                    _undoStack.Push(temp.Pop());
                }
            }
        }

        /// <summary>
        /// Undo last action - returns previous memento or null if no history
        /// </summary>
        public GameMemento Undo(GameMemento currentState)
        {
            if (_undoStack.Count == 0)
                return null;

            // Save current state to redo stack
            if (currentState != null)
                _redoStack.Push(currentState);

            return _undoStack.Pop();
        }

        /// <summary>
        /// Redo previously undone action
        /// </summary>
        public GameMemento Redo(GameMemento currentState)
        {
            if (_redoStack.Count == 0)
                return null;

            // Save current state to undo stack
            if (currentState != null)
                _undoStack.Push(currentState);

            return _redoStack.Pop();
        }

        /// <summary>
        /// Check if undo is available
        /// </summary>
        public bool CanUndo => _undoStack.Count > 0;

        /// <summary>
        /// Check if redo is available
        /// </summary>
        public bool CanRedo => _redoStack.Count > 0;

        /// <summary>
        /// Get undo history count
        /// </summary>
        public int UndoCount => _undoStack.Count;

        /// <summary>
        /// Get redo history count
        /// </summary>
        public int RedoCount => _redoStack.Count;

        /// <summary>
        /// Clear all history
        /// </summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
        }

        /// <summary>
        /// Get list of available checkpoints (for UI display)
        /// Caretaker can see timestamp/description but NOT internal state
        /// </summary>
        public List<string> GetCheckpointList()
        {
            var list = new List<string>();
            foreach (var memento in _undoStack)
            {
                list.Add(memento.Description); // Only public readonly property
            }
            return list;
        }
    }
}
