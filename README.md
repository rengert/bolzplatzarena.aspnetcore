# bolzplatzarena.aspnetcore

Provides a set of features to extend AspNetCore

# How to use

## Install

(https://www.nuget.org/packages/bolzplatzarena.aspnetcore/)

### Package Manager

```Install-Package bolzplatzarena.aspnetcore -Version 0.0.3```

### .NET CLI

```dotnet add package bolzplatzarena.aspnetcore --version 0.0.3```

### PackageReference

```<PackageReference Include="bolzplatzarena.aspnetcore" Version="0.0.3" />```


## The tag helpers

The tag helpers provided by this package, extend the razor view by some new features

### Setup

Update the file ```_ViewImports.chtml``` in you project and add:

```@addTagHelper *, Bolzplatzarena.AspNetCore```

This enables the new tags in all of your view

### Usage

#### Scripts

Inlines the script given into the html, no request to the server by the client is made to access the script file.

```<inline-script src="assets/js/scripts.js"></inline-script>```

#### Styles

Inlines the style given into the html, no request to the server by the client is made to access the style file.

```<inline-style src="css/styles.css"></inline-style>```


### Image

Inlines the image given into the html, no request to the server by the client is made to access the image file.

```<inline-image src="asset/image.png" alt="alternative description"></inline-image>```