# FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
# WORKDIR /app
# COPY . .
# RUN dotnet publish src/TeamJob.Services.Identity.Api -c release -o out

# FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
# WORKDIR /app
# COPY --from=build /app/out .
# ENV ASPNETCORE_URLS http://*:80
# ENV ASPNETCORE_ENVIRONMENT docker
# ENTRYPOINT dotnet TeamJob.Services.Identity.Api.dll

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["src/TeamJob.Services.Identity.API/TeamJob.Services.Identity.API.csproj", "src/TeamJob.Services.Identity.API/"]
RUN dotnet restore "src/TeamJob.Services.Identity.API/TeamJob.Services.Identity.API.csproj"
COPY . .
WORKDIR "src/TeamJob.Services.Identity.API"
RUN dotnet build "TeamJob.Services.Identity.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeamJob.Services.Identity.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS http://*:80
ENV ASPNETCORE_ENVIRONMENT docker
ENTRYPOINT ["dotnet", "TeamJob.Services.Identity.API.dll"]