# Silicone Heart  
Проект является тестовым заданием.

Комментарии к выполненному проекту:  
1. Для входа/выхода из режима `Размещения`/`Удаления` требуется нажать на соответствующую кнопку. Для входа в режим `Размещения` должна быть выбрана какая-либо постройка.  
   В режиме `Размещения` можно менять активную постройку. Сохранение происходит автоматически при каждом изменении конфигурации установленных зданий.

2. Для добавления нового здания нужно сделать 2 вещи:  
   2.1. В папке `Assets/SiliconeHeart/Resources/BuildingData` добавить новый скриптовый объект (ПКМ Create>Data>BuildingData) и заполнить информацию о постройке.  
   2.2. В папке `Assets/SiliconeHeart/Prefabs/Buildings` добавить префаб постройки со всеми нужными компонентами (в перспективе, сейчас это просто балванка) для его работы.  
Кнопка с постройкой сгенерируется сама, исходя из конфига.  
*Изначально я думал генерировать префаб также из конфига, но передумал из-за спорности в пользе (и потребности) такого подхода.*

3. Одной из задач было выделение механик в отдельные модули с использованием Assembly Definition. Постарался разделить механики на максимально большое количество модулей с перспективой более гибкого расширения функционала/переиспользования кода.

4. Сериализованные данные хранятся в виде JSON-файла в `Application.persistentDataPath`.

5. На реализацию проекта ушло 12 часов. Изначально предполагал, что он займет порядка 8 часов. Дополнительное время потратил на реализацию более приятной системы управления, генерацию кнопок по конфигам, а также полировку архитектурных решений.

Выполнил: Боровихин Владислав
---
![SiliconeHeart Screenshot](https://github.com/user-attachments/assets/87b8abe0-c5ea-43c3-b461-417add3cef45)
