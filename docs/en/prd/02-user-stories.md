# User stories and scenarios

## 1. User stories

### 1.1 Environment and setup

- US-01. As a Windows developer, I want to see the wslc and kernel versions, missing components and active sessions when I open the app so I can check whether my environment is ready.
- US-02. As a developer, I want the app to show the upgrade command when a component is missing or outdated so I can act on the error.
- US-03. As an enterprise user, I want to know whether the native API is available and which data channel is active so I understand differences in functionality.

### 1.2 Image management

- US-10. As a developer, I want a list of local images with name, tag, size and creation time so I can choose which to run or delete.
- US-11. As a developer, I want to enter an image name, click Pull, see progress and the result in real time, and have the list refresh automatically after a successful pull.
- US-12. As a developer, I want to delete a selected image, confirm before deletion and see the reason if deletion fails.
- US-13. As a developer, I want a readable error when pulling or deleting images fails, for example when the network restricts registry access.

### 1.3 Container management

- US-20. As a developer, I want a container list with name, image, state, ports and uptime so I can distinguish running and stopped containers at a glance.
- US-21. As a developer, I want a form for creating a container with a name, image, port mappings, environment variables, automatic removal and background execution, without writing a command.
- US-22. As a developer, I want a single click to start, stop, restart or remove a selected container and clear feedback on success or failure.
- US-23. As a developer, I want to inspect container logs to diagnose runtime problems.

### 1.4 Session management

- US-30. As a developer, I want to configure and persist the session name, storage path and CPU and memory quotas.
- US-31. As a developer, I want a list of active sessions so I know which VM sessions are running.

### 1.5 Observability and auditing

- US-40. As an administrator, I want every UI operation (pull, create, start, stop and delete) recorded with its time, category, action, result and duration.
- US-41. As an administrator, I want to review image pull history with repository, total amount and outcome.
- US-42. As a developer, I want settings, history and snapshots to survive an app restart.

## 2. End-to-end scenarios

### Scenario A. First environment check

Open the app. The dashboard shows wslc 2.9.10, kernel 6.18, all components present and zero active sessions. Open Images. On the empty list, enter `alpine:latest` and pull. Watch the progress and verify that `alpine:latest` appears after the pull succeeds.

### Scenario B. Run a web service

In Images, verify that nginx exists. In Containers, choose New. Enter `web` as the name, `nginx:latest` as the image and `8080:80` as the port mapping. Run in the background. After creation, the list shows `web` as `running` with `8080:80`. Stop it, confirm that its state changes to `exited`, then delete it.

### Scenario C. Diagnose a failure

Stopping a container fails. The InfoBar shows a readable error with the wslc error code. The dashboard identifies the active data channel. Activity shows the failed operation and its duration.

### Scenario D. Persist settings

In Settings, set the session name to `wslc-prod`, the storage path to `D:\wslc-data` and memory to 8 GB. Restart the app. The settings persist and the new session uses them.

## 3. Constraints

- US-50. As an operator, I want the app to start on networks without direct Docker Hub access. A failed pull should only affect that operation.
- US-51. As a developer, I want the application build to produce no compiler warnings.
