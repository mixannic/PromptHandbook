using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace PromptHandbook
{
    public class PromptItem : INotifyPropertyChanged
    {
        private string _name;
        private string _description;
        private string _imagePath;
        private string _folderId;

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

        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                if (_imagePath != value)
                {
                    _imagePath = value;
                    OnPropertyChanged(nameof(ImagePath));
                }
            }
        }

        public string FolderId
        {
            get => _folderId;
            set
            {
                if (_folderId != value)
                {
                    _folderId = value;
                    OnPropertyChanged(nameof(FolderId));
                }
            }
        }

        public string FolderName { get; set; }
        public DateTime CreatedDate { get; set; }

        public ObservableCollection<string> Tags { get; set; }

        public PromptItem()
        {
            Tags = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}