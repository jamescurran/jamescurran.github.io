﻿
#region License
// Copyright (c) 2007-2008, James M. Curran
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion


namespace Castle.MonoRail.ViewComponents
{
    #region Reference
    using System;
    using System.IO;
    using System.Collections;
    using Castle.MonoRail.Framework;
    using Castle.MonoRail.Framework.Helpers;
    using Castle.Core.Logging;
    using System.Web;
    using System.Collections.Specialized;
    #endregion


	/// <summary>
	/// Helper class to provide a some convenient methods for viewcomponents.
	/// </summary>
	/// <remarks>May one day be incorporated into base class.</remarks>
	public abstract class ViewComponentEx : ViewComponent
	{
		private ILogger logger;

		public ILogger Logger
		{
			get
			{
				if (logger == null) logger = NullLogger.Instance;
				return logger;
			}

			set
			{
				logger = value;
			}
		}
		/// <summary>
		/// Renders the text, formatted.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The args.</param>
		public void RenderTextFormat(string format, params object[] args)
		{
			string content = String.Format(format, args);
			base.RenderText(content);
		}

		/// <summary>
		/// Renders the optional section, or the default text, if section not present.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <param name="defaultText">The default text.</param>
		protected bool RenderOptionalSection(string section, string defaultText)
		{
			if (Context.HasSection(section))
			{
				Context.RenderSection(section);
				return true;
			}
			RenderText(defaultText);
			return false;
		}

		/// <summary>
		/// Renders the optional section.
		/// </summary>
		/// <param name="section">The section.</param>
        /// <returns>bool, [true] is the section was rendered</returns>
		protected bool RenderOptionalSection(string section)
		{
			if (Context.HasSection(section))
			{
				Context.RenderSection(section);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Gets the section text.
		/// </summary>
		/// <param name="section">The section.</param>
		/// <returns></returns>
		public string GetSectionText(string section)
		{
			using (StringWriter sw = new StringWriter())
            {
				if (String.IsNullOrEmpty(section))
					RenderBody(sw);
				else
					Context.RenderSection(section, sw);
				return sw.ToString();
            }
		}

		/// <summary>
		/// Gets the body text.
		/// </summary>
		/// <returns></returns>
		public string GetBodyText()
		{
			return GetSectionText(null);
		}
		/// <summary>
		/// Confirms a section is present.
		/// </summary>
		/// <remarks>Throws an exception if the given section is not present.
		/// </remarks>
		/// <param name="section">The section.</param>
		/// <exception cref="ViewComponentException">If specified section is not present.</exception>
		public void ConfirmSectionPresent(string section)
		{
			if (!Context.HasSection(section))
			{
				string message = String.Format("{0}: you must supply a '{1}' section", Context.ComponentName, section);
				throw new ViewComponentException(message);
			}
		}

		/// <summary>
		/// Gets a boolean parameter value.
		/// </summary>
		/// <param name="key">The key.</param>
		/// <param name="defaultValue">The value used if there is no parameter named key.</param>
		/// <returns></returns>
        [Obsolete("use GetParamValue instead")]
        public bool GetBoolParamValue(string key, bool defaultValue)
        {
            return GetParamValue(key, defaultValue);
        }
		public bool GetParamValue(string key, bool defaultValue)
		{
			object parmValue = Context.ComponentParameters[key];
			bool value = defaultValue;
			if (parmValue is string)
			{
				if (!Boolean.TryParse(parmValue as string, out value))
					value = defaultValue;
			}
			else if (parmValue is bool)
			{
				value = (bool)parmValue;
			}
			return value;
		}

        public string GetParamValue(string key, string defaultValue)
        {
            return Context.ComponentParameters[key] as string ?? defaultValue;
        }


        /// <summary>
        /// Gets the param value.
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public E GetParamValue<E>(string key, E defaultValue) where E:struct
        {
            object parmValue = Context.ComponentParameters[key];
            E value = defaultValue;
            if (parmValue is string)
            {
                try
                { value = (E)Enum.Parse(typeof(E), parmValue as string, true); }
                catch
                { value = defaultValue; }
            }
            else if (parmValue is E)
            {
                value = (E)parmValue;
            }
            return value;
        }
		/// <summary>
		/// Makes an unique id.
		/// </summary>
		/// <remarks>The given prefix is prepended to the generated number.
		/// The ID isn't actually guarenteed to be unique (which would require
		/// using all 32 digits of the guid). But this produce ids sufficently
		/// distinctive to generate multiple controls on a page.
		/// </remarks>
		/// <param name="prefix">The prefix.</param>
		/// <returns>A string usable as a Html element Id</returns>
		public string MakeUniqueId(string prefix)
		{
			byte[] bytes = Guid.NewGuid().ToByteArray();
			int a = BitConverter.ToInt32(bytes, 0);
			int b = BitConverter.ToInt32(bytes, 4);
			int c = BitConverter.ToInt32(bytes, 8);
			int d = BitConverter.ToInt32(bytes, 12);
			return prefix + (a ^ b ^ c ^ d).ToString("x08");
		}

		#region Render Composite Component
		/// <summary>
		/// Renders the component.
		/// </summary>
		/// <remarks>Intended to render a viewComponent from within a different viewComponent. <p/>
		/// Original concept devised by Joey Beninghove.<p />
		/// http://joeydotnet.com/blog/archive/2007/05/15/Creating-Composite-View-Components-In-MonoRail--Refactoring-Exercise.aspx
		/// </remarks>
		/// <example><code><![CDATA[
		///  RenderComponent<LinkSubmitButtonComponent>("linkText=Search",
		///              string.Format("formToSubmit={0}", searchFormName));
		/// ]]></code></example>
		/// <typeparam name="VC">The type of the ViewComponent.</typeparam>
		/// <param name="componentParams">The component params.</param>
		public void RenderComponent<VC>(IDictionary componentParams) where VC : ViewComponentEx, new()
		{
			ViewComponentEx component = new VC();
			RenderComponent(component, componentParams);
		}

		/// <summary>
		/// Renders the component.
		/// </summary>
		/// <remarks>For full details, <see cref="RenderComponent(VC)(IDictionary componentParams)" />
		/// </remarks>
		/// <example><code><![CDATA[
		///  RenderComponent<LinkSubmitButtonComponent>(DictHelper.N("linkText","Search").N("formToSubmit", searchFormName);
		/// ]]></code></example>
		/// <typeparam name="VC">The type of the C.</typeparam>
		/// <param name="componentParams">The component params.</param>
		public void RenderComponent<VC>(params string[] componentParams) where VC : ViewComponentEx, new()
		{
			ViewComponentEx component = new VC();
			RenderComponent(component, DictHelper.Create(componentParams));
		}

		/// <summary>
		/// Renders the component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <param name="componentParams">The component params.</param>
		/// <remarks>For full details, <see cref="RenderComponent{VC}(IDictionary componentParams)"/>
		/// </remarks>
		/// <example><code><![CDATA[
		/// RenderComponent( new LinkSubmitButtonComponent(), "linkText=Search",
		/// string.Format("formToSubmit={0}", searchFormName));
		/// ]]></code></example>
		public void RenderComponent(ViewComponentEx component, params string[] componentParams)
		{
			RenderComponent(component, DictHelper.Create(componentParams));
		}

		/// <summary>
		/// Renders the component.
		/// </summary>
		/// <param name="component">The component.</param>
		/// <param name="componentParams">The component params.</param>
		/// <remarks>For full details, <see cref="RenderComponent{VC}(IDictionary componentParams)"/>
		/// </remarks>
		/// <example><code><![CDATA[
		/// RenderComponent( new LinkSubmitButtonComponent(), DictHelper.N("linkText","Search").N("formToSubmit", searchFormName);
		/// ]]></code></example>
		public void RenderComponent(ViewComponentEx component, IDictionary componentParams)
		{
			component.Init(EngineContext, Context);
			foreach (DictionaryEntry dictionaryEntry in componentParams)
			{
				component.Context.ComponentParameters[dictionaryEntry.Key]= dictionaryEntry.Value;
			}

			component.Initialize();
			component.Render();


			string html = string.Empty;
			component.Context.Writer.Write(html);

			RenderText(html);
		}
		#endregion
	}

    /// <summary>
    /// Base class to be inherited by ViewComponents which access
    /// the ASP.NET SiteMap.   Handles the Provider &amp;  SiteMapProvider properties,
    /// defaulting to the values in web.config, as does ASP.NET
    /// 
    /// Also added a SiteMapFile property, to set it more directly.
    /// </summary>
    public abstract class ViewComponentUsingSiteMap : ViewComponentEx
    {
        /// <summary>
        /// Gets or sets the site map file.
        /// </summary>
        /// <value>The site map file.</value>
        [ViewComponentParam("SiteMapFile")]
        public string SiteMapFile { get; set; }

        SiteMapProvider m_Provider;
        /// <summary>
        /// Gets or sets a <see cref="SiteMapProvider"/> that is associated with the ViewComponent.
        /// </summary>
        /// <value>The provider.</value>
        /// <remarks>The Provider property specifies an instance of a site map provider to use with the control. 
        /// This provider may be different from the provider identified by the <see cref="SiteMapProvider"/> property, if set.
        ///</remarks>
        [ViewComponentParam]
        public SiteMapProvider Provider 
        {
            get
            {
                if (m_Provider == null)
                {
                    if (!string.IsNullOrEmpty(SiteMapProvider))
                    {
                        m_Provider= SiteMap.Providers[SiteMapProvider];
                    }
                    else if (this.SiteMapFile ==null)
                    {
                        m_Provider= SiteMap.Provider;
                    }

                    if (m_Provider== null)
                    {
                        // Create an instance of the XmlSiteMapProvider class.
                        XmlSiteMapProvider testXmlProvider = new XmlSiteMapProvider();
                        NameValueCollection providerAttributes = new NameValueCollection(1);
                        providerAttributes.Add("siteMapFile", this.SiteMapFile ?? "~/web.sitemap");

                        // Initialize the provider with a provider name and file name.
                        testXmlProvider.Initialize("ViewComponentUsingSiteMapProvider", providerAttributes);

                        // Call the BuildSiteMap to load the site map information into memory.
                        testXmlProvider.BuildSiteMap();
                        m_Provider = testXmlProvider;
                    }
                }
                return m_Provider;
            }
            set
            {
                m_Provider = value;
            }
        }

        /// <summary>
        /// Gets or sets the name of the <see cref="SiteMapProvider"/> used to render the site navigation control.
        /// </summary>
        /// <value>The site map provider.</value>
        [ViewComponentParam]
        public string SiteMapProvider { get; set; }

    }
}
