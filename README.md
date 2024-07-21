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

# Check List

- [] MoveAndSlideWithSnap so the player doesn't go shooting off skeletons' heads?

# Asset Credits

- [Pixel Hero](https://rvros.itch.io/animated-pixel-hero) for protagonist
- [Oak Woods](https://brullov.itch.io/oak-woods) for tileset
- [Skeleton](https://astrobob.itch.io/animated-pixel-art-skeleton) enemy
- Shaders:
  - [Shine by Jacob Vanluven](https://godotshaders.com/author/kingtoot/)
- Sound Effects:
  - [Game Over, Sword Effects by Pixbay](https://pixabay.com/sound-effects/game-over-arcade-6435/)
  - [Menu Click by Not_Amasingrock](https://pixabay.com/sound-effects/video-game-menu-click-sounds-148373/)
- Engine:
    - [Chrono Icons by Grafixpoint](https://www.flaticon.com/free-icons/chrono)
      - Fsm, State
    - [Bar Chart Icon by Andrean Prabowo](https://www.flaticon.com/free-icons/bar-chart) (flaticon.com)
      - Stats
- https://luizmelo.itch.io/fire-worm
- https://clembod.itch.io/bringer-of-death-free
- https://kyrise.itch.io/kyrises-free-16x16-rpg-icon-pack
- https://ansimuz.itch.io/warped-shooting-fx
- https://shikashipx.itch.io/shikashis-fantasy-icons-pack
