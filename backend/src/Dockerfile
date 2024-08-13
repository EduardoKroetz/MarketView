# Use a imagem oficial do .NET SDK como base
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Defina o diretório de trabalho na imagem
WORKDIR /app

# Copie o arquivo .csproj e restaure as dependências
COPY /*.csproj ./
RUN dotnet restore

# Copie o restante do código do projeto e compile
COPY . ./
RUN dotnet publish -c Release -o out

# Use a imagem do ASP.NET Runtime para rodar a aplicação
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out ./

# Exponha a porta em que a aplicação estará escutando
EXPOSE 80
EXPOSE 443

# Comando de inicialização da aplicação
ENTRYPOINT ["dotnet", "src.dll"]
