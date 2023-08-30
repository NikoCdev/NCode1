using Microsoft.EntityFrameworkCore;
using MovieApp.Models;


var builder = WebApplication.CreateBuilder(args);

// ѕолучаем строку подключени€ из файла конфигурации
string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
int i;
// добавл€ем контекст ApplicationContext в качестве сервиса в приложение
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connection));

// ƒобавл€ем сервисы MVC


builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
//Ѕыла ошибка, что не видело Index.cshtml хот€ пр€мо писало, что он есть, в документации сказали добавить метод AddRazorRuntimeCompilation и все сработало
//InvalidOperationException: The view '~/Views/Home/Index.cshtml' was not found. The following locations were searched:
//~/ Views / Home / Index.cshtml
//€ переписывл контроллер кучу раз, искал много решений,читал документации, а в итоге ошибка было в том, что студи€ просто
// из за своих причин что то не видела

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
