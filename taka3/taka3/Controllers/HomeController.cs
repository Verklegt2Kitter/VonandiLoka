using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using taka3.Models;
using taka3.Services;
using SecurityWebAppTest.Models;
using System.Web.Security;

namespace taka3.Controllers
{
    public class HomeController : Controller
    {
		ApplicationDbContext m_db = new ApplicationDbContext();

        public ActionResult Index()
        {
			IdentityManager manager = new IdentityManager();

			//Búa til nýjan user ef það er enginn til með UserName username	-Védís
			var passwordHash = new PasswordHasher();
			string password = passwordHash.HashPassword("Password@123");
			var user = new ApplicationUser
			{
				UserName = "SarahPalin",
				Email = "sarah@palin.com",
				PasswordHash = password,
				FirstName = "Sarah",
				LastName = "Palin",
				Country = "USA",
				BirthDate = DateTime.Parse("1948-02-05 17:30:00"),
				Gender = "female",
				LockoutEnabled = true
			};

			var temp = manager.GetUser("SarahPalin");
			if(temp == null)
			{
				manager.CreateUser(user, password);
			}



			//Sendir á Fréttaveituna(NewsFeed) ef notandi er skráður inn	-Védís
			if (Request.IsAuthenticated)
			{
				return RedirectToAction("NewsFeed");
			}
			else
			{
				return View();
			}
        }

		[Authorize]
        public ActionResult Groups()
        {
            ViewBag.Message = "Your application groups page.";

            return View();
        }

		[Authorize]
		public ActionResult NewsFeed()
		{
			NewsFeedViewModel model = new NewsFeedViewModel();
			IdentityManager manager = new IdentityManager();
			var post = new UserPostService();

			//Nær í 20 síðustu posta á síðunni og setur þá í lista með viðeigandi upplýsingum	-Védís
			model.AllUserPosts = (from p in post.GetAllUserPosts()
								  select new UserPostViewModel
								  {
									  ID = p.ID,
									  UserID = p.UserID,
									  //GroupID = p.GroupID,
									  PostBody = p.PostBody,
									  DateAndTime = p.DateAndTime,
									  Image = p.Image,
									  ImageName = p.ImageName
								  }).ToList();

			//Fer í gegnum listann og finnur FirstName og LastName notandans sem postaði	-Védís
			foreach (var item in model.AllUserPosts)
			{
				if(item != null)
				{
					var userIdTemp = post.GetUserId(item.ID);
					var userNameTemp = post.GetUserNameById(userIdTemp);
					var user = manager.GetUser(userNameTemp);

					item.UserFirstName = user.FirstName;
					item.UserLastName = user.LastName;
				}
				
			}

			//skilar model listanum	-Védís
			return View(model);
		}

		[Authorize]
		public ActionResult ProfilePage()
		{
			ProfilePageViewModel model = new ProfilePageViewModel();
			IdentityManager manager = new IdentityManager();
			var post = new UserPostService();

			var userId = User.Identity.GetUserId();
			var userName = User.Identity.GetUserName();

			model.UserPosts = post.GetPostsByUserId(userId).ToList();

			var user = manager.GetUser(userName);

			model.UserFirstName = user.FirstName;
			model.UserLastName = user.LastName;
            model.userID = user.Id;
			


			return View(model);
		}

		[Authorize]
		public ActionResult Hamburger()
		{
			ViewBag.Message = "Your settings page.";

			return View();
		}

       //fanney gerði heimasíðu hópa
		[Authorize]
        public ActionResult GroupProfilePage()
        {
            ViewBag.Message = "Group homepage.";

          //  ApplicationDbContext db = new ApplicationDbContext();

            //1. hvaða hópur --- Fanney, úr fyrirlestri Dabs

            // 2. sækja hópmeðlimi
            /*  var groupMembers = new List<string>();


               var statuses = (from s in db.GroupPosts
                               where groupMembers.Contains(s.UserName)
                               select s).ToList();*/
            return View();
        }
        //fanney, reynir að útfæra leitarbox
      // public static MembershipUserCollection FindUserByName(
         //  string userNameToMatch
          // );

		//Stjórnar Prófílsíðum annara notanda en þess sem er innskráður	-Védís
	   public ActionResult FriendProfilePage(string userid)
		{
			ProfilePageViewModel model = new ProfilePageViewModel();
			IdentityManager manager = new IdentityManager();
			var post = new UserPostService();

			var user = new UserService();
			var userName = user.GetUserNameFromID(userid);

			model.UserPosts = post.GetPostsByUserId(userid).ToList();

			var userinfo = manager.GetUser(userName);

			model.UserFirstName = userinfo.FirstName;
			model.UserLastName = userinfo.LastName;
			model.userID = userinfo.Id;

			return View(model);
		}
		
		public ActionResult PostComments(int postid)
	   {
			CommentViewModel model = new CommentViewModel();
			IdentityManager manager = new IdentityManager();

			var poster = new UserPostService();
			var comment = new CommentService();
			var post = new UserPostService();
			var commenter = new CommentService();

			var userid = poster.GetUserWhoPosted(postid);

			//Rennir í gegnum öll commentin sem eru á þessu postid og límir á þau rétt info	-Védís
			model.Comments = (from p in comment.GetAllCommentsForPost(postid)
								  select new CommentViewModel
								  {
									  ID = p.ID,
									  PostID = p.PostID,
									  UserID = p.UserID,
									  CommentBody = p.CommentBody,
									  DateAndTime = p.DateAndTime
								  }).ToList();

			//Ná í myndina og bodyið ef það er ekki null

				model.Image = post.GetImage(postid);

				model.ImageName = post.GetImageName(postid);

				model.PostBody = post.GetPostBody(postid);

				model.PostID = postid;


			//Finnur FirstName og LastName þess sem postaði commentinu	-Védís
			foreach (var item in model.Comments)
			{
				if (item != null)
				{
					//var userIdTemp = poster.GetUserId(item.ID);
					var userIdTemp = commenter.GetCommenterID(item.ID);
					var userNameTemp = poster.GetUserNameById(userIdTemp);
					var user = manager.GetUser(userNameTemp);

					item.CommenterFirstName = user.FirstName;
					item.CommenterLastName = user.LastName;
				}
			}

			return View(model);
	   }

    }
}