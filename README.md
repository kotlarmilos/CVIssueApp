## Run the app
1. Clean build:
```
rm -rf CVIssueApp/obj CVIssueApp/bin
```

2. Clear the cache:
```
dotnet nuget locals --clear all
```

3. Build and run the app:
```
dotnet build -t:Run -f net9.0-ios -p:RuntimeIdentifier=ios-arm64
```
