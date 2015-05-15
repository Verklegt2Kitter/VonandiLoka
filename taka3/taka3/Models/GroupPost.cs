using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace taka3.Models
{
	//public class GroupStatus //láta erfa status
	public class GroupPost
	{
		// Klasi sem pósta textastöðu á hópasíðu ---- Fanney
		public int ID { get; set; }
		public string Body { get; set; }
		public DateTime DateAndTime { get; set; }
		public string GroupName { get; set; }
		public object UserName { get; set; }

        public GroupPost()
        {
            DateAndTime = DateTime.Now;
        }
	}
}