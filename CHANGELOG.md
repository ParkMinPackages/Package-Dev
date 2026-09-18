# Changelog

## 2026-09-18

### Added
- Added ParkMinPackages.UGUI.Blur 1.0.0 as a dedicated URP background-blur package and PackageDev submodule.

## 2026-09-15

### Fixed
- Reverted Foundation to 10.1.1, UGUI to 14.0.0, and Workflow.Default to explicit cancellation-token ownership.
- Updated Workflow.Default to 10.0.1 so cancelled timed UI is immediately deactivated.

### Added
- Added Unity Pipeline 0.7.0-exp.1 to the PackageDev development project.

### Changed
- Updated Foundation to 10.2.0 with current-token access for auto-renewing cancellation sources.
- Updated UGUI to 15.0.0 with activation-request ownership, optional cancellation tokens, and replaceable transitions.
- Updated Workflow.Default to 10.1.0 to delegate show/hide cancellation to UGUI and cancel timed displays on newer requests.

## 2026-09-12

### Fixed
- Updated Foundation to 10.1.1 and MediaPipePlugin to 0.2.1 to keep disposal focused on resource cleanup and destruction without explicit playback or enabled-state changes.

## 2026-09-08

### Changed
- Updated UGUI to 14.0.0 with the revised PSD converter UI, explicit font selection, and persistent layer image choices.
- Recorded the development project's text output and image-layer settings.

## 2026-09-07

### Added
- Added Unity PSD Importer 14.0.3 and its resolved dependencies to the PackageDev project.
- Included the PSD Converter font configuration and MediaPipe model registration settings used in the development project.

### Changed
- Updated UGUI to 13.1.0 with the PSD-to-Canvas converter, system font import tools, and current dependency metadata.
- Recorded the released Foundation 10.1.0, MediaPipePlugin 0.2.0, Workflow.Default 10.0.0, and Workflow.Minimap 7.0.0 submodule revisions.

### Removed
- Committed the existing removal of the Assets/Workflow.Default export utilities, sample scenes, and prefab copies. Earlier versions remain recoverable from Git history.
