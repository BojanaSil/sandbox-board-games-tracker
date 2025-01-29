System Name - Board Games Tracker

Contributors - Bojana Mrdjenovic

Licence - MIT Licence

This is A Sandbox project, created for purposes of learning TDD, following directions from Valentina Jemuovic (https://www.linkedin.com/in/valentinajemuovic) on platform https://journal.optivem.com/

Use cases for this project:

1. Adding new category
  User can add a new category to database, by inputing unique code, name
2. Delete category
   User can delete existing category
3. Browse categories
   User can see a list of created categories, he can filter and sort through that list

4. Adding new board game
  User can add a new board game to database, by inputing unique code, name, year of release, avarage playing time, number of players, minimum age, difficulty on a scale 1-5, list of categories
5. Updating existing board game
   User can update existing board game, every property except code since that can be identifier
6. Delete existing board game
   User can delete existing board game
7. Browse board games
   User can see a list of created board games, he can filter and sort through that list
8. View board game
   User can see details of selected board game

9. Add game log
    User can add game log, by inputing what game was played, on what date, how long it lasted, number of players, name of winner
10. Delete game log
    User can delete existing game log
11. Update game log
    User can update existing game log

12. Get game report
    User can see game report by inputing filter for date interval, category of game etc, and output will be a list of games with number of plays, who was winner most of the time etc...


External System:
- BoardGamesGeek API is used to retrieve board games from their database by inputing a few characters in Name field and selecting one. API: https://boardgamegeek.com

System Architecture Style - Frontend + Monolithic Backend

Tech Stack:
- .NET 8
- EF Core 8
- Angular 15
- SQL Server

Repository Strategy - Mono-Repo approach

Branching Strategy - Feature branching

Deployment Model - Local only
