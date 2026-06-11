using System.Text;
using Abstracciones.Excepciones;
using Abstracciones.Servicios;
using Abstracciones.Servicios.Helpers;
using CloudinaryDotNet;
using DA;
using DA.Implementaciones;
using DA.Interfaces;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using Servicios.Helpers;
using Servicios.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger - NECESARIO para que Scalar funcione (genera el documento OpenAPI)
builder.Services.AddSwaggerGen(c =>
{
    c.MapType<IFormFile>(() => new OpenApiSchema
    {
        Type = "string",
        Format = "binary"
    });
});

builder.Services.AddHttpClient();

var config = TypeAdapterConfig.GlobalSettings;
config.Scan(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton(config);
builder.Services.AddScoped<IMapper, ServiceMapper>();

//Contexto Indentity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BD"))
);

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<string>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();


//builder.WebHost.UseUrls("https://localhost:5001");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            policy
                .WithOrigins("http://localhost:5173", "https://rehydrate-trump-bling.ngrok-free.dev")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

// JWT
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(options =>
{

    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddResponseCompression();

//DA
builder.Services.AddScoped<IUsuariosDA, UsuariosDA>();
builder.Services.AddScoped<ITelefonosDA, TelefonosDA>();
builder.Services.AddScoped<IEstudianteGrupoDA, EstudianteGrupoDA>();
builder.Services.AddScoped<IGruposDA, GruposDA>();
builder.Services.AddScoped<IEntregasAD, EntregasDA>();
builder.Services.AddScoped<ICalificacionesAD, CalificacionesDA>();
builder.Services.AddScoped<IDocumentosAD, DocumentosDA>();
builder.Services.AddScoped<IAnunciosAD, AnunciosDA>();
builder.Services.AddScoped<ICursosAD, CursosDA>();
builder.Services.AddScoped<IBitacoraAD, BitacoraDA>();
builder.Services.AddScoped<IMateriasDA, MateriasDA>();
builder.Services.AddScoped<IEventosDA, EventosDA>();
builder.Services.AddScoped<ITareasAD, TareasDA>();

//Servicios
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<ITelefonosService, TelefonosService>();
builder.Services.AddScoped<ITareasService, TareasService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEstudianteGrupoService, EstudianteGrupoService>();
builder.Services.AddScoped<IMateriasService, MateriasService>();
builder.Services.AddScoped<IEstudianteGrupoHelper, EstudianteGrupoHelper>();
builder.Services.AddScoped<IGruposService, GruposService>();
builder.Services.AddScoped<IGruposHelper, GruposHelper>();
builder.Services.AddScoped<SecretProtectorService>();
builder.Services.AddScoped<IExportService, ExportService>();
builder.Services.AddScoped<IDocumentoService, DocumentoService>();
builder.Services.AddScoped<ICursosService, CursosService>();
builder.Services.AddScoped<IAnunciosService, AnunciosService>();
var cloudinary = new Cloudinary(new Account(
    builder.Configuration["Cloudinary:CloudName"],
    builder.Configuration["Cloudinary:ApiKey"],
    builder.Configuration["Cloudinary:ApiSecret"]
));
builder.Services.AddSingleton(cloudinary);
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

//builder.Services.AddScoped<IFileStorageService>(sp =>
//{
//    var env = sp.GetRequiredService<IWebHostEnvironment>();
//    return new FileStorageService(env.ContentRootPath);
//});

builder.Services.AddAuthorization();

var app = builder.Build();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (error is BusinessException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                mensaje = error.Message
            });
        }
        else
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsJsonAsync(new
            {
                mensaje = "Ocurrió un error inesperado."
            });
        }
    });
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Swagger - NECESARIO para generar el documento OpenAPI que Scalar usa
    app.UseSwagger();

    // SwaggerUI (comentado - usamos Scalar en su lugar)
    //app.UseSwaggerUI();

    // Scalar - Configurado para leer el documento de Swagger
    app.MapScalarApiReference(options =>
    {
        options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json");
    });
}

//app.UseHttpsRedirection();
app.UseResponseCompression();
app.UseCors("AllowReact");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();