using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using WxHandle.Core.Models;
using WxHandle.Core.Options;

namespace WxHandle.Core
{
    public class WxXmlHelp
    {
        private readonly IOptions<WxConfig> options;

        public WxXmlHelp(IOptions<WxConfig> options)
        {
            this.options = options;
        }
        public string GetValue(XmlDocument doc, string name)
        {
            var text = doc.SelectSingleNode($"//{name}")?.InnerText.Trim() ?? "";
            return text;
        }

        public async Task<string> ReadBody(Stream stream)
        {
            var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public T ReadFromXml<T>(string xml) where T : IWxResult
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T), new XmlRootAttribute("xml"));
            var result = (T)serializer.Deserialize(new MemoryStream(Encoding.Unicode.GetBytes(xml)));
            return result;
        }

        public async Task<T> ReadFromStream<T>(Stream stream) where T : IWxResult
        {
            var bodyString = await ReadBody(stream);
            return ReadFromXml<T>(bodyString);
        }

        public string WriteToXml<T>(T input)
        {
            var strBuild = new StringBuilder();
            strBuild.AppendLine("<xml>");
            foreach (var item in typeof(T).GetProperties())
            {
                var value = item.GetValue(input, null)?.ToString();
                if (value == null)
                    continue;
                strBuild.AppendLine($"\t<{item.Name.ToLower()}><![CDATA[{value}]]></{item.Name.ToLower()}>");
            }
            strBuild.AppendLine("</xml>");

            var xml = strBuild.ToString();

            var doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.OuterXml;
        }

        private string MD5(string input, Encoding encode = null)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";
            if (encode == null)
                encode = Encoding.UTF8;
            MD5CryptoServiceProvider md5Hasher = new MD5CryptoServiceProvider();
            byte[] data = md5Hasher.ComputeHash(encode.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        private string HmacSHA256(string input, string key, Encoding encode = null)
        {
            if (string.IsNullOrWhiteSpace(input))
                return "";
            if (encode == null)
                encode = Encoding.UTF8;

            var secret = key;

            byte[] keyByte = encode.GetBytes(secret);
            byte[] messageBytes = encode.GetBytes(input);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return BitConverter.ToString(hashmessage).Replace("-", "");
            }
        }

        /// <summary>
        /// 创建签名(如果随机串为空,则生成一个)
        /// </summary> 
        public string CreateSign<T>(T model) where T : IHasSignModel
        {
            if (model == null)
                throw new System.ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.nonce_str))
                model.nonce_str = Guid.NewGuid().ToString().Replace("-", "");

            var props = model.GetType().GetProperties();
            var values = new SortedDictionary<string, string>();
            foreach (var prop in props)
            {
                if (prop.Name.ToLower() == "sign")
                    continue;

                var value = prop.GetValue(model, null)?.ToString();
                if (value == null)
                    continue;

                values.Add(prop.Name.ToLower(), value);
            }

            var signUrlbuilder = new List<string>();
            string signUrl = "", sign = "";

            /*第一步*/
            foreach (var kv in values)
                signUrlbuilder.Add($"{kv.Key}={kv.Value}");
            signUrl = string.Join("&", signUrlbuilder);

            /*第二步*/
            signUrl = $"{signUrl}&key={options.Value.PayKey}";
            switch (options.Value.SignMode)
            {
                case SignMode.MD5:
                    sign = MD5(signUrl).ToUpper();
                    break;
                case SignMode.HmacSHA256:
                    sign = HmacSHA256(signUrl, options.Value.PayKey).ToUpper();
                    break;
                default:
                    break;
            }

            return sign;
        }

        public bool CompareXml(string left, string right)
        {
            if (string.IsNullOrWhiteSpace(left))
                return false;
            if (string.IsNullOrWhiteSpace(right))
                return false;

            var leftDoc = new XmlDocument();
            var rightDoc = new XmlDocument();

            leftDoc.LoadXml(left);
            rightDoc.LoadXml(right);

            for (int i = 0; i < leftDoc.DocumentElement.ChildNodes.Count; i++)
            {
                var item = leftDoc.DocumentElement.ChildNodes.Item(i);
                var name = item.Name;
                var text = item.InnerText;

                var target = rightDoc.SelectSingleNode("//" + name)?.InnerText;
                if (target != text)
                    return false;
            }

            //for (int i = 0; i < rightDoc.DocumentElement.ChildNodes.Count; i++)
            //{
            //    var item = rightDoc.DocumentElement.ChildNodes.Item(i);
            //    var name = item.Name;
            //    var text = item.InnerText;

            //    var target = leftDoc.SelectSingleNode("//" + name)?.InnerText;
            //    if (target != text)
            //        return false;
            //}

            return true;
        }
    }
}
