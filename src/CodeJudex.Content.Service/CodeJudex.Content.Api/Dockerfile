FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CodeJudex.Content.Api/CodeJudex.Content.Api.csproj", "CodeJudex.Content.Api/"]
COPY ["CodeJudex.Content.Application/CodeJudex.Content.Application.csproj", "CodeJudex.Content.Application/"]
COPY ["CodeJudex.Content.Domain/CodeJudex.Content.Domain.csproj", "CodeJudex.Content.Domain/"]
COPY ["CodeJudex.Content.Infrastructure/CodeJudex.Content.Infrastructure.csproj", "CodeJudex.Content.Infrastructure/"]

RUN dotnet restore "CodeJudex.Content.Api/CodeJudex.Content.Api.csproj"

COPY . .
WORKDIR "/src/CodeJudex.Content.Api"
RUN dotnet build "CodeJudex.Content.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeJudex.Content.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CodeJudex.Content.Api.dll"]