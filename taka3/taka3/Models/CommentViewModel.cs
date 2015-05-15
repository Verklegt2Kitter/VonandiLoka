using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace taka3.Models
{
	public class CommentViewModel
	{
		ApplicationDbContext m_db = new ApplicationDbContext();

		public List<CommentViewModel> Comments { get; set; }
		public int ID { get; set; }
		public int PostID { get; set; }
		public string UserID { get; set; }
		public DateTime DateAndTime { get; set; }
		public string Image { get; set; }
		public string ImageName { get; set; }
		public string PostBody { get; set; }
		public string CommentBody { get; set; }
		public string CommenterFirstName { get; set; }
		public string CommenterLastName { get; set; }
	}
}