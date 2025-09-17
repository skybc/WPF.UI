using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Ui.Input
{
    /// <summary>
    /// 可撤销的命令接口
    /// </summary>
    public interface IUndoableCommand
    {
        /// <summary>执行命令</summary>
        void Execute();

        /// <summary>撤销命令</summary>
        void Undo();
    }

}
