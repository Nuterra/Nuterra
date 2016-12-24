[License]: https://tldrlegal.com/l/mit/
[Documentation]: https://github.com/maritaria/terratech-mod/doc/
[Issues]: https://github.com/maritaria/terratech-mod/issues/
[Releases]: https://github.com/maritaria/terratech-mod/releases/latest/
[Terra Tech]: https://terratechgame.com/
[dnSpy]: https://github.com/0xd4d/dnSpy/

# Maritaria's Mod for [Terra Tech]
[![License](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)][License]
[![Downloads for All Releases](https://img.shields.io/github/downloads/maritaria/terratech-mod/total.svg)][Releases]

# Installation
Check the [Releases] page for the latest version of the mod. Every distrubution will include a copy of seperate install instructions for that particular release. In general, the installation process should be something like:

1. Unpack the release package
2. Replace /TerraTechWin64_Data/Managed/Assembly-CSharp.dll with the same-named .dll in the package.
3. Run the game

# Tools
The main tool used to develop the mod is [dnSpy], a tool to decompile and edit managed assemblies. All modifications to the game are collected in the Maritaria namespace, merged into the original assembly and finally the original code is redirected to pass through the mod code. The documentation includes instructions about usage of [dnSpy]

# Roadmap
## V1.0
- Create modding API without references to original Assembly-CSharp.dll
- Load mods in such a way that they are hot-swappable
- Mechanism for registering new blocks

## V2.0
- Robust save system to allow loading worlds with missing mods
