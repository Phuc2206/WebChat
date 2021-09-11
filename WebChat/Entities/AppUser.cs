using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Entities
{
	[Table(nameof(AppUser))]
	[Index("Username", IsUnique = true)]
	public class AppUser
	{
		
		public int Id { get; set; }


		[Required]
		[MaxLength(200)]
		public string Username { get; set; }
		[Required]
		[MaxLength(200)]
		public byte[] PasswordHash { get; set; }
		[Required]
		[MaxLength(200)]
		public byte[] PasswordSalt { get; set; }
		[Required]
		[MaxLength(200)]
		public string Fullname { get; set; }

		public DateTime? CreateDate { get; set; } //allow null

		public virtual ICollection<AppMessage> SendMessages { get; set; }
		public virtual ICollection<AppMessage> ReciveMessages { get; set; }

		public AppUser()
		{
			SendMessages = new HashSet<AppMessage>();
			ReciveMessages = new HashSet<AppMessage>();
		}

	}
}
