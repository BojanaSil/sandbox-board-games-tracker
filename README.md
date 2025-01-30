**This project is licensed under the terms of MIT license**

# Board Games Tracker

## Contributors
[Bojana Mrdjenovic](https://github.com/BojanaSil)

## Context
This is A Sandbox project, created for purposes of learning TDD, following directions from [Valentina Jemuovic](https://www.linkedin.com/in/valentinajemuovic) via platform [Optivem Journal](https://journal.optivem.com/). Idea is to simulate legacy code in a Sandbox project so that we can learn TDD on legacy code and apply it to real life project.

## Use Cases
Use cases for this project:

**Adding new board game**

  User can add a new board game to database, by inputing unique name (retrieved from BGG API), year of release, avarage playing time, number of players, minimum age, difficulty on a scale 1-5, list of categories
  
**Updating existing board game**

   User can update existing board game, every property except name 
   
**Delete existing board game**

   User can delete existing board game
   
**Browse board games**

   User can see a list of created board games, he can filter and sort through that list
   
**View board game**

   User can see details of selected board game

**Add game log**

   User can add game log, by inputing what game was played, on what date, how long it lasted, number of players, name of winner
   
**Delete game log**

   User can delete existing game log
   
**Update game log**

   User can update existing game log

**Get game report**

   User can see game report by inputing filter for date interval, category of game etc, and output will be a list of games with number of plays, who was winner most of the time etc...


## External System:
  [BoardGamesGeek API](https://boardgamegeek.com) is used to retrieve board games from their database by inputing a few characters in Name field and selecting one.

## System Architecture Style
  Frontend + Monolithic Backend
  ![Architecture diagram](/boardGamesTracker-diagram.png)

## Tech Stack:
- .NET 8
- EF Core 8
- Angular 15
- SQL Server

## Repository Strategy 
  Mono-Repo approach

## Branching Strategy
  Feature branching

## Deployment Model 
  Local only

## Project Board
[Project Board](https://github.com/users/BojanaSil/projects/1)

