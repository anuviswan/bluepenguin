\# Copilot Instructions – .NET 10 Web API (Azure Tables + Azure Blobs)



\## Platform



\- Target framework: .NET 10

\- Use latest C# version

\- Nullable enabled

\- Implicit usings enabled

\- All I/O must be async/await

\- Use Controllers by default (do not generate minimal APIs unless explicitly requested)



---



\## Architecture



Follow Clean Architecture principles.



Layers:

\- API (Controllers)

\- Application (Use cases / Services)

\- Domain (Entities / Business rules)

\- Infrastructure (Azure Tables and Blob implementations)



Rules:

\- Controllers must be thin.

\- No business logic inside Controllers.

\- Application layer contains business rules.

\- Infrastructure layer handles Azure SDK interactions.

\- Controllers must not directly use Azure SDK clients.

\- Use dependency injection for all services.

\- No static service access.



---



\## Azure Table Storage



SDK:

\- Use Azure.Data.Tables



Entity Requirements:

\- Must include PartitionKey

\- Must include RowKey

\- Must include Timestamp

\- Must include ETag



Partitioning:

\- PartitionKey must reflect primary query pattern.

\- Avoid cross-partition queries.

\- RowKey must be deterministic and unique within partition.



Design Rules:

\- Use Repository pattern for table access.

\- Do not expose TableEntity outside Infrastructure.

\- Map Table entities to Domain models.

\- Handle RequestFailedException and convert to application-level errors.



---



\## Azure Blob Storage



SDK:

\- Use Azure.Storage.Blobs



Rules:

\- Inject BlobContainerClient via DI.

\- Do not create containers dynamically unless explicitly required.

\- Validate file size and content type before upload.

\- Always set correct ContentType metadata.

\- Use streaming uploads (avoid loading full file into memory).



Blob Naming Convention:

{entityType}/{entityId}/{guid}.{extension}



---



\## DTO and Mapping



\- API must use DTOs.

\- Domain models must not contain storage concerns.

\- Use manual mapping.

\- Validate DTOs with FluentValidation.

\- Do not expose internal domain entities in API responses.



---



\## Error Handling



\- Do not throw exceptions for expected domain errors.

\- Use Result<T> pattern for application responses.

\- Controllers convert Result<T> to proper HTTP responses.

\- Implement global exception handling middleware.



---



\## Logging



\- Use ILogger<T>.

\- Log warnings for recoverable issues.

\- Log errors for unexpected failures.

\- Never log sensitive data.



---



\## Security



\- Use Azure AD authentication unless specified otherwise.

\- Validate all inputs.

\- Never store secrets in code.

\- Use environment variables or Azure Key Vault.



---



\## Configuration



\- Use strongly typed Options pattern.

\- Do not access IConfiguration directly inside services.

\- Secrets must come from secure configuration sources.



---



\## Testing



\- Application layer must be testable without Azure dependencies.

\- Use interfaces for repositories and blob services.

\- Do not mock concrete Azure SDK classes.

\- Prefer in-memory fakes for unit tests.



---



\## Code Style



\- Prefer clarity over cleverness.

\- Avoid complex LINQ chains.

\- Keep methods concise and readable.

\- Use meaningful names.

\- No synchronous I/O.



---



\## Forbidden Patterns



\- No Azure SDK usage inside Controllers.

\- No business logic in Infrastructure.

\- No swallowing exceptions.

\- No direct access to TableClient outside Infrastructure.

\- No blob uploads without validation.



