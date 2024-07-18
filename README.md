# Silver And Steel

Silver & Steel is a game I'm using to explore the basics of Godot.

## Getting Started

This section will cover how to setup the repo for development:

### Bootstrapping

To install dependencies for your host system, run:
- `tools/bootstrap/install.ps1` on Windows (uses winget)
- `tools/bootstrap/install.sh` on Mac or Linux (uses brew or system package manager)

This will install:

- Dotnet
  - `dotnet` will be added to your machine's `%PATH%`
- Git
  - Git & Unix Tools will be added to your machine's `%PATH%`
- Godot
  - `C:\opt\godot` will be used as the root path
  - The `%GODOT_BIN%` env var will be set
- Make
  - `make` will be added to your machine's `%PATH%`
- Python3 (for GDLint and friends)
  - `python` will be added to your machine's `%PATH%`
- VsCode

### Setup

This project uses a top-level `Makefile` runner for convenience. Use
`make setup` to setup `git` and install `gdtoolkit` into a `.venv`. Once this
completes, simply open in the godot editor!

# TODO

- `Fms.cs` just assumes node `"Label"` exists.

# Asset Credits

- [Pixel Hero](https://rvros.itch.io/animated-pixel-hero) for protagonist
- [Oak Woods](https://brullov.itch.io/oak-woods) for tileset
- [Skeleton](https://astrobob.itch.io/animated-pixel-art-skeleton) enemy
- [Chrono Icons by Grafixpoint](https://www.flaticon.com/free-icons/chrono) used in engine UI
  - Fsm, State
- https://luizmelo.itch.io/fire-worm
- https://clembod.itch.io/bringer-of-death-free
- https://kyrise.itch.io/kyrises-free-16x16-rpg-icon-pack
- https://ansimuz.itch.io/warped-shooting-fx
- https://shikashipx.itch.io/shikashis-fantasy-icons-pack
