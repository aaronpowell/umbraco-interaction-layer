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
    [UmbracoDocTypeInfo(Alias="Photo", Id=1065)]
    public partial class Photo : DocTypeBase
    {
        
        private string _FullImage;
        
        private string _ThumbImage;
        
        private string _PageTitle;
        
        private string _Description;
        
        private string _Keywords;
        
        private int _umbracoNaviHide;
        
        public Photo()
        {
            this.IsDirty = true;
        }
        
        public Photo(int id) : 
                this(new umbraco.cms.businesslogic.web.Document(id))
        {
        }
        
        public Photo(System.Guid uniqueId) : 
                this(new umbraco.cms.businesslogic.web.Document(uniqueId))
        {
        }
        
        protected internal Photo(umbraco.cms.businesslogic.web.Document source) : 
                base(source)
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Full Image", Mandatory=false, IsCustom=true, Alias="FullImage")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="FullImage")]
        public virtual string FullImage
        {
            get
            {
                return this._FullImage;
            }
            set
            {
                if ((this.FullImage != value))
                {
                    this.RaisePropertyChanging();
                    this._FullImage = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("FullImage");
                }
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        [UmbracoFieldInfo(DisplayName="Thumbnail", Mandatory=false, IsCustom=true, Alias="ThumbImage")]
        [System.Runtime.Serialization.DataMemberAttribute(Name="ThumbImage")]
        public virtual string ThumbImage
        {
            get
            {
                return this._ThumbImage;
            }
            set
            {
                if ((this.ThumbImage != value))
                {
                    this.RaisePropertyChanging();
                    this._ThumbImage = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("ThumbImage");
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
    }
}