# Fluent System Icons for UWP
A UWP library that provides easy access to Microsoft's [Fluent System Icons](https://github.com/microsoft/fluentui-system-icons)

# Examples
In your app project, reference the `Microsoft.Design.Fluent` project. Then, add `xmlns:fluent="using:Microsoft.Design.Fluent"` to your pages.

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
