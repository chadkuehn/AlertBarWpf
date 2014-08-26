##Alert Bar WPF UserControl

This is a WPF usercontrol for displaying user updates through an alert bar. There are four types of alerts: success, danger, warning or information. The color scheme and icons for each are based on the type. 
  
<div><img src="/ReadME/Success.png" alt="Success" /></div>
<div><img src="/ReadME/Danger.png" alt="Danger" /></div>
<div><img src="/ReadME/Warning.png" alt="Warning" /></div>
<div><img src="/ReadME/WarningB.png" alt="Warning No Icon" /></div>
<div><img src="/ReadME/Information.png" alt="Information" /></div>

The usercontrol only takes up space whenever a message is being shown, otherwise sits at a collapsed state.  A close button is included to dismiss the alert. It also has an option to auto-close after a set amount of seconds. 

<h4>GUI</h4>
In the xaml you must reference the namespace to add the usercontrol:
```html
<Window ...
    xmlns:mbar="clr-namespace:AlertBarWpf;assembly=AlertBarWpf">
```

Using this reference place the control on the form.  I typically position this above any other controls:
```html
 <mbar:AlertBarWpf x:Name="msgbar" />
```

An optional `IconVisibility` parameter to remove icons from all alert messages:

```html
 <mbar:AlertBarWpf x:Name="msgbar" IconVisibility="False" />
```

<h4>Code Behind</h4>
To make use of the control we trigger it in the xaml.cs.  Here are some examples:
```csharp
msgbar.Clear();
msgbar.SetDangerAlert("Select an Item.");
msgbar.SetSuccessAlert("Orders Successfully Processed!", 5); //will auto-close after 5 seconds
msgbar.SetWarningAlert("No order info found so declared a re-order.");
msgbar.SetInformationAlert("Sale begins on Aug 1");
```

<h4>Intall</h4>
A <a href="https://www.nuget.org/packages/AlertBarWpf/">nuget</a> exists for this usercontrol. It can be installed within Visual Studio.  Alternatively, you can <a href="/ReadME/Library.zip">download</a> a ZIP file containing the DLL.  In Visual Studio, you can add a Reference and browse to that file.
Visit the author at <a href="http://chadkuehn.com">chadkuehn.com</a>.
