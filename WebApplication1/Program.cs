
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(o =>

{
    o.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = false,
        ValidateIssuerSigningKey = true,
    };
});






var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.MapPost("/security/createToken",
    [AllowAnonymous] (UserRequest user) =>

    {
        if (user.UserName == "aracely" && user.Password == "123456")
        {
            var issuer = builder.Configuration["Jwt:Issuer"];
            var audience = builder.Configuration["Jwt:Audience"];
            var key = Encoding.ASCII.GetBytes
            (builder.Configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor

            {
                Subject = new ClaimsIdentity(new[]
                {
                new Claim("Rol", "Administrador"),
                new Claim("Id", Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email
                , user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())

            }),

                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)

            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            return Results.Ok(stringToken);

        }
        return Results.Unauthorized();
    });

app.Run();

[Authorize]
[HttpGet(Name = "Get3")]

public IEnumerable <PersonResponse> Get3()
{
    List<PersonResponse> personas = new List<PersonResponse>();

    for (int i = 1; i <= 100; i++)
    {
        PersonResponse persona = new PersonResponse();
        persona.FirstName = "Persona" + i;
        persona.LastName = "Apellido" + i;

        personas.Add(persona);

    }
    return personas;

}

[Authorize ("Administrador")]
[HttpGet(Name = "Get")]

    public IEnumerable <PersonResponse> Get()

    { 
        List <PersonResponse> personas = new List<PersonResponse>();
        for (int i = 1; i <= 100; i++)

            {
                PersonResponse persona = new PersonResponse();
                    persona.FirstName = "Persona" + i;
                    persona.LastName = "Apellido" + i; 


                    personas.Add(persona);
            }

    return personas;
    }

[Authorize("Vendedor")]
[HttpGet(Name = "Get2")]

public IEnumerable <PersonResponse> Get2()
{
    List<PersonResponse> personas = new List<PersonResponse>();
    for (int i = 1; i <= 100; i++)

    {
        PersonResponse persona = new PersonResponse();
        persona.FirstName = "Persona" + i;
        persona.LastName = "Apellido" + i;


        personas.Add(persona);
    }
    return personas;
}



app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
