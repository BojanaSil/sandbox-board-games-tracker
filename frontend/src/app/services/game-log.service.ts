import { Injectable } from '@angular/core';
import { GameLog } from '../models/game-log.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GameLogService {
  private baseUrl: string = "http://localhost:5001/api/GameLog";
  constructor(private http: HttpClient) {}

  addGameLog(gameLog: GameLog): Observable<object> {
    return this.http.post(this.baseUrl, gameLog);
  }

  updateGameLog(gameLog: GameLog): Observable<object> {
    return this.http.put(
      `${this.baseUrl}/${gameLog.id}`,
      gameLog
    );
  }

  deleteGameLog(id: string): Observable<object> {
    let url = `${this.baseUrl}/${id}`;
    return this.http.delete(url);
  }

  getGameLog(id: string): Observable<GameLog> {
    return this.http.get<GameLog>(`${this.baseUrl}/${id}`);
  }

  getGameLogs(): Observable<GameLog[]>{
    return this.http.get<GameLog[]>(`${this.baseUrl}`);
  }
}
