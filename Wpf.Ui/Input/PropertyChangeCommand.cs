using System.Reflection;

namespace Wpf.Ui.Input
{
    public class PropertyChangeCommand<T> : IUndoableCommand
    {
        private object _target;
        private PropertyInfo _property;
        private T _oldValue;
        private T _newValue;

        public PropertyChangeCommand(object target, PropertyInfo property, T newValue)
        {
            _target = target;
            _property = property;
            _oldValue = (T)property.GetValue(target);
            _newValue = newValue;
        }

        public void Execute() => _property.SetValue(_target, _newValue);
        public void Undo() => _property.SetValue(_target, _oldValue);
    }

}
