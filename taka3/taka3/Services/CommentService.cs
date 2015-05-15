using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using taka3.Models;

namespace taka3.Services
{
	public class CommentService
	{
		ApplicationDbContext m_db = new ApplicationDbContext();

		public void AddComment(Comment post)
		{
			if (post != null)
			{
				m_db.Comments.Add(post);
				m_db.SaveChanges();
			}
		}

		public IEnumerable<Comment> GetAllCommentsForPost(int postid)
		{
			var result = (from item in m_db.Comments
						  where item.PostID == postid
						  orderby item.DateAndTime descending
						  select item).ToList().Take(10);
			return result;
		}

		public string GetCommenterID(int commentid)
		{
			var result = (from item in m_db.Comments
						  where item.ID == commentid
						  select item.UserID).SingleOrDefault();
			return result;
		}
	}
}