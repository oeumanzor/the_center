using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Exam.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Exam.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context { get; set; }
        private PasswordHasher<User> regHasher = new PasswordHasher<User> ();
        private PasswordHasher<LoginUser> logHasher = new PasswordHasher<LoginUser> ();

        public HomeController(MyContext context)
        {
            _context = context;
        }
        public User GetUser()
        {
            return _context.Users.FirstOrDefault( u => u.UserID == HttpContext.Session.GetInt32("userId"));
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost ("register")]
        public IActionResult Register (User u)
        {
            if (ModelState.IsValid)
            {
                if (_context.Users.FirstOrDefault (usr => usr.Email == u.Email) != null)
                {
                    ModelState.AddModelError ("Email", "Email is already in use, try logging in!");
                    return View ("Index");
                }
                string hash = regHasher.HashPassword (u, u.Password);
                u.Password = hash;
                _context.Users.Add (u);
                _context.SaveChanges ();
                HttpContext.Session.SetInt32 ("userId", u.UserID);
                return Redirect ("/dashboard");
            }
            return View ("Index");
        }

        [HttpPost ("login")]
        public IActionResult Login (LoginUser lu)
        {
            if (ModelState.IsValid)
            {
                User userInDB = _context.Users.FirstOrDefault (u => u.Email == lu.LoginEmail);
                if (userInDB == null)
                {
                    ModelState.AddModelError ("LoginEmail", "Invalid Email or Password!");
                    return View ("Index");
                }
                var result = logHasher.VerifyHashedPassword (lu, userInDB.Password, lu.LoginPassword);
                if (result == 0)
                {
                    ModelState.AddModelError ("LoginPassword", "Invalid Email or Password!");
                    return View ("Index");
                }
                HttpContext.Session.SetInt32 ("userId", userInDB.UserID);
                return Redirect ("/dashboard");
            }
            return View ("Index");
        }

        [HttpGet("dashboard")]
        public IActionResult dashboard()
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect("/");
            }

            ViewBag.User = current;
            List<Show> MyShows =_context.Shows
                        .Include(w => w.Planner)
                        .Include(w => w.Attendees)
                        .ThenInclude(g => g.Guest)
                        .Where(w => w.Date >= DateTime.Now)
                        .OrderBy( w => w.Date )
                        .ToList();

            return View(MyShows);
        }

        [HttpGet("{status}/{showid}")]
        public IActionResult ToggleShow (int showid, string status)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect ("/");
            }
            if(status == "join")
            {
                rsvp newrsvp = new rsvp();
                newrsvp.UserID = current.UserID;
                newrsvp.ShowID = showid;
                _context.RSVP.Add(newrsvp);
            }
            else if(status == "leave")
            {
                rsvp backout = _context.RSVP.FirstOrDefault( w => w.UserID == current.UserID && w.ShowID == showid );
                _context.RSVP.Remove(backout);
            }
            _context.SaveChanges();
            return RedirectToAction("dashboard");
        }

        [HttpGet("delete/{showid}")]
        public IActionResult DeleteShow(int showid)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect ("/");
            }
            Show remove = _context.Shows.FirstOrDefault( w => w.ShowID == showid );
            _context.Shows.Remove(remove);
            _context.SaveChanges();
            return RedirectToAction("dashboard");
        }

        [HttpGet("new/show")]
        public IActionResult NewShow()
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect ("/");
            }
            return View("NewShow");
        }
        [HttpPost("create/show")]
        public IActionResult CreateShow(Show newShow)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect ("/");
            }
            if(ModelState.IsValid)
            {
                newShow.UserID = current.UserID;
                _context.Shows.Add(newShow);
                _context.SaveChanges();
                return RedirectToAction("dashboard");
            }
            return View("newshow");
        }

        [HttpGet("show/{showid}")]
        public IActionResult DisplayShow(int showid)
        {
            User current = GetUser();
            if(current == null)
            {
                return Redirect ("/");
            }
            ViewBag.User = current;

            Show showing = _context.Shows
            .Include( w => w.Attendees )
            .ThenInclude(w => w.Guest)
            .Include( m => m.Planner)
            .FirstOrDefault( w => w.ShowID == showid );
            return View(showing);
            
        }


        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
