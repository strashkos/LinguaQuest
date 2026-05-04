const { app, BrowserWindow } = require('electron');
const { spawn } = require('child_process');
const path = require('path');

let dotnetProcess = null;

function startDotnet() {
  const projectPath = path.join(__dirname, '..');
  // spawn dotnet run
  dotnetProcess = spawn('dotnet', ['run', '--project', 'LinguaQuest.Web.csproj'], { cwd: projectPath, shell: true });

  dotnetProcess.stdout.on('data', (data) => {
    const text = data.toString();
    console.log('[dotnet]', text);
  });
  dotnetProcess.stderr.on('data', (data) => {
    console.error('[dotnet-err]', data.toString());
  });
}

function createWindow() {
  const win = new BrowserWindow({
    width: 1200,
    height: 800,
    webPreferences: {
      nodeIntegration: false,
      contextIsolation: true
    }
  });
  win.loadURL('http://localhost:5000');
}

app.whenReady().then(() => {
  startDotnet();
  // wait a bit for server to start; retry until available could be added
  setTimeout(() => createWindow(), 2500);

  app.on('activate', function () {
    if (BrowserWindow.getAllWindows().length === 0) createWindow();
  });
});

app.on('window-all-closed', () => {
  if (process.platform !== 'darwin') {
    app.quit();
  }
});

app.on('quit', () => {
  if (dotnetProcess) {
    dotnetProcess.kill();
  }
});
