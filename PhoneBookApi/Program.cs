using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using PhoneBookApi.Data;
using PhoneBookApi.Interfaces;
using PhoneBookApi.Mappings;
using PhoneBookApi.Repositories;
using PhoneBookApi.Services;
using PhoneBookApi.Validation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<IContactsService, ContactsService>();
builder.Services.AddScoped<IContactsRepository, ContactsRepository>();
builder.Services.AddAutoMapper(typeof(EntitiesToDTOMappingProfile));
// builder.Services.AddFluentValidationAutoValidation();
// builder.Services.AddFluentValidationClientsideAdapters();
// builder.Services.AddValidatorsFromAssemblyContaining<ContactsValidation>();
#pragma warning disable CS0618 // Type or member is obsolete
builder.Services.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ContactsValidation>());
#pragma warning restore CS0618 // Type or member is obsolete
builder.Services.AddScoped<IContactsValidation, ContactsValidation>();

builder.Services.AddDbContext<PhoneBookContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 40)) // Altere para sua versão do MySQL
    ));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
