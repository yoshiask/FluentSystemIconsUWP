# Fluent System Icons for UWP
A UWP library that provides easy access to Microsoft's [Fluent System Icons](https://github.com/microsoft/fluentui-system-icons)

# Installation
At the moment, there is no NuGet package available. For now, you'll have to clone this repo and do the following in your app project:

1. Add a reference to `Fluent.Icons.csproj`. You don't need the other two projects to build it.
2. Add `xmlns:fluent="using:Fluent.Icons"` to your pages.

## From NuGet
In your app project, install the [`Fluent.Icons` NuGet package](https://nuget.org/packages/Fluent.Icons). (Note that it is currently a prerelease build, so if you are searching for it in the NuGet Package Manager, make sure you have "Include prerelease" checked.)
### Build a package
1. Build the solution in `Release` mode and `Any CPU`
2. Open Command Prompt in the solution directory and run `nuget pack Fluent.Icons/Fluent.Icons.csproj -properties Configuration=Release`
3. Go to the Package Manager Console and set the default project to FluentSystemTestApp. Then run `Install-Package "{repo_path}\FluentSystemIconsUWP\Fluent.Icons.{version}.nupkg"` (change `{repo_path}` to where the solution folder is and `{version}` to the package version).
4. Test the package by deploying FluentSystemTestApp.proj


# Examples

## Example A
Use a Fluent Icon as a button's content.
```xml
<Button>
    <fluent:FluentSymbolIcon Symbol="AddCircle"/>
</Button>
```

## Example B
Use Fluent Icons in a NavigationView's MenuItems.
```xml
<NavigationView.MenuItems>
    <NavigationViewItem Content="Navigate">
        <NavigationViewItem.Icon>
            <IconSourceElement>
                <FluentIconSource Symbol="Directions" />
            </IconSourceElement>
        </NavigationViewItem.Icon>
    </NavigationViewItem>
</NavigationView.MenuItems>
```


## Example C
Use a Fluent Icon in an AppBarButton.
```xml
<AppBarButton>
    <AppBarButton.Icon>
        <IconSourceElement>
            <FluentIconSource Symbol="Favorites" />
        </IconSourceElement>
    </AppBarButton.Icon>
</AppBarButton>
```

## Example D
Show a list of all available Fluent Icons.
```xml
<ListView ItemsSource="{x:Bind fluent:FluentSymbolIcon.AllFluentIcons.Values}">
    <ListView.ItemTemplate>
        <DataTemplate x:DataType="x:String">
            <ListViewItem>
                <PathIcon Data="{x:Bind}" />
            </ListViewItem>
        </DataTemplate>
    </ListView.ItemTemplate>
</ListView>
```
