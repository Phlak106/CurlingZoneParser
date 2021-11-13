# CurlingZoneParser

Parses linescores from CurlingZone and aggregates them

Get the event ID from curling zone: E.g. 7170 here: https://www.curlingzone.com/event.php?eventid=7170&view=Scores&showdrawid=20

Determine how many draws to aggregate through. (20 in the above link)

Run:
```
/>dotnet run <eventId> <numDraws>
```