[Unreleased]: https://github.com/maritaria/nuterra/compare/v0.4.0...HEAD
[0.4.0]: https://github.com/maritaria/nuterra/compare/v0.3.1...v0.4.0
[0.3.1]: https://github.com/maritaria/nuterra/compare/v0.3.0...v0.3.1
[0.3.0]: https://github.com/maritaria/nuterra/compare/v0.2.4...v0.3.0
[0.2.4]: https://github.com/maritaria/nuterra/compare/v0.2.3...v0.2.4
[0.2.3]: https://github.com/maritaria/nuterra/compare/v0.2.2...v0.2.3
[0.2.2]: https://github.com/maritaria/nuterra/compare/v0.2.0...v0.2.2
[0.2.0]: https://github.com/maritaria/nuterra/compare/v0.1.0...v0.2.0

# Change Log
All notable changes to this project will be documented in this file.

## [0.4.0]
### Added
- Installer program
- Modloader
- Modding API
- Large cockpit block
- Lookaround support in first person mode
- Firstperson mode no longer exits on activating build beam

### Changed
- Fixed firstperson mode dragging behind on large craft speeds
- Improved customizability of firstperson blocks
- Improved buildscripts for automating modding process through VS
- Split mods into seperate projects

## [0.3.1]
### Added
- Gyro stabalized Hawkeye rotor
- Custom cursor mod
- Cheat block mod (godmode & infinite fuel)
- Support for mods to register custom blocks

### Changed
- Support for 0.7.3
- Upgraded model for the first person block
- Split off build steps from installer to avoid a circular dependency
- Fixed magnet status always showing
- Improved wiki pages

### Removed
- Support for 0.7.2

## [0.3.0]
### Added
- Modloader
- Self destruction key (Gamelynx mod)
- Bacon block
- Unity resource project (/data)
- Installer

### Changed
- Project name to Nuterra
- Code projects now added with build process for injecting into TerraTech
- Existing mods are now loaded using modloader

## [0.2.4]
### Added
- Mechanism for defining new blocktypes in an OO manner.
- Special message to bug reports to flag them as comming from a modded game
- Sylver's mod code
- Resource string and image to smiley block

### Changed
- Moved code to namespaced based folder
- Magnet status now hidden when there are no magnets on your tech

## [0.2.3]
### Fixed
- Survival gamemode not being able to save the game correctly
- Clean assembly for release

## [0.2.2]
### Added
- This CHANGELOG file
- Version identification in output_log.txt
- First person view

### Changed
- Year of LICENSE file

## [0.2.0] - 2017-01-02
### Added
- Added custom 'smiley' block

## 0.1.0 - 2016-12-17
### Added
- Bindable hotkeys for different blocks on techs
- Mobile solar panels
- Initial github upload
- Set license to MIT
