using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using BizTalkComponents.Utils;
using Microsoft.BizTalk.Component.Interop;
using Microsoft.BizTalk.Message.Interop;
using IComponent = Microsoft.BizTalk.Component.Interop.IComponent;

namespace BizTalkComponents.PipelineComponents.SetFileName
{
    [ComponentCategory(CategoryTypes.CATID_PipelineComponent)]
    [System.Runtime.InteropServices.Guid("BE531450-0F31-11E8-A320-CDDF0332DF21")]
    [ComponentCategory(CategoryTypes.CATID_Any)]
    public partial class SetFileName : IComponent, IBaseComponent,
                                        IPersistPropertyBag, IComponentUI
    {
        [DisplayName("Include original filename?")]
        [Description("{0} in custom format")]
        [RequiredRuntime]
        public bool IncludeOriginalFilename { get; set; }

        [DisplayName("XPath 1")]
        [Description("XPath to value to include in filename ({1} in custom format)")]
        public string XPath1 { get; set; }

        [DisplayName("XPath 2")]
        [Description("XPath to value to include in filename ({2} in custom format)")]
        public string XPath2 { get; set; }

        [DisplayName("XPath 2")]
        [Description("XPath to value to include in filename ({3} in custom format)")]
        public string XPath3 { get; set; }

        [DisplayName("File extension")]
        [Description("Specify a file extension if it should be changed")]
        public string Extension { get; set; }

        [DisplayName("Custom format")]
        [Description("Specify a custom format, default is {0}{1}{2}{3}{4}")]
        public string Format { get; set; }

        [DisplayName("Separator")]
        [Description("Specify a separator if it should be changed, default is _")]
        public string Separator { get; set; }

        [DisplayName("Include date?")]
        [Description("Default format is yyyy-MM-ddTHHmmss ({2} in custom format)")]
        public bool IncludeDate { get; set; }

        [DisplayName("Date format")]
        [Description("Specify a date format if it should be changed, default is yyyy-MM-ddTHH:mm:ss")]
        public string DateFormat { get; set; }

        public IBaseMessage Execute(IPipelineContext pContext, IBaseMessage pInMsg)
        {
            string errorMessage;

            if (!Validate(out errorMessage))
            {
                throw new ArgumentException(errorMessage);
            }

            //Set default values
            if (Format == null) Format = "{0}{1}{2}{3}{4}";
            if (Separator == null) Separator = "_";
            if (DateFormat == null) DateFormat = "yyyy-MM-ddTHHmmss";

            var receivedFileName = new ContextProperty(FileProperties.ReceivedFileName);

            string originalFileName = pInMsg.Context.Read(receivedFileName.PropertyName,
                    receivedFileName.PropertyNamespace).ToString();

            string originalFileNameWithoutExtension = IncludeOriginalFilename ?
                Path.GetFileNameWithoutExtension(originalFileName) + Separator : string.Empty;

            string extension = Extension ?? Path.GetExtension(originalFileName);

            var xPaths = new[] { XPath1, XPath2, XPath3 }.Where(x => x != null).ToArray();
            Dictionary<string, string> xPathToValueMap = pInMsg.SelectMultiple(xPaths);

            string outValue1;
            string value1 = XPath1 != null && xPathToValueMap.TryGetValue(XPath1, out outValue1) ?
                outValue1 + Separator : string.Empty;

            string outValue2;
            string value2 = XPath2 != null && xPathToValueMap.TryGetValue(XPath2, out outValue2) ?
                outValue2 + Separator : string.Empty;

            string outValue3;
            string value3 = XPath3 != null && xPathToValueMap.TryGetValue(XPath3, out outValue3) ?
               outValue3 + Separator : string.Empty;

            string date = IncludeDate ?
                DateTime.Now.ToString(DateFormat) + Separator : string.Empty;

            string result = string.Format(Format, originalFileNameWithoutExtension, value1, value2, value3, date)
                .TrimEnd(new char[] { char.Parse(Separator) }) + extension;

            if (Path.GetInvalidFileNameChars().Any(c => result.Contains(c)))
            {
                throw new ArgumentException("Filename result contains invalid characters");
            }

            pInMsg.Context.Write(receivedFileName.PropertyName, receivedFileName.PropertyNamespace, result);

            return pInMsg;
        }
    }
}
