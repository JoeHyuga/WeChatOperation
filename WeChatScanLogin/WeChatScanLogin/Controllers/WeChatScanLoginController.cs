using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WeChatScanLogin.Controllers
{
    public class WeChatScanLoginController : ApiController
    {
        /// <summary>
        /// 生成二维码
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [HttpGet]
        public string CreatQRCode()
        {
            return new BLL.WeChat().CreateQRCode();
        }
    }
}
