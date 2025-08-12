import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs/internal/BehaviorSubject';
import { BoardGame } from '../models/board-game.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class BoardGameService {
  private baseUrl: string = "http://localhost:5001/api/BoardGame";
  constructor(private http: HttpClient) {}

  addBoardGame(boardGame: BoardGame): Observable<object> {
    return this.http.post(this.baseUrl, boardGame);
  }

  updateBoardGame(boardGame: BoardGame): Observable<object> {
    return this.http.put(
      `${this.baseUrl}/${boardGame.id}`,
      boardGame
    );
  }

  deleteBoardGame(id: string): Observable<object> {
    let url = `${this.baseUrl}/${id}`;
    return this.http.delete(url);
  }

  getBoardGame(id: string): Observable<BoardGame> {
    return this.http.get<BoardGame>(`${this.baseUrl}/${id}`);
  }

  getBoardGames(): Observable<BoardGame[]>{
    return this.http.get<BoardGame[]>(this.baseUrl);
  }

  queryBoardGames(name: string): Observable<BoardGame[]>{
    return this.http.get<BoardGame[]>(`${this.baseUrl}/query?name=${name}`);
  }

  getBggBoardGames(name: string): Observable<string[]>{
    return this.http.get<string[]>(`${this.baseUrl}/getBggBoardGames?name=${name}`)
  }
}
