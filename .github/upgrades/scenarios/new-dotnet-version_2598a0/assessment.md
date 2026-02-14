# Projects and dependencies analysis

This document provides a comprehensive overview of the projects and their dependencies in the context of upgrading to .NETCoreApp,Version=v8.0.

## Table of Contents

- [Executive Summary](#executive-Summary)
  - [Highlevel Metrics](#highlevel-metrics)
  - [Projects Compatibility](#projects-compatibility)
  - [Package Compatibility](#package-compatibility)
  - [API Compatibility](#api-compatibility)
- [Aggregate NuGet packages details](#aggregate-nuget-packages-details)
- [Top API Migration Challenges](#top-api-migration-challenges)
  - [Technologies and Features](#technologies-and-features)
  - [Most Frequent API Issues](#most-frequent-api-issues)
- [Projects Relationship Graph](#projects-relationship-graph)
- [Project Details](#project-details)

  - [BP.Api.AppHost\BP.Api.AppHost.csproj](#bpapiapphostbpapiapphostcsproj)
  - [BP.Api.ServiceDefaults\BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj)
  - [BP.Api\BP.Api.csproj](#bpapibpapicsproj)
  - [BP.Application.Interfaces\BP.Application.Interfaces.csproj](#bpapplicationinterfacesbpapplicationinterfacescsproj)
  - [BP.Application\BP.Application.csproj](#bpapplicationbpapplicationcsproj)
  - [BP.Domain\BP.Domain.csproj](#bpdomainbpdomaincsproj)
  - [BP.Infrastructure\BP.Infrastructure.csproj](#bpinfrastructurebpinfrastructurecsproj)
  - [BP.Shared\BP.Shared.csproj](#bpsharedbpsharedcsproj)
  - [tests\BP.Api.Tests\BP.Api.Tests.csproj](#testsbpapitestsbpapitestscsproj)


## Executive Summary

### Highlevel Metrics

| Metric | Count | Status |
| :--- | :---: | :--- |
| Total Projects | 9 | 0 require upgrade |
| Total NuGet Packages | 24 | All compatible |
| Total Code Files | 76 |  |
| Total Code Files with Incidents | 0 |  |
| Total Lines of Code | 3250 |  |
| Total Number of Issues | 0 |  |
| Estimated LOC to modify | 0+ | at least 0.0% of codebase |

### Projects Compatibility

| Project | Target Framework | Difficulty | Package Issues | API Issues | Est. LOC Impact | Description |
| :--- | :---: | :---: | :---: | :---: | :---: | :--- |
| [BP.Api.AppHost\BP.Api.AppHost.csproj](#bpapiapphostbpapiapphostcsproj) | net9.0 | ✅ None | 0 | 0 |  | DotNetCoreApp, Sdk Style = True |
| [BP.Api.ServiceDefaults\BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj) | net9.0 | ✅ None | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [BP.Api\BP.Api.csproj](#bpapibpapicsproj) | net10.0 | ✅ None | 0 | 0 |  | AspNetCore, Sdk Style = True |
| [BP.Application.Interfaces\BP.Application.Interfaces.csproj](#bpapplicationinterfacesbpapplicationinterfacescsproj) | net10.0 | ✅ None | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [BP.Application\BP.Application.csproj](#bpapplicationbpapplicationcsproj) | net10.0 | ✅ None | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [BP.Domain\BP.Domain.csproj](#bpdomainbpdomaincsproj) | net10.0 | ✅ None | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [BP.Infrastructure\BP.Infrastructure.csproj](#bpinfrastructurebpinfrastructurecsproj) | net10.0 | ✅ None | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [BP.Shared\BP.Shared.csproj](#bpsharedbpsharedcsproj) | net10.0 | ✅ None | 0 | 0 |  | ClassLibrary, Sdk Style = True |
| [tests\BP.Api.Tests\BP.Api.Tests.csproj](#testsbpapitestsbpapitestscsproj) | net10.0 | ✅ None | 0 | 0 |  | ClassLibrary, Sdk Style = True |

### Package Compatibility

| Status | Count | Percentage |
| :--- | :---: | :---: |
| ✅ Compatible | 24 | 100.0% |
| ⚠️ Incompatible | 0 | 0.0% |
| 🔄 Upgrade Recommended | 0 | 0.0% |
| ***Total NuGet Packages*** | ***24*** | ***100%*** |

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

## Aggregate NuGet packages details

| Package | Current Version | Suggested Version | Projects | Description |
| :--- | :---: | :---: | :--- | :--- |
| Aspire.Hosting.AppHost | 9.2.1 |  | [BP.Api.AppHost.csproj](#bpapiapphostbpapiapphostcsproj) | ✅Compatible |
| Aspire.Hosting.Azure.Storage | 9.4.1 |  | [BP.Api.AppHost.csproj](#bpapiapphostbpapiapphostcsproj) | ✅Compatible |
| Azure.Data.Tables | 12.11.0 |  | [BP.Domain.csproj](#bpdomainbpdomaincsproj) | ✅Compatible |
| Azure.Storage.Blobs | 12.24.1 |  | [BP.Infrastructure.csproj](#bpinfrastructurebpinfrastructurecsproj) | ✅Compatible |
| Microsoft.AspNetCore.Authentication.JwtBearer | 9.0.8 |  | [BP.Api.csproj](#bpapibpapicsproj) | ✅Compatible |
| Microsoft.AspNetCore.Cryptography.KeyDerivation | 9.0.8 |  | [BP.Application.csproj](#bpapplicationbpapplicationcsproj) | ✅Compatible |
| Microsoft.AspNetCore.Http | 2.2.2 |  | [BP.Api.Tests.csproj](#testsbpapitestsbpapitestscsproj) | ✅Compatible |
| Microsoft.AspNetCore.Mvc.Core | 2.2.5 |  | [BP.Api.Tests.csproj](#testsbpapitestsbpapitestscsproj) | ✅Compatible |
| Microsoft.AspNetCore.OpenApi | 9.0.8 |  | [BP.Api.csproj](#bpapibpapicsproj) | ✅Compatible |
| Microsoft.Extensions.DependencyInjection | 10.0.0-preview.7.25380.108 |  | [BP.Api.csproj](#bpapibpapicsproj)<br/>[BP.Application.csproj](#bpapplicationbpapplicationcsproj)<br/>[BP.Infrastructure.csproj](#bpinfrastructurebpinfrastructurecsproj) | ✅Compatible |
| Microsoft.Extensions.Http.Resilience | 9.4.0 |  | [BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj) | ✅Compatible |
| Microsoft.Extensions.Options | 9.0.4 |  | [BP.Application.csproj](#bpapplicationbpapplicationcsproj) | ✅Compatible |
| Microsoft.Extensions.ServiceDiscovery | 9.2.1 |  | [BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj) | ✅Compatible |
| Microsoft.IdentityModel.Tokens | 8.14.0 |  | [BP.Application.csproj](#bpapplicationbpapplicationcsproj) | ✅Compatible |
| Moq | 4.18.4 |  | [BP.Api.Tests.csproj](#testsbpapitestsbpapitestscsproj) | ✅Compatible |
| OpenTelemetry.Exporter.OpenTelemetryProtocol | 1.11.2 |  | [BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj) | ✅Compatible |
| OpenTelemetry.Extensions.Hosting | 1.11.2 |  | [BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj) | ✅Compatible |
| OpenTelemetry.Instrumentation.AspNetCore | 1.11.1 |  | [BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj) | ✅Compatible |
| OpenTelemetry.Instrumentation.Http | 1.11.1 |  | [BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj) | ✅Compatible |
| OpenTelemetry.Instrumentation.Runtime | 1.11.1 |  | [BP.Api.ServiceDefaults.csproj](#bpapiservicedefaultsbpapiservicedefaultscsproj) | ✅Compatible |
| Swashbuckle.AspNetCore | 9.0.4 |  | [BP.Api.csproj](#bpapibpapicsproj) | ✅Compatible |
| System.IdentityModel.Tokens.Jwt | 8.14.0 |  | [BP.Application.csproj](#bpapplicationbpapplicationcsproj) | ✅Compatible |
| xunit | 2.6.2 |  | [BP.Api.Tests.csproj](#testsbpapitestsbpapitestscsproj) | ✅Compatible |
| xunit.runner.visualstudio | 2.6.2 |  | [BP.Api.Tests.csproj](#testsbpapitestsbpapitestscsproj) | ✅Compatible |

## Top API Migration Challenges

### Technologies and Features

| Technology | Issues | Percentage | Migration Path |
| :--- | :---: | :---: | :--- |

### Most Frequent API Issues

| API | Count | Percentage | Category |
| :--- | :---: | :---: | :--- |

## Projects Relationship Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart LR
    P1["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
    P2["<b>📦&nbsp;BP.Api.AppHost.csproj</b><br/><small>net9.0</small>"]
    P3["<b>📦&nbsp;BP.Api.ServiceDefaults.csproj</b><br/><small>net9.0</small>"]
    P4["<b>📦&nbsp;BP.Domain.csproj</b><br/><small>net10.0</small>"]
    P5["<b>📦&nbsp;BP.Infrastructure.csproj</b><br/><small>net10.0</small>"]
    P6["<b>📦&nbsp;BP.Application.csproj</b><br/><small>net10.0</small>"]
    P7["<b>📦&nbsp;BP.Application.Interfaces.csproj</b><br/><small>net10.0</small>"]
    P8["<b>📦&nbsp;BP.Shared.csproj</b><br/><small>net10.0</small>"]
    P9["<b>📦&nbsp;BP.Api.Tests.csproj</b><br/><small>net10.0</small>"]
    P1 --> P4
    P1 --> P3
    P1 --> P7
    P1 --> P5
    P1 --> P6
    P2 --> P1
    P4 --> P8
    P5 --> P4
    P6 --> P7
    P7 --> P4
    P9 --> P1
    click P1 "#bpapibpapicsproj"
    click P2 "#bpapiapphostbpapiapphostcsproj"
    click P3 "#bpapiservicedefaultsbpapiservicedefaultscsproj"
    click P4 "#bpdomainbpdomaincsproj"
    click P5 "#bpinfrastructurebpinfrastructurecsproj"
    click P6 "#bpapplicationbpapplicationcsproj"
    click P7 "#bpapplicationinterfacesbpapplicationinterfacescsproj"
    click P8 "#bpsharedbpsharedcsproj"
    click P9 "#testsbpapitestsbpapitestscsproj"

```

## Project Details

<a id="bpapiapphostbpapiapphostcsproj"></a>
### BP.Api.AppHost\BP.Api.AppHost.csproj

#### Project Info

- **Current Target Framework:** net9.0✅
- **SDK-style**: True
- **Project Kind:** DotNetCoreApp
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 1
- **Lines of Code**: 21
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["BP.Api.AppHost.csproj"]
        MAIN["<b>📦&nbsp;BP.Api.AppHost.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#bpapiapphostbpapiapphostcsproj"
    end
    subgraph downstream["Dependencies (1"]
        P1["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
        click P1 "#bpapibpapicsproj"
    end
    MAIN --> P1

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="bpapiservicedefaultsbpapiservicedefaultscsproj"></a>
### BP.Api.ServiceDefaults\BP.Api.ServiceDefaults.csproj

#### Project Info

- **Current Target Framework:** net9.0✅
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 1
- **Lines of Code**: 126
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P1["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
        click P1 "#bpapibpapicsproj"
    end
    subgraph current["BP.Api.ServiceDefaults.csproj"]
        MAIN["<b>📦&nbsp;BP.Api.ServiceDefaults.csproj</b><br/><small>net9.0</small>"]
        click MAIN "#bpapiservicedefaultsbpapiservicedefaultscsproj"
    end
    P1 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="bpapibpapicsproj"></a>
### BP.Api\BP.Api.csproj

#### Project Info

- **Current Target Framework:** net10.0✅
- **SDK-style**: True
- **Project Kind:** AspNetCore
- **Dependencies**: 5
- **Dependants**: 2
- **Number of Files**: 27
- **Lines of Code**: 1788
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P2["<b>📦&nbsp;BP.Api.AppHost.csproj</b><br/><small>net9.0</small>"]
        P9["<b>📦&nbsp;BP.Api.Tests.csproj</b><br/><small>net10.0</small>"]
        click P2 "#bpapiapphostbpapiapphostcsproj"
        click P9 "#testsbpapitestsbpapitestscsproj"
    end
    subgraph current["BP.Api.csproj"]
        MAIN["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
        click MAIN "#bpapibpapicsproj"
    end
    subgraph downstream["Dependencies (5"]
        P4["<b>📦&nbsp;BP.Domain.csproj</b><br/><small>net10.0</small>"]
        P3["<b>📦&nbsp;BP.Api.ServiceDefaults.csproj</b><br/><small>net9.0</small>"]
        P7["<b>📦&nbsp;BP.Application.Interfaces.csproj</b><br/><small>net10.0</small>"]
        P5["<b>📦&nbsp;BP.Infrastructure.csproj</b><br/><small>net10.0</small>"]
        P6["<b>📦&nbsp;BP.Application.csproj</b><br/><small>net10.0</small>"]
        click P4 "#bpdomainbpdomaincsproj"
        click P3 "#bpapiservicedefaultsbpapiservicedefaultscsproj"
        click P7 "#bpapplicationinterfacesbpapplicationinterfacescsproj"
        click P5 "#bpinfrastructurebpinfrastructurecsproj"
        click P6 "#bpapplicationbpapplicationcsproj"
    end
    P2 --> MAIN
    P9 --> MAIN
    MAIN --> P4
    MAIN --> P3
    MAIN --> P7
    MAIN --> P5
    MAIN --> P6

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="bpapplicationinterfacesbpapplicationinterfacescsproj"></a>
### BP.Application.Interfaces\BP.Application.Interfaces.csproj

#### Project Info

- **Current Target Framework:** net10.0✅
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 2
- **Number of Files**: 16
- **Lines of Code**: 220
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (2)"]
        P1["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
        P6["<b>📦&nbsp;BP.Application.csproj</b><br/><small>net10.0</small>"]
        click P1 "#bpapibpapicsproj"
        click P6 "#bpapplicationbpapplicationcsproj"
    end
    subgraph current["BP.Application.Interfaces.csproj"]
        MAIN["<b>📦&nbsp;BP.Application.Interfaces.csproj</b><br/><small>net10.0</small>"]
        click MAIN "#bpapplicationinterfacesbpapplicationinterfacescsproj"
    end
    subgraph downstream["Dependencies (1"]
        P4["<b>📦&nbsp;BP.Domain.csproj</b><br/><small>net10.0</small>"]
        click P4 "#bpdomainbpdomaincsproj"
    end
    P1 --> MAIN
    P6 --> MAIN
    MAIN --> P4

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="bpapplicationbpapplicationcsproj"></a>
### BP.Application\BP.Application.csproj

#### Project Info

- **Current Target Framework:** net10.0✅
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 1
- **Number of Files**: 12
- **Lines of Code**: 519
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P1["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
        click P1 "#bpapibpapicsproj"
    end
    subgraph current["BP.Application.csproj"]
        MAIN["<b>📦&nbsp;BP.Application.csproj</b><br/><small>net10.0</small>"]
        click MAIN "#bpapplicationbpapplicationcsproj"
    end
    subgraph downstream["Dependencies (1"]
        P7["<b>📦&nbsp;BP.Application.Interfaces.csproj</b><br/><small>net10.0</small>"]
        click P7 "#bpapplicationinterfacesbpapplicationinterfacescsproj"
    end
    P1 --> MAIN
    MAIN --> P7

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="bpdomainbpdomaincsproj"></a>
### BP.Domain\BP.Domain.csproj

#### Project Info

- **Current Target Framework:** net10.0✅
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 3
- **Number of Files**: 10
- **Lines of Code**: 150
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (3)"]
        P1["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
        P5["<b>📦&nbsp;BP.Infrastructure.csproj</b><br/><small>net10.0</small>"]
        P7["<b>📦&nbsp;BP.Application.Interfaces.csproj</b><br/><small>net10.0</small>"]
        click P1 "#bpapibpapicsproj"
        click P5 "#bpinfrastructurebpinfrastructurecsproj"
        click P7 "#bpapplicationinterfacesbpapplicationinterfacescsproj"
    end
    subgraph current["BP.Domain.csproj"]
        MAIN["<b>📦&nbsp;BP.Domain.csproj</b><br/><small>net10.0</small>"]
        click MAIN "#bpdomainbpdomaincsproj"
    end
    subgraph downstream["Dependencies (1"]
        P8["<b>📦&nbsp;BP.Shared.csproj</b><br/><small>net10.0</small>"]
        click P8 "#bpsharedbpsharedcsproj"
    end
    P1 --> MAIN
    P5 --> MAIN
    P7 --> MAIN
    MAIN --> P8

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="bpinfrastructurebpinfrastructurecsproj"></a>
### BP.Infrastructure\BP.Infrastructure.csproj

#### Project Info

- **Current Target Framework:** net10.0✅
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 1
- **Number of Files**: 7
- **Lines of Code**: 294
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P1["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
        click P1 "#bpapibpapicsproj"
    end
    subgraph current["BP.Infrastructure.csproj"]
        MAIN["<b>📦&nbsp;BP.Infrastructure.csproj</b><br/><small>net10.0</small>"]
        click MAIN "#bpinfrastructurebpinfrastructurecsproj"
    end
    subgraph downstream["Dependencies (1"]
        P4["<b>📦&nbsp;BP.Domain.csproj</b><br/><small>net10.0</small>"]
        click P4 "#bpdomainbpdomaincsproj"
    end
    P1 --> MAIN
    MAIN --> P4

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="bpsharedbpsharedcsproj"></a>
### BP.Shared\BP.Shared.csproj

#### Project Info

- **Current Target Framework:** net10.0✅
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 0
- **Dependants**: 1
- **Number of Files**: 3
- **Lines of Code**: 24
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph upstream["Dependants (1)"]
        P4["<b>📦&nbsp;BP.Domain.csproj</b><br/><small>net10.0</small>"]
        click P4 "#bpdomainbpdomaincsproj"
    end
    subgraph current["BP.Shared.csproj"]
        MAIN["<b>📦&nbsp;BP.Shared.csproj</b><br/><small>net10.0</small>"]
        click MAIN "#bpsharedbpsharedcsproj"
    end
    P4 --> MAIN

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

<a id="testsbpapitestsbpapitestscsproj"></a>
### tests\BP.Api.Tests\BP.Api.Tests.csproj

#### Project Info

- **Current Target Framework:** net10.0✅
- **SDK-style**: True
- **Project Kind:** ClassLibrary
- **Dependencies**: 1
- **Dependants**: 0
- **Number of Files**: 2
- **Lines of Code**: 108
- **Estimated LOC to modify**: 0+ (at least 0.0% of the project)

#### Dependency Graph

Legend:
📦 SDK-style project
⚙️ Classic project

```mermaid
flowchart TB
    subgraph current["BP.Api.Tests.csproj"]
        MAIN["<b>📦&nbsp;BP.Api.Tests.csproj</b><br/><small>net10.0</small>"]
        click MAIN "#testsbpapitestsbpapitestscsproj"
    end
    subgraph downstream["Dependencies (1"]
        P1["<b>📦&nbsp;BP.Api.csproj</b><br/><small>net10.0</small>"]
        click P1 "#bpapibpapicsproj"
    end
    MAIN --> P1

```

### API Compatibility

| Category | Count | Impact |
| :--- | :---: | :--- |
| 🔴 Binary Incompatible | 0 | High - Require code changes |
| 🟡 Source Incompatible | 0 | Medium - Needs re-compilation and potential conflicting API error fixing |
| 🔵 Behavioral change | 0 | Low - Behavioral changes that may require testing at runtime |
| ✅ Compatible | 0 |  |
| ***Total APIs Analyzed*** | ***0*** |  |

