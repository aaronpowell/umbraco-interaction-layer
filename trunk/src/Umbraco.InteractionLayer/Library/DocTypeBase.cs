using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using umbraco.BusinessLogic;
using umbraco.cms.businesslogic.web;
using Umbraco.InteractionLayer.Library.Properties;
namespace Umbraco.InteractionLayer.Library
{
    [DataContract]
    public class DocTypeBase : INotifyPropertyChanging, INotifyPropertyChanged
    {
        protected int _Id;
        protected string _Text;
        protected Guid _UniqueId;
        protected int _ParentNodeId;
        protected Document _umbracoDocument;
        protected DateTime _createdDate;
        protected User _createUser;

        protected DocTypeBase()
        {
            this.IsDirty = true;
        }

        protected DocTypeBase(Document source)
        {
            if (this.DocTypeInfo.Id != source.ContentType.Id)
            {
                throw new DocTypeMissMatchException(source.ContentType.Id, this.DocTypeInfo.Id);
            }

            this.Id = source.Id;
            this.Text = source.Text;
            this.UniqueId = source.UniqueId;
            if (source.Level != 1)
            {
                this.ParentNodeId = source.Parent.Id; 
            }
            this._umbracoDocument = source;

            var customAttributes = this
                .GetType()
                .GetProperties()
                .Where(Helper.CustomDocTypeProperties);

            var method = typeof(Helper)
                .GetMethod(
                "GetPropertyValue",
                BindingFlags.Public | BindingFlags.Static,
                null,
                new Type[] { typeof(Document), typeof(string) },
                null);

            foreach (var item in customAttributes)
            {
                var att = (UmbracoFieldInfoAttribute)item.GetCustomAttributes(typeof(UmbracoFieldInfoAttribute), false)[0];
                
                var val = method.MakeGenericMethod(item.PropertyType).Invoke(null, new object[] { source, att.Alias });
                item.SetValue(this, val, null);
            }
        }

        public bool IsDirty { get; protected set; }

        [UmbracoFieldInfoAttribute(DisplayName = "Id", Mandatory = true), DataMember(Name="Id")]
        public int Id
        {
            get
            {
                return this._Id;
            }
            protected set
            {
                if (this._Id != value)
                {
                    this.RaisePropertyChanging();
                    this._Id = value;
                    this.RaisePropertyChanged("Id");
                }
            }
        }

        [UmbracoFieldInfoAttribute(DisplayName = "Text", Mandatory = true), DataMember(Name="Text")]
        public string Text
        {
            get
            {
                return this._Text;
            }
            set
            {
                if (this._Text != value)
                {
                    this.RaisePropertyChanging();
                    this._Text = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("Text");
                }
            }
        }

        [UmbracoFieldInfoAttribute(DisplayName = "UniqueId", Mandatory = true), DataMember(Name="UniqueId")]
        public Guid UniqueId
        {
            get
            {
                return this._UniqueId;
            }
            protected set
            {
                if (this._UniqueId != value)
                {
                    this.RaisePropertyChanging();
                    this._UniqueId = value;
                    this.RaisePropertyChanged("UniqueId");
                }
            }
        }

        [UmbracoFieldInfoAttribute(DisplayName = "ParentId", Mandatory = true), DataMember(Name="ParentId")]
        public int ParentNodeId
        {
            get
            {
                return this._ParentNodeId;
            }
            set
            {
                if (this._ParentNodeId != value)
                {
                    this.RaisePropertyChanging();
                    this._ParentNodeId = value;
                    this.IsDirty = true;
                    this.RaisePropertyChanged("ParentNodeId");
                }
            }
        }

        public bool Published
        {
            get
            {
                if (this._umbracoDocument == null)
                {
                    return false;
                }
                else
                {
                    return this._umbracoDocument.Published;
                }
            }
        }

        public DateTime CreatedDate
        {
            get
            {
                if (this._umbracoDocument != null)
                {
                    this._createdDate = this._umbracoDocument.CreateDateTime;
                }
                else
                {
                    throw new NullReferenceException("Umbraco Document does not exist, thus this hasn't been created");
                }

                return this._createdDate;
            }
        }

        public User CreatedBy
        {
            get
            {
                if (this._umbracoDocument != null)
                {
                    this._createUser = this._umbracoDocument.User;
                }
                else
                {
                    throw new NullReferenceException("Umbraco Document does not exist, thus there was no user who created it");
                }

                return this._createUser;
            }
        }

        protected virtual void RaisePropertyChanging()
        {
            if (this.PropertyChanging != null)
            {
                this.PropertyChanging(this, new PropertyChangingEventArgs(String.Empty));
            }
        }

        protected virtual void RaisePropertyChanged(string name)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        protected void ValidateProperty(string regex, string value)
        {
            Regex r = new Regex(regex);
            if (!r.IsMatch(value))
            {
                throw new InvalidCastException("Value does not match validation expression from Umbraco");
            }
        }

        public virtual void Save()
        {
            this.Save(false);
        }

        public virtual void Save(bool publish)
        {
            if (this.IsDirty)
            {
                if (this.Id == 0)
                {
                    var att = this.DocTypeInfo;
                    this._umbracoDocument = Document.MakeNew(this.Text, new DocumentType(att.Id), new User(Settings.Default.CreateAsUserId), this.ParentNodeId);
                    this.Id = this._umbracoDocument.Id;
                    this.UniqueId = this._umbracoDocument.UniqueId;
                }
                else
                {
                    if (this._umbracoDocument.Parent.Id != this.ParentNodeId)
                    {
                        var parent = new Document(this.ParentNodeId);
                        this._umbracoDocument.Parent = parent;
                    }
                }

                var docProperties = this
                    .GetType()
                    .GetProperties()
                    .Where(Helper.CustomDocTypeProperties);

                foreach (var docProperty in docProperties)
                {
                    var umbAtt = (UmbracoFieldInfoAttribute)docProperty.GetCustomAttributes(typeof(UmbracoFieldInfoAttribute), false)[0];
                    ValidateProperty(docProperty, umbAtt);
                    this._umbracoDocument.getProperty(umbAtt.Alias).Value = docProperty.GetValue(this, null);
                }

                this._umbracoDocument.Save();
             
                if (publish)
                {
                    this.Publish();
                }
            }
        }

        protected void ValidateProperty(PropertyInfo docProperty, UmbracoFieldInfoAttribute umbAtt)
        {
            if (umbAtt.Mandatory)
            {
                new Switch(docProperty.GetValue(this, null))
                .Case<int>(i =>
                {
                    if (i == default(int)) throw new MandatoryFailureException(umbAtt.Alias);
                }, true)
                .Case<string>(s =>
                {
                    if (s == default(string)) throw new MandatoryFailureException(umbAtt.Alias);
                }, true)
                .Default<object>(o =>
                {
                    if (o == null) throw new MandatoryFailureException(umbAtt.Alias);
                })
                ;
            }
        }

        public void Publish()
        {
            if (this.Id != 0)
            {
                var docProperties = this
                            .GetType()
                            .GetProperties()
                            .Where(Helper.MandatoryDocTypeProperties);

                foreach (var docProperty in docProperties)
                {
                    var umbAtt = (UmbracoFieldInfoAttribute)docProperty.GetCustomAttributes(typeof(UmbracoFieldInfoAttribute), false)[0];
                    ValidateProperty(docProperty, umbAtt);
                }

                this._umbracoDocument.Publish(new User(Settings.Default.PublishAsUserId));
            }
            else
            {
                throw new Exception("This document does not exist in Umbraco yet. Make sure you save before publishing");
            }
        }

        public UmbracoDocTypeInfoAttribute DocTypeInfo
        {
            get
            {
                var att = this
                    .GetType()
                    .GetCustomAttributes(typeof(UmbracoDocTypeInfoAttribute), false)[0];
                return (UmbracoDocTypeInfoAttribute)att;
            }
        }

        public void Unpublish()
        {
            this._umbracoDocument.UnPublish();
        }

        public void Delete()
        {
            this._umbracoDocument.delete();
            this._umbracoDocument = null;
            this.UniqueId = Guid.Empty;
            this.Id = 0;
            this.IsDirty = true;
        }

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
