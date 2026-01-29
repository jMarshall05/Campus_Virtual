using System.Text;
using Abstracciones.Excepciones;
using Abstracciones.Servicios;
using AutoMapper;
using DA;
using DA.Entidades;
using DA.Implementaciones;
using DA.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Servicios.Profiles;
using Servicios.Servicios;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Swagger - NECESARIO para que Scalar funcione (genera el documento OpenAPI)
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddAutoMapper(cfg => { },
    typeof(UsuariosProfile).Assembly);

//Contexto Indentity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("BD"))
);;

builder.Services
    .AddIdentity<ApplicationUser, IdentityRole<string>>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT
var jwt = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwt["Key"]!);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
builder.Services.AddScoped<IBItacoraAD, BitacoraDA>();
builder.Services.AddScoped<IMateriasDA, MateriasDA>();
builder.Services.AddScoped<IEventosDA, EventosDA>();
builder.Services.AddScoped<IEstudianteGrupoDA, EstudianteGrupoDA>();
builder.Services.AddScoped<ITareasAD, TareasDA>();

//Servicios
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<IUsuariosService, UsuariosService>();
builder.Services.AddScoped<ITelefonosService, TelefonosService>();
builder.Services.AddScoped<ITareasService, TareasService>();

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

app.UseExceptionHandler(appError =>
{
    appError.Run(async context =>
    {
        var error = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (error is BusinessException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsJsonAsync(new
            {
                mensaje = error.Message
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

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();