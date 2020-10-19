namespace SQLiteDapperApi
{
	using Microsoft.AspNetCore.Builder;
	using Microsoft.AspNetCore.Hosting;
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.Extensions.Configuration;
	using Microsoft.Extensions.DependencyInjection;
	using SQLiteDapperApi.Database;
	using SQLiteDapperApi.Repositories;
	using System;

	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Add database related services	
			services.AddSingleton(new DatabaseConfig { Name = Configuration["DatabaseName"] });
			services.AddSingleton<IDatabaseBootstrap, DatabaseBootstrap>();
			services.AddSingleton<IProductRepository, ProductRepository>();
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(
			IApplicationBuilder app,
			IHostingEnvironment env,
			IServiceProvider serviceProvider)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseMvc();

			serviceProvider.GetService<IDatabaseBootstrap>().Setup();
		}
	}
}
