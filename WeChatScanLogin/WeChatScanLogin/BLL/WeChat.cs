using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace WeChatScanLogin.BLL
{
    /// <summary>
    /// 微信相关操作
    /// </summary>
    public class WeChat
    {
        private const string APPID = "wx0c5a89554467abcc";

        private const string APPSECRET = "6967dd7fd6e8ebfe1b519402743e21f3";

        /// <summary>
        /// 获得微信公众号token
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            string url = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid="+APPID+"&secret="+APPSECRET;

           string result=HttpGetMethod(url);

            var dic = GetResult(result);

            if (dic.ContainsKey("access_token"))
            {
                return dic["access_token"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 生成一个永久二维码ticket
        /// </summary>
        public string CreateTicket()
        {
            //永久二维码api
            string ticketUrl = "https://api.weixin.qq.com/cgi-bin/qrcode/create?access_token="+ GetAccessToken();
            //发送数据
            string postData = "{\"action_name\": \"QR_LIMIT_SCENE\", \"action_info\": {\"scene\": {\"scene_id\": 123}}}";

            string reuslt = HttpPostMethod(ticketUrl, postData);

            return reuslt;
        }
        
        /// <summary>
        /// 通过ticket获取二维码
        /// </summary>
        /// <returns></returns>
        public string CreateQRCode()
        {
            var dic = GetResult(CreateTicket());

            string qrcodeUrl = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket="+HttpUtility.UrlEncode(dic["ticket"].ToString());

            string qrcodeUrl1 = "https://mp.weixin.qq.com/cgi-bin/showqrcode?ticket=" + CreateTicket();

            return qrcodeUrl;

            //string reuslt = HttpGetMethod(qrcodeUrl);

            //return reuslt;
        }

        /// <summary>
        /// 发送post请求
        /// </summary>
        public string HttpPostMethod(string url,string postData)
        {
            //处理post发送数据
            byte[] bytes = Encoding.UTF8.GetBytes(postData);


            WebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            request.ContentType = "text/xml";
            Stream reqstream = request.GetRequestStream();
            reqstream.Write(bytes, 0, bytes.Length);

            //声明一个HttpWebRequest请求  
            request.Timeout = 90000;
            //设置连接超时时间  
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            return strResult;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public string HttpGetMethod(string url)
        {
            WebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.ContentType = "text/xml";

            //声明一个HttpWebRequest请求  
            request.Timeout = 90000;
            //设置连接超时时间  
            request.Headers.Set("Pragma", "no-cache");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream streamReceive = response.GetResponseStream();
            Encoding encoding = Encoding.UTF8;

            StreamReader streamReader = new StreamReader(streamReceive, encoding);
            string strResult = streamReader.ReadToEnd();
            streamReceive.Dispose();
            streamReader.Dispose();

            return strResult;
        }

        /// <summary>
        /// 获得转换后的集合结果
        /// </summary>
        /// <param name="json">返回json结果</param>
        /// <returns></returns>
        public Dictionary<string, object> GetResult(string json)
        {
            JavaScriptSerializer s = new JavaScriptSerializer();
            Dictionary<string, object> dic = (Dictionary<string, object>)s.DeserializeObject(json);

            return dic;
        }
    }
}