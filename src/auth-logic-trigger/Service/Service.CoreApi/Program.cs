using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllers();
builder.Services.AddAuthorization(
	options =>
	{
		options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
			.Build();
	});
builder.Services.AddHealthChecks();
builder.Services.AddApiVersioning(options =>
{
	options.ReportApiVersions = true;
	options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
});
builder.Services.AddVersionedApiExplorer(options =>
{
	options.GroupNameFormat = "'v'VVV";
	options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen(
	c =>
	{
		var authUrl = new Uri($"{config["AzureAd:Instance"]}{config["AzureAd:TenantId"]}/oauth2/v2.0/authorize");
		var scopes = new Dictionary<string, string>
		{
			{ config["AzureAd:Scopes"], "Full access" }
		};
		c.AddSecurityDefinition(
			nameof(SecuritySchemeType.OAuth2),
			new OpenApiSecurityScheme
			{
				Type = SecuritySchemeType.OAuth2,
				Flows = new OpenApiOAuthFlows
				{
					Implicit = new OpenApiOAuthFlow { AuthorizationUrl = authUrl, Scopes = scopes }
				}
			});
		c.AddSecurityRequirement(
			new OpenApiSecurityRequirement
			{
				{
					new OpenApiSecurityScheme
					{
						Name = nameof(SecuritySchemeType.OAuth2),
						Reference = new OpenApiReference
						{
							Type = ReferenceType.SecurityScheme, Id = nameof(SecuritySchemeType.OAuth2)
						}
					},
					new[] { config["AzureAd:Scope"] }
				}
			});
		var provider = builder.Services.BuildServiceProvider().GetService<IApiVersionDescriptionProvider>();
		if (provider != null)
		{
			foreach (var description in provider.ApiVersionDescriptions
				         .OrderByDescending(v => v.ApiVersion.MajorVersion)
				         .ThenByDescending(v => v.ApiVersion.MinorVersion))
			{
				var versionInfo = new OpenApiInfo { Title = $"My API {description.GroupName}", Version = "1.0"};
				c.SwaggerDoc(description.GroupName, versionInfo);
			}
		}
		var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
		var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFile);
		c.IncludeXmlComments(xmlFilePath);
	});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(
		opt =>
		{
			opt.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
			opt.DocumentTitle = "Hello";
		});
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();
app.UseHealthChecks("/health");
app.Run();
