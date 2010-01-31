Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.IO
Imports System.Text
Imports System.Web
'using Musicane.Core;

Namespace com.InspectorMu.Web
    Public Class AssetManager
#Region "Fields"

        Private Shared separator As String = "%2f"
        Private Shared ReadOnly scriptCombining As Boolean
        Private Shared ReadOnly staticResourceUrlPrepend As String
        Private Shared ReadOnly defaultUrlPrepend As String
        Private Shared ReadOnly amazonCdnPrepend As String
        Protected Shared ReadOnly unixTimeStart As New DateTime(1970, 1, 1)
        Protected ReadOnly m_cssIncludes As New List(Of String)()

        <ThreadStatic()> _
        Shared m_instance As AssetManager

        Protected ReadOnly m_jsIncludes As New List(Of String)()

#End Region

#Region "Constructors"
        Shared Sub New()
            scriptCombining = True 'Convert.ToBoolean(ConfigurationManager.AppSettings("ScriptCombining"))
            staticResourceUrlPrepend = "http://localhost:49721" 'ConfigurationManager.AppSettings("StaticResourceDomain")
            defaultUrlPrepend = "http://localhost:49721" 'ConfigurationManager.AppSettings("DefaultHostName")
            'staticResourceUrlPrepend = If(staticResourceUrlPrepend, defaultUrlPrepend)
            'amazonCdnPrepend = ConfigurationManager.AppSettings("AmazonCdnPrepend")
        End Sub

        Protected Sub New()
            m_instance = Me
        End Sub

#End Region

#Region "Properties"
        Public Shared ReadOnly Property Instance() As AssetManager
            Get
                Return m_instance
            End Get
        End Property

        Public ReadOnly Property CssIncludes() As List(Of String)
            Get
                Return m_cssIncludes
            End Get
        End Property

        Public ReadOnly Property JsIncludes() As List(Of String)
            Get
                Return m_jsIncludes
            End Get
        End Property
#End Region

#Region "Methods"

        Public Shared Sub CreateInstance()
            Dim instance As New AssetManager()
        End Sub

        ''' <summary>
        ''' Returns a CSS link tag.
        ''' </summary>
        ''' <returns>The tag.</returns>
        Public Shared Function GetCssLinkTag() As String
            If AssetManager.Instance.CssIncludes.Count = 0 Then
                Return ""
            End If
            Dim names As List(Of String) = GetFilteredNameList(AssetManager.Instance.CssIncludes)
            Dim tag As String = GetCssOrJsTag("css", names, "<link rel=""stylesheet"" media=""screen"" href=""{0}"" />")
            Return tag
        End Function

        ''' <summary>
        ''' Returns a CSS link or JavaScript script tag.
        ''' </summary>
        ''' <param name="extension">The file extension--must be "css" or "js".</param>
        ''' <param name="names">The file names (without extension).</param>
        ''' <param name="tagFormat">The tag format.</param>
        ''' <returns>The tag.</returns>
        Private Shared Function GetCssOrJsTag(ByVal extension As String, ByVal names As List(Of String), ByVal tagFormat As String) As String
            'bool debug = (ConfigurationManager.AppSettings["DEBUG"] == "true");
            If scriptCombining Then
                Dim url As String = GetCssOrJsUrl(extension, names)
                Return String.Format(tagFormat, url)
            Else
                Dim builder As New StringBuilder()
                Dim root As String = String.Format(",{0},", extension)
                For Each name As String In names
                    Dim url As String = String.Format("{0}{1}.{2}", root, name, extension)
                    builder.Append(String.Format(tagFormat, url))
                Next
                Return builder.ToString()
            End If
        End Function

        ''' <summary>
        ''' Returns a CSS or JavaScript versioned URL.
        ''' </summary>
        ''' <param name="extension">The file extension--must be "css" or "js".</param>
        ''' <param name="names">The file names (without extension).</param>
        ''' <returns>The URL.</returns>
        Private Shared Function GetCssOrJsUrl(ByVal extension As String, ByVal names As List(Of String)) As String
            Dim builder As New StringBuilder()
            Dim newestTime As DateTime = DateTime.MinValue
            builder.AppendFormat("/combine.c{0}?id=", extension)
            For Each name As String In names
                builder.AppendFormat("{0}%2c", name)

                ' Save newest time for versioning.
                Dim path As String = String.Format("~/{0}.{1}", name, extension)
                path = HttpContext.Current.Server.MapPath(path)
                Dim time As DateTime = File.GetLastWriteTimeUtc(path)
                If time > newestTime Then
                    newestTime = time
                End If
            Next
            builder.Remove(builder.Length - separator.Length, separator.Length)

            Dim unixTime As Integer = GetUnixTimeFromUniversalTime(newestTime)
            builder.AppendFormat("&timestamp={0}", unixTime)
            ' Version via Unix time.
            If Not HttpContext.Current.Request.IsSecureConnection Then
                builder.Insert(0, staticResourceUrlPrepend)
            End If
            Return builder.ToString()
        End Function

        Public Shared Function GetImageUrl(ByVal fileNameNoExtension As String) As String
            Dim builder As New StringBuilder()
            Dim newestTime As DateTime = DateTime.MinValue
            builder.AppendFormat("/images/{0}.jpg?id=", fileNameNoExtension)

            Dim path As String = String.Format("~/images/{0}.jpg", fileNameNoExtension)
            path = HttpContext.Current.Server.MapPath(path)
            Dim time As DateTime = File.GetLastWriteTimeUtc(path)
            If time > newestTime Then
                newestTime = time
            End If
            Dim unixTime As Integer = GetUnixTimeFromUniversalTime(newestTime)
            builder.AppendFormat("&timestamp={0}", unixTime)
            ' Version via Unix time.

            Return builder.ToString()
        End Function

        ''' <summary>
        ''' Returns a name list.
        ''' </summary>
        ''' <param name="nameArrayList">The file name (without extension) array list.</param>
        ''' <returns>The name list.</returns>
        Private Shared Function GetFilteredNameList(ByVal nameArrayList As List(Of String)) As List(Of String)
            Dim nameList As New List(Of String)()
            Dim nameMap As New Dictionary(Of String, Boolean)()
            For Each name As String In nameArrayList
                If Not nameMap.ContainsKey(name) Then
                    ' filter duplicates
                    nameMap(name) = True
                    nameList.Add(name)
                End If
            Next
            Return nameList
        End Function

        ''' <summary>
        ''' Returns a JavaScript script tag.
        ''' </summary>
        ''' <returns>The tag.</returns>
        Public Shared Function GetJsScriptTag() As String
            If AssetManager.Instance.JsIncludes.Count = 0 Then
                Return ""
            End If
            Dim names As List(Of String) = GetFilteredNameList(AssetManager.Instance.JsIncludes)
            Dim tag As String = GetCssOrJsTag("js", names, "<script type=""text/javascript"" src=""{0}""></script>")
            Return tag
        End Function


#End Region

#Region "helper methods"
        Public Shared Function GetLocalTimeFromUnixTime(ByVal seconds As Integer) As DateTime
            Dim time = GetUniversalTimeFromUnixTime(seconds)
            time = time.ToLocalTime()
            Return time
        End Function

        Public Shared Function GetUniversalTimeFromUnixTime(ByVal seconds As Integer) As DateTime
            Dim time = New DateTime(1970, 1, 1, 0, 0, 0, _
             DateTimeKind.Utc)
            time = time.AddSeconds(seconds)
            Return time
        End Function

        ''' <summary>
        ''' Returns the unix time--number of seconds elapsed since midnight Coordinated
        ''' Universal Time (UTC) of January 1, 1970.
        ''' </summary>
        ''' <param name="time">A DateTime represented in local time.</param>
        ''' <returns>The unix time.</returns>
        Public Shared Function GetUnixTimeFromLocalTime(ByVal time As DateTime) As Integer
            Dim seconds As Integer = GetUnixTimeFromUniversalTime(time.ToUniversalTime())
            Return seconds
        End Function

        ''' <summary>
        ''' Returns the unix time--number of seconds elapsed since midnight Coordinated
        ''' Universal Time (UTC) of January 1, 1970.
        ''' </summary>
        ''' <param name="time">A DateTime represented in UTC.</param>
        ''' <returns>The unix time.</returns>
        Public Shared Function GetUnixTimeFromUniversalTime(ByVal time As DateTime) As Integer
            Dim span As TimeSpan = (time - unixTimeStart)
            Dim seconds As Integer = CInt(span.TotalSeconds)
            Return seconds
        End Function


#End Region

    End Class
End Namespace
