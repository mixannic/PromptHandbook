# Prompt Handbook

WPF приложение для управления промптами с поддержкой изображений и текста.

## Функционал

- 📝 Создание и редактирование промптов
- 🖼️ Загрузка изображений (PNG, JPG)
- 🔍 Поиск по названиям
- ⚙️ Настройки шрифта
- 💾 Автосохранение в файловую систему

## Технологии

- C# 7.3
- WPF
- .NET Framework 4.7.2
- Newtonsoft.Json

## Установка

1. Скачайте последний релиз
2. Запустите `PromptHandbook.exe`
3. Приложение создаст папку `PromptDB` для хранения данных

## Сборка


# Клонируйте репозиторий
git clone https://github.com/mixannic/PromptHandbook.git

# Откройте в Visual Studio
# Соберите в конфигурации Release

---

##

# Список изменений v.1.1.0.0 Release/ Changelog v.1.1.0.0 Release

### 🔥 Крупные изменения
- **Добавлена система папок/категорий** для организации промптов
- **Реализован трехпанельный интерфейс** (папки → промпты → детали)
- **Создан класс Folder** для управления категориями с INotifyPropertyChanged

### ✨ Новый функционал
- Кнопки "Add Folder"/"Delete Folder" для управления папками
- Автоматическая фильтрация промптов по выбранной папке
- Папка "General" создается по умолчанию
- Поддержка перемещения промптов между папками при удалении папки

### 🛠 Технические улучшения
- Расширен класс PromptItem: добавлены FolderId и коллекция Tags
- Обновлен DataService: методы для работы с папками (LoadFolders, SaveFolders)
- Улучшена сериализация: FolderId сохраняется в metadata.json
- Исправлены проблемы с placeholder в поиске

### 🐛 Исправления ошибок
- Решена проблема исчезновения списка при взаимодействии с правой панелью
- Улучшена обработка потери фокуса в поисковой строке
- Исправлено позиционирование окна настроек (центрирование относительно главного окна)

### 📁 Структура данных
- Новая папка `folders.json` для хранения структуры категорий
- Обратная совместимость с существующими промптами
- Автоматическое назначение папки для новых промптов

---


### 🔥 Major Changes
- **Added folder/category system** for prompt organization
- **Implemented three-panel interface** (folders → prompts → details)
- **Created Folder class** for category management with INotifyPropertyChanged

### ✨ New Features
- "Add Folder"/"Delete Folder" buttons for folder management
- Automatic prompt filtering by selected folder
- "General" folder created by default
- Support for moving prompts between folders when deleting a folder

### 🛠 Technical Improvements
- Extended PromptItem class: added FolderId and Tags collection
- Updated DataService: methods for folder operations (LoadFolders, SaveFolders)
- Improved serialization: FolderId saved in metadata.json
- Fixed search placeholder issues

### 🐛 Bug Fixes
- Fixed list disappearance when interacting with right panel
- Improved focus loss handling in search box
- Fixed settings window positioning (centering relative to main window)

### 📁 Data Structure
- New `folders.json` file for category structure storage
- Backward compatibility with existing prompts
- Automatic folder assignment for new prompts

### 🔧 Updated Files

**Full Changelog**: https://github.com/mixannic/PromptHandbook/compare/v.1.0.0.0...v.1.1.0.0
