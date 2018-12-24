using AutoMoqCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using WxHandle.Core;
using WxHandle.Core.Models;
using WxHandle.Core.Options;

namespace WxHandle.Test
{
    [TestClass]
    public class WxXmlHelpTests
    {
        private readonly string xml;
        public WxXmlHelpTests()
        {
            xml = @"<xml>
  <appid><![CDATA[wx2421b1c4370ec43b]]></appid>
  <attach><![CDATA[Ö§¸¶²âÊÔ]]></attach>
  <bank_type><![CDATA[CFT]]></bank_type>
  <fee_type><![CDATA[CNY]]></fee_type>
  <is_subscribe><![CDATA[Y]]></is_subscribe>
  <mch_id><![CDATA[10000100]]></mch_id>
  <nonce_str><![CDATA[5d2b6c2a8db53831f7eda20af46e531c]]></nonce_str>
  <openid><![CDATA[oUpF8uMEb4qRXf22hE3X68TekukE]]></openid>
  <out_trade_no><![CDATA[1409811653]]></out_trade_no>
  <result_code><![CDATA[SUCCESS]]></result_code>
  <return_code><![CDATA[SUCCESS]]></return_code>
  <sign><![CDATA[B552ED6B279343CB493C5DD0D78AB241]]></sign>   
  <time_end><![CDATA[20140903131540]]></time_end>
  <total_fee>1</total_fee><coupon_fee><![CDATA[10]]></coupon_fee>
<coupon_count><![CDATA[1]]></coupon_count> 
<coupon_id><![CDATA[10000]]></coupon_id>
<coupon_fee_0><![CDATA[100]]></coupon_fee_0>
  <trade_type><![CDATA[JSAPI]]></trade_type>
  <transaction_id><![CDATA[1004400740201409030005092168]]></transaction_id>
</xml>";

            var doc = new XmlDocument();
            doc.LoadXml(xml);             
            xml = doc.OuterXml;
        }

        [TestMethod]
        public void getValue_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mocker = new AutoMoqer();
            var unitUnderTest = mocker.Create<WxXmlHelp>();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            string name = "appid";

            // Act
            var result = unitUnderTest.GetValue(
                doc,
                name);

            // Assert
            Assert.AreEqual(result, "wx2421b1c4370ec43b");
        }

        [TestMethod]
        public async Task readBody_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mocker = new AutoMoqer();
            var unitUnderTest = mocker.Create<WxXmlHelp>();

            var bytes = System.Text.Encoding.UTF8.GetBytes(xml);
            Stream stream = new MemoryStream(bytes);
            stream.Position = 0;


            // Act
            var result = await unitUnderTest.ReadBody(
                stream);

            // Assert
            Assert.AreEqual(result, xml);
        }

        [TestMethod]
        public void readAsXmlFromBody_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            var mocker = new AutoMoqer();
            var unitUnderTest = mocker.Create<WxXmlHelp>();
            string xml = this.xml;

            // Act
            var result = unitUnderTest.ReadFromXml<PayCallbackData>(
                xml);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task readAsXmlFromBody_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var mocker = new AutoMoqer();
            var unitUnderTest = mocker.Create<WxXmlHelp>();
            var bytes = System.Text.Encoding.UTF8.GetBytes(xml);
            Stream stream = new MemoryStream(bytes);
            stream.Position = 0;

            // Act
            var result = await unitUnderTest.ReadFromStream<PayCallbackData>(
                stream);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ReadXml_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange
            var mocker = new AutoMoqer();
            var unitUnderTest = mocker.Create<WxXmlHelp>();

            // Act
            var result = unitUnderTest.ReadFromXml<PayCallbackData>(
                xml);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void WriteToXml_StateUnderTest_ExpectedBehavior()
        {
            var mocker = new AutoMoqer();
            var unitUnderTest = mocker.Create<WxXmlHelp>();
            var todo = unitUnderTest.ReadFromXml<PayCallbackData>(xml);

            var result = unitUnderTest.WriteToXml(todo);

            Assert.IsTrue(unitUnderTest.CompareXml(result,xml));
        }
         
        [TestMethod]
        public void CreateSign_MD5_StateUnderTest_ExpectedBehavior()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOptions<WxConfig>>().Setup(x => x.Value).Returns(new WxConfig()
            {
                PayKey= "192006250b4c09247ec02edce69f6a2d",
                SignMode=SignMode.MD5
            }); 
       
            var unitUnderTest = mocker.Create<WxXmlHelp>();

            var todo = unitUnderTest.ReadFromXml<PayCallbackData>(xml);
            var newxml = unitUnderTest.WriteToXml(todo);

            var sign = unitUnderTest.CreateSign(todo);

            
            Assert.AreEqual("424A19186D0092B11404CF00E0B15265", sign);
        }

        [TestMethod]
        public void CreateSign_SHA256_StateUnderTest_ExpectedBehavior()
        {
            var mocker = new AutoMoqer();
            mocker.GetMock<IOptions<WxConfig>>().Setup(x => x.Value).Returns(new WxConfig()
            {
                PayKey = "192006250b4c09247ec02edce69f6a2d",
                SignMode = SignMode.HmacSHA256
            });

            var unitUnderTest = mocker.Create<WxXmlHelp>();

            var todo = unitUnderTest.ReadFromXml<PayCallbackData>(xml);
            var newxml = unitUnderTest.WriteToXml(todo);

            var sign = unitUnderTest.CreateSign(todo);

            Assert.AreEqual("90C202674D0F1E0C2B1755FD1FE175B810760C02EB14B2B414B0971E5BD6CDD7", sign);         
        }
    }
}
