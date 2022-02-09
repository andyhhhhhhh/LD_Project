using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text; 
using System.IO;
using System.Net;
 
namespace ServiceController
{
    public class HttpRequest
    {
        /// <summary>
        /// 获取测试数据
        /// </summary>
        /// <param name="strType">liv:请求liv曲线 iw:请求波长曲线</param>
        /// <returns></returns>
        public static string GetAction(string ip, string port, string strType)//模拟GET请求
        {
            try
            { 
                //String url = "http://192.168.1.107:8000/api/v1/" + strType;//请求的链接，为了方便直接填上了后面的参数
                String url = string.Format("http://{0}:{1}/api/v1/{2}", ip, port, strType);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                request.Accept = "*/*";
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream getStream = response.GetResponseStream();
                StreamReader streamreader = new StreamReader(getStream);
                String result = streamreader.ReadToEnd();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            } 
        }

        /// <summary>
        /// Post服务器开始测试
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="bstart">true:开始测试 false:终止测试</param>
        /// <param name="index">0-工位1积分球 1-工位2积分球</param>
        /// <param name="station">1 、2、3、4工站号</param>
        /// <returns></returns>
        public static string PostAction(string ip, string port, bool bstart, int index, string station, int id = 0, double current = 0)//模拟Post请求
        {
            try
            {
                //String url = "http://192.168.1.107:8000/api/v1/" + strType; 
                String url = string.Format("http://{0}:{1}/api/v1/?index={2}", ip, port, index);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";//设置对应的ContentType  
                String postdata = string.Format("{{\"Station\":\"{0}\",\"Method\":\"Start\",\"Pwd\": \"weike\", \"Id\": {1}, \"Current\": {2}}}", station, id, current);
                    
                if (!bstart)
                {
                    postdata = string.Format("{{\"Station\":\"{0}\",\"Method\":\"End\",\"Pwd\": \"weike\", \"Id\": {1}, \"Current\": {2}}}", station, id, current);
                }

                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                writer.Write(postdata);
                writer.Flush();

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();//获取服务器返回的结果
                Stream getStream = response.GetResponseStream();
                StreamReader streamreader = new StreamReader(getStream);
                String result = streamreader.ReadToEnd();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// Post服务器开始测试
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port">端口</param>
        /// <param name="paramValue">参数内容</param>
        /// <returns></returns>
        public static string PostParamAction(string ip, string port, string paramValue)//模拟Post请求
        {
            try
            {
                string url = string.Format("http://{0}:{1}/api/v1/params", ip, port);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";//设置对应的ContentType
                string postdata = paramValue;//post请求时的参数 

                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                writer.Write(postdata);
                writer.Flush();

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();//获取服务器返回的结果
                Stream getStream = response.GetResponseStream();
                StreamReader streamreader = new StreamReader(getStream);
                string result = streamreader.ReadToEnd();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static string PostCaliAction(string ip, string port, string index, string watt, string percentage, string wave)//模拟Post请求
        {
            try
            {
                string url = string.Format("http://{0}:{1}/api/v1/calibration?index={2}", ip, port, index);
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = "POST";
                req.ContentType = "application/json";//设置对应的ContentType
                String postdata = string.Format("{{\"Pwd\":\"weike\",\"Watt\":{0},\"Percentage\": {1},\"Lamda\": {2}}}", watt, percentage, wave);//post请求时的参数 

                StreamWriter writer = new StreamWriter(req.GetRequestStream());
                writer.Write(postdata);
                writer.Flush();

                HttpWebResponse response = (HttpWebResponse)req.GetResponse();//获取服务器返回的结果
                Stream getStream = response.GetResponseStream();
                StreamReader streamreader = new StreamReader(getStream);
                string result = streamreader.ReadToEnd();
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        
        public static string Post(string strPost, string strType)
        {
            Encoding myEncoding = Encoding.GetEncoding("gb2312");  //选择编码字符集
            string data = "{\n\"Method\":\"START\",\n\"Pwd\": \"weike\",\n\"Id\": 0\n}";
            byte[] bytesToPost = System.Text.Encoding.Default.GetBytes(data); //转换为bytes数据

            string responseResult = String.Empty;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create("http://192.168.1.107:8000/api/v1/iw");   //创建一个有效的httprequest请求，地址和端口和指定路径必须要和网页系统工程师确认正确，不然一直通讯不成功
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded;charset=gb2312";
            req.ContentLength = bytesToPost.Length;

            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(bytesToPost, 0, bytesToPost.Length);     //把要上传网页系统的数据通过post发送
            }
            HttpWebResponse cnblogsRespone = (HttpWebResponse)req.GetResponse();
            if (cnblogsRespone != null && cnblogsRespone.StatusCode == HttpStatusCode.OK)
            {
                StreamReader sr;
                using (sr = new StreamReader(cnblogsRespone.GetResponseStream()))
                {
                    responseResult = sr.ReadToEnd();  //网页系统的json格式的返回值，在responseResult里，具体内容就是网页系统负责工程师跟你协议号的返回值协议内容
                }
                sr.Close();
            }
            cnblogsRespone.Close();

            return responseResult;
        }

    }
}