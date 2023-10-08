ARG SDK_IMAGE=mcr.microsoft.com/dotnet/sdk:7

FROM ${SDK_IMAGE} AS builder

WORKDIR /src

RUN apt-get update && apt-get install -y unzip
RUN curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l /usr/local/bin/vsdbg

# Install NodeJs
RUN curl -sL https://deb.nodesource.com/setup_18.x | bash \
  && apt-get install nodejs

COPY ["Promag.sln", "./"]
COPY ["src/.", "src"]

RUN dotnet restore Promag.sln -nowarn:msb3202,nu1503