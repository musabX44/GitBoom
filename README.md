# GitBoom

A minimalist, high-performance command-line utility written in C# to automatically detect and forcefully purge merged or stale local Git branches. It helps developers keep their local workspaces clean without manual intervention.

## Features

- **Automated Target Discovery:** Scans all local branches and filters out default protected branches (`main`, `master`, `dev`, `develop`) as well as your currently active branch.
- **Interactive Safeguard:** Prompts for explicit user confirmation before executing any destructive operations.
- **Zero Runtime Dependencies:** Compiles into a single, high-performance native binary file. No external .NET runtime installation required on the host system.
- **Clean CLI UX:** Provides clear, distraction-free console output with standardized success and warning formats.

## Prerequisites

- Git must be installed and available in your system's `PATH`.

## Installation

### Building from Source

1. Clone the repository:
   ```bash
   git clone https://github.com
   cd GitBoom
   ```

2. Publish the standalone native binary via .NET SDK:
   ```bash
   dotnet publish -c Release -r linux-x64 --self-contained true /p:PublishSingleFile=true
   ```

3. Move the binary to your execution path and grant permissions:
   ```bash
   cp bin/Release/net10.0/linux-x64/publish/GitBoomApp ./gitboom
   chmod +x ./gitboom
   ```

## Usage

Navigate to any Git repository workspace and execute the compiled binary:

```bash
./gitboom
```

### Example Output

```text
[GitBoom] Cleaning process is starting...
WARNING: Detected 3 trash branch(es). Are you sure you want to blast them all? [y/N]: y

Access granted! Detonating trash branches...

[BOOM] feature-payment deleted.
[BOOM] fix-login-v2 deleted.
[BOOM] refactor-ui deleted.
=================================================
SUCCESS: Successfully destroyed 3 trash branch(es)!
```

## Protected Branches

By default, GitBoom will never delete or prompt to delete the following branches:
- `main`
- `master`
- `dev`
- `develop`
- The currently checked-out (active) branch

## License

This project is licensed under the MIT License - see the LICENSE file for details.
