[License]: https://tldrlegal.com/l/mit/
[Documentation]: https://github.com/maritaria/nuterra/doc/
[Issues]: https://github.com/maritaria/nuterra/issues/
[Releases]: https://github.com/maritaria/nuterra/releases/latest/
[Terra Tech]: https://terratechgame.com/
[dnSpy]: https://github.com/0xd4d/dnSpy/

# Nuterra Mod for [Terra Tech]
[![License](http://img.shields.io/badge/license-MIT-blue.svg?style=flat)][License]
[![Downloads for All Releases](https://img.shields.io/github/downloads/maritaria/nuterra/total.svg)][Releases]

# Installation
Check the [Releases] page for the latest version of the mod.

1. Download Nuterra release package (.zip)
2. Unpack into Terra Tech install directory (the one where the executable is located)
3. Run `Nuterra.Installer.exe`
4. Start the game

For more detailed information on installing Nuterra and troubleshooting, see this [wiki page](https://github.com/Nuterra/nuterra/wiki/How-to-install-Nuterra).

# Development
The main tool used to develop the mod is [dnSpy], a tool to decompile and edit managed assemblies. All modifications to the game are collected in the Maritaria namespace, merged into the original assembly and finally the original code is redirected to pass through the mod code. The documentation includes instructions about usage of [dnSpy].

# Roadmap
## 0.4.0 - Mod API, initial release
- [x] Create event/hook system such that hooking will only be used for nuterra code
- [x] Restructure project and refactor into per-feature mods
- [x] Separate mods into individual projects/dlls
- [x] Add loading external dll support to modloader
- [ ] Update wiki with aquired knowledge and tutorials

## 0.4.1 - Mod API, improvements
- [ ] Configuration API
- [ ] Improving build tool to add properties for hidden fields
- [ ] Improving build scripts to copy over mods to local TT install
- [ ] In-game console
- [ ] Console commands API
- [ ] Adding new HUD elements API
- [ ] Mod reloading

## Future works
- [ ] UI/HUD elements
- [ ] World generation
- [ ] Recipies
- [ ] Custom factions
- [ ] Robuust saves
