using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class UserController : Controller
    {
        ProjectDBEntities r = new ProjectDBEntities();

        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Homepage()
        {
            return View();
        }
        [HttpGet]
        public ActionResult DietPlan()
        {
            ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");
            return View();
        }
        [HttpPost]
        public ActionResult DietPlan(int? Name, int? sub)
        {
            if (sub == null)
            {
                ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");

            }
            else
            {
                ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");

                
            }
            return View("DietPlan");
        }


        public ActionResult DietPlanDetails(string Name,string gender, string Age)
        {
            TempData["imgsrc"] = "";
            if (Age == "15-25")
            {
                TempData["imgsrc"] = @"../images/DietPlan/bodybuilding.jpg";
            }
            else if (Age == "26-40")
            {
                TempData["imgsrc"] = @"../images/DietPlan/weightgain.jpg";
            }
            else if (Age == "41-60")
            {
                TempData["imgsrc"] = @"../images/DietPlan/Weightloss.jpg";
            }

            return RedirectToAction("DietPlan");

        }
            public ActionResult BMR()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GetBMR(string gender, string Height, string Weight, string age)
        {
           

            if (gender == "Male")
            {
                double bmr = (10 * (Convert.ToDouble(Weight))) + (6.25 * (Convert.ToDouble(Height))) - (5 * Convert.ToInt32(age)) + 5;
                Session["bmr"] = bmr;
            }
            else
            {
                double bmr = (10 * (Convert.ToDouble(Weight))) + (6.25 * (Convert.ToDouble(Height))) - (5 * Convert.ToInt32(age)) - 161;
                Session["bmr"] = bmr;
            }



            return RedirectToAction("BMR");
        }
        public ActionResult Chatbot()
        {
            return View();
        }
        public ActionResult Schedule()
        {
            return View();
        }
        public ActionResult UserProfileEmpty()
        {
            return View();
        }
        public ActionResult Exercise()
        {
            ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");
            ViewBag.sublist = new SelectList(r.SubCategories.Where(g => g.MainCategoryID == 0).ToList(), "Sub_Id", "SubCategoryName");
            ViewBag.subsublist = new SelectList(r.SubSubCategories.Where(g => g.SubCategoryId == 0).ToList(), "Sub_SubCategoryId", "Sub_SubCategoryName");

            return View();
        }
        [HttpPost]
        public ActionResult Exercise(int? Name, int? sub)
        {
            if (sub == null)
            {
                ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");

                ViewBag.sublist = new SelectList(r.SubCategories.Where(g => g.MainCategoryID == Name).ToList(), "Sub_Id", "SubCategoryName");
                ViewBag.subsublist = new SelectList(r.SubSubCategories.Where(g => g.SubCategoryId == 0).ToList(), "Sub_SubCategoryId", "Sub_SubCategoryName");

            }
            else
            {
                ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");

                ViewBag.sublist = new SelectList(r.SubCategories.Where(g => g.MainCategoryID == Name).ToList(), "Sub_Id", "SubCategoryName");
                ViewBag.subsublist = new SelectList(r.SubSubCategories.Where(g => g.SubCategoryId == sub).ToList(), "Sub_SubCategoryId", "Sub_SubCategoryName");

            }
            return View("Exercise");
        }

        public ActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Home(string Height, string Weight)
        {
            double meter = (Convert.ToDouble(Height) * 0.3048);
            meter = meter * 2;
            double bmi = (Convert.ToDouble(Weight)) / meter;
            if (bmi < 18.5)
                Session["bmi"] = "Underweight";
            else if (bmi >= 18.5 && bmi <= 24.9)
                Session["bmi"] = "Normal";
            else if (bmi >= 25 && bmi <= 29.9)
                Session["bmi"] = "Overweight";
            else if (bmi >= 30)
                Session["bmi"] = "Obese";
            
            return RedirectToAction("Home");
        }
        public ActionResult Trainers()
        {
            return View();
        }
        public ActionResult Pricing()
        {
            return View();
        }
        public ActionResult Team()
        {
            return View();
        }
        public ActionResult Contact()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveContact(string Name, string Email, string Subject, string Message)
        {

            var a = new Contact
            {
                Name = Name,
                Email = Email,
                Subject = Subject,
                Message = Message
            };
            r.Contacts.Add(a);
            r.SaveChanges();
            return RedirectToAction("Contact");
        }
        public ActionResult Comment()
        {
            var sLIst = r.Blogs.SqlQuery("select * from Blog where Email='"+ Request.QueryString["id"] +"'").ToList<Blog>();
            return View(sLIst);

        }
        [HttpPost]
        public ActionResult SaveComment(string Name, string Email,string CommentDescription,string Blogid)
        {

            var a = new Comment
            {
                Name = Name,
                Email = Email,
                CommentDescription = CommentDescription,
                BlogId = Blogid


            };
            r.Comments.Add(a);
            r.SaveChanges();
            Response.Redirect("Comment?id=" + Blogid);
            return RedirectToAction("Comment");
        }
        public ActionResult Blog()
        {

            var sLIst = r.Blogs.SqlQuery("select * from Blog").ToList<Blog>();
            return View(sLIst);
        }
        [HttpPost]
        public ActionResult SaveBlog(string BlogTitle, string BlogDescription, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var a = new Blog
            {
                Email= Convert.ToString(Session["UserID"]),
                BlogDate =DateTime.Now.Date.ToShortDateString(),
                Likes=0,
                Comments=0,
                BlogTitle = BlogTitle,
                BlogDescription = BlogDescription,
                BlogImage = bytes,
                imgCon = postedFile.ContentType

            };
            r.Blogs.Add(a);
            r.SaveChanges();
            return RedirectToAction("Blog");
        }



        public ActionResult ImageDataPV(int Name, int sub, int subsub)
        {
            var sLIst = r.ImageDatas.SqlQuery("select * from ImageData where MainCategoryId=" + Name + " and SubCategoryId=" + sub + " and SubSubCategoryId=" + subsub + "").ToList<ImageData>();
            return View(sLIst);
        }

        public ActionResult VedioAndImg(int Name, int sub, int subsub)
            {

            TempData["url"] = "Name=" + Name + "&sub=" + sub + "&subsub=" + subsub;
            var sLIst = r.VideoDatas.SqlQuery("select * from VideoData where MainCategoryId="+ Name +" and SubCategoryId="+ sub +" and SubSubCategoryId="+ subsub+" ").ToList<VideoData>();

            return View(sLIst);
            }


            public ActionResult Membership()
        {
            ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");
            return View();
        }

        public ActionResult SaveMembership(string cat,string Period, string Fee, string StartDate, string Payment, string CardNumber, string ExpiryDate, string CVV)
        {
           
            var a = new MemberShip
            {
                MainCategory = cat,
                Email = Convert.ToString(Session["UserID"]),
                Period = Period,
                Fee = Fee,
                StartDate = StartDate,
                Payment = Payment,
                CardNumber = CardNumber,
                ExpiryDate = ExpiryDate,
                CVV=CVV

            };
            r.MemberShips.Add(a);
            r.SaveChanges();

            return RedirectToAction("Membership");
        }
        public ActionResult BodyMassIndex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetBodyMassIndex(string Height, string Weight)
        {
            double meter = (Convert.ToDouble(Height) * 0.3048);
            meter = meter * 2;
            double bmi = (Convert.ToDouble(Weight)) / meter;
            if (bmi < 18.5)
                Session["bmi"] = "Underweight";
            else if (bmi >= 18.5 && bmi <= 24.9)
                Session["bmi"] = "Normal";
            else if (bmi >= 25 && bmi <= 29.9)
                Session["bmi"] = "Overweight";
            else if (bmi >= 30)
                Session["bmi"] = "Obese";

            return RedirectToAction("BodyMassIndex");
        }
        public ActionResult UserProfile()
        {
            var sLIst = r.UserProfiles.SqlQuery("select * from UserProfile where Email='" + Convert.ToString(Session["UserID"]) + "'").FirstOrDefault<UserProfile>();
            if(sLIst==null)
            {
                return RedirectToAction("UserProfileEmpty");
            }
            else
            {
                return View(sLIst);
            }

            
        }
        [HttpPost]
        public ActionResult SaveProfile(string FullName,string Address,string Email,string DateofBirth,string BloodGroup, string gender,string Status,string Age,string Height,string Weight, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var a = new UserProfile
            {
                Fullname =FullName,
                Address=Address, 
                Email=Email,
                DateofBirth=DateofBirth,
                BloodGroup=BloodGroup,
                Gender=gender,
                Age=Age,
                Height=Height,
                Weight=Weight,
                userphoto=bytes,
                photoCon=postedFile.ContentType

            };
            r.UserProfiles.Add(a);
            r.SaveChanges();

            return RedirectToAction("UserProfile");
        }
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult AdminRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult GEtRegister(string fname,string lname, string email, string mobile, string password)
        {

            var a = new UserRegister
            {

                fname=fname,
                lname=lname,
                emailid=email,
                mobile=mobile,
                password=password,

            };
            r.UserRegisters.Add(a);
            r.SaveChanges();
            return RedirectToAction("Login"); 
        }

        [HttpPost]
        public ActionResult GEtAdminRegister(string fname, string lname, string email, string mobile, string password)
        {

            var a = new AdminRegister
            {

                fname = fname,
                lname = lname,
                emailid = email,
                mobile = mobile,
                password = password,

            };
            r.AdminRegisters.Add(a);
            r.SaveChanges();
            return RedirectToAction("Login"); ;
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CheckLogin(string email, string password)
        {

            var user = (from userlist in r.UserRegisters
                        where userlist.emailid == email && userlist.password == password
                        select new
                        {
                            userlist.emailid,
                            userlist.fname
                        }).ToList();

            if (user.FirstOrDefault() != null)
            {
                Session["UserName"] = user.FirstOrDefault().fname;
                Session["UserID"] = user.FirstOrDefault().emailid;
                
                return RedirectToAction("Home");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login credentials.");
                return View("Login");
            }

           
        }


        [HttpPost]
        public ActionResult CheckLoginAdmin(string email, string password)
        {

            var user = (from userlist in r.AdminRegisters
                        where userlist.emailid == email && userlist.password == password
                        select new
                        {
                            userlist.emailid,
                            userlist.fname
                        }).ToList();

            if (user.FirstOrDefault() != null)
            {
                Session["UserName"] = user.FirstOrDefault().fname;
                Session["UserID"] = user.FirstOrDefault().emailid;
                return RedirectToAction("AdminHome", "Admin");
            }
            else
            {
                ModelState.AddModelError("", "Invalid login credentials.");
                return View("AdminLogin");
            }


        }



    }
}