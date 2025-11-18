using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;

namespace PromptHandbook
{
    public static class DataService
    {
        private static readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string DatabasePath = Path.Combine(BaseDirectory, "PromptDB");
        private static readonly string SettingsPath = Path.Combine(BaseDirectory, "settings.json");

        public static void EnsureDatabaseExists()
        {
            try
            {
                if (!Directory.Exists(DatabasePath))
                {
                    Directory.CreateDirectory(DatabasePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating database directory: {ex.Message}");
            }
        }

        public static List<PromptItem> LoadAllPrompts()
        {
            var prompts = new List<PromptItem>();

            if (!Directory.Exists(DatabasePath))
                return prompts;

            try
            {
                foreach (var folder in Directory.GetDirectories(DatabasePath))
                {
                    try
                    {
                        var metadataFile = Path.Combine(folder, "metadata.json");
                        var descriptionFile = Path.Combine(folder, "description.txt");

                        if (File.Exists(metadataFile))
                        {
                            var metadataJson = File.ReadAllText(metadataFile);
                            var metadata = JsonConvert.DeserializeObject<Dictionary<string, string>>(metadataJson);

                            var prompt = new PromptItem
                            {
                                FolderName = Path.GetFileName(folder),
                                Name = metadata.ContainsKey("Name") ? metadata["Name"] : "Unnamed",
                                CreatedDate = DateTime.Parse(metadata["CreatedDate"])
                            };

                            // Загрузка описания
                            if (File.Exists(descriptionFile))
                            {
                                prompt.Description = File.ReadAllText(descriptionFile);
                            }

                            // Поиск изображения
                            var imageFiles = Directory.GetFiles(folder)
                                .Where(f => f.ToLower().EndsWith(".png") ||
                                           f.ToLower().EndsWith(".jpg") ||
                                           f.ToLower().EndsWith(".jpeg"))
                                .ToList();

                            if (imageFiles.Any())
                            {
                                prompt.ImagePath = imageFiles.First();
                            }

                            prompts.Add(prompt);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error loading prompt from {folder}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading prompts: {ex.Message}");
            }

            return prompts.OrderByDescending(p => p.CreatedDate).ToList();
        }

        public static PromptItem CreateNewPrompt()
        {
            try
            {
                var folderName = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var promptFolder = Path.Combine(DatabasePath, folderName);
                Directory.CreateDirectory(promptFolder);

                var prompt = new PromptItem
                {
                    FolderName = folderName,
                    Name = "New Prompt",
                    CreatedDate = DateTime.Now
                };

                SavePrompt(prompt);
                return prompt;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error creating new prompt: {ex.Message}");
                return null;
            }
        }

        public static void SavePrompt(PromptItem prompt)
        {
            if (prompt == null) return;

            try
            {
                var promptFolder = Path.Combine(DatabasePath, prompt.FolderName);

                // Убедимся, что папка существует
                if (!Directory.Exists(promptFolder))
                {
                    Directory.CreateDirectory(promptFolder);
                }

                // Сохранение метаданных
                var metadata = new Dictionary<string, string>
                {
                    ["Name"] = prompt.Name,
                    ["CreatedDate"] = prompt.CreatedDate.ToString("O")
                };

                File.WriteAllText(Path.Combine(promptFolder, "metadata.json"),
                    JsonConvert.SerializeObject(metadata, Formatting.Indented));

                // Сохранение описания
                File.WriteAllText(Path.Combine(promptFolder, "description.txt"),
                    prompt.Description ?? "");

                // Копирование изображения если путь изменился
                if (!string.IsNullOrEmpty(prompt.ImagePath) &&
                    File.Exists(prompt.ImagePath) &&
                    !prompt.ImagePath.StartsWith(promptFolder))
                {
                    var extension = Path.GetExtension(prompt.ImagePath);
                    var newImagePath = Path.Combine(promptFolder, $"image{extension}");
                    File.Copy(prompt.ImagePath, newImagePath, true);
                    prompt.ImagePath = newImagePath;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving prompt: {ex.Message}");
            }
        }

        public static void DeletePrompt(PromptItem prompt)
        {
            if (prompt == null) return;

            try
            {
                var promptFolder = Path.Combine(DatabasePath, prompt.FolderName);
                if (Directory.Exists(promptFolder))
                {
                    Directory.Delete(promptFolder, true);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting prompt: {ex.Message}");
            }
        }

        public static AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsPath))
                {
                    var settingsJson = File.ReadAllText(SettingsPath);
                    return JsonConvert.DeserializeObject<AppSettings>(settingsJson);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading settings: {ex.Message}");
            }
            return new AppSettings();
        }

        public static void SaveSettings(AppSettings settings)
        {
            try
            {
                File.WriteAllText(SettingsPath, JsonConvert.SerializeObject(settings, Formatting.Indented));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}");
            }
        }
    }

    public class AppSettings
    {
        public string FontFamily { get; set; } = "Segoe UI";
        public double FontSize { get; set; } = 12;
    }
}