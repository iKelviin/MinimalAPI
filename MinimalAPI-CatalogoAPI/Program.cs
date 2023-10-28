using Azure.Core;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MinimalAPI_CatalogoAPI.ApiEndpoints;
using MinimalAPI_CatalogoAPI.AppServicesExtensions;
using MinimalAPI_CatalogoAPI.Context;
using MinimalAPI_CatalogoAPI.Models;
using MinimalAPI_CatalogoAPI.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddApiSwagger();
builder.AddPersistence();
builder.Services.AddCors();
builder.AddAutenticationJwt();

var app = builder.Build();


//EndPoints
app.MapAutenticacaoEndpoints();
app.MapCategoriasEndpoints();
app.MapProdutosEndpoints();


// Configure the HTTP request pipeline.
var environment = app.Environment;
app.UseExceptionHandling(environment)
    .UseSwaggerMiddleware()
    .UseAppCors();


app.UseAuthentication();
app.UseAuthorization();

app.Run();

