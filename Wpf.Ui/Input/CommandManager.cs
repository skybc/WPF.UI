// <copyright>
//   Copyright (c) 2025 skybc.
//   All rights reserved.
// </copyright>
// <license>
//   Licensed under the project license.
// </license>
namespace Wpf.Ui.Input
{
    /// <summary>
    /// Command manager supporting execute, undo and redo.
    /// </summary>
    public class CommandManager
    {
        private readonly Stack<IUndoableCommand> undoStack = new Stack<IUndoableCommand>();
        private readonly Stack<IUndoableCommand> redoStack = new Stack<IUndoableCommand>();

        /// <summary>
        /// Occurs when undo/redo stacks change to refresh UI state.
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Gets a value indicating whether an undo operation is available.
        /// </summary>
        public bool CanUndo => this.undoStack.Any();

        /// <summary>
        /// Gets a value indicating whether a redo operation is available.
        /// </summary>
        public bool CanRedo => this.redoStack.Any();

        /// <summary>
        /// zh: 执行一个可撤销的命令，并将其添加到撤销栈中.
        /// </summary>
        /// <param name="cmd">The command to execute and add to the undo stack.</param>
        public void ExecuteCommand(IUndoableCommand cmd)
        {
            cmd.Execute();
            this.undoStack.Push(cmd);
            this.redoStack.Clear();
            this.Changed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// 撤销上一个命令.
        /// </summary>
        public void Undo()
        {
            if (this.undoStack.Any())
            {
                var cmd = this.undoStack.Pop();
                cmd.Undo();
                this.redoStack.Push(cmd);
                this.Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// 重做上一个被撤销的命令.
        /// </summary>
        public void Redo()
        {
            if (this.redoStack.Any())
            {
                var cmd = this.redoStack.Pop();
                cmd.Execute();
                this.undoStack.Push(cmd);
                this.Changed?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Clears the undo/redo history.
        /// </summary>
        public void Clear()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
            this.Changed?.Invoke(this, EventArgs.Empty);
        }
    }
}




