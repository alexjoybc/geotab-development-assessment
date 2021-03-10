# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog][Keep a Changelog] and this project adheres to [Semantic Versioning][Semantic Versioning].

## [Unreleased]


---

## [Released]

---

### Changed

- Removed printer class, for builtin Console.Writeline().
- improved user flow by showing categories when user wants to filter a spcific category
- changed jsonFeed to not be static

### Fixed

- fix code smell, nested statements 6 levels down to 3 levels.
- fix code smell, massive switch statement replaced with builtin c#
- fix bug return only 1 joke
- fix bug with assigning joke category
- fix bug retrieving categories
