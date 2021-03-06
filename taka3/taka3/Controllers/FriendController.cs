﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using taka3.Models;
using taka3.Services;
using Microsoft.AspNet.Identity;

namespace taka3.Controllers
{
    public class FriendController : Controller
    {
        private ApplicationDbContext m_db = new ApplicationDbContext();

        FollowingService fservice = new FollowingService();

        public ActionResult Index()
        {
            return View(fservice.GetAllFriendList().ToList());
        }

        [HttpPost]
        public ActionResult FollowUser(string userID)
        {
            string currUserId = User.Identity.GetUserId();
            var fService = new FollowingService();
            fService.AddFollowingToUser(currUserId, userID);

			//Redirectar á prófílinn og sendir id notandans með	-Védís
			return RedirectToAction("FriendProfilePage", "Home", new { userId = userID });
        }

        // GET: friends/Details/5
        public ActionResult Details(int? id)
        {
            FriendModel friendModel = m_db.FriendModel.Find(id);
            if (friendModel == null)
            {
                return HttpNotFound();
            }
            return View(friendModel);
        }

        [Authorize]
        public ActionResult GetUserFriends()
        {
            var userid = User.Identity.GetUserId();

            var model = fservice.GetUserFriendInfoById(userid);

            return View(model);
        }

    }
}