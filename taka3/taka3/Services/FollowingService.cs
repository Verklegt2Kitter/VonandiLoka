using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using taka3.Models;
using System.Web.Security;


namespace taka3.Services
{
    public class FollowingService
    {
        ApplicationDbContext m_db = new ApplicationDbContext();

        //fanney/steindór fengu hjálp og til varð follow 
        public List<FriendModel> GetAllFriendList()
        {
            return m_db.FriendModel.ToList();
        }

        public void AddFollowingToUser(string thisUser, string userToFollow)
        {

            var userService = new UserService(m_db);

            var followMe = new FriendModel();

            followMe.User = userService.GetThisUserById(thisUser); // Application user.
            followMe.FollowingUserId = userToFollow;
            followMe.isFollowing = true;

            m_db.FriendModel.Add(followMe);
            m_db.SaveChanges();
        }
        public List<FindFriendNameModelView> MyFollowingList(string userId)
        {
            var userService = new UserService();

            var friends = GetAllFriendList();
            var users = userService.GetAllUsers();

            var friendList = new List<FindFriendNameModelView>();

            foreach(var friend in friends)
            {
                if (friend.User.Id == userId)
                {
                    var user = users.Where(x => x.Id == friend.FollowingUserId).FirstOrDefault();
                    if (user != null)
                    {
                        friendList.Add(new FindFriendNameModelView
                        {
                            UserFirstName = user.FirstName,
                            UserId = user.Id
                        });
                    }
                }
            }

            return friendList;
        }
        public FriendModel GetUserFriendInfoById(int id)
        {
            var list = GetAllFriendList();
            var thisUser = (from u in list
                            where u.Id == id
                            select u).SingleOrDefault();

            return thisUser;
        }
        public void UnFollwUser(string userId, string stopFollowId)
        {
            var list = MyFollowingList(userId);
            var stopFollow = (from f in list
                              where f.UserId == stopFollowId
                              select f).SingleOrDefault();

            var unfollow = GetUserFriendInfoById(stopFollowId);

//            m_db.FriendModel.Remove(unfollow);

            m_db.SaveChanges();
        }

        internal object GetUserFriendInfoById(string userid)
        {
            throw new NotImplementedException();
        }
    }
}