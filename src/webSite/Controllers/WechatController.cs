using System;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http;
using System.Text;
using Microsoft.AspNet.Mvc;
using System.Security.Cryptography;
using webSite.ViewModels.Wechat;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Caching;
using webSite.Models;
using System.Diagnostics;
using System.Web.Security;
using System.IO;
using System.Xml.Linq;
using ifunction.WebChatApi.Contract;
using System.Collections.Generic;

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
        private const string JumpUrl = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=2652365914&idx=1&sn=3564beb4d5718971e06a1bad6a1640ea&chksm=8b5e856ebc290c7869b62dff8b3d02af4ae0b8410c34f338b0ae9646f39c11cdf591695b7060&mpshare=1&scene=1&srcid=1003j5eHSBVQcFHGfnWNWxlt#rd";

        public WechatController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> ClassExpired(string id)
        {
            if (string.IsNullOrEmpty(id)) id = "General";
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.Title = "您正在访问的课件已过期";

            return View(id + "_Expired", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> MorningNightSharing(string id)
        {
            ViewData["enableRolling"] = Request.Query["enableRolling"].ToString();
            var matchedItem = _context.MorningNightSharings.First<MorningNightSharing>(mns =>
                mns.Type == "MorningNightSharing" && string.Format("{0}_{1}", mns.Date.ToString("yyyyMMdd"), mns.IsMorning.Value ? "1" : "2") == id
            );
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.ID = string.Format("{0}_{1}", matchedItem.Date.ToString("yyyyMMdd"), matchedItem.IsMorning.Value ? "1" : "2");
            viewModel.Author = matchedItem.Author;
            viewModel.Description = matchedItem.Description;
            viewModel.Title = matchedItem.Title;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Courseware(string id)
        {
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.ID = id;
            switch (id)
            {
                case "StepIntoReading_1":
                    viewModel.Title = "《兰登英语》第一阶段基础课程";
                    viewModel.Author = "月亮姐姐";
                    viewModel.Description = "Step into Reading分为五个级别，循序渐进，由浅入深，内容涵盖经典故事、数学、生活、科普类等。全套书都是原汁原味的英语原版图书，书中用词地道，词汇丰富，插画漂亮，有助于提高孩子的英语阅读能力。每一个级别都为不同阅读能力的孩子设置了适合其应对的小挑战，这让每个孩子都能从阅读中获得快乐并保持良好的信心。";
                    viewModel.ImgUrl = "http://tigerstudio.blob.core.chinacloudapi.cn/mediafiles/StepIntoReading/StepIntoReading_s.png";
                    break;
                case "DKHomeDoctor":
                    viewModel.Title = "经营你的人生从健康开始";
                    viewModel.Author = "二喵居士";
                    viewModel.Description = "家中对慢性疾病的自我管理和有效地处理？突发疾病和紧急状况该怎样处理呢？往返医院不仅浪费时间和金钱，有老人和孩子的话更是要受劳顿之苦。<br/><br/>我们应该怎样才能做到明明白白地看病？在与医生对话时，如何有助于医生快速而清楚地了解症状表现，及时准确地诊断病情？诊疗设备与检查方法在哪里可以得以了解？我们就医时做到心中有数，避免一些重复检查，甚至过度医疗。";
                    viewModel.ImgUrl = "https://tigerstudio.blob.core.chinacloudapi.cn/mediafiles/Others/DKHomeDoctor_s.png";
                    break;
            }            

            if (Request.Query["enableSharing"].ToString() == "true")
            {
                viewModel.WechatUrl = viewModel.Url.Replace("enableSharing=true", "enableSharing=false");
                viewModel.WechatTitle = viewModel.Title;
                viewModel.WechatDescription = viewModel.Description;
                viewModel.WechatImgUrl = viewModel.ImgUrl;

                switch (id)
                {
                    case "StepIntoReading_1":
                        viewModel.WechatDescription = "每一个级别都为不同阅读能力的孩子设置了适合其应对的小挑战，这让每个孩子都能从阅读中获得快乐并保持良好的信心。";
                        break;
                    case "DKHomeDoctor":
                        viewModel.WechatDescription = "防病于未然，实现与医生间的有效沟通与合作，使身体尽快恢复健康。提高生活质量，使身体和心理均处于一种良好的状态。";
                        break;
                }
            }
            else
            {
                viewModel.WechatUrl = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=2652364991&idx=2&sn=2a63bb2ea57dc6f5af7556c16e1fe7aa&scene=23&srcid=0807jer6rXaJMHPuRNXjHpl0#rd";
                viewModel.WechatTitle = "【我要报名】100周不间断教唱，玩转鹅妈妈童谣 | 英语启蒙、情商启蒙、认知及身体发展全面培养";
                viewModel.WechatDescription = "鹅妈妈是英美儿童朗朗上口、耳熟能详的童谣。它独特的声音趣味，入耳难忘，又容易念诵，因此，靠着口耳相传传诵许久。";
                viewModel.WechatImgUrl = "https://tigerstudio.blob.core.chinacloudapi.cn/mediafiles/MotherGoose/MotherGoose.png";
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EnglishClass(string id)
        {            
            ViewData["enableRolling"] = Request.Query["enableRolling"].ToString();
            ViewData["sharable"] = Request.Query["sharable"].ToString();
            ViewData["viponly"] = Request.Query["viponly"].ToString();
            ViewData["disableAudio"] = Request.Query["disableAudio"].ToString();
            var matchedItem = _context.MorningNightSharings.First<MorningNightSharing>(mns =>
                (mns.Type == "EnglishClass" || mns.Type == "ParentClass" || mns.Type == "ChildClass") && mns.AudioName == id
            );
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.ID = matchedItem.AudioName;
            viewModel.Author = matchedItem.Author;
            viewModel.Description = matchedItem.Description;
            viewModel.Title = matchedItem.Title;
            viewModel.CreateDate = matchedItem.Date;
            viewModel.InsidePagePictures = matchedItem.InsidePagePictures == null ? null : matchedItem.InsidePagePictures.Split(',');

            if (Request.Query["enableSharing"].ToString() == "true")
            {
                viewModel.WechatUrl = viewModel.Url.Replace("enableSharing=true", "enableSharing=false");
                viewModel.WechatTitle = viewModel.Title;
                viewModel.WechatDescription = viewModel.Description;
                viewModel.WechatImgUrl = viewModel.ImgUrl;

                if (id.Contains("StepIntoReading_"))
                {
                    viewModel.WechatDescription = "每一个级别都为不同阅读能力的孩子设置了适合其应对的小挑战，这让每个孩子都能从阅读中获得快乐并保持良好的信心。";
                }
                else if (id.Contains("Oxford_"))
                {
                    viewModel.WechatDescription = "通过专业的英语教学设计，整套书特别容易引发孩子的学习兴趣，好看好玩又学得容易，所以使得Oxford Reading Tree《牛津阅读树》成为了全球风靡的英语分级阅读教材。";
                }
                else if (id.Contains("MotherGoose_"))
                {
                    viewModel.WechatDescription = "有了语感，才能把学过的一颗颗漂亮的珍珠串起来，变成美丽的珍珠项链。光有珍珠没有线，那只能算是一盘散沙而已。";
                }
                else if (id.Contains("CC_Math_"))
                {
                    viewModel.WechatDescription = "孩子们最初都是通过数数学习数学的。物品的数量有几个？自己的家在几楼？爸爸妈妈的手机号码是多少等等，孩子们很自然地接触到各种数。";
                }
                else if (id.Contains("PeppaPig3_"))
                {
                    viewModel.WechatDescription = "不少父母虽然有心教孩子英语，却苦于自己发音不准，怕误导了孩子。很多妈妈说孩子不爱看英文书，因为对孩子来说，英文就是一门他们听不懂的语言，人天生都是畏难的，自己不懂的东西就不自主地想逃避。";
                }
                else if (id.Contains("Biscuit_"))
                {
                    viewModel.WechatDescription = "小饼干的故事就是孩子们自己的故事，它讲述了孩子们在成长过程中的点点滴滴。文字朴实生动，平易近人；画面温馨可爱，极具亲和力。通过一个个充满生活气息的小故事，给孩子们展现了一个触手可及的小伙伴的形象。";
                }
                else if (id.Contains("TheVery_"))
                {
                    viewModel.WechatDescription = "艾瑞卡尔的文笔带有诗趣，故事简洁轻快，每本书都有多重的内涵：趣味、想象、色彩、幽默……他的创作流露出童稚般的天真，表达出对自然的了解与关爱，也引导小朋友从身边的事物中学习。";
                }
            }
            else
            {
                viewModel.WechatUrl = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=2652364991&idx=2&sn=2a63bb2ea57dc6f5af7556c16e1fe7aa&scene=23&srcid=0807jer6rXaJMHPuRNXjHpl0#rd";
                viewModel.WechatTitle = "【我要报名】100周不间断教唱，玩转鹅妈妈童谣 | 英语启蒙、情商启蒙、认知及身体发展全面培养";
                viewModel.WechatDescription = "鹅妈妈是英美儿童朗朗上口、耳熟能详的童谣。它独特的声音趣味，入耳难忘，又容易念诵，因此，靠着口耳相传传诵许久。";
                viewModel.WechatImgUrl = "https://tigerstudio.blob.core.chinacloudapi.cn/mediafiles/MotherGoose/MotherGoose.png";
            }

            if (Request.Query["directAccess"].ToString() != "true")
            {
                if ((new string[] { "Elmer_Elmer", "Elmer_ElmerInTheSnow_1", "Elmer_ElmerInTheSnow_2", "Elmer_ElmerAndTheStranger_1", "Elmer_ElmerAndTheStranger_2", "Elmer_ElmerAndTheLostTeddy_1", "Elmer_ElmerAndTheLostTeddy_2" }).Contains(id))
                {
                    return Redirect("http://www.tigerartstudio.cn/wechat/ClassExpired/General");
                }
                else if (id.StartsWith("StepIntoReading_") && viewModel.CreateDate < DateTime.Parse("2016-09-15"))
                {
                    return Redirect("http://www.tigerartstudio.cn/wechat/ClassExpired/StepIntoReading");
                }
                else if (id.StartsWith("MotherGoose_") && viewModel.CreateDate < DateTime.Parse("2016-09-24"))
                {
                    return Redirect("http://www.tigerartstudio.cn/wechat/ClassExpired/MotherGoose_1");
                }
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> StepIntoReading_GradedReader()
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

            jsSDKViewModel.JumpUrl = JumpUrl;

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
