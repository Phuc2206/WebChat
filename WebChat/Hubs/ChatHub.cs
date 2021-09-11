using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebChat.Common;
using WebChat.Entities;

namespace WebChat.Hubs
{
	
	public class ChatHub : Hub

	{
		readonly WebChatDbContext db;
		public ChatHub(WebChatDbContext _db)
		{
			db = _db;
		}
		public async Task SendMessage(string targetUserId, string message)
		{
			var currentUserId = Context.UserIdentifier;
			var users = new string[] {currentUserId, targetUserId};
			var response = new
			{
				sender = currentUserId,
				reciver = targetUserId,
				mesg = message,
				datetime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss")

			};
			//Lưu tin nhắn vào database
			AppMessage mesg = new AppMessage()
			{
				Message = AESThenHMAC.SimpleEncryptWithPassword(message,AppConfig.MESG_KEY),
				SendAt= DateTime.Now,
				ReciverId = Convert.ToInt32(targetUserId),
				SenderId = Convert.ToInt32(currentUserId),
			};
			await db.AddAsync(mesg);
			await db.SaveChangesAsync();
			await Clients.Users(users).SendAsync("ReceiveMessage", response);
		}
		static List<int> onlineUsers = new List<int>();
		public override async Task<Task> OnConnectedAsync()
		{
			var currentUserId = Context.UserIdentifier;
			onlineUsers.Add(Convert.ToInt32(currentUserId));
			var response = new
			{
				onlineUsers
			};
			await Clients.All.SendAsync("GetUsers", response);
			return base.OnConnectedAsync();
		}
		public override async Task<Task> OnDisconnectedAsync(Exception exception)
		{
			var currentUserId = Context.UserIdentifier;
			onlineUsers.Remove(Convert.ToInt32(currentUserId));

			var response = new
			{
				onlineUsers,
				disconnectedId = Convert.ToInt32(currentUserId)
			};
			await Clients.All.SendAsync("GetUsers", response);
			return base.OnDisconnectedAsync(exception);
		}

	}
}
