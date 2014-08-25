##AlertBarWpf

This is a WPF user control for displaying status updates through an alert bar. There are four types of alerts: success, danger, warning or information. The color scheme and icons for each are based on the type.
  
<div><img src="/ReadME/Success.png" alt="Success" /></div>
<div><img src="/ReadME/Danger.png" alt="Danger" /></div>
<div><img src="/ReadME/Warning.png" alt="Warning" /></div>
<div><img src="/ReadME/WarningB.png" alt="Warning No Icon" /></div>
<div><img src="/ReadME/Information.png" alt="Information" /></div>

The user control only takes up space whenever a message is being shown, otherwise sits at a collapsed state. It also has an option to auto-close after a set amount of seconds. 

<h4>GUI</h4>
In the xaml you must reference the namespace to add the user control:
```html
<Window ...
    xmlns:mbar="clr-namespace:AlertBarWpfNamespace;assembly=AlertBarWpfControl">
```

Using this reference place the control on the form.  I typically position this above any other controls:
```html
 <mbar:AlertBarWpfControl x:Name="msgbar" />
```

An optional `IconVisibility` parameter to remove icons from all alert messages:

```html
 <mbar:AlertBarWpfControl x:Name="msgbar" IconVisibility="False" />
```

<h4>Code Behind</h4>
To make use of the control we trigger it in the xaml.cs.  Here are some examples:
```csharp
msgbar.Clear();
msgbar.SetDangerMessage("Select an Item.");
msgbar.SetSuccessMessage("Orders Successfully Processed!", 5); //will auto-close after 5 seconds
msgbar.SetWarningMessage("No order info found so declared a re-order.");
msgbar.SetInformationMessage("Sale begins on Aug 1");
```

<h4>Compiled Version</h4>
<a href="/ReadME/Library.zip">Download</a> a ZIP file containing the DLL and other library files.
Visit the author at <a href="http://chadkuehn.com">chadkuehn.com</a>.
