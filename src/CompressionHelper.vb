Imports Microsoft.VisualBasic
Imports System.IO
Imports System.Text
Imports System.Web

Namespace com.InspectorMu.Web
    Public Class CompressionHelper

        Public Shared Sub GetTextResult(ByRef context As HttpContext, ByVal type As MimeType)
            Dim text = Compress(context, type)
            context.Response.ContentType = MimeTypeHelper.GetString(type)
            context.Response.Output.Write(text)
        End Sub

        Protected Shared Function Compress(ByRef context As HttpContext, ByVal type As MimeType) As String
            Dim builder As New StringBuilder()
            Dim extension As String
            Select Case type
                Case MimeType.TextCss
                    extension = "css"
                    Exit Select
                Case MimeType.ApplicationJavaScript
                    extension = "js"
                    Exit Select
                Case Else
                    Throw New NotImplementedException()
            End Select
            Dim id = context.Request.QueryString("id")
            Dim names As String() = id.Split(","c)
            For Each name As String In names
                Dim path = String.Format("~/{0}.{1}", name, extension)
                path = context.Server.MapPath(path)
                If File.Exists(path) Then
                    Try
                        builder.AppendLine(File.ReadAllText(path).Replace("url(", "url(" + AssetManager.UrlPrepend))
                    Catch ex As Exception
                        'Warning need to add email messaging
                    End Try
                End If
            Next
            Dim combinedResult = builder.ToString()
            Dim result As String
            Select Case type
                Case MimeType.ApplicationJavaScript
                    result = combinedResult
                    'result = packer.Pack(combinedResult);
                    Exit Select
                Case MimeType.TextCss
                    result = combinedResult
                    Exit Select
                Case Else
                    Throw New NotImplementedException()
            End Select
            Return result
        End Function

    End Class
End Namespace