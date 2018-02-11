using Microsoft.VisualStudio.TestTools.UnitTesting;
using Winterdom.BizTalk.PipelineTesting;
using Microsoft.BizTalk.Message.Interop;
using BizTalkComponents.Utils;
using System;
using System.IO;

namespace BizTalkComponents.PipelineComponents.SetFileName.Tests.UnitTests
{
    [TestClass]
    public class SetFileNameTests
    {
        IBaseMessage msg;
        ContextProperty receivedFileName = new ContextProperty(FileProperties.ReceivedFileName);
        string originalFileName = "invoice1.xml";
        SendPipelineWrapper pipeline;

        [TestInitialize]
        public void Initialize()
        {
            string inputXml =
                 @"<root>
                    <element1>value1</element1>
                    <element2>value2</element2>
                    <element3>value3</element3>
                </root>";

            msg = MessageHelper.CreateFromString(inputXml);
            msg.Context.Write(receivedFileName.PropertyName, receivedFileName.PropertyNamespace, originalFileName);
            pipeline = PipelineFactory.CreateEmptySendPipeline();
        }


        [TestMethod]
        public void SetFileName_ShouldIncludeSourceFilename()
        {
            var component = new SetFileName()
            {
                IncludeOriginalFilename = true
            };

            pipeline.AddComponent(component, PipelineStage.Encode);

            var result = pipeline.Execute(msg);

            string newFileName = result.Context.Read(receivedFileName.PropertyName,
                    receivedFileName.PropertyNamespace).ToString();

            Assert.AreEqual(originalFileName, newFileName);
        }

        [TestMethod]
        public void SetFileName_ShouldIncludeValuesFromXPaths()
        {
            var component = new SetFileName()
            {
                XPath1 = "/root/element1[1]",
                XPath2 = "/root/element2[1]",
                XPath3 = "/root/element3[1]"
            };

            pipeline.AddComponent(component, PipelineStage.Encode);

            var result = pipeline.Execute(msg);

            string newFileName = result.Context.Read(receivedFileName.PropertyName,
                    receivedFileName.PropertyNamespace).ToString();

            Assert.AreEqual("value1_value2_value3.xml", newFileName);
        }

        [TestMethod]
        public void SetFileName_ShouldIncludeDateInCorrectFormat()
        {
            var component = new SetFileName()
            {
                IncludeDate = true,
                DateFormat = "yyyy-MM"
            };

            pipeline.AddComponent(component, PipelineStage.Encode);

            var result = pipeline.Execute(msg);

            string newFileName = result.Context.Read(receivedFileName.PropertyName,
                    receivedFileName.PropertyNamespace).ToString();

            Assert.AreEqual(DateTime.Now.ToString(component.DateFormat) + ".xml", newFileName);
        }

        [TestMethod]
        public void SetFileName_ShouldChangeExtension()
        {
            var component = new SetFileName()
            {
                IncludeOriginalFilename = true,
                Extension = ".txt"
            };

            pipeline.AddComponent(component, PipelineStage.Encode);

            var result = pipeline.Execute(msg);

            string newFileName = result.Context.Read(receivedFileName.PropertyName,
                    receivedFileName.PropertyNamespace).ToString();

            Assert.AreEqual(Path.GetFileNameWithoutExtension(originalFileName) + ".txt", newFileName);
        }

        [TestMethod]
        public void SetFileName_ShouldChangeSeparator()
        {
            var component = new SetFileName()
            {
                XPath1 = "/root/element1[1]",
                XPath2 = "/root/element2[1]",
                XPath3 = "/root/element3[1]",
                Separator = ' '
            };

            pipeline.AddComponent(component, PipelineStage.Encode);

            var result = pipeline.Execute(msg);

            string newFileName = result.Context.Read(receivedFileName.PropertyName,
                    receivedFileName.PropertyNamespace).ToString();

            Assert.AreEqual("value1 value2 value3.xml", newFileName);
        }

        [TestMethod]
        public void SetFileName_ShouldChangeFormat()
        {
            var component = new SetFileName()
            {
                XPath1 = "/root/element1[1]",
                XPath2 = "/root/element2[1]",
                XPath3 = "/root/element3[1]",
                Format = "{3}{2}{1}"
            };

            pipeline.AddComponent(component, PipelineStage.Encode);

            var result = pipeline.Execute(msg);

            string newFileName = result.Context.Read(receivedFileName.PropertyName,
                    receivedFileName.PropertyNamespace).ToString();

            Assert.AreEqual("value3_value2_value1.xml", newFileName);
        }

        [TestMethod]
        public void SetFileName_ShouldNotThrow_WhenXPathDoesNotFindValue()
        {
            var component = new SetFileName()
            {
                IncludeOriginalFilename = true,
                XPath1 = "/notfound/element1[1]",
                XPath2 = "/root/notfound[1]",
            };
            pipeline.AddComponent(component, PipelineStage.Encode);

            var result = pipeline.Execute(msg);

            string newFileName = result.Context.Read(receivedFileName.PropertyName,
                   receivedFileName.PropertyNamespace).ToString();

            Assert.AreEqual(originalFileName, newFileName);
        }

    }
}
