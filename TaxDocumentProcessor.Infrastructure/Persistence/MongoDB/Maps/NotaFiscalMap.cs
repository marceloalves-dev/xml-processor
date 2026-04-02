using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using TaxDocumentProcessor.Domain.Entities;

namespace TaxDocumentProcessor.Infrastructure.Persistence.MongoDB.Maps
{
    public static class NotaFiscalMap
    {
        private static bool _registered = false;
        private static readonly object _lock = new();

        public static void Configure()
        {
            lock (_lock)
            {
                if (_registered) return;

                BsonClassMap.RegisterClassMap<NotaFiscal>(map =>
                {
                    map.AutoMap();
                    map.MapIdMember(n => n.Id)
                       .SetSerializer(new GuidSerializer(BsonType.String));
                    map.SetIsRootClass(true);
                });

                BsonClassMap.RegisterClassMap<Nfe>();
                BsonClassMap.RegisterClassMap<Cte>();
                BsonClassMap.RegisterClassMap<Nfse>();

                _registered = true;
            }
        }
    }
}
