using MongoDB.Bson.Serialization.Attributes;

namespace DataBase {
	public class Entity {
		[BsonId]
		public long Id { get; set; }
	}
}
