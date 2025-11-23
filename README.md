Prompt Handbook / Хранилище Промптов

https://img.shields.io/badge/WPF-.NET%25204.7.2-blue
https://img.shields.io/badge/version-1.3.1-green
https://img.shields.io/badge/license-MIT-lightgrey
Русская версия
🚀 Мощное WPF приложение для управления AI промптами

Prompt Handbook - это профессиональное десктопное приложение для эффективного управления промптами искусственного интеллекта с поддержкой изображений и текста. Организуйте, ищите и категоризируйте ваши промпты с помощью интуитивного трехпанельного интерфейса.
✨ Возможности
🗂️ Умная организация

    Система папок - Категоризируйте промпты в пользовательские папки

    Перетаскивание - Легко перемещайте промпты между папками с помощью drag-and-drop

    Поиск и фильтрация - Быстрый поиск по всем промптам с фильтрацией в реальном времени

    Визуальные иконки - Интуитивные иконки папок с состояниями "открыто/закрыто"

📝 Управление промптами

    Поддержка текста и изображений - Храните текстовые промпты и reference-изображения

    Автоматическое извлечение метаданных - Автоматическое извлечение промптов из PNG файлов Automatic1111

    Детальное редактирование - Комплексный редактор для названий и описаний промптов

    Автосохранение - Изменения автоматически сохраняются в файловую систему

🎨 Пользовательский опыт

    Трехпанельный интерфейс - Чистый, современный интерфейс с изменяемыми панелями

    Настраиваемые шрифты - Настройте семейство и размер шрифта под ваши предпочтения

    Визуальная обратная связь - Четкие визуальные индикаторы для всех взаимодействий

💾 Управление данными

    JSON-хранилище - Все данные хранятся в человеко-читаемом JSON формате

    Работа с изображениями - Поддержка форматов PNG, JPG, JPEG с автоматическим управлением файлами

    Резервное копирование - Простое файловое хранилище для легкого бэкапа и миграции

🚀 Быстрый старт
Установка

    Скачайте последнюю версию со страницы Releases

    Запустите PromptHandbook.exe

    Приложение создаст папку PromptDB для хранения данных

Основное использование

    Создайте папки: Используйте "Add Folder" для организации промптов

    Добавьте промпты: Нажмите "Add Prompt" для создания новых записей

    Загрузите изображения: Используйте "Load Image" для прикрепления reference-изображений

    Авто-импорт промптов: При загрузке PNG файлов из Automatic1111 метаданные автоматически извлекаются в описание

    Организуйте: Перетаскивайте промпты между папками

    Ищите: Используйте поле поиска для быстрого нахождения промптов

🛠️ Технические детали
Архитектура

    Платформа: WPF Desktop Application

    Фреймворк: .NET Framework 4.7.2

    Язык: C# 7.3

    Хранилище: JSON файловая база данных

    Зависимости: Newtonsoft.Json 13.0.4

Извлечение метаданных Automatic1111

    Форматы: PNG файлы с tEXt чанками

    Автоматизация: Метаданные автоматически помещаются в поле описания

    Совместимость: Полная обратная совместимость с существующими файлами

📈 История версий
v1.3.1 - Извлечение метаданных Automatic1111

    ✅ Автоматическое извлечение промптов из PNG файлов

    ✅ Интеграция с Automatic1111 WebUI

    ✅ Умное заполнение поля описания

v1.2.0 - Улучшение Drag-and-Drop

    ✅ Реализовано интуитивное перетаскивание между папками

    ✅ Добавлена визуальная обратная связь при перетаскивании

    ✅ Улучшена точность определения целевой папки

v1.1.0 - Система папок

    ✅ Добавлена организация по папкам

    ✅ Реализовано управление папками

    ✅ Улучшен поиск с фильтрацией по папкам

v1.0.0 - Первый релиз

    ✅ Базовое управление промптами

    ✅ Поддержка изображений

    ✅ Функциональность поиска

🔧 Разработка
Требования для сборки

    Visual Studio 2022

    .NET Framework 4.7.2 Developer Pack

Сборка из исходного кода
bash

git clone https://github.com/your-repo/prompt-handbook.git
cd prompt-handbook
open PromptHandbook.sln

English Version
🚀 Powerful WPF Application for AI Prompt Management

Prompt Handbook is a professional desktop application for efficient management of AI prompts with image and text support. Organize, search, and categorize your prompts using an intuitive three-panel interface.
✨ Features
🗂️ Smart Organization

    Folder System - Categorize prompts into custom folders

    Drag & Drop - Easily move prompts between folders with intuitive drag-and-drop

    Search & Filter - Quick search across all prompts with real-time filtering

    Visual Icons - Intuitive folder icons with open/closed states

📝 Rich Prompt Management

    Text & Image Support - Store both text prompts and reference images

    Automatic Metadata Extraction - Automatic prompt extraction from Automatic1111 PNG files

    Detailed Editing - Comprehensive editor for prompt names and descriptions

    Auto-Save - Changes are automatically saved to the file system

🎨 User Experience

    Three-Panel Interface - Clean, modern interface with resizable panels

    Customizable Fonts - Adjust font family and size to your preference

    Visual Feedback - Clear visual indicators for all interactions

💾 Data Management

    JSON-Based Storage - All data stored in human-readable JSON format

    Image Handling - Support for PNG, JPG, JPEG formats with automatic file management

    Backup Ready - Simple file-based storage for easy backup and migration

🚀 Quick Start
Installation

    Download the latest release from Releases Page

    Run PromptHandbook.exe

    The application will create a PromptDB folder for data storage

Basic Usage

    Create Folders: Use "Add Folder" to organize your prompts

    Add Prompts: Click "Add Prompt" to create new entries

    Add Images: Use "Load Image" to attach reference images

    Auto-Import Prompts: When loading PNG files from Automatic1111, metadata is automatically extracted to description

    Organize: Drag and drop prompts between folders

    Search: Use the search box to quickly find prompts

🛠️ Technical Details
Architecture

    Platform: WPF Desktop Application

    Framework: .NET Framework 4.7.2

    Language: C# 7.3

    Storage: JSON file-based database

    Dependencies: Newtonsoft.Json 13.0.4

Automatic1111 Metadata Extraction

    Formats: PNG files with tEXt chunks

    Automation: Metadata automatically populated to description field

    Compatibility: Full backward compatibility with existing files

📈 Version History
v1.3.1 - Automatic1111 Metadata Extraction

    ✅ Automatic prompt extraction from PNG files

    ✅ Integration with Automatic1111 WebUI

    ✅ Smart description field population

v1.2.0 - Drag & Drop Enhancement

    ✅ Implemented intuitive drag-and-drop between folders

    ✅ Added visual feedback during drag operations

    ✅ Improved folder targeting precision

v1.1.0 - Folder System

    ✅ Added folder-based organization

    ✅ Implemented folder management

    ✅ Enhanced search with folder filtering

v1.0.0 - Initial Release

    ✅ Basic prompt management

    ✅ Image support

    ✅ Search functionality

🔧 Development
Build Requirements

    Visual Studio 2022

    .NET Framework 4.7.2 Developer Pack

Building from Source
bash

git clone https://github.com/your-repo/prompt-handbook.git
cd prompt-handbook
open PromptHandbook.sln

📄 License

This project is licensed under the MIT License - see the LICENSE file for details.
🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request.
🐛 Reporting Issues

If you find any bugs or have feature requests, please create an issue in the GitHub Issues section.

Теги/Tags: AI prompt-management WPF Automatic1111 stable-diffusion productivity-tool
