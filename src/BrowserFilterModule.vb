﻿Imports Microsoft.VisualBasic
Imports System.Web
Imports System.Text.RegularExpressions
Namespace com.InspectorMu.Web
    Public Class BrowserFilterModule
        Implements IHttpModule

#Region "Methods"

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
            Return (Regex.IsMatch(app.Context.Request.UserAgent, "^.*MSIE [0-6].*$") Or Regex.IsMatch(app.Context.Request.UserAgent, "^.*Firefox\/[0-2].*$") Or Regex.IsMatch(app.Context.Request.UserAgent, "^.*Firefox\/[3]\.[0-4]*$")) And Not app.Context.Request.Path.Contains("/IEUpgrade.aspx") And (app.Context.Request.Cookies("forcebrowser") Is Nothing)
        End Function
#End Region

    End Class
End Namespace
