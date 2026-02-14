# Blue Penguin API .NET 8.0 Downgrade Tasks

## Overview

This document tracks the downgrade of the Blue Penguin API solution from .NET 9/10 to .NET 8.0. All project `TargetFramework` properties and package references will be updated in a single coordinated operation, followed by automated test execution and validation.

**Progress**: 2/3 tasks complete (67%) ![0%](https://progress-bar.xyz/67)

---

## Tasks

### [✓] TASK-001: Verify prerequisites *(Completed: 2026-02-14 13:13)*
**References**: Plan §Pre-Upgrade Preparation, Plan §Source Control Strategy

- [✓] (1) Verify required .NET SDK (net8.0) is installed per Plan §Pre-Upgrade Preparation
- [✓] (2) Runtime/SDK version meets minimum requirements (**Verify**)
- [✓] (3) If a `global.json` file is present, verify or update its SDK entry to a compatible net8.0 SDK per Plan §Pre-Upgrade Preparation
- [✓] (4) Verify configuration files (Directory.Build.props, Directory.Packages.props) are compatible with net8.0 or note required changes per Plan §Package Update Reference (**Verify**)

---

### [✓] TASK-002: Atomic framework and dependency downgrade with compilation fixes *(Completed: 2026-02-14 13:24)*
**References**: Plan §Project-by-Project Migration Plans, Plan §Package Update Reference, Plan §Breaking Changes Catalog

- [✓] (1) Update `TargetFramework` to `net8.0` in all projects listed in Plan §Project-by-Project Migration Plans (e.g., `BP.Shared.csproj`, `BP.Api.ServiceDefaults.csproj`, `BP.Domain.csproj`, `BP.Application.Interfaces.csproj`, `BP.Application.csproj`, `BP.Infrastructure.csproj`, `BP.Api.csproj`, `BP.Api.AppHost.csproj`, `BP.Api.Tests.csproj`)
- [✓] (2) All project files updated to `net8.0` (**Verify**)
- [✓] (3) Update dependency references as required per Plan §Package Update Reference (note: Plan indicates no package version changes required; still validate references)
- [✓] (4) Restore dependencies (`dotnet restore`) and ensure all packages restore successfully (**Verify**)
- [✓] (5) Build the full solution to identify compilation issues: `dotnet build BP.Api.sln --configuration Release` (reference Plan §Testing & Validation Strategy)
- [✓] (6) Fix all compilation errors found (follow guidance in Plan §Breaking Changes Catalog)
- [✓] (7) Rebuild the solution to verify fixes: `dotnet build BP.Api.sln --configuration Release`
- [✓] (8) Solution builds with 0 errors (**Verify**)
- [✓] (9) Commit changes with message: "TASK-002: Downgrade all projects to net8.0 and update packages" (per Plan §Source Control Strategy)

---

### [✗] TASK-003: Run full test suite and validate downgrade
**References**: Plan §Testing & Validation Strategy, Plan §Project-by-Project Migration Plans, Plan §Breaking Changes Catalog

- [✗] (1) Run tests in the test project: `dotnet test ./BP.Api.Tests/BP.Api.Tests.csproj --logger "console;verbosity=detailed"` (or run solution tests with `dotnet test BP.Api.sln`) per Plan §Testing & Validation Strategy
- [ ] (2) Fix any test failures (reference common issues in Plan §Breaking Changes Catalog)
- [ ] (3) Re-run tests after fixes: `dotnet test ./BP.Api.Tests/BP.Api.Tests.csproj --logger "console;verbosity=detailed"`
- [ ] (4) All tests pass with 0 failures (**Verify**)
- [ ] (5) Commit test fixes with message: "TASK-003: Complete testing and validation"

---





