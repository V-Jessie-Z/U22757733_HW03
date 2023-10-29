using U22757733_HW03.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using GemBox.Document;
using System.Text.RegularExpressions;
using PagedList.Mvc;

namespace U22757733_HW03.Controllers
{
    public class HomeController : Controller
    {
         private LibraryEntities db = new LibraryEntities();
        private const int PageSize = 10;


        public async Task<ActionResult>CombinedHomeIndex(int? page)
        {
            

            var viewModel = new ViewModel
            {
                books = await db.books
                                    .Include(s => s.author)
                                    .Include(s => s.type)
                                    .ToListAsync(),
                borrows = await db.borrows
                                    .Include(c => c.student)
                                    .Include(c => c.book)
                                    .ToListAsync(),
                students = await db.students.ToListAsync(),
                types = await db.types.ToListAsync(),
                authors = await db.authors.ToListAsync()
            };

            int pageNumber = page ?? 1;
            int pageSize = 10; // Set your desired page size here.

            // Paginate the students data.
            var studentsQuery = db.students.OrderBy(s => s.studentId).AsQueryable();
            var studentsPagedList = studentsQuery.ToPagedList(pageNumber, pageSize);
            viewModel.students = studentsPagedList;

            // Paginate the books data.
            var booksQuery = db.books
                .Include(b => b.author)
                .Include(b => b.type)
                .OrderBy(b => b.bookId)
                .AsQueryable();
            var booksPagedList = booksQuery.ToPagedList(pageNumber, pageSize);
            viewModel.books = booksPagedList;

        

            return View(viewModel);
        }

        public async Task<ActionResult> Maintain(int? page)
        {
            
            var viewModel = new ViewModel
            {
                books = await db.books
                                    .Include(s => s.author)
                                    .Include(s => s.type)
                                    .ToListAsync(),
                borrows = await db.borrows
                                    .Include(c => c.student)
                                    .Include(c => c.book)
                                    .ToListAsync(),
                students = await db.students.ToListAsync(),
                types = await db.types.ToListAsync(),
                authors = await db.authors.ToListAsync()

            };
            int pageNumber = page ?? 1;
            int pageSize = 10; // Set your desired page size here.

            // Paginate the students data.
            var studentsQuery = db.students.OrderBy(s => s.studentId).AsQueryable();
            var studentsPagedList = studentsQuery.ToPagedList(pageNumber, pageSize);
            viewModel.students = studentsPagedList;

            // Paginate the books data.
            var booksQuery = db.books
                .Include(b => b.author)
                .Include(b => b.type)
                .OrderBy(b => b.bookId)
                .AsQueryable();
            var booksPagedList = booksQuery.ToPagedList(pageNumber, pageSize);
            viewModel.books = booksPagedList;

            return View(viewModel);
        }

        public ActionResult Report()
        {
            var monthdata = db.borrows
            .Where(b => b.takenDate.HasValue )
            .GroupBy(b => b.takenDate.Value.Year)
            .Select(g => new
            {
                Year =g.Key,
                ACount = (double)g.Count() /12
            })
            .OrderBy(y=> y.Year)
            .ToList();

            ViewBag.MonthData = monthdata ;

            return View();
        }

     
       
        public async Task<ActionResult> SIndex()
        {
            return View(await db.students.ToListAsync());
        }

        // GET: students/Details/5
        public async Task<ActionResult> SDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // GET: students/Create
        public ActionResult SCreate()
        {
            return View();
        }

        // POST: students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SCreate([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] student student)
        {
            if (ModelState.IsValid)
            {
                db.students.Add(student);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(student);
        }

        // GET: students/Edit/5
        public async Task<ActionResult> SEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SEdit([Bind(Include = "studentId,name,surname,birthdate,gender,class,point")] student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        // GET: students/Delete/5
        public async Task<ActionResult> SDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            student student = await db.students.FindAsync(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }

        // POST: students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SDeleteConfirmed(int id)
        {
            student student = await db.students.FindAsync(id);
            db.students.Remove(student);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public async Task<ActionResult> BIndex()
        {
            var books = db.books.Include(b => b.author).Include(b => b.type);
            return View(await books.ToListAsync());
        }

        // GET: books/Details/5
        public async Task<ActionResult> BDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = await db.books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // GET: books/Create
        public ActionResult BCreate()
        {
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name");
            ViewBag.typeId = new SelectList(db.types, "typeId", "name");
            return View();
        }

        // POST: books/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BCreate([Bind(Include = "bookId,name,pagecount,point,authorId,typeId")] book book)
        {
            if (ModelState.IsValid)
            {
                db.books.Add(book);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", book.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", book.typeId);
            return View(book);
        }

        // GET: books/Edit/5
        public async Task<ActionResult> BEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = await db.books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", book.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", book.typeId);
            return View(book);
        }

        // POST: books/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BEdit([Bind(Include = "bookId,name,pagecount,point,authorId,typeId")] book book)
        {
            if (ModelState.IsValid)
            {
                db.Entry(book).State = System.Data.Entity.EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.authorId = new SelectList(db.authors, "authorId", "name", book.authorId);
            ViewBag.typeId = new SelectList(db.types, "typeId", "name", book.typeId);
            return View(book);
        }

        // GET: books/Delete/5
        public async Task<ActionResult> BDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            book book = await db.books.FindAsync(id);
            if (book == null)
            {
                return HttpNotFound();
            }
            return View(book);
        }

        // POST: books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> BDeleteConfirmed(int id)
        {
            book book = await db.books.FindAsync(id);
            db.books.Remove(book);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

       
       
    }
}
