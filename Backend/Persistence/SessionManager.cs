using Cassandra;
using Cassandra.Mapping;
using Models;

namespace Persistence;

internal static class SessionManager {

    static ISession Session { get; set; } = null!;

    internal static ISession GetSession() {
        
        if (Session == null) {
            MappingConfiguration.Global.Define(
                new Map<User>().TableName("user")
                               .PartitionKey(u => u.UserId)
                               .Column(u => u.UserId, cm => cm.WithName("user_id"))
                               .Column(u => u.Username, cm => cm.WithName("username"))
                               .Column(u => u.Password, cm => cm.WithName("password")));

            var cluster = Cluster.Builder()
                                 .AddContactPoints("127.0.0.1")
                                 .Build();
            Session = cluster.Connect("docs");
        }

        return Session;
    } 
    
}