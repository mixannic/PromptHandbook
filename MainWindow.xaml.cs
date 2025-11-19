using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace PromptHandbook
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<Folder> _folders;
        private ObservableCollection<PromptItem> _prompts;
        private ObservableCollection<PromptItem> _filteredPrompts;
        private AppSettings _settings;
        private PromptItem _currentPrompt;
        private Folder _currentFolder;
        private bool _isSearchPlaceholder = true;
        private Point _dragStartPoint;
        private bool _isDragging = false;

        public MainWindow()
        {
            InitializeComponent();
            LoadSettings();
            InitializeData();
            ApplyFontSettings();

            // Инициализация placeholder для поиска
            SearchTextBox.GotFocus += SearchTextBox_GotFocus;
            SearchTextBox.LostFocus += SearchTextBox_LostFocus;

            // Подключение событий Drag and Drop
            PromptsListBox.PreviewMouseLeftButtonDown += PromptsListBox_PreviewMouseLeftButtonDown;
            PromptsListBox.PreviewMouseMove += PromptsListBox_PreviewMouseMove;
            PromptsListBox.Drop += PromptsListBox_Drop;
            PromptsListBox.DragOver += PromptsListBox_DragOver;

            FoldersListBox.Drop += FoldersListBox_Drop;
            FoldersListBox.DragOver += FoldersListBox_DragOver;

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

            // Загрузка папок
            _folders = new ObservableCollection<Folder>(DataService.LoadFolders());
            FoldersListBox.ItemsSource = _folders;

            // Загрузка промптов
            _prompts = new ObservableCollection<PromptItem>(DataService.LoadAllPrompts());
            _filteredPrompts = new ObservableCollection<PromptItem>(_prompts);
            PromptsListBox.ItemsSource = _filteredPrompts;

            // Выбор первой папки и первого промпта
            if (_folders.Count > 0)
            {
                FoldersListBox.SelectedIndex = 0;
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

            // Фильтруем по выбранной папке и тексту поиска
            var filtered = _prompts.Where(p =>
                (_currentFolder == null || p.FolderId == _currentFolder.Id) &&
                (string.IsNullOrEmpty(searchText) || p.Name.ToLower().Contains(searchText))
            );

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

            // Присваиваем текущую выбранную папку
            if (_currentFolder != null)
            {
                newPrompt.FolderId = _currentFolder.Id;
                DataService.SavePrompt(newPrompt);
            }

            _prompts.Insert(0, newPrompt);

            // Обновляем фильтрованный список
            if (_isSearchPlaceholder || string.IsNullOrEmpty(SearchTextBox.Text))
            {
                if (_currentFolder == null || newPrompt.FolderId == _currentFolder.Id)
                {
                    _filteredPrompts.Insert(0, newPrompt);
                }
            }
            else
            {
                // Проверяем соответствует ли новый элемент фильтру
                var searchText = SearchTextBox.Text.ToLower();
                if (newPrompt.Name.ToLower().Contains(searchText) &&
                    (_currentFolder == null || newPrompt.FolderId == _currentFolder.Id))
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

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            var newFolder = new Folder { Name = "New Folder" };
            _folders.Add(newFolder);
            DataService.SaveFolders(_folders.ToList());
            FoldersListBox.SelectedItem = newFolder;
        }

        private void DeleteFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (FoldersListBox.SelectedItem is Folder selectedFolder)
            {
                // Не позволяем удалить последнюю папку
                if (_folders.Count <= 1)
                {
                    MessageBox.Show("Cannot delete the last folder.", "Warning",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Проверяем есть ли промпты в этой папке
                var promptsInFolder = _prompts.Where(p => p.FolderId == selectedFolder.Id).ToList();
                if (promptsInFolder.Any())
                {
                    var result = MessageBox.Show(
                        $"Folder '{selectedFolder.Name}' contains {promptsInFolder.Count} prompt(s). " +
                        "These prompts will be moved to the default folder. Continue?",
                        "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result != MessageBoxResult.Yes)
                        return;

                    // Перемещаем промпты в первую папку (General)
                    var defaultFolder = _folders.First(f => f != selectedFolder);
                    foreach (var prompt in promptsInFolder)
                    {
                        prompt.FolderId = defaultFolder.Id;
                        DataService.SavePrompt(prompt);
                    }
                }

                _folders.Remove(selectedFolder);
                DataService.SaveFolders(_folders.ToList());

                // Выбираем другую папку
                if (_folders.Count > 0)
                {
                    FoldersListBox.SelectedIndex = 0;
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
                    var shouldBeVisible = _currentPrompt.Name.ToLower().Contains(searchText) &&
                                         (_currentFolder == null || _currentPrompt.FolderId == _currentFolder.Id);
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

        private void FoldersListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentFolder = FoldersListBox.SelectedItem as Folder;
            ApplyFilter(_isSearchPlaceholder ? "" : SearchTextBox.Text);
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

        // ========== DRAG AND DROP IMPLEMENTATION ==========

        private void PromptsListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Запоминаем точку начала перетаскивания
            _dragStartPoint = e.GetPosition(null);
            _isDragging = false;
        }

        private void PromptsListBox_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && !_isDragging)
            {
                Point currentPoint = e.GetPosition(null);
                Vector diff = _dragStartPoint - currentPoint;

                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    _isDragging = true;

                    if (PromptsListBox.SelectedItem is PromptItem selectedPrompt)
                    {
                        DragDrop.DoDragDrop(PromptsListBox, selectedPrompt, DragDropEffects.Move);
                    }
                }
            }
        }

        private void PromptsListBox_DragOver(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(PromptItem)))
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void PromptsListBox_Drop(object sender, DragEventArgs e)
        {
            // Для будущей реализации перетаскивания между промптами
        }

        private void FoldersListBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PromptItem)))
            {
                e.Effects = DragDropEffects.Move;
                e.Handled = true;
            }
            else
            {
                e.Effects = DragDropEffects.None;
                e.Handled = true;
            }
        }

        private void FoldersListBox_Drop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(PromptItem)))
            {
                var prompt = (PromptItem)e.Data.GetData(typeof(PromptItem));

                // Получаем папку, на которую бросили
                var folder = GetFolderFromDropPosition(e, (ListBox)sender);

                if (folder != null && prompt.FolderId != folder.Id)
                {
                    MovePromptToFolder(prompt, folder);
                }
            }
        }

        private Folder GetFolderFromDropPosition(DragEventArgs e, ListBox listBox)
        {
            var point = e.GetPosition(listBox);
            var hit = VisualTreeHelper.HitTest(listBox, point);

            if (hit != null)
            {
                var listBoxItem = FindParent<ListBoxItem>(hit.VisualHit);
                if (listBoxItem != null)
                {
                    return listBoxItem.DataContext as Folder;
                }
            }

            return listBox.SelectedItem as Folder;
        }

        private static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            while (child != null && !(child is T))
            {
                child = VisualTreeHelper.GetParent(child);
            }
            return child as T;
        }

        private void MovePromptToFolder(PromptItem prompt, Folder targetFolder)
        {
            try
            {
                // Сохраняем старую папку для обновления UI
                var oldFolderId = prompt.FolderId;

                // Обновляем папку промпта
                prompt.FolderId = targetFolder.Id;
                DataService.SavePrompt(prompt);

                // Обновляем UI - удаляем из текущего отфильтрованного списка
                _filteredPrompts.Remove(prompt);

                // Показываем сообщение об успехе
                MessageBox.Show($"Prompt '{prompt.Name}' moved to '{targetFolder.Name}'",
                               "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error moving prompt: {ex.Message}", "Error",
                               MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}