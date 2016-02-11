using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

namespace webSite.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult BookStore()
        {
            ViewData["Message"] = "听故事、读好书，老虎工作室依托于电台几十万粉丝，和各大出版社合作推出了适合于各个年龄阶段的优质童书以及适合于父母阅读的各类儿童教育书籍，以最合理的价格，带给您和您家宝贝不一样的精彩。进入书店分两种模式，逛街模式适用于按书籍类别随意浏览，搜索模式适用于按特定需求精准定位。";

            return View();
        }

        public IActionResult RadioStation()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
