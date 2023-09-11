using DependencyInjection.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
//builder.Services.AddScoped<IMessageSender, EmailMessageSender>();
builder.Services.AddScoped<IMessageSender, SmsMessageSender>();

var app = builder.Build();

app.UseStaticFiles();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
