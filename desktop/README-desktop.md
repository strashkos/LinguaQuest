Running the desktop wrapper (Windows)

Requirements:
- Node.js (14+)
- npm
- .NET 8 SDK

Steps:

1. Install Electron dependency (once):

```powershell
cd desktop
npm install
```

2. Start the desktop wrapper (this will run `dotnet run` and open an Electron window):

```powershell
npm run start
```

Notes:
- The wrapper spawns `dotnet run --project LinguaQuest.Web.csproj` from the repository root. Ensure ports are free.
- For production packaging use `electron-builder` or `electron-packager` to produce installers.
