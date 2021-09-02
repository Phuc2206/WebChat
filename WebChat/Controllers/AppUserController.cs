using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebChat.Entities;
using WebChat.ViewModels.AppUser;

namespace WebChat.Controllers
{
	public class AppUserController : Controller
	{

		private readonly WebChatDbContext db;
		public AppUserController(WebChatDbContext _db)
		{
			this.db = _db ;
		}
		public IActionResult SignUp()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> SignUp(AddUser model)
		{
			AppUser user = new AppUser();
			if (ModelState.IsValid) //neu du lieu hop le
			{
				try
				{
					//mã hóa mật khẩu
					HMACSHA512 hmac = new();
					var pwByte = Encoding.UTF8.GetBytes(model.Password);
					user.PasswordHash = hmac.ComputeHash(pwByte);
					user.PasswordSalt = hmac.Key;

					//sao chep du lieu
					user.Username = model.Username.Trim().ToLower();
					user.Fullname = model.Fullname;
					user.CreateDate = DateTime.Now;

					//Luu du lieu
					await db.AppUsers.AddAsync(user);
					await db.SaveChangesAsync();
				}
				catch
				{
					@TempData["Mesg"] = "Error!!!!!";
				}	
			}
			else
			{
				TempData["Mesg"] = "da xay ra loi trong qua trinh dang ky tai khoan";
			}
			return RedirectToAction(nameof(SignUp));
		}
		//Dang nhap
		public IActionResult Login()
			{
				return View();
			}
		[HttpPost]
		public async Task<IActionResult> Login(Login loginData)
		{
			loginData.Username = loginData.Username.Replace(" ", "").ToLower();
			var user = db.AppUsers
				.AsNoTracking()
				.SingleOrDefault(u => u.Username == loginData.Username);
			if(user != null)
			{
				HMACSHA512 hmac = new HMACSHA512(user.PasswordSalt);
				var pwByte = Encoding.UTF8.GetBytes(loginData.Password);
				var pwHash = hmac.ComputeHash(pwByte);
				if (pwHash.SequenceEqual(user.PasswordHash))
				{
					//luu thong tin cho phien dang nhap nay
					var claims = new List<Claim>()
					{
						new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
						new Claim(ClaimTypes.Name, user.Username),
						new Claim(ClaimTypes.GivenName, user.Fullname)
					};
					var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
					var principal = new ClaimsPrincipal(claimsIdentity);
					var authProperties = new AuthenticationProperties
					{
						ExpiresUtc = DateTime.UtcNow.AddHours(6),
						IsPersistent = loginData.RememberMe
					};
					await HttpContext.SignInAsync("Cookies", principal, authProperties);
					//ve trang dang nhap
					return RedirectToAction("Index", "Home");
				}
				else
				{
					TempData["msg"] = "Ten dang nhap hoac mat khau khong chinh xac !!!!!";
				}
			}
			else
			{
				TempData["msg"] = "Ten dang nhap hoac mat khau khong chinh xac !!!!!";
			}
			return RedirectToAction(nameof(Login));
		}


		public async Task<IActionResult> LogOut()
		{
			await HttpContext.SignOutAsync("Cookies");
			return RedirectToAction(nameof(Login));
		}
	}
}
