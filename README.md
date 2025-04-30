## Run the app
1. Clear the cache:
```
dotnet nuget locals --clear all
```

2. Build and run the app:
```
dotnet build -t:Run -f net9.0-ios -p:RuntimeIdentifier=ios-arm64
```
