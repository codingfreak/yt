using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
var config = builder.Configuration;
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
	});
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(
		opt =>
		{
			opt.OAuthClientId(builder.Configuration["AzureAd:ClientId"]);
		});
}
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseAuthorization();
app.MapControllers();
app.Run();
