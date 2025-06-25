import { Injectable } from '@angular/core';
import { GameLog } from '../models/game-log.model';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class GameLogService {
  constructor(private http: HttpClient) {}

  addGameLog(gameLog: GameLog): Observable<object> {
    return this.http.post('https://localhost:7212/api/GameLog', gameLog);
  }

  updateGameLog(gameLog: GameLog): Observable<object> {
    return this.http.put(
      `https://localhost:7212/api/GameLog/${gameLog.id}`,
      gameLog
    );
  }

  deleteGameLog(id: string): Observable<object> {
    let url = `https://localhost:7212/api/GameLog/${id}`;
    return this.http.delete(url);
  }

  getGameLog(id: string): Observable<GameLog> {
    return this.http.get<GameLog>(`https://localhost:7212/api/GameLog/${id}`);
  }

  getGameLogs(): Observable<GameLog[]>{
    return this.http.get<GameLog[]>('https://localhost:7212/api/GameLog');
  }
}
