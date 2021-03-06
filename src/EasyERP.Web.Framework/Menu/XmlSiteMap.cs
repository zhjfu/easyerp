﻿//code from Telerik MVC Extensions

namespace EasyERP.Web.Framework.Menu
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Web.Routing;
    using System.Xml;
    using Doamin.Service.Security;
    using EasyErp.Core;
    using EasyErp.Core.Infrastructure;

    public class XmlSiteMap
    {
        public XmlSiteMap()
        {
            RootNode = new SiteMapNode();
        }

        public SiteMapNode RootNode { get; set; }

        public virtual void LoadFrom(string physicalPath)
        {
            var webHelper = EngineContext.Current.Resolve<IWebHelper>();
            var filePath = webHelper.MapPath(physicalPath);
            var content = File.ReadAllText(filePath);

            if (!string.IsNullOrEmpty(content))
            {
                using (var sr = new StringReader(content))
                {
                    using (var xr = XmlReader.Create(
                        sr,
                        new XmlReaderSettings
                        {
                            CloseInput = true,
                            IgnoreWhitespace = true,
                            IgnoreComments = true,
                            IgnoreProcessingInstructions = true
                        }))
                    {
                        var doc = new XmlDocument();
                        doc.Load(xr);

                        if ((doc.DocumentElement != null) &&
                            doc.HasChildNodes)
                        {
                            var xmlRootNode = doc.DocumentElement.FirstChild;
                            Iterate(RootNode, xmlRootNode);
                        }
                    }
                }
            }
        }

        private static void Iterate(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            PopulateNode(siteMapNode, xmlNode);

            foreach (XmlNode xmlChildNode in xmlNode.ChildNodes)
            {
                if (xmlChildNode.LocalName.Equals("siteMapNode", StringComparison.InvariantCultureIgnoreCase))
                {
                    var siteMapChildNode = new SiteMapNode();
                    siteMapNode.ChildNodes.Add(siteMapChildNode);

                    Iterate(siteMapChildNode, xmlChildNode);
                }
            }
        }

        private static void PopulateNode(SiteMapNode siteMapNode, XmlNode xmlNode)
        {
            //title
            var nopResource = GetStringValueFromAttribute(xmlNode, "nopResource");

            siteMapNode.Title = GetStringValueFromAttribute(xmlNode, "title");

            //routes, url
            var controllerName = GetStringValueFromAttribute(xmlNode, "controller");
            var actionName = GetStringValueFromAttribute(xmlNode, "action");
            var url = GetStringValueFromAttribute(xmlNode, "url");
            if (!string.IsNullOrEmpty(controllerName) &&
                !string.IsNullOrEmpty(actionName))
            {
                siteMapNode.ControllerName = controllerName;
                siteMapNode.ActionName = actionName;

                siteMapNode.RouteValues = new RouteValueDictionary
                {
                    { "area", "Admin" }
                };
            }
            else if (!string.IsNullOrEmpty(url))
            {
                siteMapNode.Url = url;
            }

            //image URL
            siteMapNode.ImageUrl = GetStringValueFromAttribute(xmlNode, "ImageUrl");

            //permission name
            var permissionNames = GetStringValueFromAttribute(xmlNode, "PermissionNames");
            if (!string.IsNullOrEmpty(permissionNames))
            {
                var permissionService = EngineContext.Current.Resolve<IPermissionService>();
                siteMapNode.Visible = permissionNames.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                     .Any(
                                                         permissionName =>
                                                         permissionService.Authorize(permissionName.Trim()));
            }
            else
            {
                siteMapNode.Visible = true;
            }
        }

        private static string GetStringValueFromAttribute(XmlNode node, string attributeName)
        {
            string value = null;

            if (node.Attributes != null &&
                node.Attributes.Count > 0)
            {
                var attribute = node.Attributes[attributeName];

                if (attribute != null)
                {
                    value = attribute.Value;
                }
            }

            return value;
        }
    }
}