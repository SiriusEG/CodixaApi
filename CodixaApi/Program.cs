
namespace CodxiaApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.ConfigureServices(builder.Configuration);
      

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            //if (app.Environment.IsDevelopment())
           // {
                app.UseSwagger();
                app.UseSwaggerUI();
            //}

            app.UseStaticFiles();
            //Setting Core Policy

            app.UseCors("MyPolicy");
            app.UseHttpsRedirection();
          
            app.UseAuthentication();
            app.UseAuthorization();
    
            app.MapControllers();
      

            app.Run();
        }
    }
}
