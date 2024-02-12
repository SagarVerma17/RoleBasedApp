using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RoleBasedApp.Data;
using RoleBasedApp.Models;

namespace RoleBasedApp.Controllers
{
    //[Route("api/[controller]")]
    //[Controller]
    public class BlogsController : Controller
    {
        //private readonly RoleBasedAppContext _context;

        //public BlogsController(RoleBasedAppContext context)
        //{
        //    _context = context;
        //}

        //// GET: Blogs
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Blog.ToListAsync());
        //}

        //// GET: Blogs/Details/5
        //public async Task<IActionResult> Details(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var blog = await _context.Blog
        //        .FirstOrDefaultAsync(m => m.Title == id);
        //    if (blog == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(blog);
        //}

        //// GET: Blogs/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Blogs/Create
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Title,Username,Description,TimeStamp")] Blog blog)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _context.Add(blog);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(blog);
        //}

        //// GET: Blogs/Edit/5
        //public async Task<IActionResult> Edit(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var blog = await _context.Blog.FindAsync(id);
        //    if (blog == null)
        //    {
        //        return NotFound();
        //    }
        //    return View(blog);
        //}

        //[HttpGet("get-all-blogs")]
        //[Authorize]
        //public ActionResult<IEnumerable<Blog>> GetAllBlogs()
        //{
        //    // Retrieve all blogs from the database in descending order based on timestamp
        //    var blogs = _context.Blog.OrderByDescending(b => b.TimeStamp).ToList();
        //    return Ok(blogs);
        //}


        //// POST: Blogs/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Edit(string id, [Bind("Title,Username,Description,TimeStamp")] Blog blog)
        //{
        //    if (id != blog.Title)
        //    {
        //        return NotFound();
        //    }

        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _context.Update(blog);
        //            await _context.SaveChangesAsync();
        //        }
        //        catch (DbUpdateConcurrencyException)
        //        {
        //            if (!BlogExists(blog.Title))
        //            {
        //                return NotFound();
        //            }
        //            else
        //            {
        //                throw;
        //            }
        //        }
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(blog);
        //}

        //// GET: Blogs/Delete/5
        //public async Task<IActionResult> Delete(string id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var blog = await _context.Blog
        //        .FirstOrDefaultAsync(m => m.Title == id);
        //    if (blog == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(blog);
        //}

        //// POST: Blogs/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(string id)
        //{
        //    var blog = await _context.Blog.FindAsync(id);
        //    if (blog != null)
        //    {
        //        _context.Blog.Remove(blog);
        //    }

        //    await _context.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}

        //private bool BlogExists(string id)
        //{
        //    return _context.Blog.Any(e => e.Title == id);
        //}
    }
}
