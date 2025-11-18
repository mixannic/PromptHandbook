using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace PromptHandbook
{
    public partial class SettingsWindow : Window
    {
        private AppSettings _settings;

        public SettingsWindow(AppSettings currentSettings)
        {
            InitializeComponent();
            _settings = currentSettings;

            // Устанавливаем владельца для правильного позиционирования
            this.Owner = Application.Current.MainWindow;

            InitializeControls();
        }

        private void InitializeControls()
        {
            var fonts = Fonts.SystemFontFamilies.OrderBy(f => f.Source);
            foreach (var font in fonts)
            {
                FontFamilyComboBox.Items.Add(font.Source);
            }
            FontFamilyComboBox.Text = _settings.FontFamily;

            var fontSizes = new[] { 8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24 };
            foreach (var size in fontSizes)
            {
                FontSizeComboBox.Items.Add(size);
            }
            FontSizeComboBox.Text = _settings.FontSize.ToString();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(FontSizeComboBox.Text, out double fontSize) && fontSize > 0)
            {
                _settings.FontSize = fontSize;
            }

            _settings.FontFamily = FontFamilyComboBox.Text;
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}