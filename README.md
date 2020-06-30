# Fluent System Icons for UWP
A UWP library that provides easy access to Microsoft's [Fluent System Icons](https://github.com/microsoft/fluentui-system-icons)

# Installation
At the moment, there is no NuGet package available. For now, you'll have to clone this repo and do the following in your app project:

1. Add a reference to `Fluent.Icons.csproj`. You don't need the other two projects to build it.
2. Add `xmlns:fluent="using:Fluent.Icons"` to your pages.

<!--In your app project, install the [`Fluent.Icons` NuGet package](https://nuget.org/packages/Fluent.Icons). (Note that it is currently a prerelease build, so if you are searching for it in the NuGet Package Manager, make sure you have "Include prerelease" checked.)-->

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
