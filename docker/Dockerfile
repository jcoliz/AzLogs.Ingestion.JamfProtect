FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /source

ARG MAIN=WorkerApp

RUN mkdir -p "$MAIN"
COPY "$MAIN/*.csproj" "$MAIN"
WORKDIR "/source/$MAIN"

# This seems to cause issues in this project
#RUN dotnet restore

# Software version number
#   - Should correspond to tag
#   - Including default value so if someone just runs "docker build", it will work
ARG SOLUTION_VERSION=docker

# copy everything else and build app
WORKDIR /source
COPY . .

# Handle config.toml.
# Current setup is to ensure that config.toml is in place before building.
# This is useful locally, but in order to publish it, we'll have to
# set up a volume to a local version.
RUN rm -f "$MAIN/config.toml" || true

# Build app
WORKDIR "/source/$MAIN"
RUN dotnet publish --self-contained false -o /app

FROM mcr.microsoft.com/dotnet/runtime:9.0
WORKDIR /app
COPY --from=build /app .

RUN useradd -ms /bin/bash moduleuser
USER moduleuser

ENTRYPOINT ["dotnet", "WorkerApp.dll"]