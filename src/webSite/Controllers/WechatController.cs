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
        public async Task<IActionResult> EnglishClass(string id)
        {
            ViewData["enableRolling"] = Request.Query["enableRolling"].ToString();
            var matchedItem = _context.MorningNightSharings.First<MorningNightSharing>(mns =>
                mns.Type == "EnglishClass" && mns.AudioName == id
            );
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.ID = matchedItem.AudioName;
            viewModel.Author = matchedItem.Author;
            viewModel.Description = matchedItem.Description;
            viewModel.Title = matchedItem.Title;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BrownBearCollection(string id)
        {
            ViewData["enableRolling"] = Request.Query["enableRolling"].ToString();
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.ID = id;
            switch (id)
            {
                case "BrownBearClass1":
                    viewModel.Description = "本课件有效期截止<span style=\"color: red; font - weight:bold\">7/15</span>,如需领取第一阶段完整课程音频包，请遵照管理员提供的<span style=\"font - weight:bold\">签到流程</span>每次课后坚持打卡，满<span style=\"color: red; font - weight:bold\">8次</span>即可顺利毕业获赠伴读音频。<br/><br/>今日课后互动时间欢迎小朋友踊跃参与，活动规则如下：选择本书中的一句完整的话练习熟练之后，在群中通过微信语音的方式念出来或唱出来，如 Yellow duck, yellow duck, What do you see? I see a blue horse looking at me. 大家尽量挑选不同的句子，这样我们有可能拼装出完整的段落。<br/><br/>我们会采集每一句讲得/唱得最好的一个声音用来制作特别电台节目并发布到微信公众号，系列课程全部结束之后我们也会挑选10位英文讲得最地道的孩子送出奖励。";
                    viewModel.Title = "♪【边读边唱】Brown Bear, Brown Bear, What do you see?";
                    break;
                case "BrownBearClass2":
                    viewModel.Description = "本课件有效期截止<span style=\"color: red; font - weight:bold\">7/15</span>,如需领取第一阶段完整课程音频包，请遵照管理员提供的<span style=\"font - weight:bold\">签到流程</span>每次课后坚持打卡，满<span style=\"color: red; font - weight:bold\">8次</span>即可顺利毕业获赠伴读音频。<br/><br/>暑期特价书籍火热团购中，点击下方的<span style=\"color: red; font - weight:bold\">精品童书</span>按钮了解详情。";
                    viewModel.Title = "♪【边玩边学】Brown Bear, Brown Bear, What do you see?";
                    break;
                case "PandaBearClass1":
                    viewModel.Description = "本课件有效期截止<span style=\"color: red; font - weight:bold\">7/15</span>,如需领取第一阶段完整课程音频包，请遵照管理员提供的<span style=\"font - weight:bold\">签到流程</span>每次课后坚持打卡，满<span style=\"color: red; font - weight:bold\">8次</span>即可顺利毕业获赠伴读音频。<br/><br/>今日课后互动时间欢迎小朋友踊跃参与，活动规则如下：选择本书中的一句完整的话练习熟练之后，在群中通过微信语音的方式念出来或唱出来，如 Yellow duck, yellow duck, What do you see? I see a blue horse looking at me. 大家尽量挑选不同的句子，这样我们有可能拼装出完整的段落。<br/><br/>我们会采集每一句讲得/唱得最好的一个声音用来制作特别电台节目并发布到微信公众号，系列课程全部结束之后我们也会挑选10位表现最出色的孩子送出奖励。";
                    viewModel.Title = "♪【伴读】Panda Bear第一课，进一步了解动物";
                    break;
                case "PandaBearClass2":
                    viewModel.Description = "本课件有效期截止<span style=\"color: red; font - weight:bold\">7/15</span>,如需领取第一阶段完整课程音频包，请遵照管理员提供的<span style=\"font - weight:bold\">签到流程</span>每次课后坚持打卡，满<span style=\"color: red; font - weight:bold\">8次</span>即可顺利毕业获赠伴读音频。<br/><br/>兰登英语88周陪伴阅读计划持续招募中，点击右下角的<span style=\"font-weight:bold; font-size:medium\">精彩课程</span>了解详情。";
                    viewModel.Title = "♪【伴读】Panda Bear第二课，每一个孩子都是独一无二的，家长所能给予最好的是陪伴";
                    break;
                case "PolarBear":
                    viewModel.Description = "Polar Bear, Polar Bear, what do you hear?<br/>北极熊，北极熊，你听到什么？<br/>I hear a lion roaring in my ear.<br/>我听到一头狮子在我的耳朵里咆哮。<br/><br/>Lion, Lion, what do you hear?<br/>狮子，狮子，你听到什么？<br/>I hear a hippopotamus snorting in my ear.<br/>我听到一只河马在我的耳朵里打哼声。<br/><br/>Hippopotamus, Hippopotamus, what do you hear?<br/>河马，河马，你听到什么？<br/>I hear a flamingo fluting in my ear.<br/>我听到一只红鹤在我耳朵里吹笛。<br/><br/>Flamingo, Flamingo, what do you hear?<br/>红鹤，红鹤，你听到什么？<br/>I hear a zebra braying in my ear.<br/>我听到一只斑马在我耳朵里粗声叫。<br/><br/>Zebra, Zebra, what do you hear?<br/>斑马，斑马，你听到什么？<br/>I hear a boa constrictor hissing in my ear.<br/>我听到一条蟒蛇在我耳朵里发出嘶嘶声。<br/><br/>Boa Constrictor, Boa Constrictor, what do you hear?<br/>蟒蛇，蟒蛇，你听到什么？<br/>I hear an elephant trumpeting in my ear.<br/>我听到一头大象在我耳朵里吼叫。<br/><br/>Elephant, Elephant, what do you hear?<br/>大象，大象，你听到什么？<br/>I hear a leopard snarling in my ear.<br/>我听到一头豹在我的耳朵里吼叫。<br/><br/>Leopard, Leopard, what do you hear?<br/>豹，豹，你听到什？<br/>I hear a peacock yelping in my ear.<br/>我听到一只孔雀在我耳朵里尖叫。<br/><br/>Peacock, Peacock, what do you hear?<br/>孔雀，孔雀，你听到什么？<br/>I hear a walrus bellowing in my ear.<br/>我听到一头海象在我耳朵里吼叫。<br/><br/>Walrus, Walrus, what do you hear?<br/>海象，海象，你听到什么？<br/>I hear a zookeeper whistling in my ear.<br/>我听到一个动物管理员在我耳朵里吹口哨。<br/><br/>Zookeeper, Zookeeper, what do you hear?<br/>管理员，管理员，你听到什么？<br/>I hear children...<br/>我听到孩子们在...<br/>...growling like a polar bear, roaring like a lion, snorting like a hippopotamus, fluting like a flamingo, braying like a zebra, hissing like a boa constrictor, trumpeting like an elephant, snarling like a leopard, yelping like a peacock, bellowing like a walrus...<br/>...学北极熊吼叫，学狮子咆哮，学河马打哼声，学红鹤的笛声，学斑马粗声叫，学蟒蛇发嘶嘶声，学大象吼叫，学豹吼叫，学孔雀尖叫，学海象吼叫...<br/><br/>that's what I hear.<br/>这就是我所听到的。";
                    viewModel.Title = "♪【跟我唱】Polar Bear, Polar Bear, What do you hear?";
                    break;
                case "BabyBearClass1":
                    viewModel.Description = "本课件有效期截止<span style=\"color: red; font - weight:bold\">7/15</span>,如需领取第一阶段完整课程音频包，请遵照管理员提供的<span style=\"font - weight:bold\">签到流程</span>每次课后坚持打卡，满<span style=\"color: red; font - weight:bold\">8次</span>即可顺利毕业获赠伴读音频。<br/><br/>今日课后互动时间欢迎小朋友踊跃参与，活动规则如下：选择本书中的一句完整的话练习熟练之后，在群中通过微信语音的方式念出来或唱出来，如 Yellow duck, yellow duck, What do you see? I see a blue horse looking at me. 大家尽量挑选不同的句子，这样我们有可能拼装出完整的段落。<br/><br/>我们会采集每一句讲得/唱得最好的一个声音用来制作特别电台节目并发布到微信公众号，系列课程全部结束之后我们也会挑选10位英文讲得最地道的孩子送出奖励。";
                    viewModel.Title = "♪【伴读】Baby Bear第一课，认识可爱的动物";
                    break;
                case "BabyBearClass2":
                    viewModel.Description = "本课件有效期截止<span style=\"color: red; font - weight:bold\">7/15</span>,如需领取第一阶段完整课程音频包，请遵照管理员提供的<span style=\"font - weight:bold\">签到流程</span>每次课后坚持打卡，满<span style=\"color: red; font - weight:bold\">8次</span>即可顺利毕业获赠伴读音频。<br/><br/>「<span style=\"color: red; font - weight:bold\">Step into Reading</span>」火热团购中，点击左下角的<b>原版进口</b>了解详情，点击右下角的<b>精彩课程</b>报名参与兰登88周阅读计划。";
                    viewModel.Title = "♪【伴读试听】Baby Bear第二课，培养孩子的英文思维";
                    break;
            }            

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BrownBearCollection2(string id)
        {
            ViewData["enableRolling"] = Request.Query["enableRolling"].ToString();
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.ID = id;
            switch (id)
            {
                case "BrownBear":
                    viewModel.Description = "Brown Bear,Brown Bear, What do you see?<br/>棕熊，棕熊，你在看什么？<br/>I see a red bird looking at me.<br/>我看到了一只红色的鸟，它在看着我<br/><br/>Red Bird,Red Bird, What do you see?<br/>红鸟，红鸟，你在看什么？<br/>I see a yellow duck，looking at me.<br/>我看到了一只黄色的鸭子，它在看着我<br/><br/>Yellow duck, yellow duck, What do you see?<br/>黄色的鸭子，黄色的鸭子，你看到了什么？<br/>I see a blue horse looking at me.<br/>我看见一匹蓝色的马正在看着我。<br/><br/>Blue horse, blue horse, What do you see?<br/>蓝色的马，蓝色的马，你看到了什么？<br/>I see a green frog looking at me.<br/>我看见一只绿色的青蛙正在看着我。<br/><br/>Green frog, green frog, What do you see?<br/>绿色的青蛙，绿色的青蛙，你看到了什么？<br/>I see a purple cat looking at me.<br/>我看见一只紫色的猫正在看着我。<br/><br/>Purple cat, purple cat, What do you see?<br/>紫色的猫，紫色的猫，你看到了什么？<br/>I see a white dog looking at me.<br/>我看见一只白色的狗正在看着我。<br/><br/>White dog, white dog, What do you see?<br/>白狗，白狗，你在看什么？<br/>I see a black sheep looking at me.<br/>我看到一只黑色的绵羊，它在看着我。<br/><br/>Black sheep, black sheep, What do you see?<br/>黑羊，黑羊，你在看什么？<br/>I see a goldfish looking at me.<br/>我看见一条金鱼，正在看着我。<br/><br/>Goldfish, goldfish, What do you see?<br/>金鱼，金鱼，你看到了什么？<br/>I see a teacher looking at me.<br/>我看见了一个老师，正在看着我<br/><br/>Teacher, teacher, What do you see?<br/>老师，老师，你在看什么？<br/>I see children looking at me.<br/>我看到一群孩子正在看着我。<br/><br/>Children,Children,What do you see?<br/>孩子们，孩子们，你在看什么？";
                    viewModel.Title = "♪【跟我唱】Brown Bear, Brown Bear, What do you see?";
                    break;
                case "PandaBear":
                    viewModel.Description = "Panda bear, Panda bear, What do you see?<br/>熊猫，熊猫，你看见了什么？<br/>I see a bald eagle soaring by me.<br/>我看见一只秃鹰在我身边盘旋。<br/><br/>Bald Eagle, Bald Eagle, what do you see？<br/>秃鹰，秃鹰，你看见了什么？<br/>I see a water buffalo charging by me.<br/>我看见头水牛在我身边横冲直撞。<br/><br/>Water Buffalo, Water Buffalo, what do you see?<br/>水牛，水牛，你看见了什么？<br/>I see a spider monkey swinging by me.<br/>我看见一只蜘蛛猴在我身边荡来荡去。<br/><br/>Spider Monkey, Spider Monkey, what do you see?<br/>蜘蛛猴，蜘蛛猴，你看见了什么？<br/>I see a green sea turtle swimming by me.<br/>我看见一只绿海龟在我身边游泳。<br/><br/>Green Sea Turtle, Green Sea Turtle, what do you see?<br/>绿海龟，绿海龟，你看见了什么？<br/>I see a macaroni penguin strutting by me.<br/>我看见一只长冠企鹅在我身边趾高气昂地走。<br/><br/>Macaroni Penguin, Macaroni Penguin, what do you see?<br/>长冠企鹅，长冠企鹅，你看见了什么？<br/>I see a sea lion splashing by me.<br/>我看见一头海狮在我身边拍溅着水。<br/><br/>Sea Lion, Sea Lion, what do you see?<br/>海狮，海狮，你看见了什么？<br/>I see a red wolf sneaking by me.<br/> 我看见一头红狼在我身边鬼鬼祟祟的。<br/><br/>Red Wolf, Red Wolf, what do you see?<br/>红狼，红狼，你看见了什么？<br/>I see a whooping crane flying by me.<br/>我看见一只北美鹤在我身边飞翔。<br/><br/>Whooping Crane, Whooping Crane, what do you see?<br/>北美鹤，北美鹤，你看见了什么？<br/>I see a black panther strolling by me.<br/>我看见一头黑豹在我身边漫步 <br/><br/>Black Panther, Black Panther, what do you see?<br/>黑豹，黑豹，你看见了什么？<br/>I see a dreaming child watching over me.<br/>我看见一个爱幻想的孩子正看着我。<br/><br/>Dreaming Child, Dreaming Child,  what do you see?<br/>爱幻想的孩子，爱幻想的孩子，你看见了什么？<br/>I see...<br/>我看见……<br/>a panda bear, a bald eagle,a water buffalo, a spider monkey, a green sea turtle, a macaroni penguin, a sealion, a red wolf, a whooping crane, and a black panther...all wild and free ---that's what I see.<br/>一只熊猫，一只秃鹰，一头水牛，一只蜘蛛猴，一只绿海龟，一只长冠企鹅，一头海狮，一只红狼，一只北美鹤，一头黑豹，都自由自在的-- - 这是我所看见的。";
                    viewModel.Title = "♪【跟我唱】Panda Bear, Panda Bear, What do you see?";
                    break;
                case "PolarBearClass1":
                    viewModel.Description = "本课件有效期截止<span style=\"color: red; font - weight:bold\">7/15</span>,如需领取第一阶段完整课程音频包，请遵照管理员提供的<span style=\"font - weight:bold\">签到流程</span>每次课后坚持打卡，满<span style=\"color: red; font - weight:bold\">8次</span>即可顺利毕业获赠伴读音频。<br/><br/>今日课后互动时间欢迎小朋友踊跃参与，活动规则如下：选择本书中的一句完整的话练习熟练之后，在群中通过微信语音的方式念出来或唱出来，如 Yellow duck, yellow duck, What do you see? I see a blue horse looking at me. 大家尽量挑选不同的句子，这样我们有可能拼装出完整的段落。<br/><br/>我们会采集每一句讲得/唱得最好的一个声音用来制作特别电台节目并发布到微信公众号，系列课程全部结束之后我们也会挑选10位表现最出色的孩子送出奖励。";
                    viewModel.Title = "♪【伴读】Polar Bear第一课，跟着视频一起跳起来";
                    break;
                case "PolarBearClass2":
                    viewModel.Description = "本课件有效期截止<span style=\"color: red; font - weight:bold\">7/15</span>,如需领取第一阶段完整课程音频包，请遵照管理员提供的<span style=\"font - weight:bold\">签到流程</span>每次课后坚持打卡，满<span style=\"color: red; font - weight:bold\">8次</span>即可顺利毕业获赠伴读音频。<br/><br/>暑期特价书籍火热团购中，点击下方的<span style=\"color: red; font - weight:bold\">精品童书</span>按钮了解详情。";
                    viewModel.Title = "♪【伴读试听】Polar Bear第二课，寓教于乐、轻松培养孩子的英文好感度";
                    break;
                case "BabyBear":
                    viewModel.Description = "Baby Bear, Baby Bear, what do you see?<br/>小熊，小熊，你看到了什么？ <br/>I see a red fox slipping by me.<br/>我看到一头赤狐从我身边溜过。<br/><br/>Red Fox, Red Fox, what do you see?<br/>赤狐，赤狐，你看到了什么？<br/>I see a flying squirrel sliding by me.<br/>我看到一只鼯鼠从我身边滑过。<br/><br/>Flying Squirrel, Flying Squirrel, what do you see?<br/>鼯鼠，鼯鼠，你看到了什么？<br/>I see a mountain goat climbing near me.<br/>我看到一头山羊从我身旁攀过。<br/><br/>Mountain Goat, Mountain Goat, what do you see?<br/>山羊，山羊，你看到了什么？<br/>I see a blue heron flying by me.<br/>我看到一只蓝鹭从我身边飞过。<br/><br/>Blue Heron, Blue Heron, what do you see?<br/>蓝鹭，蓝鹭，你看到了什么？<br/>I see a prairie dog digging by me.<br/>我看到一只草原犬鼠在我身边挖洞。<br/><br/>Prairie Dog, Prairie Dog, what do you see?<br/>犬鼠，犬鼠，你看到了什么？<br/>I see a striped skunk strutting by me.<br/>我看到一只条纹臭鼬大摇大摆地从我身边走过。<br/><br/>Striped Skunk, Striped Skunk, what do you see?<br/>臭鼬，臭鼬，你看到了什么？<br/>I see a mule deer running by me.<br/>我看到一头长耳鹿从我身边跑过。<br/><br/>Mule Deer, Mule Deer, what do you see?<br/>长耳鹿，长耳鹿，你看到了什么？<br/>I see a rattlesnake sliding by me.<br/>我看到一条响尾蛇从我身边爬过。<br/><br/>Rattlesnake, Rattlesnake, what do you see?<br/>响尾蛇，响尾蛇，你看到了什么？<br/>I see a screech owl hooting at me.<br/>我看到一只长耳鸮（xiao）对着我号叫。<br/><br/>Screech Owl, Screech Owl, what do you see?<br/>长耳鸮，长耳鸮，你看到了什么？<br/>I see a mama bear looking at me.<br/>我看到一头熊妈妈在看着我。<br/><br/>Mama Bear, Mama Bear, what do you see?<br/>熊妈妈，熊妈妈，你看到了什么？<br/>I see…<br/>我看到···<br/>a red fox, a flying squirrel, a mountain goat, a blue heron, a prairie dog, a striped skunk, a mule deer, a rattlesnake, a screech owl…my baby bear looking at me－that’s what I see!<br/>一头赤狐、一只鼯鼠、一头山羊、一只蓝鹭、一只草原犬鼠、一只条纹臭鼬、一头长耳鹿、一条响尾蛇、一只长耳鸮、还有我的熊宝宝在看着我···<br/>这就是我所看到的！";
                    viewModel.Title = "♪【跟我唱】Baby Bear, Baby Bear, What do you see?";
                    break;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BabyPhisics(string id)
        {
            JSSDKViewModel viewModel = await ConstructJSSDKViewModel();
            viewModel.ID = id;
            switch (id)
            {
                case "TheABCsOfPhysics":
                    viewModel.Description = "克里斯·费利所著的《宝宝的物理学ABC》根据ABC……XYZ的次序将物理学中最基本的概念，如原子（Atom）、黑洞（BlackHole）、电荷（Charge）等等，通过生动有趣又浅显易懂的绘本形式传递给学龄前的孩子们，让孩子们在婴幼儿时期就早早感受到了科学的魅力。";
                    viewModel.Title = "♪【伴读试听】宝宝的物理学ABC，告诉你什么是原子（Atom）、黑洞（BlackHole）、电荷（Charge）";
                    break; 
                case "QuantumInformationForBabies":
                    viewModel.Description = "量子信息是现代物理学最前沿的内容之一，也是最热门的应用技术之一，量子密钥、量子计算机均是这一学科热门的研究课题。克里斯·费利所著的《宝宝的量子信息学》通过绘本的形式介绍量子信息学的基本内容，从计算机的比特开始，逐步介绍了量子比特的强大功能，展示了量子信息无法估量的应用前景。";
                    viewModel.Title = "♪【伴读试听】宝宝的量子信息学，量子密钥、量子计算机均是这一学科热门的研究课题";
                    break;
                case "QuantumEntanglementForBabies":
                    viewModel.Description = "克里斯·费利所著的《宝宝的量子纠缠学》通过绘本的形式将很可能研究生才能接触的内容介绍给婴幼儿，而重要的是所讲述的确实是量子纠缠最本质的物理内容，其物理内涵至今人们并未真正懂得，所以这是一个开放的问题，看到这本书的孩子，量子纠缠的奥秘要靠你来揭开了！";
                    viewModel.Title = "♪【伴读试听】宝宝的量子纠缠学,研究生才能接触的内容婴幼儿也能懂";
                    break;
                case "NewtonianPhysicsForBabies":
                    viewModel.Description = "牛顿力学是现代物理学乃至现代科学的基石，牛顿三大定律精炼而全面概况了牛顿物理学的几乎全部内容。克里斯·费利所著的《宝宝的牛顿力学》通过绘本的形式将牛顿三大定律予以完美阐释，给宝宝们的物理学打下了坚实基础。本书可作为婴幼儿早教的绘本书，亦可作为学龄前孩子的科普书，是带领宝宝们叩响科学之门的启蒙佳作。";
                    viewModel.Title = "♪【伴读试听】宝宝的牛顿力学，通过绘本的形式将牛顿三大定律予以完美阐释，给宝宝们的物理学打下坚实基础";
                    break;
                case "OpticalPhysicsForBabies":
                    viewModel.Description = "光学是经典物理学非常重要的基石之一，也是十分生动有趣的物理内容。克里斯·费利主编的《宝宝的光学》通过绘本的形式依次阐释了光的反射、投射、折射、色散等诸多现象，直至解释了彩虹的产生原理，条理清晰，寓教于乐，给宝宝们上了一堂丰富、全面的光学课。";
                    viewModel.Title = "♪【伴读试听】宝宝的光学，通过绘本的形式依次阐释了光的反射、投射、折射、色散等诸多现象，直至解释了彩虹的产生原理";
                    break;
                case "QuantumPhysicsForBabies":
                    viewModel.Description = "克里斯·费利所著的《宝宝的量子物理学》通过绘本的形式将原子结构中电子的量子化分布以及跃迁清晰地表达了出来，使得婴幼儿得以接触最前沿的当然也是最基本的量子物理学，成为小小量子物理学家。这样的孩子从小就奠定了良好的量子物理学直觉，在长大以后也就不会对量子物理学感到陌生或害怕了。";
                    viewModel.Title = "♪【伴读试听】宝宝的量子物理学，从小奠定良好的量子物理学直觉，长大以后就不会对量子物理学感到陌生或害怕";
                    break;
                case "HowToUseResources":
                    viewModel.Description = "为了让热爱科学的孩子们更有兴趣、学以致用，我们为大家准备了多达68页的Physical Science Acitivity Pack，点击下方的<span style=\"font-weight:bold; font-size:medium; color:deeppink\">免费领取</span>获取该拓展资源。";
                    viewModel.Title = "♪【伴读试听】物理科学启蒙拓展手册的 使用方法";
                    break;
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BabyPhisicsIndex()
        {
            return View(await ConstructJSSDKViewModel());
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
        public async Task<IActionResult> DinasourReading_2()
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
        public async Task<IActionResult> Oxford_FloppysFunPhonics()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_SixInABed()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_BiffsWonderWords_FirstEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_BiffsWonderWords_SecondEpisode()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_ThePancake()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_AGoodTrick()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_BiffsFunPhonics()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_KippersRhymes()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> Oxford_KippersRhymes_2()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> StepIntoReading_GradedReader()
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
        public async Task<IActionResult> CalabashBrothersReading_4()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CalabashBrothersReading_5()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CalabashBrothersReading_6()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> CalabashBrothersReading_7()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> ReadingAdviceToPreSchoolKids()
        {
            return View(await ConstructJSSDKViewModel());
        }

        [HttpGet]
        public async Task<IActionResult> RebelliousStage()
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
