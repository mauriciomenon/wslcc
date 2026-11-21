# Release notes template

Choose the template that matches the release. The original project publishes its notes in Simplified Chinese. Use short entries and avoid promotional language.

---

## Feature release

Use this template for releases with new features, behavior changes or breaking changes, such as v2.0.0 or v1.8.0.

```markdown
# vX.Y.Z

Summarize the main change in one or two sentences.

## Added

- Describe one user-facing feature per entry.
- For a new setting, tell the reader where to find it.

## Improvements

- Describe how an existing feature behaved before and after the change.

## Fixes

- Describe the symptom and the conditions that triggered it.

## Breaking changes

- Explain who the change affects and how to migrate.
- Remove this section when there are no breaking changes.

## Installation

- Self-contained edition (ready to run): WSLCC-Setup.exe / WSLCC-win-x64.zip
- Framework-dependent edition (smaller, requires .NET 10 Desktop Runtime and Windows App Runtime 2.4): WSLCC-Setup-framework.exe / WSLCC-win-x64-framework.zip

## Full changelog

- https://github.com/iwvw/wslcc/compare/v<previous-version>...vX.Y.Z
```

---

## Patch release

Use this template for bug fixes and small improvements that preserve compatibility and add no features, such as v1.7.2.

```markdown
# vX.Y.Z

Describe the release in one sentence.

## Changes

- Write one sentence per item, in order of importance.

## Installation

- Self-contained edition: WSLCC-Setup.exe / WSLCC-win-x64.zip
- Framework-dependent edition: WSLCC-Setup-framework.exe / WSLCC-win-x64-framework.zip
```

---

## Conventions

- Use only the version number in the title, such as `v1.7.2`, without a prefix or suffix.
- Match the Git tag exactly.
- Name the self-contained artifacts `WSLCC-Setup.exe` and `WSLCC-win-x64.zip`. Name the framework-dependent artifacts `WSLCC-Setup-framework.exe` and `WSLCC-win-x64-framework.zip`.
- For small patches, keep Installation and omit other empty sections.
- Include the comparison link for feature releases. It is optional for patch releases.
