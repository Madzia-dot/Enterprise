using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcNews.Data;
using MvcNews.Models;

namespace MvcNews.Controllers
{
    public class NewsController : Controller
    {
        private readonly NewsDbContext _context;

        public NewsController(NewsDbContext context)
        {
            _context = context;
        }

        // GET: News
        public async Task<IActionResult> Index()
        {
            return View(await _context.News.ToListAsync());
        }

        // GET: News/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // GET: News/Create
        public IActionResult Create()
        {
            var newsItem = new NewsItem { TimeStamp = DateTime.Now };
            return View(newsItem);
        }

        // POST: News/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TimeStamp,Text,RowVersion")] NewsItem newsItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(newsItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(newsItem);
        }

        // GET: News/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News.FindAsync(id);
            if (newsItem == null)
            {
                return NotFound();
            }
            return View(newsItem);
        }

        // POST: News/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TimeStamp,Text,RowVersion")] NewsItem newsItem)
        {
            if (id != newsItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(newsItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException e)
                {
                    if (!NewsItemExists(newsItem.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Edytowane dane zostały zmodyfikowane lub usunięte przez innego użytkownika.");
                        var entry = e.Entries.Single();
                        var databaseEntry = entry.GetDatabaseValues();
                        var databaseEntity = (NewsItem)databaseEntry.ToObject();
                        newsItem.RowVersion = (byte[])databaseEntity.RowVersion;
                        ModelState.Remove("RowVersion");
                        ModelState.AddModelError("TimeStamp", "Aktualna wartość: " + (DateTime)databaseEntity.TimeStamp);
                        ModelState.AddModelError("Text", "Aktualna wartość: " + (string)databaseEntity.Text);
                        return View(newsItem);
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(newsItem);
        }

        // GET: News/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var newsItem = await _context.News
                .FirstOrDefaultAsync(m => m.Id == id);
            if (newsItem == null)
            {
                return NotFound();
            }

            return View(newsItem);
        }

        // POST: News/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, NewsItem newsItem)
        {
            if (id != newsItem.Id)
            {
                return NotFound();
            }

            try
            {
                _context.News.Remove(newsItem);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException e)
            {
                if (!NewsItemExists(newsItem.Id))
                {
                    return NotFound();
                }
                else
                {
                    ModelState.AddModelError("", "Nie można zapisać zmian. Rekord został zmodyfikowany przez innego użytkownika.");
                    var entry = e.Entries.Single();
                    var databaseEntry = entry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        return NotFound();
                    }
                    var databaseEntity = (NewsItem)databaseEntry.ToObject();
                    ModelState.Remove("RowVersion");
                    return View(databaseEntity);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        private bool NewsItemExists(int id)
        {
            return _context.News.Any(e => e.Id == id);
        }
    }
}
