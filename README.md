# VinnyRengaPlugin

Adapter for Renga

# Установка

Файлы плагина к Renga расположены в папке `plugins\renga` пакета `VinnyLibConverter`(см. [здесь](https://github.com/Vinny-Environment/VinnyLibConverter#%D1%83%D1%81%D1%82%D0%B0%D0%BD%D0%BE%D0%B2%D0%BA%D0%B0))

Приложение собрано под .NET8, протестировано на версии Renga Standard 8.8.

1. Зайти в папку с установленной Renga нужной версии (папка, в корне которой лежит приложение `Renga.exe`);

2. Создать при отсутствии папку `Plugins`;

3. Создать в ней папку `VinnyRengaAdapter`;

4. Скопировать в папку `VinnyRengaAdapter` файл `VinnyRengaAdapter.rndesc` из `plugins\renga`;

5. Открыть скопированный файл `VinnyRengaAdapter.rndesc` и отредактировать XML-тэг `PluginFilename`, задав абсолютный файловый путь до `VinnyRengaLoader.dll`, куда вы распаковали `VinnyLibConverter` у себя на ПК;

Если всё сделано корректно, то при запуске Renga на панели инструментов появится кнопка плагина:

![](assets/2025-09-03-22-44-10-image.png)
