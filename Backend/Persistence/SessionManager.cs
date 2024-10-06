using Cassandra;
using Cassandra.Mapping;
using Models;

namespace Persistence;

internal static class SessionManager {

    static ISession Session { get; set; } = null!;

    internal static ISession GetSession() {
        
        if (Session == null) {
            var usersByUsernameMap = new Map<User>()
                .TableName("users_by_username")
                .PartitionKey(u => u.Username)
                .ClusteringKey(u => u.UserId)
                .Column(u => u.UserId, cm => cm.WithName("user_id"))
                .Column(u => u.Username, cm => cm.WithName("username"))
                .Column(u => u.Password, cm => cm.WithName("password"));

            var usersByEmailMap = new Map<User>()
                .TableName("users_by_id")
                .PartitionKey(u => u.UserId)
                .ClusteringKey(u => u.Username)
                .Column(u => u.UserId, cm => cm.WithName("user_id"))
                .Column(u => u.Username, cm => cm.WithName("username"))
                .Column(u => u.Password, cm => cm.WithName("password"));

            MappingConfiguration.Global.Define(usersByUsernameMap);
            MappingConfiguration.Global.Define(usersByEmailMap);

            var cluster = Cluster.Builder()
                                 .AddContactPoints("127.0.0.1")
                                 .Build();
            Session = cluster.Connect("docs");
        }

        return Session;
    } 
    
}