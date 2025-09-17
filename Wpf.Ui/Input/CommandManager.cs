namespace Wpf.Ui.Input
{
    /// <summary>
    ///  命令管理器，支持命令的执行、撤销和重做
    /// </summary>
    public class CommandManager
    {
         
        private readonly Stack<IUndoableCommand> _undoStack = new();
        private readonly Stack<IUndoableCommand> _redoStack = new();
        /// <summary>
        /// zh: 执行一个可撤销的命令，并将其添加到撤销栈中
        /// </summary>
        /// <param name="cmd"></param>
        public void ExecuteCommand(IUndoableCommand cmd)
        {
            cmd.Execute();
            _undoStack.Push(cmd);
            _redoStack.Clear();
        }

        public void Undo()
        {
            if (_undoStack.Any())
            {
                var cmd = _undoStack.Pop();
                cmd.Undo();
                _redoStack.Push(cmd);
            }
        }

        public void Redo()
        {
            if (_redoStack.Any())
            {
                var cmd = _redoStack.Pop();
                cmd.Execute();
                _undoStack.Push(cmd);
            }
        }
    }

}
