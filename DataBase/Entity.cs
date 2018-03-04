using System;
using System.ComponentModel.DataAnnotations;

namespace DataBase {
	public class Entity {
        [Key]
		public long Id { get; set; }
        public DateTime Date { get; set; }
        public virtual double Value() => 0;
    }
}
