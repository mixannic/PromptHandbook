![WPF .NET 4.7.2](https://img.shields.io/badge/WPF-.NET4.7.2-blue)
![Version 1.3.1](https://img.shields.io/badge/version-1.3.1-green)
![License MIT](https://img.shields.io/badge/license-MIT-lightgrey)

# Prompt Handbook / Хранилище Промптов
Русская версия

WPF desktop application for AI prompt management with image support.

Десктопное WPF-приложение для управления AI-промптами с поддержкой изображений.

## Features / Возможности

Folder system — Categorize prompts into custom folders

Система папок — Категоризируйте промпты в пользовательские папки

Drag & Drop — Move prompts between folders intuitively

Перетаскивание — Легко перемещайте промпты между папками

Search & Filter — Quick search across all prompts with real-time filtering

Поиск и фильтрация — Быстрый поиск по всем промптам с фильтрацией в реальном времени

Text & Image Support — Store both text prompts and reference images

Поддержка текста и изображений — Храните текстовые промпты и reference-изображения

Automatic1111 Metadata Extraction — Extract prompts from PNG files with tEXt chunks

Автоматическое извлечение метаданных — Автоматическое извлечение промптов из PNG-файлов Automatic1111

Auto-Save — Changes are automatically saved to the file system

Автосохранение — Изменения автоматически сохраняются в файловую систему

Three-Panel Interface — Clean interface with resizable panels

Трехпанельный интерфейс — Чистый интерфейс с изменяемыми панелями

Customizable Fonts — Adjust font family and size

Настраиваемые шрифты — Настройте семейство и размер шрифта

JSON-Based Storage — Human-readable JSON file-based database

JSON-хранилище — Все данные хранятся в человеко-читаемом JSON формате

## Tech Stack / Стек технологий

WPF Desktop Application

Рабочий стол WPF

.NET Framework 4.7.2

C#

Newtonsoft.Json 13.0.4

JSON file-based storage

## Quick Start / Быстрый старт

### From source / Из исходного кода

```bash
git clone https://github.com/mixannic/PromptHandbook.git
cd PromptHandbook
```

Open `PromptHandbook.csproj` in Visual Studio 2022 with .NET Framework 4.7.2 Developer Pack installed, then build and run.

Откройте `PromptHandbook.csproj` в Visual Studio 2022 с установленным пакетом разработчика .NET Framework 4.7.2, затем соберите и запустите.

### Pre-built executable / Готовый исполняемый файл

Download the latest release from the Releases page and run `PromptHandbook.exe`. The application will create a `PromptDB` folder for data storage.

Скачайте последнюю версию со страницы Releases и запустите `PromptHandbook.exe`. Приложение создаст папку `PromptDB` для хранения данных.

## Project Structure / Структура проекта

```
PromptHandbook/
├── App.xaml                 # Application entry point / Точка входа приложения
├── App.xaml.cs              # Application code-behind / Код-behind приложения
├── MainWindow.xaml          # Main window UI / Интерфейс главного окна
├── MainWindow.xaml.cs       # Main window logic / Логика главного окна
├── Folder.cs                # Folder model / Модель папки
├── Models/
│   └── PromptItem.cs        # Prompt model / Модель промпта
├── Services/
│   └── DataService.cs       # Data access layer / Слой доступа к данным
├── RenameFolderWindow.xaml  # Rename folder dialog / Диалог переименования папки
├── RenameFolderWindow.xaml.cs
├── Windows/
│   ├── SettingsWindow.xaml  # Settings dialog / Диалог настроек
│   └── SettingsWindow.xaml.cs
├── Properties/
│   ├── AssemblyInfo.cs      # Assembly metadata / Метаданные сборки
│   ├── Resources.resx       # Resources / Ресурсы
│   └── Settings.settings    # User settings / Настройки пользователя
├── App.config               # .NET Framework configuration / Конфигурация .NET Framework
├── packages.config          # NuGet dependencies / Зависимости NuGet
├── PromptHandbook.csproj    # Project file / Файл проекта
├── icon.ico                 # Application icon / Иконка приложения
├── folderclosed.png         # Closed folder icon / Иконка закрытой папки
├── folderopen.png           # Open folder icon / Иконка открытой папки
├── LICENSE                  # MIT License / Лицензия MIT
├── README.md                # This file / Этот файл
└── .gitignore               # Git ignored files / Игнорируемые файлы Git
```

Data is stored in `PromptDB/` next to the executable:

Данные хранятся в `PromptDB/` рядом с исполняемым файлом:

```
PromptDB/
├── folders.json             # List of folders / Список папок
├── settings.json            # User settings / Настройки пользователя
└── {timestamp}/             # Prompt folder / Папка промпта
    ├── metadata.json        # Prompt metadata / Метаданные промпта
    ├── description.txt      # Prompt description / Описание промпта
    └── image.png            # Attached image / Прикрепленное изображение
```


## Usage / Использование

1. Create folders with **Add Folder** to organize your prompts.

   Создавайте папки через **Add Folder** для организации промптов.

2. Click **Add Prompt** to create a new entry.

   Нажмите **Add Prompt** для создания новой записи.

3. Select a prompt to edit its name and description. Changes save automatically on focus loss.

   Выберите промпт для редактирования имени и описания. Изменения сохраняются автоматически при потере фокуса.

4. Click **Load Image** to attach a reference image (PNG, JPG, JPEG). For PNG files from Automatic1111, metadata is extracted automatically to the description field.

   Нажмите **Load Image** для прикрепления reference-изображения (PNG, JPG, JPEG). Для PNG-файлов из Automatic1111 метаданные извлекаются автоматически в поле описания.

5. Drag prompts between folders to reorganize.

   Перетаскивайте промпты между папками для реорганизации.

6. Use the search box to filter prompts by name in real time.

   Используйте поле поиска для фильтрации промптов по имени в реальном времени.

7. Open **Settings** to customize font family and size.

   Откройте **Settings** для настройки семейства и размера шрифта.

## Data Storage / Хранение данных

All data is stored in human-readable JSON files under `PromptDB/`:

Все данные хранятся в человеко-читаемых JSON-файлах в `PromptDB/`:

- `folders.json` — list of folders with their GUID IDs / список папок с их GUID-идентификаторами
- `settings.json` — font family and size / размер и семейство шрифта
- Each prompt gets its own timestamped folder containing `metadata.json`, `description.txt`, and an optional `image.png` / Каждый промпт получает собственную папку с меткой времени, содержащую `metadata.json`, `description.txt` и опциональный `image.png`

This makes backup and migration simple — just copy the `PromptDB` folder.

Это упрощает бэкап и миграцию — просто скопируйте папку `PromptDB`.

## Version History / История версий

### v1.3.1 — Automatic1111 Metadata Extraction

- Automatic prompt extraction from PNG files / Автоматическое извлечение промптов из PNG-файлов
- Integration with Automatic1111 WebUI / Интеграция с Automatic1111 WebUI
- Smart description field population / Умное заполнение поля описания

### v1.2.0 — Drag & Drop Enhancement

- Drag-and-drop between folders / Перетаскивание между папками
- Visual feedback during drag operations / Визуальная обратная связь при перетаскивании
- Improved folder targeting precision / Улучшена точность определения целевой папки

### v1.1.0 — Folder System

- Folder-based organization / Организация на основе папок
- Folder management / Управление папками
- Enhanced search with folder filtering / Улучшенный поиск с фильтрацией по папкам

### v1.0.0 — Initial Release

- Basic prompt management / Базовое управление промптами
- Image support / Поддержка изображений
- Search functionality / Функционал поиска

## License / Лицензия

Distributed under the MIT License. See the `LICENSE` file.

Распространяется под лицензией MIT. См. файл `LICENSE`.

## Roadmap / Планы развития

- MVVM architecture / Архитектура MVVM
- Unit tests for DataService / Юнит-тесты для DataService
- Export/import prompts (JSON) / Экспорт/импорт промптов (JSON)
- Dark theme / Тёмная тема
- Prompt tags and categories / Теги и категории промптов
