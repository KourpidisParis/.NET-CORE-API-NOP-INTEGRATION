# Git Commands for Repository Update

## Step 1: Navigate to your repository
```bash
cd "C:\Users\Παράσχος Κουρπίδης\Desktop\.NET-CORE-API-NOP-INTEGRATION"
```

## Step 2: Check Git status
```bash
git status
```

## Step 3: Add all new files and changes
```bash
git add .
```

## Step 4: Commit the restructured project
```bash
git commit -m "Restructure project: Add ErpConnector.Tests and solution file

- Move main project to ErpConnector/ subfolder
- Add ErpConnector.Tests/ with 36 unit tests
- Add ErpConnector.sln solution file
- Add comprehensive validator tests for CategoryFromApiDto and ProductFromApiDto
- Update README with new project structure
- Follow Microsoft's recommended project structure"
```

## Step 5: Push to remote repository
```bash
git push origin main
```

## Alternative: If you want to see what will be added first
```bash
git add --dry-run .
git diff --cached --name-only
```

## Verify the new structure works
```bash
# Test the solution builds
dotnet build ErpConnector.sln

# Test the unit tests run
dotnet test ErpConnector.sln
```

## Clean up old temporary files (optional)
```bash
# Remove the temporary test project we created outside the repo
rmdir /s "C:\Users\Παράσχος Κουρπίδης\Desktop\ErpConnector.Tests"
del "C:\Users\Παράσχος Κουρπίδης\Desktop\ErpConnector.sln"
```
