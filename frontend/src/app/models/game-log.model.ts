import { BoardGame } from "./board-game.model";

export interface GameLog{
  id: string;
  boardGameName: string;
  boardGame: BoardGame;
  dateOfPlay: string;
  timesPlayed: number;
  numberOfPlayers: number;
  winner: string;
  averageDuration: number;
}
