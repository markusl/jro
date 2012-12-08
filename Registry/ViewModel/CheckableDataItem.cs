using System.ComponentModel;

namespace Registry.ViewModel
{
    /// <summary>
    /// Data item that is toggable in user interface.
    /// </summary>
    public class CheckableDataItem : INotifyPropertyChanged
    {
        private string _displayName;
        private string _dbField;
        private bool _isChecked;

        /// <summary>
        /// Construct new data item.
        /// </summary>
        /// <param name="displayName">Name to display to user</param>
        /// <param name="dbfield">Optional database field this item represents</param>
        /// <param name="initialValue">Initial value of the item</param>
        public CheckableDataItem(string displayName, string dbfield, bool initialValue)
        {
            _displayName = displayName;
            _isChecked = initialValue;
            _dbField = dbfield;
        }

        public bool IsChecked { get { return _isChecked; } set { _isChecked = value; PropertyChanged(this, new PropertyChangedEventArgs("IsChecked")); } }
        public string DisplayName { get { return _displayName; } }
        public string DatabaseField { get { return _dbField; } }

        /// <summary>
        /// Raised when IsChecked changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
