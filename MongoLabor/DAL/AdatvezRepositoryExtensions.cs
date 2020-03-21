using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace MongoLabor.DAL
{
    public static class AdatvezRepositoryExtensions
    {
        public static void AddAdatvezRepository(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var configuration = serviceProvider.GetRequiredService<IConfiguration>();
                var connectionString = configuration.GetConnectionString("Mongo");
                return new MongoClient(connectionString);
            });

            services.AddSingleton(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return client.GetDatabase("aaf");
            });

            services.AddTransient<IAdatvezRepository, AdatvezRepository>();

            var pack = new ConventionPack
            {
                new AAFElementNameConvention(),
            };
            ConventionRegistry.Register("AAFConventions", pack, _ => true);
        }
    }
}
