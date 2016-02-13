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

        public IActionResult Community()
        {
            ViewData["Message"] = "老虎工作室社区依托于微信平台，旨在为爱学习爱分享的家长建立一个互相分享互相学习的平台。目前已经了汇集了全国各地数万名爱听故事、爱阅读的孩子的家长，每天在群内就孩子的成长各抒己见，话题涉及学习娱乐生活的方方面面。";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
