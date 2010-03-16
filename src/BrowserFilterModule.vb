Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Text.RegularExpressions
Imports System.Configuration

Namespace com.InspectorMu.Web
    Public Class BrowserFilterModule
        Implements IHttpModule

#Region "Methods"
        Shared deniedBrowsers As String
        Shared allowAccessOnDeny As String
        Shared Sub New()
            deniedBrowsers = ConfigurationManager.AppSettings("deniedBrowsers")
            allowAccessOnDeny = ConfigurationManager.AppSettings("allowAccessOnDeny")
        End Sub


        Public Sub Init(ByVal app As HttpApplication) _
   Implements IHttpModule.Init
            AddHandler app.BeginRequest, _
               AddressOf MyBeginRequest

        End Sub

        Public Sub Dispose() Implements IHttpModule.Dispose
            ' add clean-up code here if required
        End Sub

        Public Sub MyBeginRequest(ByVal s As Object, _
           ByVal e As EventArgs)
            Dim app As HttpApplication = TryCast(s, HttpApplication)

            If FilterBrowser(app) Then
                app.Context.Response.Redirect("~/IEUpgrade.aspx")
            End If
        End Sub

        Private Function FilterBrowser(ByRef app As HttpApplication) As Boolean
            Dim path As String = app.Context.Request.Path.ToLower

            'Customer want to go at their own risk..
            If Not (app.Context.Request.Cookies("forcebrowser") Is Nothing) Then
                Return False
            End If

            'Things to allow
            If Regex.IsMatch(path, allowAccessOnDeny, RegexOptions.IgnoreCase) Then
                Return False
            End If


            'Things to deny
            If Regex.IsMatch(app.Context.Request.UserAgent, deniedBrowsers, RegexOptions.IgnoreCase) Then
                Return True
            End If


            Return False
        End Function
#End Region

    End Class
End Namespace
