FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["CodeJudex.Identity.Api/CodeJudex.Identity.Api.csproj", "CodeJudex.Identity.Api/"]
COPY ["CodeJudex.Identity.Application/CodeJudex.Identity.Application.csproj", "CodeJudex.Identity.Application/"]
COPY ["CodeJudex.Identity.Domain/CodeJudex.Identity.Domain.csproj", "CodeJudex.Identity.Domain/"]
COPY ["CodeJudex.Identity.Infrastructure/CodeJudex.Identity.Infrastructure.csproj", "CodeJudex.Identity.Infrastructure/"]

RUN dotnet restore "CodeJudex.Identity.Api/CodeJudex.Identity.Api.csproj"

COPY . .
WORKDIR "/src/CodeJudex.Identity.Api"
RUN dotnet build "CodeJudex.Identity.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CodeJudex.Identity.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
RUN apt-get update && apt-get install -y libgssapi-krb5-2 && rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "CodeJudex.Identity.Api.dll"]