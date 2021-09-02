using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebChat.Entities;

namespace WebChat.Controllers
{
	public class HomeController : Controller
	{
		private readonly WebChatDbContext db;
		public HomeController(WebChatDbContext _db)
		{
			db = _db;
		}
		public async Task<IActionResult> Index()
		{
			if (User.Identity.IsAuthenticated)
			{
				var currentUserName = HttpContext.User.Identity.Name;
				var listUser = await db.AppUsers
									.AsNoTracking()
									.Where(u => u.Username != currentUserName)
									.ToListAsync();
				return View("Chat", listUser);
			}
			else
			{
				return View();
			}
		}
	}
}
