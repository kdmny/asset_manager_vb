Imports Microsoft.VisualBasic
Imports System.Web
Imports System.IO
Namespace com.InspectorMu.Web
    Public Class CompressJsHandler
        Implements IHttpHandler
#Region "Properties"
        Public ReadOnly Property IsReusable() As Boolean Implements System.Web.IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property
#End Region

#Region "Methods"
        Public Sub ProcessRequest(ByVal context As HttpContext) _
        Implements IHttpHandler.ProcessRequest
            CompressionHelper.GetTextResult(context, MimeType.ApplicationJavaScript)
        End Sub

#End Region

    End Class
End Namespace