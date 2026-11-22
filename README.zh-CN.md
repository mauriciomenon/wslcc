<div align="center">
  <img src="src/WSLCC.App/Assets/AppIcon.png" alt="WSLCC" width="120" />
  <p><strong>WSLCC | 仅用于翻译的分支</strong></p>
  <h1><a href="https://github.com/iwvw/wslcc">原项目：iwvw/wslcc</a></h1>
  <p>WSLCC 由 iwvw 和原项目的贡献者开发。本分支将界面和文档翻译为巴西葡萄牙语和英语。</p>
</div>

[Portugues (Brasil)](README.md) | [English](README.en.md) | [简体中文](README.zh-CN.md)

## 本分支为何在发布时重新编译三个 SQLite DLL

WSLCC 使用 `Microsoft.Data.Sqlite` 10.0.11 保存界面语言等本地数据。该软件包通过 SQLitePCLRaw 2.1.12 调用 SQLite。检查 NuGet 软件包时，我们在下列三个 DLL 中发现了维护者的构建目录路径。[SQLitePCLRaw 2.1.12 源代码](https://github.com/ericsink/SQLitePCL.raw/tree/v2.1.12)

| 文件 | 用途 |
| --- | --- |
| `SQLitePCLRaw.core.dll` | 提供调用 SQLite 的 .NET 接口。 |
| `SQLitePCLRaw.provider.e_sqlite3.dll` | 将这些调用转发给原生库 `e_sqlite3.dll`。 |
| `SQLitePCLRaw.batteries_v2.dll` | 在应用使用 SQLite 时选择并初始化该提供程序。 |

每个 DLL 的 **CodeView** 调试记录都保存了编译时生成的 `.pdb` 符号文件路径，供调试器查找符号。WSLCC 下载依赖项之前，这些路径就已写入 DLL。用户的数据库和设置与这些构建元数据分开存储。

为避免在发行包中公开个人构建路径，本分支在制作 Windows x64 发行包时，从相同版本的源代码重新编译上述三个 DLL。原生 SQLite 引擎 `e_sqlite3.dll` 仍取自原软件包。发行说明列出了所用源代码和可用于校验下载文件的 SHA-256 哈希值。[本分支的发行版](https://github.com/mauriciomenon/wslcc/releases)

**来源与验证。** 我们使用 SQLitePCLRaw 的 `v2.1.12` 标签，将编译路径映射为中性的 `/_/` 前缀。我们核对了各程序集的名称、程序集版本 `2.1.12.3116`、公钥标识以及公开和受保护 API 的签名。在解压后的 ZIP 中运行 WSLCC，程序打开临时数据库，保存语言设置 `pt-BR`，重启后以葡萄牙语显示。SQLite 对该数据库的完整性检查返回 `ok`。直接从本仓库按常规方式构建时，仍会使用 NuGet 提供的 DLL。[本次构建的源代码提交](https://github.com/ericsink/SQLitePCL.raw/commit/ca835d21508bff43121c65081035840ac5006c4c)

原版 WSLCC 使用 MIT 许可证，版权所有者为 DSUK（2026 年）。重新编译的三个 DLL 使用 SQLitePCLRaw 的 Apache 2.0 许可证。两种许可证均位于发行包的 `licenses` 目录。[WSLCC 许可证](LICENSE) | [SQLitePCLRaw 许可证](https://github.com/ericsink/SQLitePCL.raw/blob/v2.1.12/LICENSE.TXT)

## 界面语言

在本分支中，打开 **设置 > 外观 > 语言**，可选择跟随系统、英语、巴西葡萄牙语或简体中文。完全退出 WSLCC（包括托盘图标）后重新打开即可生效。

参见[原项目文档](https://github.com/iwvw/wslcc/tree/main/docs)。

## 原项目文档

以下各节来自 iwvw/wslcc 的中文 README。[阅读原项目 README](https://github.com/iwvw/wslcc#readme)

WSLCC 是 Windows 上的 WSL 容器（wslc）桌面管理器。基于微软官方 WSL Container API 与 wslc CLI，提供镜像、容器、Compose 编排、卷、日志的图形化管理。

## 特性

- 仪表盘：环境状态、资源配额、活动会话（可一键打开会话终端）
- 容器：列表（自动抓取网页图标）、资源统计、启停/重启/删除、打开端口网址、日志（尾行数/时间戳/时间范围）、详情、终端、文件复制（可选跟随符号链接）、创建（含卷/挂载、环境、健康检查、网络别名、资源限制等）
- 编排：Compose 项目部署、编辑、重建、停止、删除；部署进度可视化（分步进度 + 日志），支持复用本地镜像或强制拉取最新
- 镜像：查看（可显示摘要、中间层）、删除、拉取记录、详情、清理未使用、全标签拉取
- 卷：挂载卷管理、清理未使用
- 网络：查看、创建、删除、连接/断开容器、详情、清理未使用
- 事件：容器与网络生命周期事件（历史查询与实时监听）
- 资源清理：容器/镜像/卷/网络一键 prune
- 日志与审计：容器日志查看、操作留痕
- 迷你面板：托盘左键唤出右下角浮层，快速查看容器状态、启停/重启、打开端口、启停 wslc 会话，支持下拉手势收起
- 错误反馈：一键复制诊断信息并跳转 GitHub 新建 issue
- 设置：会话（写入官方 wslc settings.yaml）、镜像加速、外观（主题色 / 背景材质）、启动时最小化到托盘，采用 Windows 11 设置式分组卡片
- 内存优化：隐藏到托盘后释放主窗口页面，工作集可降至约 40 MB

## 环境要求

- Windows 10/11（x64）
- [WSL 容器（wslc）](https://github.com/MicrosoftDocs/wsl/blob/main/WSL/wsl-container.md)（微软官方 WSL 容器 CLI，现已正式发布，需 `wsl --update`）
- .NET 10 SDK（开发构建）
- 分离版额外需要：.NET 10 桌面运行时 与 Windows App Runtime 2.4（合并版无需）

## 构建

```powershell
powershell -ExecutionPolicy Bypass -File build.ps1
```

首次构建会自动下载 `Microsoft.WSL.Containers` SDK 包到 `nuget-packages/`（该包仅随 microsoft/WSL 的 GitHub Release 发布，不在 nuget.org）。也可手动执行 `scripts/fetch-wslc-sdk.ps1`。

## 打包发布

提供两种发布模式：

- **合并版（自包含）**：内置 .NET 运行时，开箱即用，体积较大。
- **分离版（框架依赖）**：不含运行时，体积小，需系统已安装 .NET 10 桌面运行时与 Windows App Runtime 2.4。

```powershell
# 合并版（默认）
powershell -ExecutionPolicy Bypass -File publish.ps1

# 分离版（框架依赖）
powershell -ExecutionPolicy Bypass -File publish.ps1 -Framework
```

产物为自包含/框架依赖单目录 zip，解压即可运行。加 `-InnoSetup` 可一并生成对应安装器。

推送 `v*` 标签会触发 GitHub Actions 自动构建并创建 Release，产物包括：

- 合并版：`WSLCC-win-x64.zip`、`WSLCC-Setup.exe`
- 分离版：`WSLCC-win-x64-framework.zip`、`WSLCC-Setup-framework.exe`

应用内置自更新流程，检测到新版本后按当前安装模式下载对应安装包并自动完成替换升级。

## 贡献者

- [lemonno2333](https://github.com/lemonno2333) - 应用图标设计
- [DSUK](https://github.com/iwvw) - 项目开发

## 友情链接

- [Linux.do](https://linux.do) - 技术社区

## 许可证

本项目采用 [MIT License](LICENSE)。
