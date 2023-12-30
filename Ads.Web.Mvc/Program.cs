using App.Business;
using App.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;



        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddAutoMapper(typeof(Program));



        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            var cs = builder.Configuration.GetConnectionString("DefaultConnection");
            options.UseSqlServer(cs);
        });

        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.LoginPath = "/auth/login";
            });



        builder.Services.AddSingleton<IHashService, HashService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<IAuthService, CookieAuthService>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IAdvertService, AdvertService>();


        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();


        app.Run();
