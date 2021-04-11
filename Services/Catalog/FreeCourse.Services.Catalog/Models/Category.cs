using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Models
{
    public class Category
    {
        [BsonId] //Property'nin id olarak algınlanması için eklendi.
        [BsonRepresentation(BsonType.ObjectId)] //MongoDb de tutulan ıd'yi string olarak tutacak
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
