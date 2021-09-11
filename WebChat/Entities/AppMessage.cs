using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Entities
{
	[Table("AppMessage")]
	public class AppMessage
	{
		public long Id { get; set; }
		[MaxLength(1000)]
		public string Message { get; set; }
		public DateTime SendAt { get; set; }
		public int SenderId { get; set; }
		public int ReciverId { get; set; }

		public virtual AppUser Sender { get; set; }

		public virtual AppUser Reciver { get; set; }

	}
}
