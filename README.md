# Alert Bar WPF UserControl

This is a WPF usercontrol for displaying user updates through an alert bar. There are four types of alerts: success, danger, warning or information. The color scheme and icons for each are based on the type. 

![Screenshot 1](https://raw.github.com/chadkuehn/AlertBarWpf/master/asset/screenshots/screenshot1.gif)

## Dependencies
 - WPF Application

## Usage
Download the [archive](https://github.com/chadkuehn/AlertBarWpf/releases/latest) or install the [nuget](https://www.nuget.org/packages/AlertBarWpf/).  If you downloaded the archive then in Visual Studio you must go to Solution Explorer » right-click on References » Add a Reference » browse to DLL in the archive bin folder.

**GUI:**  
In the xaml you must reference the namespace to add the usercontrol:
```html
<Window ...
    xmlns:mbar="clr-namespace:AlertBarWpf;assembly=AlertBarWpf">
```

Using this reference place the control on the form.  I typically position this above any other controls:
```html
 <mbar:AlertBarWpf x:Name="msgbar" />
```

An optional `IconVisibility` parameter to remove icons from all alert messages.  There is also a `Theme` parameter to adjust the look of the bar:

```html
 <mbar:AlertBarWpf x:Name="msgbar" IconVisibility="False" Theme="Outline" />
```

**Code Behind:**  
To make use of the control we trigger it in the xaml.cs.  Call the methods like so:
```csharp
msgbar.Clear();
msgbar.SetDangerAlert("Select an Item.");
```


## Features
 - Multiple themes
 - Recognizable color scheme/icons for danger, success, warning, or information
 - Does not occupy space when not in use
 - Auto-closes (if desired)


## API
**Methods:**

 - Clear
 - SetDangerAlert
 - SetSuccessAlert
 - SetWarningAlert
 - SetInformationAlert  

Each of the creation methods above takes a message parameter and an optional timeout parameter (based on seconds). 

**XAML Properties:**
 - Theme
 - IconVisibility

**Themes:**  

 - Outline
 - Standard



## Support
Found a bug or have a feature request? [Open an issue](https://github.com/chadkuehn/AlertBarWpf/issues/new ).  
 
## Author
**Chad Kuehn** ([@ChadillacMan](https://twitter.com/ChadillacMan))  
[http://chadkuehn.com](http://chadkuehn.com)

## Copyright & License
Copyright (c) 2014 Chad Kuehn  

AlertBarWpf is available under the MIT license. See the [LICENSE file][7.1]
for more information.

[7.1]: ./LICENSE.txt
