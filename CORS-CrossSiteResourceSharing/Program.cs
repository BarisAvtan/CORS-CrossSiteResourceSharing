using Microsoft.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//EXAMPLE 1

//builder.Services.AddCors(opts =>
//{
//    opts.AddDefaultPolicy(builder =>
//    {
//        builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();//Hangi orjinden gelirse gelsin,header'ı ne olursa olsun, metodu ne olursa olsun get veya post hepisini kabul et!

//    });
//});

//EXAMPLE 2

string ApiCorsPolicy = "AllowSites";

//builder.Services.AddCors(opts =>
//{
//    opts.AddPolicy(ApiCorsPolicy, builder =>
//    {
//        builder.WithOrigins("https://localhost:7159/", "https://localhost:1010/").AllowAnyHeader().AllowAnyMethod();
//    });
//});


//builder.Services.AddCors(options => options.AddPolicy(name: "AllowSites", builder => {
//    builder.WithOrigins("https://localhost:7159/", "https://localhost:1010/").AllowAnyHeader().AllowAnyMethod();
//}));


builder.Services.AddCors(options =>
{
    //1- sadece https://localhost:7159", "http://localhost:4200" bu urlleden istek gelebilir bunun dısındaki urllerden istek  gelirse kabul etmez.
    options.AddPolicy(name: "AllowSites",
        builder =>
        {
            builder.WithOrigins("https://localhost:7159", "http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });


    //2-bu urllerden istek gelebilir ve geldiğinde headercontent type 'ı x-custom-header olmalı.
    options.AddPolicy(name: "AllowSites2",
        builder =>
        {
            builder.WithOrigins("https://localhost:1010", "http://localhost:1212").WithHeaders(HeaderNames.ContentType, "x-custom-header");

        });


    //3-sadece subdomainlerden gelen istekler için

    options.AddPolicy(name: "SubdomainPolicy",
    builder =>
    {
        builder.WithOrigins("https//*.arv.com", "https//*.example.com").SetIsOriginAllowedToAllowWildcardSubdomains().AllowAnyHeader().AllowAnyMethod();

    });

    //4-sadece get ve post metodlarına izin verme
    options.AddPolicy(name: "UseGetPostMethod",
    builder =>
    {
     builder.WithOrigins("https//example.com").WithMethods("POST", "GET").AllowAnyHeader();

    });

    //app.UseCors(); parantez içersinde hiç birşey belirtilmezse uyguluma içinde istedigin policiy name i kullanabilirsin ama isim yazarsan sadece o policy çalışacaktır.


});


builder.Services.AddControllers();
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

app.UseHttpsRedirection();

//app.UseAuthorization();

////EXAMPLE 2
//app.UseCors("AllowSites");

//EXAMPLE 1
app.UseCors();//default policy yani kurallar (yani yukarda yazdıgımız) usercors() içine bişey yazılmaz
//[EnableCors("AllowSites")] controller üstüne eklendi.Dikkat ! 
//[HttpGet(Name = "GetWeatherForecast")]

app.MapControllers();

app.Run();
