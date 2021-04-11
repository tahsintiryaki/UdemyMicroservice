using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Models
{
    public class Course
    {
        [BsonId] //Property'nin id olarak algınlanması için eklendi.
        [BsonRepresentation(BsonType.ObjectId)] //MongoDb de tutulan ıd'yi string olarak tutacak
        public string Id { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
        public string Name { get; set; }
        public string Picture { get; set; }
        public string Description { get; set; }

        public string UserId { get; set; }

        [BsonRepresentation(BsonType.DateTime)]
        public DateTime CreatedTime { get; set; }

        public Feature Feature { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }

        [BsonIgnore] //Bu prop mongo db'ye yansımasın. Ben bu propertyi sadece proje içeirisinde kullanacağım.
        public Category Category { get; set; }
    }
}
