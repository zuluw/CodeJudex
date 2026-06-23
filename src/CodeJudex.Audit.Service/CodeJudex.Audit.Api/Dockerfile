FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CodeJudex.Audit.Api/CodeJudex.Audit.Api.csproj", "CodeJudex.Audit.Api/"]
COPY ["CodeJudex.Audit.Application/CodeJudex.Audit.Application.csproj", "CodeJudex.Audit.Application/"]
COPY ["CodeJudex.Audit.Domain/CodeJudex.Audit.Domain.csproj", "CodeJudex.Audit.Domain/"]
COPY ["CodeJudex.Audit.Infrastructure/CodeJudex.Audit.Infrastructure.csproj", "CodeJudex.Audit.Infrastructure/"]

RUN dotnet restore "CodeJudex.Audit.Api/CodeJudex.Audit.Api.csproj"

COPY . .
WORKDIR "/src/CodeJudex.Audit.Api"
RUN dotnet build "CodeJudex.Audit.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeJudex.Audit.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CodeJudex.Audit.Api.dll"]