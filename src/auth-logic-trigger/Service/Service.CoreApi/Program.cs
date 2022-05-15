using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// read out config values
var config = builder.Configuration;
var configInstance = config["AzureAd:Instance"];
var configTenantId = config["AzureAd:TenantId"];
var configScopes = config["AzureAd:Scopes"];
var configClientId = config["AzureAd:ClientId"];
// configure services
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
builder.Services.AddSwaggerGen(
	c =>
	{
		var authUrl = new Uri($"{configInstance}{configTenantId}/oauth2/v2.0/authorize");
		var scopes = new Dictionary<string, string> { { configScopes, "Full access" } };
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
					new[] { configScopes }
				}
			});
	});
var app = builder.Build();
// configure app
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerUI(
		opt =>
		{
			opt.OAuthClientId(configClientId);
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
