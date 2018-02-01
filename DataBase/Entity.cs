using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataBase {
	public class Entity {
		[BsonId]
		public ObjectId Id { get; set; }
	}
}
