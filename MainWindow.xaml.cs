using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace PromptHandbook
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<PromptItem> _prompts;
        private ObservableCollection<PromptItem> _filteredPrompts;
        private AppSettings _settings;
        private PromptItem _currentPrompt;
        private bool _isSearchPlaceholder = true;

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            InitializeData();
            ApplyFontSettings();

            // Инициализация placeholder для поиска
            SearchTextBox.GotFocus += SearchTextBox_GotFocus;
            SearchTextBox.LostFocus += SearchTextBox_LostFocus;
            SetSearchPlaceholder();
        }

        private void LoadSettings()
        {
            _settings = DataService.LoadSettings();
        }

        private void SaveSettings()
        {
            DataService.SaveSettings(_settings);
        }

        private void ApplyFontSettings()
        {
            this.FontFamily = new FontFamily(_settings.FontFamily);
            this.FontSize = _settings.FontSize;
        }

        private void InitializeData()
        {
            DataService.EnsureDatabaseExists();

            _prompts = new ObservableCollection<PromptItem>(DataService.LoadAllPrompts());
            _filteredPrompts = new ObservableCollection<PromptItem>(_prompts);
            PromptsListBox.ItemsSource = _filteredPrompts;

            if (_filteredPrompts.Count > 0)
            {
                PromptsListBox.SelectedIndex = 0;
            }
        }

        private void SetSearchPlaceholder()
        {
            _isSearchPlaceholder = true;
            SearchTextBox.Text = "Search...";
            SearchTextBox.Foreground = Brushes.Gray;
            ApplyFilter(""); // Показываем все элементы при placeholder
        }

        private void ClearSearchPlaceholder()
        {
            _isSearchPlaceholder = false;
            SearchTextBox.Foreground = Brushes.Black;
        }

        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (_isSearchPlaceholder)
            {
                SearchTextBox.Text = "";
                ClearSearchPlaceholder();
            }
        }

        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(SearchTextBox.Text) && !_isSearchPlaceholder)
            {
                SetSearchPlaceholder();
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (_isSearchPlaceholder)
                return;

            var searchText = SearchTextBox.Text.ToLower();
            ApplyFilter(searchText);
        }

        private void ApplyFilter(string searchText)
        {
            _filteredPrompts.Clear();

            var filtered = string.IsNullOrEmpty(searchText)
                ? _prompts
                : _prompts.Where(p => p.Name.ToLower().Contains(searchText));

            foreach (var item in filtered)
            {
                _filteredPrompts.Add(item);
            }

            // Автоматически выбираем первый элемент после фильтрации
            if (_filteredPrompts.Count > 0 && PromptsListBox.SelectedItem == null)
            {
                PromptsListBox.SelectedIndex = 0;
            }
            else if (_filteredPrompts.Count == 0)
            {
                ClearDetailView();
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var newPrompt = DataService.CreateNewPrompt();
            _prompts.Insert(0, newPrompt);

            // Обновляем фильтрованный список
            if (_isSearchPlaceholder || string.IsNullOrEmpty(SearchTextBox.Text))
            {
                _filteredPrompts.Insert(0, newPrompt);
            }
            else
            {
                // Проверяем соответствует ли новый элемент фильтру
                var searchText = SearchTextBox.Text.ToLower();
                if (newPrompt.Name.ToLower().Contains(searchText))
                {
                    _filteredPrompts.Insert(0, newPrompt);
                }
            }

            PromptsListBox.SelectedItem = newPrompt;
            UpdateDetailView(newPrompt);
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (PromptsListBox.SelectedItem is PromptItem selectedPrompt)
            {
                var result = MessageBox.Show($"Delete prompt '{selectedPrompt.Name}'?",
                    "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DataService.DeletePrompt(selectedPrompt);
                    _prompts.Remove(selectedPrompt);
                    _filteredPrompts.Remove(selectedPrompt);

                    if (_filteredPrompts.Count > 0)
                    {
                        PromptsListBox.SelectedIndex = 0;
                    }
                    else
                    {
                        ClearDetailView();
                    }
                }
            }
        }

        private void LoadImageButton_Click(object sender, RoutedEventArgs e)
        {
            if (PromptsListBox.SelectedItem is PromptItem selectedPrompt)
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.png;*.jpg;*.jpeg)|*.png;*.jpg;*.jpeg",
                    Title = "Select prompt image"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    try
                    {
                        selectedPrompt.ImagePath = openFileDialog.FileName;
                        DataService.SavePrompt(selectedPrompt);
                        LoadImage(openFileDialog.FileName);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading image: {ex.Message}");
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a prompt first.");
            }
        }

        private void NameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_currentPrompt != null && !string.IsNullOrEmpty(NameTextBox.Text))
            {
                _currentPrompt.Name = NameTextBox.Text;
                DataService.SavePrompt(_currentPrompt);

                // Обновляем отображение в списке
                var index = _prompts.IndexOf(_currentPrompt);
                if (index >= 0)
                {
                    _prompts[index] = _currentPrompt;
                }

                // Обновляем фильтрованный список если нужно
                if (!_isSearchPlaceholder && !string.IsNullOrEmpty(SearchTextBox.Text))
                {
                    var searchText = SearchTextBox.Text.ToLower();
                    var shouldBeVisible = _currentPrompt.Name.ToLower().Contains(searchText);
                    var isCurrentlyVisible = _filteredPrompts.Contains(_currentPrompt);

                    if (shouldBeVisible && !isCurrentlyVisible)
                    {
                        _filteredPrompts.Add(_currentPrompt);
                    }
                    else if (!shouldBeVisible && isCurrentlyVisible)
                    {
                        _filteredPrompts.Remove(_currentPrompt);
                        if (_filteredPrompts.Count > 0)
                        {
                            PromptsListBox.SelectedIndex = 0;
                        }
                    }
                }
            }
        }

        private void DescriptionTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (_currentPrompt != null)
            {
                _currentPrompt.Description = DescriptionTextBox.Text;
                DataService.SavePrompt(_currentPrompt);
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow(_settings);
            if (settingsWindow.ShowDialog() == true)
            {
                SaveSettings();
                ApplyFontSettings();
            }
        }

        private void PromptsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PromptsListBox.SelectedItem is PromptItem selectedPrompt)
            {
                UpdateDetailView(selectedPrompt);
                LoadImageButton.Visibility = Visibility.Visible;
            }
            else
            {
                ClearDetailView();
                LoadImageButton.Visibility = Visibility.Collapsed;
            }
        }

        private void UpdateDetailView(PromptItem prompt)
        {
            _currentPrompt = prompt;

            // Сохраняем фокус чтобы избежать рекурсивных обновлений
            NameTextBox.Text = prompt?.Name ?? "";
            DescriptionTextBox.Text = prompt?.Description ?? "";

            if (prompt != null && !string.IsNullOrEmpty(prompt.ImagePath))
            {
                LoadImage(prompt.ImagePath);
            }
            else
            {
                DetailImage.Source = null;
            }
        }

        private void ClearDetailView()
        {
            _currentPrompt = null;
            NameTextBox.Text = "";
            DescriptionTextBox.Text = "";
            DetailImage.Source = null;
        }

        private void LoadImage(string imagePath)
        {
            try
            {
                if (File.Exists(imagePath))
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.UriSource = new Uri(imagePath);
                    bitmap.EndInit();
                    DetailImage.Source = bitmap;
                }
                else
                {
                    DetailImage.Source = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
                DetailImage.Source = null;
            }
        }
    }
}