using ReadMangaWS.DataAccess;
using ReadMangaWS.Repository;

// �������� ������� ����������, ������� ������������ ��� ��������� ����������
var builder = WebApplication.CreateBuilder(args);

// ��������� ������� � ���������
// ��������� ��������� ������������. ��� ���������� ��� ��������� HTTP-�������� � �������� �������
builder.Services.AddControllers();
// ��������� ��������� OpenAPI (Swagger) ��� ������ ����������. Swagger ������������� ��������� ��� ������������ API � ������������, ��� ������ ��� ������� ��� �������������
builder.Services.AddOpenApi();

// ����������� DBConnection
// ����� ������������ ����� DBConnection � ���������� ������������ � �������� ��������� "Scoped"
// ��� ��������, ��� ��� ������� HTTP-������� ����� ������ ����� ��������� DBConnection, ������� ����� �������������� � ������� ����� �������
builder.Services.AddScoped<DBConnection>(provider =>
{
    // �������� ������������ ����������, ������� ���� ��������� �� ����� appsettings.json
    var configuration = provider.GetRequiredService<IConfiguration>();
    // ��������� ������ ����������� � ���� ������ �� ������������
    string? connectionString = configuration.GetConnectionString("DefaultConnection");

    // �������� ������ �����������
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new ArgumentException("������ ����������� �� ����� ���� ������ ��� ������ null.", nameof(connectionString));
    }

    return new DBConnection(connectionString);
});


// ����������� �����������
// ����� ������������ ���������� MangaRepository ��� ���������� IMangaRepository
// ��� ����� �������� � �������� ��������� "Scoped", ��� ��������� ������������ ���� ��������� MangaRepository � ������� ������ HTTP-�������.
builder.Services.AddScoped<IMangaRepository, MangaRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<ITegRepository, TegRepository>();
builder.Services.AddScoped<IMangaCollection, MangaCollectionRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
builder.Services.AddScoped<IMangaScoreRepository, MangaScoreRepository>();
builder.Services.AddScoped<IPagesRepository, PagesRepository>();
builder.Services.AddScoped<IPublisherRepository, PublisherRepository>();


// ��������� ������ ����� �� ���������� ���������
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://*:{port}");

// ����� ������� ��������� WebApplication �� ������ ��������, ������� ���� ������ �����. ������ ���������� ������ � �������
var app = builder.Build();

// ��������� HTTP-���������
// �������� ����� ����������: ���� ���������� �������� � ����� ����������, ����������� ��������� OpenAPI.
// ��� ��������� ������ ��������� Swagger ��� ������������ API
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection(); // �������������� ��� HTTP-������� �� HTTPS, ��� �������� ������������ ������ ����������
app.UseAuthorization(); // ��������� ��������� ����������� � ������ ����������. ��� ��������� �������� ������������ �������� � �����������
app.MapControllers(); // ����������� ������������� ��� ������������, ����� ��� ����� ������������ �������� HTTP-�������

app.Run(); // ��������� ���������� � �������� ������������ �������� HTTP-�������
