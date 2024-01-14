using BusinessLogic;
using Domain;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.DatabaseRepository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddDbContext<SqlContext>( options => options.UseSqlServer("name=ConnectionStrings:DefaultConnection"));

builder.Services.AddScoped<IRepository<User>, UserDatabaseRepository>();
builder.Services.AddScoped<IRepository<Category>, CategoryDatabaseRepository>();
builder.Services.AddScoped<IRepository<Account>, AccountDatabaseRepository>();
builder.Services.AddScoped<IRepository<Transaction>, TransactionDatabaseRepository>();
builder.Services.AddScoped<IRepository<SpendingGoals>, SpendingGoalsDatabaseRepository>();
builder.Services.AddScoped<IRepository<Space>, SpaceDatabaseRepository>();
builder.Services.AddScoped<IRepository<CreditCard>, CreditCardDatabaseRepository>();
builder.Services.AddScoped<IRepository<Exchange>, ExchangeDatabaseRepository>();

builder.Services.AddScoped<CreditCardLogic>();
builder.Services.AddSingleton<SessionLogic>();
builder.Services.AddScoped<LogInLogic>();
builder.Services.AddScoped<UserLogic>();
builder.Services.AddScoped<CategoryLogic>();
builder.Services.AddScoped<AccountLogic>();
builder.Services.AddScoped<TransactionLogic>();
builder.Services.AddScoped<SpendingGoalsLogic>();
builder.Services.AddScoped<SpaceLogic>();
builder.Services.AddScoped<ExchangeLogic>();

var app = builder.Build();

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();