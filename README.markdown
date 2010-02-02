# InspectorMu's VB.NET AssetManager

## Getting Started

### Add assembly reference

Add an assembly reference to com.InspectorMu.Web.dll

### Update your web.config

#### IIS 6

Add this to your web.config:  
`<system.web>
  ...
  <httpHandlers>
    ....
    <add path="*.cjs" verb="*" type="com.InspectorMu.Web.CompressJsHandler, com.InspectorMu.Web"/>
    <add path="*.ccss" verb="*" type="com.InspectorMu.Web.CompressCssHandler, com.InspectorMu.Web"/>
    ....
  <httpHandlers>
  <httpModules> 
    ....
    <add name="AssetModule" type="com.InspectorMu.Web.AssetModule, com.InspectorMu.Web"/> 
    ....
  </httpModules>
... 
</system.web>`

#### IIS 7

Add this to your web.config:  

`<system.webServer>
  ....
      <modules>
          ....
          <add name="AssetModule" type="com.InspectorMu.Web.AssetModule, com.InspectorMu.Web"/>
          ....
      </modules>
      <handlers>
          ....
          <add name="CompressJsHandler" path="*.cjs" verb="*" type="com.InspectorMu.Web.CompressJsHandler, com.InspectorMu.Web"/>
          <add name="CompressCssHandler" path="*.ccss" verb="*" type="com.InspectorMu.Web.CompressCssHandler, com.InspectorMu.Web"/>
          ....
      </handlers>
  </system.webServer>`

### Use it in your views

Add this at the top of your view (aspx/layout) *note there is no extension required*: 
 
`<%@ Import Namespace="com.InspectorMu.Web" %`

Include some js files:  

`<% AssetManager.Instance.JsIncludes.Add("path/from/root/to/jsfile1")%>
<% AssetManager.Instance.JsIncludes.Add("path/from/root/to/jsfile2")%> 
<% AssetManager.Instance.JsIncludes.Add("path/from/root/to/jsfileN")%>
<%=AssetManager.GetJsScriptTag()%>`

InspectorMu's asset manager also works with css files *note there is no extension required*:  

`<% AssetManager.Instance.CssIncludes.Add("path/from/root/to/file1")%>
<% AssetManager.Instance.CssIncludes.Add("path/from/root/to/file2")%>
<% AssetManager.Instance.CssIncludes.Add("path/from/root/to/fileN")%>
<%=AssetManager.GetCssLinkTag()%>`

 