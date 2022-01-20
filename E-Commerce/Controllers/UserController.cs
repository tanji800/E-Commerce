using E_Commerce.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace E_Commerce.Controllers
{
    public class UserController : Controller
    {
        EShoppingDBEntities db = new EShoppingDBEntities();
        // GET: User



        public ActionResult Index(int? page)
        {
            if (TempData["cart"]!=null)
            {

                double x = 0;
                List<cart> li2 = TempData["cart"] as List<cart>;
                foreach (var item in li2)
                {

                    x += Convert.ToInt32(item.o_bill);
                }

                TempData["total"] = x;
            }
            TempData.Keep();

            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.cateogories.Where(x => x.cat_status == 1).OrderByDescending(x => x.cat_id).ToList();
            IPagedList<cateogory> cate = list.ToPagedList(pageindex, pagesize);

            return View(cate);
        }

        public ActionResult About()
        {

            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(tbl_user us, HttpPostedFileBase imgfile)
        {

            string path = uploadimage(imgfile);

            if (path.Equals("-1"))
            {
                ViewBag.error = "image could not be uploaded";

            }
            else
            {
                tbl_user u = new tbl_user();
                u.u_name = us.u_name;
                u.u_password = us.u_password;
                u.u_mobile = us.u_mobile;
                u.u_email = us.u_email;
                u.u_image = path;
                u.u_company = us.u_company;

                db.tbl_user.Add(u);
                db.SaveChanges();

                return RedirectToAction("Login");

            }

            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(tbl_user svm)
        {

            tbl_user ad = db.tbl_user.Where(x => x.u_email == svm.u_email && x.u_password == svm.u_password).SingleOrDefault();
            if (ad != null)
            {
                Session["u_id"] = ad.u_id.ToString();
                Session["user"] = ad.u_name;
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Invalid User Name or Password";
            }

            return View();
        }

        [HttpGet]
        public ActionResult CreateAdd()
        {
            List<cateogory> li = db.cateogories.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");

            return View();
        }

        [HttpPost]
        public ActionResult CreateAdd(product p, HttpPostedFileBase imgfile)
        {
            List<cateogory> li = db.cateogories.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");

            string path = uploadimage(imgfile);

            if (path.Equals("-1"))
            {
                ViewBag.error = "image could not be uploaded";

            }
            else
            {
                product pr = new product();
                pr.pro_name = p.pro_name;
                pr.pro_price = p.pro_price;
                pr.pro_image = path;

                pr.cat_id = p.pro_user_id;

                pr.pro_desc = p.pro_desc;
                pr.pro_user_id = Convert.ToInt32(Session["u_id"].ToString());
                db.products.Add(pr);
                db.SaveChanges();

                Response.Redirect("Index");
            }

            return View();
        }


        public ActionResult DisplayAdd(int? id, int? page)
        {
            TempData.Keep();
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.products.Where(x => x.cat_id == id).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<product> cate = list.ToPagedList(pageindex, pagesize);

            return View(cate);
        }

        [HttpPost]
        public ActionResult DisplayAdd(int? id, int? page,string search)
        {
            
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.products.Where(x => x.pro_name.Contains(search)).OrderByDescending(x => x.pro_id).ToList();
            IPagedList<product> cate = list.ToPagedList(pageindex, pagesize);

            return View(cate);
        }


        private string uploadimage(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);

                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try

                    {

                        path = Path.Combine(Server.MapPath("~/Content/upload"), random + Path.GetFileName(file.FileName));

                        file.SaveAs(path);

                        path = "~/Content/upload/" + random + Path.GetFileName(file.FileName);



                        //    ViewBag.Message = "File uploaded successfully";

                    }
                    catch (Exception ex)

                    {

                        path = "-1";

                    }
                }
                else

                {

                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");

                }

            }



            else

            {

                Response.Write("<script>alert('Please select a file'); </script>");

                path = "-1";

            }
            return path;
        }

        public ActionResult SignOut()
        {
            Session.RemoveAll();
            Session.Abandon();

            return RedirectToAction("Index");
        }

        public ActionResult Add_Delete(int ? id)
        {
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();
            db.products.Remove(p);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

            public ActionResult ViewAdds(int ? id )
        {
            ad_view_model adm = new ad_view_model();

            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();
            adm.pro_id = p.pro_id;
            adm.pro_name = p.pro_name;
            adm.pro_image = p.pro_image;
            adm.pro_price = p.pro_price;
            adm.pro_desc = p.pro_desc;

            cateogory cat = db.cateogories.Where(x => x.cat_id == p.cat_id).SingleOrDefault();
            adm.cat_name = cat.cat_name;
            tbl_user u = db.tbl_user.Where(x => x.u_id == p.pro_user_id).SingleOrDefault();
            adm.u_name = u.u_name;
            adm.u_image = u.u_image;
            adm.u_mobile = u.u_mobile;
            adm.u_company = u.u_company;
            adm.pro_user_id = u.u_id;



            return View(adm);
        }

        public ActionResult Ad_tocart (int? id)
        {
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();

            return View(p);
        }





        List<cart> li = new List<cart>();
        [HttpPost]
        public ActionResult Ad_tocart(product pr, string qty, int id)
        {
            
            product p = db.products.Where(x => x.pro_id == id).SingleOrDefault();
            cart c = new cart();
            c.pro_id = p.pro_id;
            c.pro_name = p.pro_name;
            c.pro_price = p.pro_price;

            c.o_qty = Convert.ToInt32(qty);
            c.o_bill = c.pro_price * c.o_qty;
            if (TempData["cart"] == null)
            {

                li.Add(c);

                TempData["cart"] = li;

            }
            else
            {
                List<cart> li2 = TempData["cart"] as List<cart>;
                int flag = 0;
                
                foreach (var item in li2)
                {
                    if (item.pro_id == c.pro_id)
                    {

                        item.o_qty += c.o_qty;
                        item.o_bill += c.o_bill;
                        flag = 1;
                    }


                }

                if (flag == 0)
                {
                    li2.Add(c);

                    // item is new......
                }


                TempData["cart"] = li2;

            }
            TempData.Keep();

            return RedirectToAction("Index");
        
        
        }

        public ActionResult remove(int ? id)
        {
            List<cart> li2 = TempData["cart"] as List<cart>;
            cart c = li2.Where(x => x.pro_id == id).SingleOrDefault();
            li2.Remove(c);

            int h = 0;
            foreach (var item in li2)
            {
                h += Convert.ToInt32(item.o_bill);
            }
            TempData["total"] = h;

            

            return RedirectToAction("checkout") ;
        }

        public ActionResult checkout()
        {

            TempData.Keep();

            return View(); ;
        }

        [HttpPost]
        public ActionResult checkout(order_table O)
        {
            List<cart> li = TempData["cart"] as List<cart>;
            tbl_invoice iv = new tbl_invoice();
            iv.in_fk_user = Convert.ToInt32(Session["u_id"].ToString());
            iv.in_date = DateTime.Now;
            iv.in_totalbill = (Convert.ToInt32(TempData["total"]));
            db.tbl_invoice.Add(iv);
            db.SaveChanges();

            foreach (var item in li)
            {

                order_table od = new order_table();
                od.o_fk_pro = item.pro_id;
                od.o_fk_invoice = iv.in_id;
                od.o_date = DateTime.Now;
                od.o_qty = item.o_qty;
                od.o_unitprice = item.pro_price;
                od.o_bill = item.o_bill;
                db.order_table.Add(od);
                db.SaveChanges();

            }
            TempData.Remove("total");
            TempData.Remove("cart");

            TempData["msg"] = "Transaction Successfully Completed........";
            TempData.Keep();



            return RedirectToAction("Index");

        }
    }
}