using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace taka3.Models
{
    //Reyna að gera nýjan lista til að geta tekið inn meira en einn lista.... sjá homecontrlller newsfeed

    public class AddFriendToGroupModelView
    {
        public List<FindFriendNameModelView> GetAllFriends { get; set; }
    }

    public class FindFriendNameModelView
    {
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserId { get; set; }
    }
}