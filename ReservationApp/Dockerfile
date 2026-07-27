FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ReservationApp.csproj .
RUN dotnet restore ReservationApp.csproj

COPY . .
RUN dotnet publish ReservationApp.csproj -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
# Reservation hours/dates are real-world wall-clock values for the restaurant's own
# timezone, not the container host's — without this, DateTime.Now/Today default to
# UTC and drift a day off from local "today" for part of every day.
ENV TZ=Europe/Istanbul
EXPOSE 8080

ENTRYPOINT ["dotnet", "ReservationApp.dll"]
