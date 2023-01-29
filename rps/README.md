# RPS - By Mike Chambers
👾 Roshambo as a .NET minimal API 👾

## Wait, what?

Specifically, this repo:
- contains a [.NET Minimal API](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-7.0)
- features a swagger page acting as a simple UI
- demonstrates a rudimentary Roshambo (rock/paper/scissors) game
- accepts extensible rule sets
- tracks game results using an in-memory db


## 🤡 Send in the Clones

Want to try it?

- Clone this repo: `git clone https://github.com/cyberchambers/rps_sln.git`
- Load this solution in Visual Studio (or comparable IDE)
- Start the application locally using your IDE's web server (i.e. Visual Studio's [Kestrel web server](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/servers/kestrel?view=aspnetcore-7.0))

## 🤔Additional Details

The swagger UI is relatively self-explanatory. Nonetheless, here are some highlights...
> - The "rules" are a very simple JSON dictionary. It is deserialized and leveraged to play the game and judge the results.
> - There is a key for every possible option of play (i.e. rock or paper or scissors)
> - Each key holds values for victory conditions (i.e. "rock": "paper" or "scissors": "rock")
> - Rules are adjusted by simply adjusting keys and values in the API's POST parameter {rules}
> - There is no format checking, per say, but as a quick and dirty fix `ToLower()` is applied to strings when evaluated.

If you want to play [Rock/Paper/Scissors/Spock/Lizard](http://www.samkass.com/theories/RPSSL.html) use the following rules:

`{"rock":"paper, Spock","paper": "scissors, lizard","scissors": "Spock, rock","Spock": "lizard, paper","lizard": "rock, scissors"}`
