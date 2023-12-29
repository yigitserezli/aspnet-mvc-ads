using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Data;
using App.Data.Entities;
using App.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.AspNetCore.Http.HttpResults;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{

    public class BloggerController : Controller
    {
        private readonly AppDbContext _dbContext;
       
        public BloggerController(AppDbContext dbContext)

        {
            _dbContext = dbContext;
        }



        public int userId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


       



        //My posts
        [HttpGet]
        public IActionResult Index(bool? confirm = null)
        {
            // user id cookiden gelecek
            IQueryable<BlogEntity> BlogsQuery = _dbContext.Blogs.Where(c => c.UserId == userId);

            if (confirm == false)
            {
                BlogsQuery = BlogsQuery.Where(c => c.Confirm == false);
            }
            if (confirm == true)
            {
                BlogsQuery = BlogsQuery.Where(c => c.Confirm == true);
            }

            var MyBlogs = BlogsQuery.ToList();

            return View("index", MyBlogs);
        }









        public IActionResult MyAdDetail([FromRoute] int id)
        {
            var MyBlog = _dbContext.Blogs
         .Include(a => a.BlogComments)
         .FirstOrDefault(a => a.Id == id);


            return View(MyBlog);

        }


        // GET: Advert/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SelectItem = _dbContext.Categories.
                ToList().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),

                });
            ViewBag.ProductCount = _dbContext.Categories.CountAsync();


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(BlogEntity model)
        {

            var Blog = new BlogEntity
            {
                Title = model.Title,
                Content = model.Content,
                CreatedAt = DateTime.UtcNow,
                CategoryId = model.CategoryId,
                Confirm = false,
                UserId = userId,
            };

            _dbContext.Blogs.Add(Blog);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existingAdvert = await _dbContext.Blogs. //admin de işlem yapabilir bu kontrol de sağlanacak
                FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId);


            ViewBag.SelectItem = _dbContext.Categories.
           ToList().Select(x => new SelectListItem()
           {
               Text = x.Name,
               Value = x.Id.ToString(),

           });
            ViewBag.ProductCount = _dbContext.Categories.CountAsync();


            return View(existingAdvert);
        }



        [HttpPost]
        public async Task<IActionResult> Edit(int id, BlogEntity model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var existingBlog = await _dbContext.Blogs.FindAsync(id);

          //      if (ModelState.IsValid)            {
                existingBlog.Title = model.Title;
                existingBlog.Content = model.Content;
                existingBlog.CreatedAt = DateTime.UtcNow;
                existingBlog.CategoryId = model.CategoryId;
                existingBlog.Confirm = false;// satıcının her güncellemesinde false çekilir ancak admin için bu işlem yapılmaz

                _dbContext.Entry(existingBlog).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
           //      }                   return View(model);
            }
        }

    }
