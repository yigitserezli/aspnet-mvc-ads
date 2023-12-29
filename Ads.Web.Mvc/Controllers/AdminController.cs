using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Data;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AutoMapper;




// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{

    public class AdminController : Controller
    {
        private readonly AppDbContext _dbContext;
       
        public AdminController(AppDbContext dbContext)

        {
            _dbContext = dbContext;
        }



        public int userId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));




        [HttpGet]
        public IActionResult Blogs(int page = 1)
        {
            int pageSize = 10;

            IQueryable<BlogEntity> BlogsQuery = _dbContext.Blogs
                .Where(c => c.Confirm == false)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var Blogs = BlogsQuery.ToList();

            ViewBag.Page = page; 

            return View("Blogs", Blogs);
        }



        [HttpGet]
        public IActionResult BlogConfirm(int id)
        {
            var blogToConfirm = _dbContext.Blogs.FirstOrDefault(blog => blog.Id == id && blog.Confirm == false);

            if (blogToConfirm != null)
            {
                blogToConfirm.Confirm = true;
                _dbContext.SaveChanges();
            }           

            return RedirectToAction("Blogs");
        }

        [HttpGet]
        public IActionResult BlogDelete(int id)
        {
            var blogToDelete = _dbContext.Blogs.FirstOrDefault(blog => blog.Id == id);

            if (blogToDelete != null)
            {
                _dbContext.Blogs.Remove(blogToDelete);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Blogs");
        }

        //blogcomments


        [HttpGet]
        public IActionResult BlogComments(int page = 1)
        {
            int pageSize = 10;

            IQueryable<BlogCommentsEntity> BlogComments = _dbContext.BlogComments
                .Where(c => c.Confirm == false)
                .OrderByDescending(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var Blogs = BlogComments.ToList();

            ViewBag.Page = page;

            return View("Blogs", Blogs);
        }

        [HttpGet]
        public IActionResult BCommentConfirm(int commentId)
        {
           
            var commentToConfirm = _dbContext.BlogComments.FirstOrDefault(comment => comment.Id == commentId && comment.Confirm == false);

            if (commentToConfirm != null)
            {
                commentToConfirm.Confirm = true;
                _dbContext.SaveChanges();
            }

            // Yorum onaylandıktan sonra, yönlendirme yapılabilir
            return RedirectToAction("Index", "Admin");
        }

        [HttpGet]
        public IActionResult BCommentDelete(int commentId)
        {
            var commentToDelete = _dbContext.BlogComments.FirstOrDefault(comment => comment.Id == commentId);
            if (commentToDelete != null)
            {
                _dbContext.BlogComments.Remove(commentToDelete);
                _dbContext.SaveChanges();
            }
           return RedirectToAction("Index", "Admin");
        }

        //advert
        [HttpGet]
        public IActionResult Adverts(int page = 1)
        {
            int pageSize = 10;

            IQueryable<AdvertEntity> AdvertsQuery = _dbContext.Adverts
                .Where(a => a.Confirm == false)
                .OrderBy(a => a.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var Adverts = AdvertsQuery.ToList();

            ViewBag.Page = page;

            return View("Adverts", Adverts);
        }

        [HttpGet]
        public IActionResult AdvertConfirm(int id)
        {
            var advertToConfirm = _dbContext.Adverts.FirstOrDefault(advert => advert.Id == id && advert.Confirm == false);

            if (advertToConfirm != null)
            {
                advertToConfirm.Confirm = true;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("Adverts");
        }

        [HttpGet]
        public IActionResult AdvertDelete(int id)
        {
            var advertToDelete = _dbContext.Adverts.FirstOrDefault(advert => advert.Id == id);

            if (advertToDelete != null)
            {
                _dbContext.Adverts.Remove(advertToDelete);
                _dbContext.SaveChanges();
            }

            //dosya silme de eklenecek

            return RedirectToAction("Adverts");

        }

        [HttpGet]
        public IActionResult AdvertComments(int page = 1)
        {
            int pageSize = 10;

            IQueryable<AdvertCommentsEntity> AdvertComments = _dbContext.AdvertComments
                .Where(c => c.Confirm == false)
                .OrderBy(c => c.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            var Comments = AdvertComments.ToList();
            ViewBag.Page = page;
            return View("AdvertComments", Comments);
        }

        [HttpGet]
        public IActionResult AdvertCommentConfirm(int commentId)
        {
            var commentToConfirm = _dbContext.AdvertComments.FirstOrDefault(comment => comment.Id == commentId && comment.Confirm == false);

            if (commentToConfirm != null)
            {
                commentToConfirm.Confirm = true;
                _dbContext.SaveChanges();
            }

            return RedirectToAction("AdvertComments");
        }

        [HttpGet]
        public IActionResult AdvertCommentDelete(int commentId)
        {
            var commentToDelete = _dbContext.AdvertComments.FirstOrDefault(comment => comment.Id == commentId);

            if (commentToDelete != null)
            {
                _dbContext.AdvertComments.Remove(commentToDelete);
                _dbContext.SaveChanges();
            }

            return RedirectToAction("AdvertComments");
        }




    }
}
