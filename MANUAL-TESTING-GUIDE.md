# Manual Testing Guide - Board Games Tracker

This guide provides step-by-step instructions for manually testing the Board Games Tracker application.

## Prerequisites

Ensure the application is running:
```bash
docker-compose up -d
```

Verify services are accessible:
- Frontend: http://localhost:4300
- Backend API: http://localhost:5001

## 1. UI Testing - Board Games Management

### Test 1.1: View Board Games List
1. Open browser and navigate to http://localhost:4300
2. Click on "Board Games" in the navigation menu
3. **Expected:** Board games list page loads showing existing games (or empty state if no games)

### Test 1.2: Add a New Board Game
1. From the Board Games list page, click "Add Board Game" button
2. Fill in the form:
   - Name: "Ticket to Ride"
   - Release date: 2020
   - Min Players: 2
   - Max Players: 5
   - Minimum age: 10
   - Difficulty: 2.2
   - Playing Time (minutes): 90
   - Category: Family
3. Click "Save"
4. **Expected:** 
   - Popup closes without error
   - After refreshing, new game appears in the list

### Test 1.3: Edit a Board Game
1. From the Board Games list, click "Edit" button next to any game
2. Modify some fields:
   - Change Playing Time to 120
   - Change Max Players to 6
3. Click "Save"
4. **Expected:**
   - Popup closes
   - Changes are reflected in the list

### Test 1.4: Delete a Board Game
1. From the Board Games list, click "Delete" button next to any game
2. Confirm deletion in the dialog (if prompted)
3. **Expected:**
   - After refreshing, game is removed from the list

### Test 1.5: Search from BoardGameGeek API
1. Click "Add Board Game" button
2. Look for "Search BGG" or similar button
3. Enter a game name (e.g., "Catan")
4. Click Search
5. Select a game from results
6. **Expected:**
   - Can save with the name of selected game

## 2. UI Testing - Game Logs

### Test 2.1: View Game Logs
1. Navigate to "Game Logs" from the menu
2. **Expected:** Game logs list page loads showing previous game sessions

### Test 2.2: Add a New Game Log
1. Click "Add Game Log" button
2. Fill in the form:
   - Select a Board Game from dropdown by writing first few characters of game name
   - Date of play: Write today's date
   - Number of times played: 2
   - Number of players: 3
   - Winner: "Jane Doe"
   - Average duration: 75
3. Click "Save"
4. **Expected:**
   - After refresh, new log entry appears in the list

### Test 2.3: Edit a Game Log
1. Click "Edit" next to any game log
2. Change any field
3. Click "Save"
4. **Expected:** Changes are saved and visible in the list after refresh

### Test 2.4: Delete a Game Log
1. Click "Delete" next to any game log
2. Confirm deletion
3. **Expected:** Log entry is removed from the list after refresh


## 4. Validation Testing

### Test 4.1: Required Fields
1. Try to create a board game without filling required fields
2. **Expected:** Game cannot be saved, it border required field that is missing
