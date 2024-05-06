using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

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
    public string Welcome(string name, int ID = 1)
        {
            return HtmlEncoder.Default.Encode($"Hello {name}, ID is: {ID}");
        }
    }
}
