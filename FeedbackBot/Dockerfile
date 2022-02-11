FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["FeedbackBot/FeedbackBot.csproj", "FeedbackBot/"]
COPY ["TGUI.CoreLib/TGUI.CoreLib.csproj", "TGUI.CoreLib/"]
RUN dotnet restore "FeedbackBot/FeedbackBot.csproj"
COPY . .
WORKDIR "/src/FeedbackBot"
RUN dotnet build "FeedbackBot.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FeedbackBot.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FeedbackBot.dll"]