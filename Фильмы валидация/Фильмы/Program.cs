using Microsoft.EntityFrameworkCore;
using MovieApp.Models;


var builder = WebApplication.CreateBuilder(args);

// �������� ������ ����������� �� ����� ������������
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
int i;
// ��������� �������� ApplicationContext � �������� ������� � ����������
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

// ��������� ������� MVC


builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//���� ������, ��� �� ������ Index.cshtml ���� ����� ������, ��� �� ����, � ������������ ������� �������� ����� AddRazorRuntimeCompilation � ��� ���������
//InvalidOperationException: The view '~/Views/Home/Index.cshtml' was not found. The following locations were searched:
//~/ Views / Home / Index.cshtml
//� ���������� ���������� ���� ���, ����� ����� �������,����� ������������, � � ����� ������ ���� � ���, ��� ������ ������
// �� �� ����� ������ ��� �� �� ������

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
