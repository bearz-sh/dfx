# Bearz.Internal

This assembly should not be compiled and referenced directly other than
for the purpose of testing. It is meant to be used through links.

```xml
<ItemGroup>
    <Compile Include="$(InternalDir)/Polyfill/Range.cs" Link="Polyfill/Range.cs" />
</ItemGroup>
```

MIT
