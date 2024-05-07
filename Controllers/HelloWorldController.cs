using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
/*
 * 视图模板不应该直接执行业务逻辑或与数据库进行交互。相反，视图模板应该使用控制器所提供的数据。
 * 可以让控制器将视图模板所需的动态数据放置再视图模板可以稍后访问的viewdata字典
 * 
 */

namespace MvcMovie.Controllers
{
    public class HelloWorldController : Controller
    {
        //
        // GET: /HelloWorld/
        //Index方法是默认方法，如果不另指定方法，默认执行Index方法
        public IActionResult Index()
        {
            return View();//指定此方法使用视图模板来呈现对浏览器的响应，因为未指定视图模板名称默认使用index
        }
             

    //
    // GET: /HelloWorld/Welcome/
    //public string Welcome(string name,int numTimes=1)
    //{
    //    return HtmlEncoder.Default.Encode($"Hello {name}, NumTimes is: {numTimes}");
    //}
    public IActionResult Welcome(string name, int numTimes = 1)
        {
            ViewData["Message"] = "Hello" + name;
            ViewData["NumTimes"] = numTimes;
            return View();
        }
    }
}
