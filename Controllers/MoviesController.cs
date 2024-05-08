using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcMovie.Data;
using MvcMovie.Models;

namespace MvcMovie.Controllers
{
    public class MoviesController : Controller
    {
        private readonly MvcMovieContext _context;//构造函数使用依赖关系注入将数据库上下文注入到控制器中，在每个CURD方法中使用

        public MoviesController(MvcMovieContext context)
        {
            _context = context;
        }

        // GET: Movies
        public async Task<IActionResult> Index(string searchString, string movieGenre)
        {
            IQueryable<string> genreQuery = from m in _context.Movie
                                            orderby m.Genre
                                            select m.Genre;

            var movies = from m in _context.Movie
						 select m;

			if (!string.IsNullOrEmpty(searchString))
			{
				movies = movies.Where(s => s.Title.Contains(searchString));
			}

            //if (!string.IsNullOrEmpty(movieGenre))
            //{
            //    movies = movies.Where(x => x.Genre == movieGenre);
            //}

            //var movieGenreVM = new MovieGenreViewModel
            //{
            //    Genres = new SelectList(await genreQuery.Distinct().ToListAsync()),
            //    Movies = await movies.ToListAsync()
            //};

            return _context.Movie != null ? 
                          View(await movies.ToListAsync()) :
                          Problem("Entity set 'MvcMovieContext.Movie'  is null.");
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);//重要安全功能，代码会先验证搜索方法已经找到电影，然后再执行操作。
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // GET: Movies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]//防止请求伪造，并与视图文件中生成的防伪标记进行配对。
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Genre,Price")] Movie movie)
		//限制控制器操作方法接收和绑定的属性，只绑定特定属性，而不是将所有请求数据都绑定到模型。这有助于增强安全性和控制数据流。
		{
			if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movie == null)
            {
                return NotFound();
            }

            var movie = await _context.Movie
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]//向DeleteConfirmed 方法添加 ActionName("Delete") 属性。 该属性对路由系统执行映
        //射，以便包括 POST 请求的 /Delete/ 的 URL可找到 DeleteConfirmed 方法。
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)//该方法不删除指定的电影，而是返回可在其中提交删除的电影的视图。
        {
            if (_context.Movie == null)
            {
                return Problem("Entity set 'MvcMovieContext.Movie'  is null.");
            }
            var movie = await _context.Movie.FindAsync(id);
            if (movie != null)
            {
                _context.Movie.Remove(movie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
          return (_context.Movie?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
