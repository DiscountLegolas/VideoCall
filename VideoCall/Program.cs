using Microsoft.Extensions.Configuration;
using MongoDB.Driver.Core.Configuration;
using MongoDB.Driver;
using VideoCall.Identity.Models;
using VideoCall.Settings;
using VideoCall.SignalRtc;
using VideoCall.RepoModel.Repos;

var builder = WebApplication.CreateBuilder(args);
var mongoDbSettings = builder.Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();

builder.Services.Configure<VidroMongoDbConfig>(
    builder.Configuration.GetSection("VidroDatabase"));
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
        .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
        (
            mongoDbSettings.ConnectionString, mongoDbSettings.Name
        );
ExtensionFunctions.RegisterServices(builder.Services.BuildServiceProvider());
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();
builder.Services.AddSession();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("localhost")
                .AllowAnyHeader()
                .WithMethods("GET", "POST")
                .AllowCredentials();
        });
});
builder.Services.AddSingleton<RoomRepo>();
var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors();

app.MapRazorPages();
app.MapHub<SignalRtcHub>("/rtcHub");

app.Run();
