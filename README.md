# DiagnosticsVS - Azure Portal Extensions Dashboard

A Visual Studio extension that provides a dashboard for viewing Azure Portal extension diagnostics and information.

## Building the VSIX

### Prerequisites
- Visual Studio 2019 or 2022 with Visual Studio extension development workload
- .NET Framework 4.8

### Build Instructions

1. **Using the build script**:
   ```
   build-verbose.bat
   ```

2. **Using MSBuild directly**:
   ```
   msbuild DiagnosticsVS.Adapters\DiagnosticsVS.Adapters.csproj /p:Configuration=Debug
   ```

3. **Using Visual Studio**: Open `DiagnosticsVS.sln` and build the solution

The VSIX file will be generated in:
```
DiagnosticsVS.Adapters\bin\Debug\net48\*.vsix
```

## Testing the VSIX

### Method 1: Debug Mode (Recommended for Development)
1. Open `DiagnosticsVS.sln` in Visual Studio
2. Press **F5** to launch an experimental instance of Visual Studio
3. The extension will be automatically loaded in the experimental instance
4. Access the extension via **Tools** → **Show Azure Extensions** menu

### Method 2: Manual Installation
1. Build the project to generate the `.vsix` file
2. Double-click the `.vsix` file to install it in Visual Studio
3. Restart Visual Studio
4. Access the extension via **Tools** → **Show Azure Extensions** menu

### Method 3: Command Line Installation
```
devenv /installvsixtemplates
```

## Using the Extension

1. Once installed, go to **Tools** → **Show Azure Extensions**
2. Select an Azure environment to load extension diagnostics
3. Browse through the list of extensions to view detailed information
4. View extension errors and diagnostic information in the tool window

## Project Structure

- **DiagnosticsVS.Core**: Core functionality and models
- **DiagnosticsVS.Adapters**: Visual Studio extension implementation (generates the VSIX)
- **DiagnosticsVS.Tailwind**: Tailwind CSS build project for styling

## Development Notes

- The extension targets .NET Framework 4.8
- Uses WebView2 for rendering HTML content with Tailwind CSS styling
- Configured to run in Visual Studio's experimental instance during development
- VSIX generation is enabled by default when building the project