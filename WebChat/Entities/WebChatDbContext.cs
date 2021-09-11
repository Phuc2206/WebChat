using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.Entities
{
	public class WebChatDbContext : DbContext
	{
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<AppMessage> AppMessages {get; set;}

		public WebChatDbContext(DbContextOptions options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<AppMessage>()
				.HasOne(m => m.Sender)
				.WithMany(u => u.SendMessages)
				.HasForeignKey(m => m.SenderId)
				.OnDelete(DeleteBehavior.NoAction);

			modelBuilder.Entity<AppMessage>()
				.HasOne(m => m.Reciver)
				.WithMany(u => u.ReciveMessages)
				.HasForeignKey(m => m.ReciverId)
				.OnDelete(DeleteBehavior.NoAction);
		}
	}
}
