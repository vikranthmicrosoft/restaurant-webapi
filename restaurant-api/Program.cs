using Amazon.DynamoDBv2;
using Amazon.SimpleEmail;
using restaurant_api.Common.Middlewares;
using restaurant_api.Common.Service.Implementation;
using restaurant_api.Common.Service.Interface;
using restaurant_api.Services.Implementation;
using restaurant_api.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var allowedOrigin = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("cors", policy =>
    {
        policy.WithOrigins(allowedOrigin)
                .AllowAnyHeader()
                .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
//builder.Services.AddScoped<GlobalExceptionHandlerMiddleware>();
// Configure AWS services
builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddAWSService<IAmazonDynamoDB>();
builder.Services.AddAWSService<IAmazonSimpleEmailService>();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("cors");


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.Run();
