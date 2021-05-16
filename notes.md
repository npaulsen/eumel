
https://medium.com/@tobyhede/event-sourcing-with-postgresql-28c5e8f211a2


https://dev.to/maxx_don/integration-testing-with-ef-core-part-1-1l40

https://github.com/mrts/docker-postgresql-multiple-databases


# recreate db:
remove migrations,
`dotnet ef database drop -f -v
dotnet ef migrations add Initial
dotnet ef database update`