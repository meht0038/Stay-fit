using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Project.Models;

namespace Project.Controllers
{
    public class AdminController : Controller
    {

        ProjectDBEntities r = new ProjectDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
       
        public ActionResult DietitianProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveDietitianProfile(string Name, string TypeofDietitian, string Quote, string Qualifications, string Speciality, string Messenger, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var a = new DietitianProfile
            {
                Name = Name,
                TypeofDietitian = TypeofDietitian,
                Quote = Quote,
                Qualifications = Qualifications,
                Speciality = Speciality,
                Messenger = Messenger,
                DietitianPhoto = bytes,
                photoCon = postedFile.ContentType

            };
            r.DietitianProfiles.Add(a);
            r.SaveChanges();

            return RedirectToAction("DietitianProfile");
        }
        public ActionResult TrainerProfile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SaveTrainerProfile(string Name, string TypeofTrainer, string Quote, string Qualifications, string Speciality, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var a = new TrainerProfile
            {
                Name = Name,
                TypeofTrainer = TypeofTrainer,
                Quote = Quote,
                Qualifications = Qualifications,
                Speciality = Speciality,
                TrainerPhoto = bytes,
                photoCon = postedFile.ContentType

            };
            r.TrainerProfiles.Add(a);
            r.SaveChanges();

            return RedirectToAction("TrainerProfile");
        }
        public ActionResult AdminHome()
        {
            return View();
        }

        public ActionResult ImageData()
        {
            ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");
            ViewBag.sublist = new SelectList(r.SubCategories.Where(g => g.MainCategoryID == 0).ToList(), "Sub_Id", "SubCategoryName");
            ViewBag.subsublist = new SelectList(r.SubSubCategories.Where(g => g.SubCategoryId == 0).ToList(), "Sub_SubCategoryId", "Sub_SubCategoryName");

            return View();
        }
        [HttpPost]
        public ActionResult ImageData(int? Name, int? sub)
        {
            if (sub == null)
            {
                ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");

                ViewBag.sublist = new SelectList(r.SubCategories.Where(g => g.MainCategoryID == Name).ToList(), "Sub_Id", "SubCategoryName");
                ViewBag.subsublist = new SelectList(r.SubSubCategories.Where(g => g.SubCategoryId == sub).ToList(), "Sub_SubCategoryId", "Sub_SubCategoryName");

            }
            else
            {
                ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");

                ViewBag.sublist = new SelectList(r.SubCategories.Where(g => g.MainCategoryID == Name).ToList(), "Sub_Id", "SubCategoryName");
                ViewBag.subsublist = new SelectList(r.SubSubCategories.Where(g => g.SubCategoryId == sub).ToList(), "Sub_SubCategoryId", "Sub_SubCategoryName");

            }
            return View("ImageData");
        }
        public ActionResult SaveImageData(string Name, string sub,string subsub, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var a = new ImageData
            {
                MaincategoryId = Convert.ToInt32(Name),
                SubCategoryId = Convert.ToInt32(sub),
                SubSubCategoryId = Convert.ToInt32(subsub),
                Image = bytes,
                imgeCon = postedFile.ContentType

            };
            r.ImageDatas.Add(a);
            r.SaveChanges();

            return RedirectToAction("ImageData");
        }
        public ActionResult VideoData()
        {

            ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");
            ViewBag.sublist = new SelectList(r.SubCategories.Where(g => g.MainCategoryID == 0).ToList(), "Sub_Id", "SubCategoryName");
            ViewBag.subsublist = new SelectList(r.SubSubCategories.Where(g => g.SubCategoryId == 0).ToList(), "Sub_SubCategoryId", "Sub_SubCategoryName");

            return View();
        }
       
        [HttpPost]
        public ActionResult VideoData(int? Name, int? sub )
        {
            if(sub==null)
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
            return View("VideoData");
        }
        public ActionResult SaveVideoData(string Name, string sub, string subsub, HttpPostedFileBase postedFile)
        {
           
            var a = new VideoData
            {
                MainCategoryId = Convert.ToInt32(Name),
                SubCategoryId = Convert.ToInt32(sub),
                SubSubCategoryId = Convert.ToInt32(subsub),
                FileName = Name + sub + subsub+postedFile.FileName,
                ConType = postedFile.ContentType,
                FilePath = "/VideoFiles/"+ Name + sub + subsub + postedFile.FileName
                               
            };
            postedFile.SaveAs( Server.MapPath("~/VideoFiles/") + Name + sub + subsub + postedFile.FileName);
            r.VideoDatas.Add(a);
            r.SaveChanges();

            return RedirectToAction("VideoData");
        }
        public ActionResult SubSubCategory()
        {
            ViewBag.NameList = new SelectList(r.SubCategories.ToList(), "Sub_Id", "SubCategoryName");
            return View();
        }
        public ActionResult SaveSubSubCategory(string Sub_SubCategoryName, int MainCategory, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var dd = new SubSubCategory
            {
                SubCategoryId=MainCategory,
                Sub_SubCategoryName = Sub_SubCategoryName,
                Image = bytes,
                imgCon = postedFile.ContentType
            };
            r.SubSubCategories.Add(dd);
            r.SaveChanges();

            return RedirectToAction("SubSubCategory");
        }

      
        public ActionResult SubCategory()
        {

            ViewBag.NameList = new SelectList(r.MainCategories.ToList(), "main_Id", "MainCategoryName");
            return View();
        }
        public ActionResult SaveSubCategory(string SubCategoryName,string MainC, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var a = new SubCategory
            {
                MainCategoryID= Convert.ToInt32(MainC),
                SubCategoryName = SubCategoryName,
                Image = bytes,
                imgCon = postedFile.ContentType

            };
            r.SubCategories.Add(a);
            r.SaveChanges();

            return RedirectToAction("SubCategory");
        }
        public ActionResult MainCategory()
        {
            return View();
        }
        public ActionResult SaveCategory(string CategoryName, HttpPostedFileBase postedFile)
        {
            byte[] bytes;
            using (BinaryReader br = new BinaryReader(postedFile.InputStream))
            {
                bytes = br.ReadBytes(postedFile.ContentLength);
            }
            var a = new MainCategory
            {
                MainCategoryName = CategoryName,
                Image = bytes,
                imgCon = postedFile.ContentType

            };
            r.MainCategories.Add(a);
            r.SaveChanges();

            return RedirectToAction("MainCategory");
        }
    }
}