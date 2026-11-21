# ADR-0006. Accept alternative field names when parsing CLI JSON output

Status: Accepted

## Context

The CLI uses Docker-style output. With `--format json`, each line contains one JSON object. An empty result produces no output or an empty table. Preview releases have changed the fields. For example, 2.9.9 expanded `volume list --format json` from two fields to ten Docker-compatible fields. Some commands (container/volume list) return E_UNEXPECTED in the current environment, so local samples of their actual fields are unavailable.

## Decision

- `WslcRunner.RunJsonLinesAsync` reads object-shaped lines beginning with `{`, parses them using JsonDocument, skips malformed lines, and returns a stream of JsonElement values.
- Try candidate names in order, including `Names` for a container name, `ID`/`Id` for its identifier, and alternatives for the image. Use defaults such as `"-"`, `""`, or `<none>` when missing.
- Combine stdout and stderr. Recognize `错误代码： X` and `Error code: X`, including the localized catastrophic failure message `灾难性故障`. Map the code to `WslcCliException.WslcErrorCode`.
- For pull, run, and logs, pass each progress line to a callback and show the raw text. Changes in the progress structure have no effect on the display.

## Consequences

Field changes do not break the entire list. Error codes remain readable and progress reporting is robust. Some fields, such as sizes, are text with limited display precision. Actual output samples can inform future refinements.
