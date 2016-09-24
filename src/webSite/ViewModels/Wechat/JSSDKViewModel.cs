using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace webSite.ViewModels.Wechat
{
    public class JSSDKViewModel
    {
        public string ID { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Title { get; set; }

        public long Timestamp { get; set; }

        public string NouceString { get; set; }

        public string JsApiTicket { get; set; }

        public string Url { get; set; }

        public string ImgUrl { get; set; }

        public string Signature { get; set; }

        public string WechatDescription { get; set; }

        public string WechatTitle { get; set; }

        public string WechatUrl { get; set; }

        public string WechatImgUrl { get; set; }

        public string JumpUrl { get; set; }

        public string[] InsidePagePictures { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
