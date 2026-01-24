using ISEMES.API.Services;
using log4net.Config;
using log4net;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ISEMES.Repositories;
using ISEMES.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configure log4net
var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

// Add log4net to logging
builder.Logging.ClearProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Add DbContext services
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryConnection")), ServiceLifetime.Scoped);
builder.Services.AddDbContext<TFSDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("InventoryTFS_Prod2Connection")), ServiceLifetime.Scoped);

// Register the services
builder.Services.AddScoped<TokenProvider>();

// User
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Master Data
builder.Services.AddScoped<IMasterDataRepository, MasterDataRepository>();
builder.Services.AddScoped<IMasterDataService, MasterDataService>();

// Shipment
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();

// Checkin/Checkout
builder.Services.AddScoped<ICheckinCheckoutInventoryRepository, CheckinCheckoutInventoryRepository>();
builder.Services.AddScoped<ICheckinCheckoutInventoryService, CheckinCheckoutInventoryService>();

// Customer Orders
builder.Services.AddScoped<ICustomerOrderRepository, CustomerOrderRepository>();
builder.Services.AddScoped<ICustomersOrderService, CustomersOrderService>();

// Receiving
builder.Services.AddScoped<IReceivingRepository, ReceivingRepository>();
builder.Services.AddScoped<IReceivingService, ReceivingService>();

// Inventory Data
builder.Services.AddScoped<IInventoryDataRepository, InventoryDataRepository>();
builder.Services.AddScoped<IInventoryDataService, InventoryDataService>();

// Hold Inventory
builder.Services.AddScoped<IHoldInventoryRepositories, HoldInventoryRepository>();
builder.Services.AddScoped<IHoldInventoryService, HoldInventoryService>();

// Ticket
builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<ITicketService, TicketService>();

// Inventory Report
builder.Services.AddScoped<IInventoryReportRepository, InventoryReportRepository>();
builder.Services.AddScoped<IInventoryReportService, InventoryReportService>();

// Combination Lot
builder.Services.AddScoped<ICombinationLotRepository, CombinationLotRepository>();
builder.Services.AddScoped<ICombinationLotService, CombinationLotService>();

// Other Inventory
builder.Services.AddScoped<IOtherInventoryRepository, OtherInventoryRepository>();
builder.Services.AddScoped<IOtherInventoryService, OtherInventoryService>();

// Internal Transfer Receiving
builder.Services.AddScoped<IIntTranferReceivingRepository, IntTranferReceivingRepository>();
builder.Services.AddScoped<IIntTranferReceivingService, IntTranferReceivingService>();

// Split Merge
builder.Services.AddScoped<ISplitMergeRepository, SplitMergeRepository>();
builder.Services.AddScoped<ISplitMergeService, SplitMergeService>();

// Device Master
builder.Services.AddScoped<IDeviceMasterRepository, DeviceMasterRepository>();
builder.Services.AddScoped<IDeviceMasterService, DeviceMasterService>();

// Probe Card (Hardware)
builder.Services.AddScoped<IProbeCardRepository, ProbeCardRepository>();
builder.Services.AddScoped<IProbeCardService, ProbeCardService>();

// Configure JSON serialization to use camelCase for property names
// This ensures frontend camelCase properties (e.g., usHtsCodeId) map correctly to backend PascalCase (USHTSCodeId)
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });
builder.Services.AddEndpointsApiExplorer();

// Add Swagger and configure Bearer token authorization with separate definitions
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("inventory", new OpenApiInfo 
    { 
        Title = "ISEMES Inventory API", 
        Version = "v1",
        Description = "API for Inventory Management System"
    });
    
    c.SwaggerDoc("ticketing", new OpenApiInfo 
    { 
        Title = "ISEMES Ticketing API", 
        Version = "v1",
        Description = "API for Ticketing System"
    });
    
    c.SwaggerDoc("devicemaster", new OpenApiInfo 
    { 
        Title = "ISEMES Device Master API", 
        Version = "v1",
        Description = "API for Device Master Management"
    });
    
    c.SwaggerDoc("hardware", new OpenApiInfo 
    { 
        Title = "ISEMES Hardware API", 
        Version = "v1",
        Description = "API for Hardware Management"
    });

    // Configure Swagger to use the Authorization header with Bearer token
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your valid token in the text input below.\n\nExample: \"abcdef12345\""
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

var app = builder.Build();
app.UseCors("AllowAllOrigins");

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/inventory/swagger.json", "ISEMES Inventory API");
    c.SwaggerEndpoint("/swagger/ticketing/swagger.json", "ISEMES Ticketing API");
    c.SwaggerEndpoint("/swagger/devicemaster/swagger.json", "ISEMES Device Master API");
    c.SwaggerEndpoint("/swagger/hardware/swagger.json", "ISEMES Hardware API");
});

app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

