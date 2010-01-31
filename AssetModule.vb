Imports Microsoft.VisualBasic
Imports System.Web
Namespace com.InspectorMu.Web
    Public Class AssetModule
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
            AssetManager.CreateInstance()
        End Sub

#End Region

    End Class
End Namespace
