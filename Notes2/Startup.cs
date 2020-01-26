using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Notes2.Data;
using Notes2.Domain;
using Notes2.Models;
using Notes2.Repositories;

namespace Notes2
{
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
            //services.Configure<CookiePolicyOptions>(options =>
            //{
            //    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //    options.CheckConsentNeeded = context => true;
            //    options.MinimumSameSitePolicy = SameSiteMode.None;
            //});

            services.AddDbContext<DbNoteContext>(c => c.UseMySql(Configuration.GetConnectionString("NoteConnMySQL")));
            //services.AddDbContext<DbNoteContext>(c => c.UseSqlServer(Configuration.GetConnectionString("NoteConnSQL")));
            //services.AddDbContext<DbNoteContext>(c => c.UseFirebird(Configuration.GetConnectionString("NoteConnFirebird")));

            services.AddScoped<UsuarioRepository>();
            services.AddScoped<CategoriaRepository>();
            services.AddScoped<NotaRepository>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUser, AspNetUser>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme =
                options.DefaultSignInScheme =
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie(options => options.LoginPath = "/auth/signin");

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(@".\key\"))
                .SetDefaultKeyLifetime(TimeSpan.FromDays(14));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Home/Error");
            //}

//            Migrate.DoMigration(app);

            app.UseStaticFiles();
//            app.UseSession();
            app.UseCookiePolicy();
            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute("edit", "{controller}/editar/{id}", new { Action = "AddEdit" });
                routes.MapRoute("add", "{controller}/add", new { Action = "AddEdit" });
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Notas}/{action=Index}/{id?}");
            });
        }
    }
}
