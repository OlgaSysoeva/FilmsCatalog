## Тестовое задание

Результат можно посмотреть в gif: https://drive.google.com/file/d/1-iXS6PqH0u1SPloYQNEw-pd-4tPhmCZ-/view?usp=sharing

![Результат работы](https://i.giphy.com/media/Iz9BuiMSQqoshXC6Q0/giphy.webp)

#### Суть задания

Нужно доработать приложение в репозитории. В проекте используется паттерн MVC. Уже реализованы методы для регистрации и входа пользователей, настроено подключение к LocalDB, задействован Entity Framework Core. Приложение представляет собой каталог фильмов. 

Необходимо реализовать страницы:

- страницу для просмотра списка всех фильмов (с пагинацией);
- страницу просмотра информации об отдельном фильме;
- страницу добавления в каталог фильма;
- страницу редактирования имеющегося в каталоге фильма.

Для каждого фильма должно храниться название, описание, год выпуска, режиссёр, пользователь, который выложил информацию, постер. Постер - это файл-изображение. Должна быть возможность загрузить постер и посмотреть его на странице детальной информации о фильме. Функциональность по выкладыванию видеофайла фильма не нужна. 
Редактировать фильм имеет право только тот, кто изначально выложил информацию об этом фильме. 
При реализации каталога учитывать, что фильмов в каталоге может быть потенциально сотни тысяч.


#### Дополнительные инструкции по запуску проекта из репозитория

Проект реализован на .NET 5. Если у Вас еще не установлен SDK для этой версии .NET, необходимо обновить Visual Studio или вручную установить [SDK](https://dotnet.microsoft.com/download/dotnet/5.0) (нужно установить именно SDK, а не Runtime). 

Перед первым запуском проекта необходимо выполнить команду *update-database* для инициализации LocalDB.
