# Use a imagem do .NET SDK para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia os arquivos do projeto
COPY *.csproj ./
RUN dotnet restore

# Instala a ferramenta dotnet-ef
RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

# Copia o restante dos arquivos e faz o publish
COPY . ./
RUN dotnet publish -c Release -o out

# Usa a imagem do runtime do .NET para rodar o app
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Instala o curl
RUN apt-get update && apt-get install -y curl

COPY --from=build /app/out .

# Definindo a variável de ambiente para a porta
ENV ASPNETCORE_URLS="http://+:5011"

# Expondo a porta que o app irá rodar
EXPOSE 5011

# Comando para rodar o servidor da aplicação
CMD ["dotnet", "PhoneBookApi.dll"]
