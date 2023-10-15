# ProMag - A Project Management Web Application

[![wakatime](https://wakatime.com/badge/github/cowienduckie/promag.svg)](https://wakatime.com/badge/github/cowienduckie/promag)

## Introduction

## Installation and Build

```SHELL
npm install
dotnet build Promag.sln
```

## Docker Compose

From the root directory `promag` of the project, run the following command:

```SHELL
docker compose -f docker-compose.yml -f docker-compose.override.yml -p promag up -d --build
```
