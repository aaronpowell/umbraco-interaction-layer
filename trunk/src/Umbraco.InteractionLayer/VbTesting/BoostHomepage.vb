'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3053
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Umbraco.InteractionLayer.Library

Namespace Umbraco.Generated
    
    '''<summary>
    '''The homepage of a boost website.
    '''</summary>
    <UmbracoDocTypeInfo([Alias]:="Boost Homepage", Id:=1044)>  _
    Partial Public Class BoostHomepage
        Inherits DocTypeBase
        
        Private _bodyText As String
        
        Private _siteName As String
        
        Private _siteDescription As String
        
        Private _BoostTextpages As System.Collections.Generic.IEnumerable(Of BoostTextpage)
        
        Public Sub New()
            MyBase.New
            Me.IsDirty = true
        End Sub
        
        Public Sub New(ByVal id As Integer)
            Me.New(New Global.umbraco.cms.businesslogic.web.Document(id))
        End Sub
        
        Public Sub New(ByVal uniqueId As System.Guid)
            Me.New(New Global.umbraco.cms.businesslogic.web.Document(uniqueId))
        End Sub
        
        Protected Friend Sub New(ByVal source As Global.umbraco.cms.businesslogic.web.Document)
            MyBase.New(source)
        End Sub
        
        '''<summary>
        '''
        '''</summary>
        <UmbracoFieldInfo(DisplayName:="Body text", Mandatory:=false, IsCustom:=true, [Alias]:="bodyText"),  _
         Global.System.Runtime.Serialization.DataMemberAttribute(Name:="bodyText")>  _
        Public Overridable Property bodyText() As String
            Get
                Return Me._bodyText
            End Get
            Set
                If (Me.bodyText <> value) Then
                    Me.RaisePropertyChanging
                    Me._bodyText = value
                    Me.IsDirty = true
                    Me.RaisePropertyChanged("bodyText")
                End If
            End Set
        End Property
        
        '''<summary>
        '''
        '''</summary>
        <UmbracoFieldInfo(DisplayName:="Site Name", Mandatory:=false, IsCustom:=true, [Alias]:="siteName"),  _
         Global.System.Runtime.Serialization.DataMemberAttribute(Name:="siteName")>  _
        Public Overridable Property siteName() As String
            Get
                Return Me._siteName
            End Get
            Set
                If (Me.siteName <> value) Then
                    Me.RaisePropertyChanging
                    Me._siteName = value
                    Me.IsDirty = true
                    Me.RaisePropertyChanged("siteName")
                End If
            End Set
        End Property
        
        '''<summary>
        '''
        '''</summary>
        <UmbracoFieldInfo(DisplayName:="Site Description", Mandatory:=false, IsCustom:=true, [Alias]:="siteDescription"),  _
         Global.System.Runtime.Serialization.DataMemberAttribute(Name:="siteDescription")>  _
        Public Overridable Property siteDescription() As String
            Get
                Return Me._siteDescription
            End Get
            Set
                If (Me.siteDescription <> value) Then
                    Me.RaisePropertyChanging
                    Me._siteDescription = value
                    Me.IsDirty = true
                    Me.RaisePropertyChanged("siteDescription")
                End If
            End Set
        End Property
        
        Public ReadOnly Property BoostTextpages() As System.Collections.Generic.IEnumerable(Of BoostTextpage)
            Get
                If (Me._BoostTextpages Is Nothing) Then
                    Me._BoostTextpages = Me._umbracoDocument.Children.Where(Function(d) d.ContentType.Id =  1045).Select(Function(d) New BoostTextpage(d))
                End If
                Return Me._BoostTextpages
            End Get
        End Property
    End Class
End Namespace