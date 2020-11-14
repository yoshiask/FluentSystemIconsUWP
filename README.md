# Fluent System Icons for UWP
A UWP library that provides easy access to Microsoft's [Fluent System Icons](https://github.com/microsoft/fluentui-system-icons)

# Installation
## From NuGet
In your app project, install the [`Fluent.Icons` NuGet package](https://nuget.org/packages/Fluent.Icons). (Note that it is currently a prerelease build, so if you are searching for it in the NuGet Package Manager, make sure you have "Include prerelease" checked.)
You can install the latest version using the following command in the NuGet Package Manager:
```ps
Install-Package Fluent.Icons
```
### Build a package
1. Build the solution in `Release` mode and `Any CPU`
2. Open Command Prompt in the solution directory and run `nuget pack Fluent.Icons/Fluent.Icons.csproj -properties Configuration=Release`
3. Go to the Package Manager Console and set the default project to FluentSystemTestApp. Then run `Install-Package "{repo_path}\FluentSystemIconsUWP\Fluent.Icons.{version}.nupkg"` (change `{repo_path}` to where the solution folder is and `{version}` to the package version).
4. Test the package by deploying FluentSystemTestApp.proj

## Build from source
1. Clone the repo
2. Add a reference to `Fluent.Icons.csproj`. You don't need the other two projects to build it.
3. Add `xmlns:fluent="using:Fluent.Icons"` to your pages.

# Examples
The following examples assume that you have imported the `Fluent.Icons` namespace as follows.
```xml
xmlns:fluent="using:Fluent.Icons"
```
```cs
using Fluent.Icons;
```

## Example A
Use a Fluent Icon as a button's content.
```xml
<Button>
    <fluent:FluentSymbolIcon Symbol="AddCircle24"/>
</Button>
```
```cs
myButton.Content = new FluentSymbolIcon(FluentSymbol.AddCircle24);
```

## Example B
Use Fluent Icons in a NavigationView's MenuItems.
```xml
<NavigationView.MenuItems>
    <NavigationViewItem Content="Navigate">
        <NavigationViewItem.Icon>
            <FluentIconElement Symbol="Directions24" />
        </NavigationViewItem.Icon>
    </NavigationViewItem>
</NavigationView.MenuItems>
```
```cs
myNavView.MenuItems.Add(new NavigationViewItem()
{
    Icon = new FluentIconElement(FluentSymbol.Directions24);
});
```

## Example C
Use a Fluent Icon in an AppBarButton.
```xml
<AppBarButton>
    <AppBarButton.Icon>
        <FluentIconElement Symbol="Star24" />
    </AppBarButton.Icon>
</AppBarButton>
```
```cs
myAppBarButton.Icon = new FluentIconElement(FluentSymbol.Star24);
```

## Example D
Show a list of all available Fluent Icons.
```xml
<ListView ItemsSource="{x:Bind fluent:FluentSymbolIcon.AllFluentIcons.Keys}">
    <ListView.ItemTemplate>
        <DataTemplate x:DataType="fluent:FluentSymbol">
            <ListViewItem>
                <StackPanel Spacing="10" Orientation="Horizontal">
                    <fluent:FluentSymbolIcon Symbol="{x:Bind}"/>
                    <TextBlock Text="{x:Bind}"/>
                </StackPanel>
            </ListViewItem>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```

## Example E
Get the `Windows.UI.Xaml.Media.Geometry` object that represents an icon
```cs
FluentSymbolIcon.GetPathData(FluentSymbol.Home);
```

## Example F
Get a PathIcon object that represents an icon
```cs
FluentSymbolIcon.GetPathIcon(FluentSymbol.Home);
```

## Example G
Get the raw SVG path data for a given icon
```cs
FluentSymbolIcon.AllFluentIcons[FluentSymbol.Home];
```
