# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog][Keep a Changelog] and this project adheres to [Semantic Versioning][Semantic Versioning].

## [Unreleased]


---

## [Released]

---

### Added


- Abstraction: supports new INameGen, for implementing random name generators.
- Abstraction: supports new IJokeGen, for implementing joke generators.
- Validation: duplicate detections on jokes
- Validation: add validation on number of jokes to generate
- UX: possibility to exit the application
- UX: user feedback loop on user exit
- UX: new Application Banner

### Changed

- Improved instructions to start the program.
- improved user flow by showing categories when user wants to filter a spcific category
- changed jsonFeed to not be static

### Fixed

- fix issue with replace multiple main characters
- fix integer conversion for number of jokes
- fix code smell, 3 nested statements down to 2 levels.
- fix code smell, nested statements 6 levels down to 3 levels.
- fix code smell, massive switch statement replaced with builtin c#
- fix bug return only 1 joke
- fix bug with assigning joke category
- fix bug retrieving categories

### Removed

- removed json Feed
- removed Console Printer, for builtin Console.Writeline().