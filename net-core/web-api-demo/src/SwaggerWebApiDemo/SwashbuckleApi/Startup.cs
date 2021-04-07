namespace SwashbuckleApi
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using Microsoft.Extensions.Hosting;
	using Microsoft.OpenApi.Models;

	public class Startup
	{
		public static string ApiTitle = "Swashbuckle Demo API";
		public static string ApiVersion = "v1";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			// Registering the Swagger generator, defining one or more Swagger documents.
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = ApiTitle, Version = ApiVersion });
			});

			//TODO: Explicit opt-in to ensure that Newtonsoft settings/attributes are automatically honored by the Swagger generator
			// Error: 'IServiceCollection' does not contain a definition for 'AddSwaggerGenNewtonsoftSupport'
			//services.AddSwaggerGenNewtonsoftSupport();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			// Inserting middle-ware to expose the generated Swagger as JSON endpoint(s)
			app.UseSwagger();

			// Optionally, inserting the swagger-ui middle-ware if you want to expose interactive documentation,
			// specifying the Swagger JSON endpoint(s) to power it from.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("v1/swagger.json", $"{ApiTitle} {ApiVersion}");
			});
		}
	}
}
