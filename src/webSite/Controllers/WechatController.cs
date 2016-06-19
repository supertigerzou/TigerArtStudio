using System;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Xml.Linq;
using Microsoft.AspNet.Mvc;
using System.Security.Cryptography;
using webSite.ViewModels.Wechat;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using System.Web;
using System.Web.Caching;
using webSite.Models;

namespace TigerStudio.Wechat.Controllers
{
    public class TokenUrlResponse { public string access_token { get; set; } public int expires_in { get; set; } }
    public class TicketUrlResponse { public string ticket { get; set; } public int expires_in { get; set; } }

    public class WechatController : Controller
    {
        private ApplicationDbContext _context;

        private const string Token = "alibaba";
        private const string TokenUrl = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=wxf0aa207b2437d9bc&secret=c1665fa38e6445c9d415c812bb0d61fc";
        private const string TicketUrl = "https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi";

        public WechatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> MorningNightSharing(string id)
        {
            var matchedItem = _context.MorningNightSharings.First<MorningNightSharing>(mns =>
                string.Format("{0}_{1}", mns.Date.ToString("yyyyMMdd"), mns.IsMorning ? "1" : "2") == id
            );
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.ID = string.Format("{0}_{1}", matchedItem.Date.ToString("yyyyMMdd"), matchedItem.IsMorning ? "1" : "2");
            viewModel.Author = matchedItem.Author;
            viewModel.Description = matchedItem.Description;
            viewModel.Title = matchedItem.Title;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> DinasourIsComing()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Dinasour_4()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Dinasour_5()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Dinasour_6()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> DinasourReading_1()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> OrientalBaby()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> RedHat()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> RedHat_OakBookLibrary()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Present()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_GetOn_FirstEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_GetOn_SecondEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_FloppyDidThis_FirstEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_FloppyDidThis_SecondEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_KippersAlphabetISpy_FirstEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_KippersAlphabetISpy_SecondEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_ChipsLetterSounds_FirstEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_ChipsLetterSounds_SecondEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_UpYouGo()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_SixInABed()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> LittlePrincess_IWantMyPotty()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> LittlePrincess_IWantMyDummy()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> LittlePrincess_IWantAParty()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> LittlePrincess_IDontWantToGoToHospital()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CalabashBrothers_AD()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CalabashBrothers_1()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CalabashBrothersReading_1()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CalabashBrothersReading_2()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CalabashBrothersReading_3()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> ReadingAdviceToPreSchoolKids()
        {
            return View(await ConstructJSSDKViewModel());
        }

        #region private methods
        private async Task<JSSDKViewModel> ConstructJSSDKViewModel()
        {
            var jsSDKViewModel = new JSSDKViewModel
            {
                NouceString = "alibaba",
                Timestamp = 1464483842,//DateTime.Now.ToFileTime(),
                Url = string.Format("{0}://{1}{2}{3}", Request.Scheme, Request.Host.Value, Request.Path.Value, Request.QueryString.Value)
            };

            if (HttpRuntime.Cache.Get("JsApiTicket") == null)
            {
                using (var handler = new HttpClientHandler())
                {
                    handler.ClientCertificateOptions = ClientCertificateOption.Automatic;
                    using (var client = new HttpClient(handler))
                    {
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        HttpResponseMessage response = await client.GetAsync(TokenUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            TokenUrlResponse token = await response.Content.ReadAsAsync<TokenUrlResponse>();

                            response = await client.GetAsync(string.Format(TicketUrl, token.access_token));
                            TicketUrlResponse ticket = await response.Content.ReadAsAsync<TicketUrlResponse>();
                            if (ticket.ticket == null) ticket.ticket = "sM4AOVdWfPE4DxkXGEs8VMCPGGVi4C3VM0P37wVUCFvkVAy_90u5h9nbSlYy3-Sl-HhTdfl2fzFy1AOcHKP7qg";
                            HttpRuntime.Cache.Insert("JsApiTicket", ticket.ticket, null, DateTime.Now.AddSeconds(7000), Cache.NoSlidingExpiration, CacheItemPriority.High, null);
                        }
                    }
                }
            }
            jsSDKViewModel.JsApiTicket = HttpRuntime.Cache.Get("JsApiTicket") as string;
            jsSDKViewModel.Signature = GenerateSignature(jsSDKViewModel);

            return jsSDKViewModel;
        }

        private string GenerateSignature(JSSDKViewModel model)
        {
            return SHA1_Hash(string.Format("jsapi_ticket={0}&noncestr={1}&timestamp={2}&url={3}", model.JsApiTicket, model.NouceString, model.Timestamp, model.Url));
        }

        static public string SHA1_Hash(string str_sha1_in)
        {
            HashAlgorithm algorithm = SHA1.Create();
            byte[] bytes_sha1_in = Encoding.UTF8.GetBytes(str_sha1_in);
            byte[] bytes_sha1_out = algorithm.ComputeHash(bytes_sha1_in);
            string str_sha1_out = BitConverter.ToString(bytes_sha1_out);
            str_sha1_out = str_sha1_out.ToLower().Replace("-", "");
            return str_sha1_out;
        }
        #endregion
    }
}
