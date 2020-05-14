using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Asparagus.Data;
using Asparagus.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using X.PagedList;

namespace Asparagus.Controllers
{
    public class PeopleController : Controller
    {
        private readonly AsparagusContext _context;

        AsparagusContext db;
        public PeopleController(AsparagusContext context, AsparagusContext db)
        {
            _context = context;
            this.db = db;
        }

        public async Task<IActionResult> Index(SortState sortOrder = SortState.ModifiedDateDesc, int pageNumber = 1)
        {
            IQueryable<Person> users = db.People;
            ViewData["CreateDataSort"] = sortOrder == SortState.CreateDateAsc ? SortState.CreateDateDesc : SortState.CreateDateAsc;
            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["CountOfAsparagusSort"] = sortOrder == SortState.CountOfAsparagusAsc ? SortState.CountOfAsparagusDesc : SortState.CountOfAsparagusAsc;
            ViewData["ModifiedDataSort"] = sortOrder == SortState.ModifiedDateAsc ? SortState.ModifiedDateDesc : SortState.ModifiedDateAsc;

            users = sortOrder switch
            {
                SortState.CreateDateAsc => users.OrderBy(s => s.CreateDate),
                SortState.CreateDateDesc => users.OrderByDescending(s => s.CreateDate),
                SortState.NameDesc => users.OrderByDescending(s => s.Name),
                SortState.NameAsc => users.OrderBy(s => s.Name),
                SortState.CountOfAsparagusAsc => users.OrderBy(s => s.CountOfAsparagus),
                SortState.CountOfAsparagusDesc => users.OrderByDescending(s => s.CountOfAsparagus),
                SortState.ModifiedDateAsc => users.OrderBy(s => s.ModifiedDate),
                _ => users.OrderByDescending(s => s.ModifiedDate),
            };
            return View(await PaginatedList<Person>.CreateAsync(users.AsNoTracking(), pageNumber, 15));
            //return View(await users.ToListAsync());
        }


        // GET: People/Create
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Id,CreateDate,CountOfAsparagus,ModifiedDate,Email")] Person person)
        {
            if (ModelState.IsValid)
            {
                var personData = _context.People.FirstOrDefault(p => p.Name == person.Name && p.Email == person.Email);

                if (personData == null)
                {
                    if (_context.People.Any(p => p.Email == person.Email))
                    {
                        ModelState.AddModelError("Email", "Этот логин используется другим пользователем");
                    }
                    else
                    {
                        _context.Add(person);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
                else
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(person);
        }
    }
}
