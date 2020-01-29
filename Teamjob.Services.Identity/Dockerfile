#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["TeamJob.Services.Identity/TeamJob.Services.Identity.csproj", "TeamJob.Services.Identity/"]
RUN dotnet restore "TeamJob.Services.Identity/TeamJob.Services.Identity.csproj"
COPY . .
WORKDIR "/src/TeamJob.Services.Identity"
RUN dotnet build "TeamJob.Services.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TeamJob.Services.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TeamJob.Services.Identity.dll"]