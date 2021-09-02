using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebChat.ViewModels.AppUser
{
	public class AddUser
	{
		[Required(ErrorMessage ="Du lieu nay la bat buoc")]
		[MinLength(3, ErrorMessage ="Du lieu qua ngan")]
		[MaxLength(200,ErrorMessage ="Du lieu qua dai")]
		public string Username { get; set; }


		[Required(ErrorMessage = "Du lieu nay la bat buoc")]
		[DataType(DataType.Password)] //dau pass *****
		public string Password { get; set; }

		[DisplayName("Xac nhan mat khau")]
		[DataType(DataType.Password)] 
		[Compare (nameof(Password), ErrorMessage ="Mat khau khong khop")]
		public string ConfirmPassword { get; set; }

		[Display(Name = "Ten day du")]
		[Required(ErrorMessage = "Du lieu nay la bat buoc")]
		public string Fullname { get; set; }
	}
}
