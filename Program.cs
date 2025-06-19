using ReadMangaWS.DataAccess;
using ReadMangaWS.Repository;

// создание билдера приложения, который используется для настройки приложения
var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы в контейнер
// добавляет поддержку контроллеров. Это необходимо для обработки HTTP-запросов и возврата ответов
builder.Services.AddControllers();
// добавляет поддержку OpenAPI (Swagger) для вашего приложения. Swagger предоставляет интерфейс для тестирования API и документацию, что делает его удобным для разработчиков
builder.Services.AddOpenApi();

// Регистрация DBConnection
// метод регистрирует класс DBConnection в контейнере зависимостей с областью видимости "Scoped"
// Это означает, что для каждого HTTP-запроса будет создан новый экземпляр DBConnection, который будет использоваться в течение этого запроса
builder.Services.AddScoped<DBConnection>(provider =>
{
    // Получает конфигурацию приложения, которая была загружена из файла appsettings.json
    var configuration = provider.GetRequiredService<IConfiguration>();
    // Извлекает строку подключения к базе данных из конфигурации
    string? connectionString = configuration.GetConnectionString("DefaultConnection");

    // проверка строки подключения
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new ArgumentException("Строка подключения не может быть пустой или равной null.", nameof(connectionString));
    }

    return new DBConnection(connectionString);
});


// Регистрация репозитория
// метод регистрирует реализацию MangaRepository для интерфейса IMangaRepository
// Это также делается с областью видимости "Scoped", что позволяет использовать один экземпляр MangaRepository в течение одного HTTP-запроса.
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<ITegRepository, TegRepository>();
builder.Services.AddScoped<IMangaCollection, MangaCollectionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IMangaScoreRepository, MangaScoreRepository>();
builder.Services.AddScoped<IPagesRepository, PagesRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();


// Добавляем чтение порта из переменной окружения
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

// метод создает экземпляр WebApplication на основе настроек, которые были заданы ранее. Теперь приложение готово к запуску
var app = builder.Build();

// Настройка HTTP-пайплайна
// Проверка среды разработки: Если приложение запущено в среде разработки, добавляется поддержка OpenAPI.
// Это позволяет видеть интерфейс Swagger для тестирования API
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection(); // перенаправляет все HTTP-запросы на HTTPS, что повышает безопасность вашего приложения
app.UseAuthorization(); // добавляет поддержку авторизации к вашему приложению. Это позволяет защищать определенные маршруты и контроллеры
app.MapControllers(); // настраивает маршрутизацию для контроллеров, чтобы они могли обрабатывать входящие HTTP-запросы

app.Run(); // запускает приложение и начинает прослушивать входящие HTTP-запросы
