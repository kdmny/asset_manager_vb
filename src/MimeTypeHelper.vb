Imports System.Collections.Generic

Namespace com.InspectorMu.Web
    Public Class MimeTypeHelper
#Region "Fields"

        Private Shared ReadOnly stringTypeMap As Dictionary(Of String, MimeType)
        Private Shared ReadOnly typeStringMap As Dictionary(Of MimeType, String)

#End Region

#Region "Constructors"

        Shared Sub New()
            typeStringMap = New Dictionary(Of MimeType, String)()
            typeStringMap.Add(MimeType.ApplicationAtomXml, "application/atom+xml")
            typeStringMap.Add(MimeType.ApplicationJavaScript, "application/javascript")
            typeStringMap.Add(MimeType.ApplicationJson, "application/json")
            typeStringMap.Add(MimeType.ApplicationOctetStream, "application/octet-stream")
            typeStringMap.Add(MimeType.ApplicationRssXml, "application/rss+xml")
            typeStringMap.Add(MimeType.ApplicationXhtmlXml, "application/xhtml+xml")
            typeStringMap.Add(MimeType.ApplicationXml, "application/xml")
            typeStringMap.Add(MimeType.ApplicationXMPlayer2, "application/x-mplayer2")
            typeStringMap.Add(MimeType.ApplicationXShockwaveFlash, "application/x-shockwave-flash")
            typeStringMap.Add(MimeType.ApplicationXWwwFormUrlEncoded, "application/x-www-form-urlencoded")
            typeStringMap.Add(MimeType.AudioMpeg, "audio/mpeg")
            typeStringMap.Add(MimeType.AudioQcelp, "audio/QCELP")
            typeStringMap.Add(MimeType.ImageBmp, "image/bmp")
            typeStringMap.Add(MimeType.ImageGif, "image/gif")
            typeStringMap.Add(MimeType.ImagePJpeg, "image/pjpeg")
            typeStringMap.Add(MimeType.ImageJpeg, "image/jpeg")
            typeStringMap.Add(MimeType.ImagePng, "image/png")
            typeStringMap.Add(MimeType.ImageXPng, "image/x-png")
            typeStringMap.Add(MimeType.TextCss, "text/css")
            typeStringMap.Add(MimeType.TextHtml, "text/html")
            typeStringMap.Add(MimeType.TextPlain, "text/plain")
            typeStringMap.Add(MimeType.TextXml, "text/xml")
            typeStringMap.Add(MimeType.VideoQuickTime, "video/quicktime")
            typeStringMap.Add(MimeType.VideoXFlv, "video/x-flv")
            typeStringMap.Add(MimeType.VideoXMsWm, "video/x-ms-wm")
            typeStringMap.Add(MimeType.VideoXMsWmv, "video/x-ms-wmv")

            stringTypeMap = New Dictionary(Of String, MimeType)(typeStringMap.Count)
            For Each pair In typeStringMap
                stringTypeMap.Add(pair.Value, pair.Key)
            Next
        End Sub

#End Region

#Region "Methods"

        Public Shared Function GetString(ByVal value As MimeType) As String
            Dim result = typeStringMap(value)
            Return result
        End Function

        Public Shared Function GetMimeType(ByVal value As String) As MimeType
            Dim result = stringTypeMap(value)
            Return result
        End Function

        Public Shared Function GetMimeTypeByContentType(ByVal value As String) As MimeType
            Dim tokens = value.Split(";"c)
            Dim mimeTypeString = tokens(0)
            Dim result = GetMimeType(mimeTypeString)
            Return result
        End Function

#End Region
    End Class
End Namespace
