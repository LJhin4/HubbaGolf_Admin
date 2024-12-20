using HubbaGolf_Admin;
using HubbaGolfAdmin.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//register services
builder.Services.AddCustomServices(builder.Configuration);

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Add this to ensure environment-specific appsettings file is loaded
builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;

    // This will load appsettings.[environment].json
    config.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
});

//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen(options =>
//{
//    options.DocInclusionPredicate((_, api) =>
//    {
//        return api.ActionDescriptor.EndpointMetadata
//                   .OfType<Microsoft.AspNetCore.Mvc.Routing.HttpMethodAttribute>()
//                   .Any();
//    });
//});
var app = builder.Build();

app.UseCors("AllowAllOrigins");
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
//app.UseSwagger();
//app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();

//check login
app.UseMiddleware<AuthenMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
