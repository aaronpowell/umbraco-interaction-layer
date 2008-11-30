//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Umbraco.Generated
{
    using System;
    using Umbraco.InteractionLayer.Library;
    using System.Collections.Generic;
    using System.Linq;
    
    
    /// <summary>
    /// 
    /// </summary>
    [UmbracoDocTypeInfo(Alias="Galleries List Page", Id=1063)]
    public partial class GalleriesListPage : DocTypeBase
    {
        
        private string _PageHeader;
        
        private string _bodyText;
        
        private string _secondaryText;
        
        private string _PageTitle;
        
        private string _Description;
        
        private string _Keywords;
        
        private int _umbracoNaviHide;
        
        private string _SortBy;
        
        private string _SortOrder;
        
        private System.Collections.Generic.IEnumerable<Gallery> _Galleries;
        
        public GalleriesListPage()
        {
            this.IsDirty = true;
        }
        
        public GalleriesListPage(int id) : 
                this(new umbraco.cms.businesslogic.web.Document(id))
        {
        }
        
        public GalleriesListPage(System.Guid uniqueId) : 
                this(new umbraco.cms.businesslogic.web.Document(uniqueId))
        {
        }
        
        protected internal GalleriesListPage(umbraco.cms.businesslogic.web.Document source) : 
                base(source)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Page Header", Mandatory=true, IsCustom=true, Alias="PageHeader")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="PageHeader")]
        public virtual string PageHeader
        {
            get
            {
                return this._PageHeader;
            }
            set
            {
                if ((this.PageHeader != value))
                {
                    this.RaisePropertyChanging();
                    this._PageHeader = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("PageHeader");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Content", Mandatory=false, IsCustom=true, Alias="bodyText")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="bodyText")]
        public virtual string bodyText
        {
            get
            {
                return this._bodyText;
            }
            set
            {
                if ((this.bodyText != value))
                {
                    this.RaisePropertyChanging();
                    this._bodyText = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("bodyText");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Secondary Content", Mandatory=false, IsCustom=true, Alias="secondaryText")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="secondaryText")]
        public virtual string secondaryText
        {
            get
            {
                return this._secondaryText;
            }
            set
            {
                if ((this.secondaryText != value))
                {
                    this.RaisePropertyChanging();
                    this._secondaryText = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("secondaryText");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Page Title", Mandatory=true, IsCustom=true, Alias="PageTitle")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="PageTitle")]
        public virtual string PageTitle
        {
            get
            {
                return this._PageTitle;
            }
            set
            {
                if ((this.PageTitle != value))
                {
                    this.RaisePropertyChanging();
                    this._PageTitle = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("PageTitle");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Description (Upto 255 characters)", Mandatory=false, IsCustom=true, Alias="Description")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="Description")]
        public virtual string Description
        {
            get
            {
                return this._Description;
            }
            set
            {
                if ((this.Description != value))
                {
                    this.RaisePropertyChanging();
                    this._Description = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("Description");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Keywords (Seperate keywords with commas)", Mandatory=false, IsCustom=true, Alias="Keywords")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="Keywords")]
        public virtual string Keywords
        {
            get
            {
                return this._Keywords;
            }
            set
            {
                if ((this.Keywords != value))
                {
                    this.RaisePropertyChanging();
                    this._Keywords = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("Keywords");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Hidden page (Hidden from Navi and Sitemap)", Mandatory=false, IsCustom=true, Alias="umbracoNaviHide")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="umbracoNaviHide")]
        public virtual int umbracoNaviHide
        {
            get
            {
                return this._umbracoNaviHide;
            }
            set
            {
                if ((this.umbracoNaviHide != value))
                {
                    this.RaisePropertyChanging();
                    this._umbracoNaviHide = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("umbracoNaviHide");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Sort By", Mandatory=false, IsCustom=true, Alias="SortBy")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="SortBy")]
        public virtual string SortBy
        {
            get
            {
                return this._SortBy;
            }
            set
            {
                if ((this.SortBy != value))
                {
                    this.RaisePropertyChanging();
                    this._SortBy = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("SortBy");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Sort Order", Mandatory=false, IsCustom=true, Alias="SortOrder")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="SortOrder")]
        public virtual string SortOrder
        {
            get
            {
                return this._SortOrder;
            }
            set
            {
                if ((this.SortOrder != value))
                {
                    this.RaisePropertyChanging();
                    this._SortOrder = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("SortOrder");
                }
            }
        }
        
        public System.Collections.Generic.IEnumerable<Gallery> Galleries
        {
            get
            {
                if ((this._Galleries == null))
                {
                    this._Galleries = this._umbracoDocument.Children.Where(d => d.ContentType.Id == 1064).Select(d => new Gallery(d));;
                }
                return this._Galleries;
            }
        }
    }
}