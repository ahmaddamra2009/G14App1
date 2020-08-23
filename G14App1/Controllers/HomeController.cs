using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using G14App1.Models;
using Microsoft.AspNetCore.Authorization;

namespace G14App1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;
        public HomeController(ApplicationDbContext _db)
        {
            db = _db;
        }
        [AllowAnonymous]
        public IActionResult AllProducts()
        {
            // Select * from prod
            var pro = db.Products.ToList(); // Collection of Products
            return View(pro);
        } 
        [HttpPost]
        [AllowAnonymous]
        public IActionResult AllProducts(string term)
        {
            if (term==null)
            {
                return View(db.Products.ToList());
            }

            var data = db.Products.Where(x => x.ProductName.Contains(term) || x.Email.Contains(term));
            if (data.Count()==0)
            {
                ViewBag.NoData = "Not Found .... ";
                return View(db.Products.ToList());
            }

            return View(data);
        }
        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var findProduct = db.Products.Find(id);

            return View(findProduct);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("AllProducts");
            }

            return View(product);


        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var findProduct = db.Products.Find(id);

            return View(findProduct);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            db.Products.Update(product);
            db.SaveChanges();
            return RedirectToAction("AllProducts");
        }

        public IActionResult Delete(int id)
        {
            var findProduct = db.Products.Find(id);

            return View(findProduct);
        }

        [HttpPost]
        public IActionResult Delete(Product product)
        {
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("AllProducts");

        }
    }
}
