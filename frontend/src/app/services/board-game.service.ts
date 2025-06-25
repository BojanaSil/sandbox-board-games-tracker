import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { BoardGame } from '../models/board-game.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BoardGameService {
  constructor(private http: HttpClient) {}

  addBoardGame(boardGame: BoardGame): Observable<object> {
    return this.http.post('https://localhost:7212/api/BoardGame', boardGame);
  }

  updateBoardGame(boardGame: BoardGame): Observable<object> {
    return this.http.put(
      `https://localhost:7212/api/BoardGame/${boardGame.id}`,
      boardGame
    );
  }

  deleteBoardGame(id: string): Observable<object> {
    let url = `https://localhost:7212/api/BoardGame/${id}`;
    return this.http.delete(url);
  }

  getBoardGame(id: string): Observable<BoardGame> {
    return this.http.get<BoardGame>(`https://localhost:7212/api/BoardGame/${id}`);
  }

  getBoardGames(): Observable<BoardGame[]>{
    return this.http.get<BoardGame[]>('https://localhost:7212/api/BoardGame');
  }

  queryBoardGames(name: string): Observable<BoardGame[]>{
    return this.http.get<BoardGame[]>(`https://localhost:7212/api/BoardGame/query?name=${name}`);
  }

  getBggBoardGames(name: string): Observable<string[]>{
    return this.http.get<string[]>(`https://localhost:7212/api/BoardGame/getBggBoardGames?name=${name}`)
  }
}
