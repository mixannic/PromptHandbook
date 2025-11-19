using System;
using System.ComponentModel;

namespace PromptHandbook
{
    public class Folder : INotifyPropertyChanged
    {
        private string _name;
        private string _id;
        private bool _isSelected;

        public string Id
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged(nameof(Name));
                }
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged(nameof(IsSelected));
                    OnPropertyChanged(nameof(IconPath));
                }
            }
        }

        public string IconPath
        {
            get
            {
                return IsSelected ? "folderopen.png" : "folderclosed.png";
            }
        }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public Folder()
        {
            Id = Guid.NewGuid().ToString();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}