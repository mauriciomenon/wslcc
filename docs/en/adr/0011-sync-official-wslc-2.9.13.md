# ADR-0011. Support digests, all-tags, follow-link, and events in wslc 2.9.13

Status: Accepted

## Context

On 2026-09-25 WSL released prerelease 2.9.13. It added the following Docker-compatible features to the 2.9.12 baseline:

1. `image list --digests` adds a DIGEST column. JSON contains the actual digest only with `--digests`; otherwise the value is `<none>`, matching Docker's `needDigest` check.
2. `pull --all-tags` (or `-a`) pulls every tag from a repository. References that already contain a tag or digest are rejected with `tag can't be used with --all-tags/-a`.
3. `container cp --follow-link` (or `-L`) follows symbolic links during copying. By default it still copies the link itself.
4. `events` (alias `system events`) streams events and supports `--since`, `--until`, and repeatable `--filter` with keys `type`, `container`, `image`, `network`, or `event`. It can filter container and network lifecycle events including create, start, stop, destroy, connect, and disconnect.
5. The `CreatedSince` field in JSON lists now uses locale-independent English text.

The CLI also introduced command-scoped global options in preparation for compose. Options such as `--session` must follow the command that declares them.

## Decision

- Upgrade `Microsoft.WSL.Containers` from 2.9.12 to 2.9.13 and update the default version in `scripts/fetch-wslc-sdk.ps1`.
- Add `WslcCapabilities` (`IWslcCapabilities`) to parse and cache `wslc --version`. `ImageDigests`, `PullAllTags`, `CopyFollowLink`, and `Events` require at least 2.9.13. Version 2.9.12 rejects these flags as unrecognized options. Check availability before adding them and call `Invalidate()` when the session or environment changes.
- Add `CreatedSince` and `Digest` to `ImageItem`. Request `--digests` when supported, prefer `CreatedSince` for age, and show the digest when present. Expose `--all-tags` on supported versions.
- Add a `followLink` parameter to `CopyAsync` and append `--follow-link` when supported.
- Add `WslcEventService` (`IWslcEventService`). `QueryAsync` parses historical results into `WslcEventEntry` (timestamp, type, action, target, attributes). `StreamAsync` reports live events using `RunStreamingAsync`. The Events navigation page supports since/until/type/container filters and switches between history and live listening.

## Consequences

The GUI exposes digests, all-tag pulls, symbolic-link copying, and container/network lifecycle events. Version 2.9.12 and earlier disable these controls and offer an upgrade hint. The minimum-version table must be updated if upstream changes flag semantics. Event parsing follows the official end-to-end tests' text format, `<timestamp> <type> <action> <ID> (k=v, ...)`. If upstream changes the format, unrecognized entries may be omitted.

## Test results on 2026-09-27

Tests against 2.9.13 confirmed the official contract:

- `image list --digests --format json` returned real `sha256:` digests. Omitting `--digests` returned `<none>` for `Digest`.
- `pull <tagged-reference> --all-tags` was rejected with `E_INVALIDARG` and the localized message `tag 不能与 --all-tags/-a 一起使用`.
- `container cp -L` copied the target contents (13 bytes). Without `-L` it copied the link (0 bytes).
- Historical `events` and `--filter type=network|container` worked, and the live stream could be cancelled.

### Implementation details confirmed during testing

1. `events` streams indefinitely without `--until`. `QueryAsync` must provide an upper bound. An event in the exact same second as the bound can be missed, so history queries use the current Unix second plus one, as in the upstream end-to-end tests.
2. In 2.9.13, RedirectionGuard rejects traversal through reparse points. If `%LOCALAPPDATA%\wslc` or `storagePath` is a junction or symbolic link, wslc operations fail with `0x800701c0` (untrusted mount point). Set `storagePath` in `settings.yaml` to a real directory using the application's settings page. Ordinary default paths without reparse points are unaffected.

## Follow-up

WSL 3.0.1 reached general availability on 2026-09-29. All features requiring 2.9.13 or later remain available in the stable line. [ADR-0012](0012-sync-official-wslc-3.0.1-ga.md) records the updated upgrade channel.
