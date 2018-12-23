using System.IO;
using System.Threading.Tasks;
using System.Xml;

namespace WxHandle.Core
{
    public class WxXmlHelp
    {
        public string GetValue(XmlDocument doc, string name)
        {
            var text = doc.SelectSingleNode($"//{name}")?.InnerText.Trim() ?? "";
            //return text.Substring(9, text.Length - 8 - 4);
            return text;
        }

        public async Task<string> ReadBody(Stream stream)
        {
            var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public async Task<WxResult<T>> ReadAsXmlFromBody<T>(string xml) where T : class, new()
        {
            var result = new WxResult<T>();
            var doc = new XmlDocument();
            doc.LoadXml(xml);

            result.return_code = GetValue(doc, "return_code");
            result.return_msg = GetValue(doc, "return_msg");

            if (result.return_code == "SUCCESS")
            {

            }
            await Task.CompletedTask;
            return result;
        }

        public async Task<WxResult<T>> ReadAsXmlFromBody<T>(Stream stream) where T : class, new()
        {
            var bodyString = await ReadBody(stream);
            return await ReadAsXmlFromBody<T>(bodyString);
        }
    }
}
