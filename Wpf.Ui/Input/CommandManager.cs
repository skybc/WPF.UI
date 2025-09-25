namespace Wpf.Ui.Input
{
    /// <summary>
    ///  命令管理器，支持命令的执行、撤销和重做
    /// </summary>
    public class CommandManager
    {

        private readonly Stack<IUndoableCommand> undoStack = new Stack<IUndoableCommand>();
        private readonly Stack<IUndoableCommand> redoStack = new Stack<IUndoableCommand>();

        /// <summary>
        /// zh: 执行一个可撤销的命令，并将其添加到撤销栈中.
        /// </summary>
        /// <param name="cmd">The command to execute and add to the undo stack.</param>
        public void ExecuteCommand(IUndoableCommand cmd)
        {
            cmd.Execute();
            this.undoStack.Push(cmd);
            this.redoStack.Clear();
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
            }
        }
    }

}


