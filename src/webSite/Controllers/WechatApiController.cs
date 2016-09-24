using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using System.Net.Http;
using System.Diagnostics;
using System.Text;
using System.IO;
using System.Xml.Linq;
using ifunction.WebChatApi.Contract;
using System.Web.Security;
using Microsoft.Net.Http.Headers;
using System.Xml;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace webSite.Controllers
{
    [Route("api/wechat")]
    public class WechatApiController : Controller
    {
        private const string Token = "alibaba";

        // sample: api/wechat?signature=af30f7826dd7c71e23291dcb9ab94be53b7b87dd&timestamp=1425805977&nonce=739636270&echostr=hello
        public IActionResult Get(string signature, string timestamp, string nonce, string echostr)
        {
            var stringToReturn = string.Empty;

            Trace.TraceInformation(string.Format("Signature: {0}, Timestamp: {1}, Nonce: {2}, Echostr: {3}", signature, timestamp, nonce, echostr));
            if (CheckSignature(signature, timestamp, nonce)) stringToReturn = echostr;

            return new ContentResult { Content = stringToReturn };
        }

        // <xml><URL><![CDATA[http://www.tigerartstudio.cn/api/wechat/message]]></URL><ToUserName><![CDATA[tigerartstudio]]></ToUserName><FromUserName><![CDATA[supertigerzou]]></FromUserName><CreateTime>1448841860</CreateTime><MsgType><![CDATA[text]]></MsgType><Content><![CDATA[test]]></Content><MsgId>1234367812123456</MsgId></xml>
        [HttpPost]
        public IActionResult ReplyUser()
        {
            var inputMessage = Message.ConvertMessage(XElement.Load(Request.Body));

            ContentResult replyContent = null;
            Message mediaMessage = null;
            if (inputMessage is ContentMessage)
            {
                var contentMessage = (ContentMessage)inputMessage;
                if (contentMessage.Content.Contains("葫芦"))
                {
                    mediaMessage = inputMessage.ReplyMedia(new List<ImageObject>
                    {
                        new ImageObject
                        {
                            Description = "葫芦山里关着蝎子精和蛇精。一只穿山甲不小心打穿了山洞，两个妖精逃了出来，从此百姓遭难。穿山甲急忙去告诉一个老汉，只有种出七色葫芦，才能消灭这两个妖精。老汉种出了红、橙、黄、绿、青、蓝、紫七个大葫芦，却被妖精从如意镜中窥见。他们摧毁不了这七个葫芦，就把老汉和穿山甲抓去。七个葫芦成熟了，相继落地变成七个男孩，一个接一个去与妖精搏斗，却被妖精抓住弱点，各个击破。最终7个葫芦娃齐心协力与妖精展开殊死拼搏，并打败妖精，把他们收进宝葫芦里。",
                            ImageUrl = "https://mmbiz.qlogo.cn/mmbiz/u1YPNZkLJC6SI64hPj9FlEvhj2hH4t6azlULlE05icc4qEnRKFNqv2LKlhhDu4iaeibHiawcRebTL6rSOdxr43pkYQ/0",
                            Title = "葫芦娃",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=1&sn=e3dff38e2f8816e95362466e0021902f#rd"
                        },
                        new ImageObject
                        {
                            Description = "一位老爷爷在山上采药，无意中进入了一个山洞，在洞中他救下一只穿山甲。穿山甲告诉老爷爷自己不小心穿破葫芦山，放走了蛇蝎二妖。穿山甲帮助老爷爷取出了能降妖服魔的宝葫芦籽。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC7ZiaX1JJrQJiclnemAosfwGXPP0X5OvLnkvCTpAbPCzJRr9QxVn9Sib0GUDSeWasu9S3KfaCPiaqXYDA/640?tp=webp&wxfrom=5",
                            Title = "第一集： 神峰奇遇",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=2&sn=3131ea09b538535d6f41ac4aafc067d7#rd"
                        },
                        new ImageObject
                        {
                            Description = "老爷爷种下葫芦籽，有灵性的葫芦籽长得飞快，不久藤上便结出了七个色彩不同的葫芦。蛇蝎二妖得知后，决定除掉葫芦藤。夜晚，老爷爷没有听从葫芦们的劝告，出屋相救，不幸被妖怪擒走。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC7ZiaX1JJrQJiclnemAosfwGXjMTDRn1icI9aq1t1sL0VPhSOmAK5b4aCa508GdXMLdzrBB42jDYnqHA/640?tp=webp&wxfrom=5",
                            Title = "第二集： 七色葫芦",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=3&sn=5c43e79a63c5784ab67aadb0ffe7384b#rd"
                        },
                        new ImageObject
                        {
                            Description = "大力娃迫不及待地出来，去妖洞救老爷爷。大力娃力大如牛，而且能变大变小，闯入妖洞如入无人之境。蛇精眼看抵挡不过，就施下毒计，将大力娃骗入泥潭。大力娃有力无处使，被妖精擒住。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC7ZiaX1JJrQJiclnemAosfwGXCFNOibLAHG2PDGfWwN3L1ukrrJDP4m6Mxic8K6D4ziazwiaFB5HD8ZRs2w/640?tp=webp&wxfrom=5",
                            Title = "第三集： 误入泥潭",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=4&sn=df7eb47cc21ef7463aa6638c4b2ac6d0#rd"
                        },
                        new ImageObject
                        {
                            Description = "第二个葫芦娃降生了，他用千里眼和顺风耳先探明了妖洞的情况，赶去救大哥和老爷爷。蛇精知道他耳聪目明、机敏过人，使毒计诱他进入迷镜宫。在迷镜宫，千里眼娃被幻象所迷惑,被蛇精幻化成的美女诱惑,被妖怪残忍的刺瞎了双眼、刺聋了一只耳。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC7ZiaX1JJrQJiclnemAosfwGXaadQMCWtHms8LOlEPjSU9VdvyMSFtQ1jHxhQyAFDjO3ztOzw3WFz1A/640?tp=webp&wxfrom=5",
                            Title = "第四集： 梦窟迷境",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=5&sn=be18363c8ae7135976cb66d845bc2423#rd"
                        },
                        new ImageObject
                        {
                            Description = "穿山甲带老爷爷和千里眼娃顺利逃出了妖洞，却在洞门外惊动了看门蝙蝠。穿山甲牺牲自己，去引开蝙蝠的注意。就在老爷爷将被蝙蝠抓住的紧急关头，还未出世的钢筋铁骨娃赶来相救。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC7ZiaX1JJrQJiclnemAosfwGX2nGblEp6h3jZ4qB6F3YpYeK7AvuA9L9ray3q0zKMDm1rpyhZH5jCYA/640?tp=webp&wxfrom=5",
                            Title = "第五集： 绝路逢生",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=6&sn=16c351a45204370462602ff38561bbca#rd"
                        },
                        new ImageObject
                        {
                            Description = "钢筋铁骨娃与蝎子精一阵恶斗，蝎子精落荒而逃。坚固的石门挡不住葫芦娃的钢筋铁骨，妖洞里，他一闯到底，撞碎了蛇精的宝贝。蛇精拔出一把“刚柔阴阳剑”，以柔克刚，死死的缠住了葫芦娃。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC7ZiaX1JJrQJiclnemAosfwGXevPMrhjz9ia84rq81KJqbAo7eWRJ5e5BMUjwZj9YOlH78OpvvAMXcOQ/640?tp=webp&wxfrom=5",
                            Title = "第六集： 钢筋铁骨",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=7&sn=5e3a463f6d2fad7165ce20831624a38f#rd"
                        },
                        new ImageObject
                        {
                            Description = "水娃和火娃双双出世，他俩的水火奇功对老爷爷的生活帮助很大。蛇蝎二妖为炼七星丹，从深潭底下取出了炼丹宝炉。不料，炉内的火种迅速点燃山头，将二妖困住。善良的水、火二娃不明就里地救了二妖，还被骗进了妖洞。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC7ZiaX1JJrQJiclnemAosfwGXTBQ9Of7rg62ricW9XTvFJ26HL4RHrvC0IOWOqhgiaoPtuF5SxHiajLUCw/640?tp=webp&wxfrom=5",
                            Title = "第七集： 水火奇功",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=8&sn=79c93f255e72c881e2977fabfc713325#rd"
                        },
                        new ImageObject
                        {
                            Description = "水、火二娃被骗入洞后，二妖用美酒佳肴款待、以歌舞表演助兴。见二娃喜欢喝酒，蛇精暗地里用如意宝贝调出毒酒。火娃喝下后连心都被冻住了，水娃更是醉得不省人事。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC4ztsUzibgc6q5nQ92fP17gpWiawZia0oYVWz7cQB0Dpc6RJ7Hh6GntT7JyAv8Nr9HLN12oibYB1m49MA/640?tp=webp&wxfrom=5",
                            Title = "第八集：酒酣心冰",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204860621&idx=2&sn=1bd412664c1ae360abbc39ac8e0cda5a#rd"
                        },
                        new ImageObject
                        {
                            Description = "蛇蝎二妖又来攻击葫芦藤，他们将老爷爷扔下了山崖，又将千里眼娃和剩下的两个葫芦带回了妖洞。老爷爷被山鹰救下，被带到了葫芦山。葫芦山的山神将七色莲花交给了老爷爷，并告诉他只有再加上这个莲花，葫芦娃才能同心协力、铲妖除魔。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC4ztsUzibgc6q5nQ92fP17gp9dzr6tRXP3rGaib6A4KGz0qGNzI7ENI0R8GH2WsnohxLJZwH5adbnbQ/640?tp=webp&wxfrom=5",
                            Title = "第九集：幽谷彩莲",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204860621&idx=3&sn=c785d752a46db2f2ec2b517309585121#rd"
                        }
                    });
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("图文测试"))
                {
                    mediaMessage = inputMessage.ReplyMedia(new List<ImageObject>
                    {
                        new ImageObject
                        {
                            Description = "葫芦山里关着蝎子精和蛇精。一只穿山甲不小心打穿了山洞，两个妖精逃了出来，从此百姓遭难。穿山甲急忙去告诉一个老汉，只有种出七色葫芦，才能消灭这两个妖精。老汉种出了红、橙、黄、绿、青、蓝、紫七个大葫芦，却被妖精从如意镜中窥见。他们摧毁不了这七个葫芦，就把老汉和穿山甲抓去。七个葫芦成熟了，相继落地变成七个男孩，一个接一个去与妖精搏斗，却被妖精抓住弱点，各个击破。最终7个葫芦娃齐心协力与妖精展开殊死拼搏，并打败妖精，把他们收进宝葫芦里。",
                            ImageUrl = "https://mmbiz.qlogo.cn/mmbiz/u1YPNZkLJC6SI64hPj9FlEvhj2hH4t6azlULlE05icc4qEnRKFNqv2LKlhhDu4iaeibHiawcRebTL6rSOdxr43pkYQ/0",
                            Title = "葫芦娃",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=1&sn=e3dff38e2f8816e95362466e0021902f#rd"
                        },
                        new ImageObject
                        {
                            Description = "一位老爷爷在山上采药，无意中进入了一个山洞，在洞中他救下一只穿山甲。穿山甲告诉老爷爷自己不小心穿破葫芦山，放走了蛇蝎二妖。穿山甲帮助老爷爷取出了能降妖服魔的宝葫芦籽。",
                            ImageUrl = "http://mmbiz.qpic.cn/mmbiz/u1YPNZkLJC7ZiaX1JJrQJiclnemAosfwGXPP0X5OvLnkvCTpAbPCzJRr9QxVn9Sib0GUDSeWasu9S3KfaCPiaqXYDA/640?tp=webp&wxfrom=5",
                            Title = "第一集： 神峰奇遇",
                            Url = "http://mp.weixin.qq.com/s?__biz=MzA5NTU0MTMzOQ==&mid=204243491&idx=2&sn=3131ea09b538535d6f41ac4aafc067d7#rd"
                        }
                    });
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("20160914"))
                {
                    mediaMessage = inputMessage.ReplyMedia(new List<ImageObject>
                    {
                        new ImageObject
                        {
                            Description = "艾瑞卡尔的文笔带有诗趣，故事简洁轻快，每本书都有多重的内涵：趣味、想象、色彩、幽默……他的创作流露出童稚般的天真，表达出对自然的了解与关爱，也引导小朋友从身边的事物中学习。",
                            ImageUrl = "https://tigerstudio.blob.core.chinacloudapi.cn/mediafiles/EnglishClass/TheVery_TheVeryHungryCaterpillar.jpg",
                            Title = "♪【伴读】《卡尔The Very系列》第一课 - The Very Hungry Caterpillar",
                            Url = "http://www.tigerartstudio.cn/wechat/EnglishClass/TheVery_TheVeryHungryCaterpillar?enableRolling=true"
                        }
                    });
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("兰登英语"))
                {
                    mediaMessage = inputMessage.ReplyContent("《兰登英语》1-3阶原版音频下载链接: https://pan.baidu.com/s/1nvwEFzV 密码: ywfx");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("粉红猪小妹"))
                {
                    mediaMessage = inputMessage.ReplyContent("《粉红猪小妹》第一辑、第三辑原版音视频下载链接: https://pan.baidu.com/s/1gfa6ra7 密码: u434");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("小玻翻翻书"))
                {
                    mediaMessage = inputMessage.ReplyContent("《小玻翻翻书》10册纸板书原版音频下载链接: https://pan.baidu.com/s/1qXDTubE 密码: xu9w");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("鹅妈妈童谣"))
                {
                    mediaMessage = inputMessage.ReplyContent("《鹅妈妈童谣》出版社配套音频下载链接: https://pan.baidu.com/s/1eS9Kome 密码: 2xf3");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("安东尼"))
                {
                    mediaMessage = inputMessage.ReplyContent("安东尼绘本精选12册原版音频下载链接: https://pan.baidu.com/s/1slCL9nv 密码: un88");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("花格子大象"))
                {
                    mediaMessage = inputMessage.ReplyContent("《花格子大象艾玛》10册原版音频下载链接: https://pan.baidu.com/s/1bp2yJgV 密码: jnit");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("洞洞书第一辑"))
                {
                    mediaMessage = inputMessage.ReplyContent("《Child's Play儿歌洞洞书第一辑》原版音视频下载链接: https://pan.baidu.com/s/1eR4KowE 密码: w2c1");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("洞洞书第二辑"))
                {
                    mediaMessage = inputMessage.ReplyContent("《Child's Play儿歌洞洞书第二辑》原版音视频下载链接: https://pan.baidu.com/s/1o7Hvijw 密码: q4tr");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("廖彩杏书单精选9册"))
                {
                    mediaMessage = inputMessage.ReplyContent("《廖彩杏书单精选9册》原版音视频下载链接: https://pan.baidu.com/s/1boXqEFt 密码: t83k");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
                else if (contentMessage.Content.Contains("韵文大师"))
                {
                    mediaMessage = inputMessage.ReplyContent("《韵文大师Audrey Wood作品9册》原版音频下载链接: https://pan.baidu.com/s/1qYKVg88 密码: 9dun");
                    replyContent = new ContentResult
                    {
                        Content = mediaMessage.ToXml().ToString(SaveOptions.DisableFormatting),
                        ContentType = new MediaTypeHeaderValue("xml/application")
                    };
                }
            }

            if (replyContent == null)
                replyContent = new ContentResult
                {
                    Content = inputMessage.ReplyContent("这里是老虎工作室，只为宠坏你的耳朵！").ToXml().ToString(),
                    ContentType = new MediaTypeHeaderValue("xml/application")
                };

            return replyContent;
        }

        private bool CheckSignature(string signature, string timestamp, string nonce)
        {
            string[] arrTmp = { Token, timestamp, nonce };
            Array.Sort(arrTmp);

            var tmpStr = string.Join("", arrTmp);
            tmpStr = FormsAuthentication.HashPasswordForStoringInConfigFile(tmpStr, "SHA1");

            return tmpStr != null && tmpStr.ToLower() == signature;
        }
    }
}
