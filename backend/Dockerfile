# Usa a imagem oficial do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copia os arquivos do projeto
COPY . .

# Restaura as dependências e compila o projeto
RUN dotnet restore
RUN dotnet build --no-restore -c Release
RUN dotnet publish -c Release -o /publish

# Usa a imagem do .NET Runtime para execução
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /publish .

# Define a porta padrão
EXPOSE 5000
EXPOSE 5001

# Comando para iniciar a aplicação
ENTRYPOINT ["dotnet", "SisandUserManagement.API.dll"]
