using Microsoft.EntityFrameworkCore;
using MvcMovie.Models;

namespace MvcMovie.Data//数据库上下文类
{
	public class MvcMovieContext : DbContext
	{
		public MvcMovieContext(DbContextOptions<MvcMovieContext> options)
		: base(options)
		{
		}
		public DbSet<Movie> Movie { get; set; }
	}
}
