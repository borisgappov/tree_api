using Microsoft.EntityFrameworkCore;
using TreeApi.Data;
using TreeApi.Data.UnitOfWork;
using TreeApi.Services;
using TreeApi.Services.Mappers;
using TreeApi.Middleware;
using TreeApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<TreeApiDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITreeService, TreeService>();
builder.Services.AddScoped<IExceptionJournalService, ExceptionJournalService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IJournalService, JournalService>();

builder.Services.AddAutoMapper(typeof(TreeProfile), typeof(NodeProfile), typeof(ExceptionJournalProfile), typeof(PartnerProfile));

builder.Services.AddCustomSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(c =>
    {
        c.SerializeAsV2 = true;       
    });

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseExceptionHandling();
app.UseDotRoute();
app.UseRouting();
app.MapControllers();

await DatabaseInitializer.InitializeAsync(app.Services);

app.Run();
