using Microsoft.Data.SqlClient;
/**/
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;
using ToDoList.DTO;
// hi haura 2 usings mes
// *Falta aquest: using IdentificationModel
using Microsoft.AspNetCore.Authentication.JwtBearer
// afegeix directius de les classe Models i packet/nuget EntityFrameworkCore
// injeccio de dependencies

/**/
var builder = WebApplication.CreateBuilder(args);

// add sevices to the container, envia uns headers que ho permeten tot, CORS, HABILITAT PER TOT.
// en produccio, within origins, localhost, parametres i permetre alguns origens predeterminats, i lo que tenim mai
// es pojaria a produccio
builder.Services.AddCors(opt => opt.AddDefaultPolicy(policy => 
    policy.AllowAnyHeader()
    .AllowAnyMethod()
    .AllowAnyOrigin()));

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// nova linea per API, afegeix la DBContext en el DI contenedor, usant un in-memory database, que serà usat
// bbdd context, que es faci de servir una BBDD de memoriaRAM que sanomena TodoList
/**/
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddEndpointsApiExplorer();
// es MODIFICA el swagger
builder.Services.AddSwaggerGen(c =>
    {
        option.AddSecurityDefinition("Bearer", new OpenApiSecurity)
    }
);
// hem fet una funcio lambda per dir de com mapejar. desde el todotask, cap a todoitem, i a la inversa. i no cal fer res mes perque el nom dels camps coincideixen
builder.Services.AddAutoMapper(cfg => {
    cfg.CreateMap<TodoTask, TodoItem>().ReverseMap();
});

// instalar un paquet, farem servir jwt, es MODIFICA
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer((options => {

    validateIssuerSign
    ValidIssuer = builderConfiguration(Jwt:Visual)
    ValidAudience = builder.builderConfiguration
    IssuerSigningKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
})));
// validem per a qui el token
// validem el temps de vida
// abans de lhora actual i que caduqui despres de la hora actua
// validem clau de firma

// hem creat una configuracio de mapeig de todotask a todoitem, i també a la inversa, i com que els
// noms dels camps concideixen, no tenim que fer res més, si no coincidis, dins del createmat( creariem un metode on ficariem de com mapegem d'un cap a laltre completed -> iscompleted o a la inversa)
// per exemple ().ForMember(dest => dest.Name, opt => opt.MapFrom), del desti, destiname, per aquest membre agafa del origen -> opt

/**/
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// usa Cors, pero habilita per tot allowed-origin
app.UseCors();

// usa autentication en la app
app.UseAuthentication();
app.UseAuthorization();

/**/
app.MapControllers();

app.Run();
