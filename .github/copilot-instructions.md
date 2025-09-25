# Ursa.Avalonia

Ursa.Avalonia is an enterprise-level UI library for building cross-platform applications with Avalonia UI. This is a .NET 8.0 library that provides advanced UI controls and themes for Avalonia applications.

**Always reference these instructions first and fallback to search or bash commands only when you encounter unexpected information that does not match the info here.**

## Working Effectively

### Prerequisites and Setup
- Requires .NET 8.0 SDK (minimum 8.0.0). Check version with `dotnet --version`
- Install required workloads for full functionality:
  - `dotnet workload restore` -- installs wasi-experimental workload automatically
  - `dotnet workload install wasm-tools` -- required for browser demos. Takes 30-60 seconds.
  - Mobile workloads (Android/iOS) may not be available in all environments

### Building the Library
- Use the provided build scripts for core libraries:
  - Linux/macOS: `./build.sh` -- builds core Ursa libraries. NEVER CANCEL. Takes ~26 seconds. Set timeout to 120+ seconds.
  - Windows: `./build.ps1` -- builds core Ursa libraries
- Build specific projects:
  - Core library: `dotnet build src/Ursa/Ursa.csproj`
  - Themes: `dotnet build src/Ursa.Themes.Semi/Ursa.Themes.Semi.csproj`
  - Prism extension: `dotnet build src/Ursa.PrismExtension/Ursa.PrismExtension.csproj`
- Build entire solution (excluding mobile): `dotnet build` -- may fail on mobile projects without workloads

### Running Demo Applications
- Desktop demo: `dotnet run --project demo/Ursa.Demo.Desktop/Ursa.Demo.Desktop.csproj` -- NEVER CANCEL. Build takes ~22 seconds. Set timeout to 120+ seconds.
  - Cannot run in headless environments (will fail with XOpenDisplay error)
- Browser demo: 
  - Build: `dotnet build demo/Ursa.Demo.Browser/Ursa.Demo.Browser.csproj` -- NEVER CANCEL. Takes ~14 seconds. Set timeout to 120+ seconds.
  - Serve locally: `python3 -m http.server 8000 --directory demo/Ursa.Demo.Browser/bin/Debug/net8.0-browser/`
  - Access at: `http://127.0.0.1:8000`
- Sandbox project: `dotnet run --project demo/Sandbox/Sandbox.csproj` -- minimal test environment

### Testing
- Unit tests: `dotnet test tests/Test.Ursa` -- NEVER CANCEL. Takes ~13 seconds, runs 66 tests. Set timeout to 60+ seconds.
- Headless tests: `dotnet test tests/HeadlessTest.Ursa` -- NEVER CANCEL. Takes ~29 seconds, runs 584 tests. Set timeout to 120+ seconds.
- Both test suites should pass completely in a properly configured environment

### Validation Steps
- Always run these commands after making changes:
  - `./build.sh` to validate core library builds
  - `dotnet test tests/Test.Ursa && dotnet test tests/HeadlessTest.Ursa` to validate functionality
- Test browser demo functionality by building and serving locally
- Verify mobile demo builds work if mobile workloads are available

## Repository Structure

### Key Directories
- `/src/` -- Core library source code
  - `Ursa/` -- Main UI library (netstandard2.0 and net8.0)
  - `Ursa.Themes.Semi/` -- Semi Design theme package
  - `Ursa.PrismExtension/` -- Prism.Avalonia integration
  - `Ursa.ReactiveUIExtension/` -- ReactiveUI integration
- `/demo/` -- Example applications
  - `Ursa.Demo/` -- Shared demo code
  - `Ursa.Demo.Desktop/` -- Desktop application (Windows/Linux/macOS)
  - `Ursa.Demo.Browser/` -- WebAssembly browser application
  - `Ursa.Demo.Android/` -- Android application (requires workloads)
  - `Ursa.Demo.iOS/` -- iOS application (requires workloads)
  - `Sandbox/` -- Simple test application
- `/tests/` -- Test projects
  - `Test.Ursa/` -- Unit tests (66 tests)
  - `HeadlessTest.Ursa/` -- Headless UI tests (584 tests)

### Important Files
- `global.json` -- Specifies .NET 8.0.0 SDK requirement
- `Ursa.sln` -- Main solution file with all projects
- `src/Package.props` -- Shared package properties (Avalonia 11.1.1)
- `demo/Directory.Build.props` -- Demo-specific properties (Avalonia 11.2.5)
- `build.sh`/`build.ps1` -- Core library build scripts

## Common Development Tasks

### Making Changes to Controls
- Ursa controls are located in `src/Ursa/Controls/`
- Themes and styles are in `src/Ursa.Themes.Semi/`
- Always test changes with both unit tests and headless tests
- Verify demo applications still function correctly

### Adding New Features
- Follow existing patterns in `src/Ursa/Controls/`
- Add corresponding tests in `tests/HeadlessTest.Ursa/Controls/`
- Update demo applications to showcase new functionality
- Ensure compatibility with both netstandard2.0 and net8.0 targets

### Working with Themes
- Semi theme implementation is in `src/Ursa.Themes.Semi/`
- Reference theme in applications: `xmlns:u-semi="https://irihi.tech/ursa/themes/semi"`
- Theme must be included after Semi base theme in App.axaml

### CI/CD Integration
- GitHub Actions are configured in `.github/workflows/`
- `test.yml` runs on both Windows and Ubuntu
- Tests include code coverage reporting
- Mobile builds may be excluded from CI in environments without workloads

## Known Issues and Workarounds

### Build Warnings
- Nullable reference type warnings are common and generally non-blocking
- Async method warnings in demo code are cosmetic

### Platform Limitations
- Desktop applications require display server (fail in headless environments)
- Mobile applications require specific workloads that may not be available
- Browser applications require WASM workloads

### Environment Dependencies
- .NET 8.0 SDK is required (specified in global.json)
- Avalonia versions differ between library (11.1.1) and demos (11.2.5)
- Some workloads may have long installation times

## Troubleshooting

### Build Failures
- Ensure correct .NET SDK version: `dotnet --version` should show 8.0.x
- Install missing workloads: `dotnet workload restore`
- Clean build artifacts: `dotnet clean` then rebuild

### Test Failures
- Verify all dependencies are installed
- Check that Avalonia themes are properly configured
- Ensure headless environment is properly set up for UI tests

### Demo Issues
- Desktop demos require display server
- Browser demos need proper WASM workload installation
- Mobile demos need platform-specific workloads

Always validate that your changes work by running the full test suite and building all target projects before committing changes.