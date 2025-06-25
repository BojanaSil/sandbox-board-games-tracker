import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { debounceTime, finalize, Observable, of, switchMap, tap } from 'rxjs';
import { BoardGame } from 'src/app/models/board-game.model';
import { GameLog } from 'src/app/models/game-log.model';
import { BoardGameService } from 'src/app/services/board-game.service';
import { GameLogService } from 'src/app/services/game-log.service';

@Component({
  selector: 'app-game-log-edit',
  templateUrl: './game-log-edit.component.html',
  styleUrls: ['./game-log-edit.component.scss'],
})
export class GameLogEditComponent {
  form: FormGroup;
  results: BoardGame[] = [];
  isLoading = false;
  selectedGame: BoardGame | null = null;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<GameLogEditComponent>,
    private gameLogService: GameLogService,
    private boardGameService: BoardGameService,
    @Inject(MAT_DIALOG_DATA) public data: GameLog
  ) {
    this.form = this.fb.group({
      id: [data?.id || null],
      boardGameName: [data?.boardGameName || '', Validators.required],
      dateOfPlay: [data?.dateOfPlay || '', Validators.required],
      timesPlayed: [data?.timesPlayed || 0, Validators.required],
      numberOfPlayers: [data?.numberOfPlayers || 0, Validators.required],
      averageDuration: [data?.averageDuration || 0, Validators.required],
      winner: [data?.winner || ''],
    });
  }

  ngOnInit() {
    this.form
      .get('boardGameName')
      ?.valueChanges.pipe(
        debounceTime(1000),
        tap(() => (this.isLoading = true)),
        switchMap((value) =>
          this.search(value).pipe(finalize(() => (this.isLoading = false)))
        )
      )
      .subscribe((results) => (this.results = results));
  }

  onSave() {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

  search(query: string): Observable<BoardGame[]> {
    if (!query) {
      return of([]);
    }

    return this.boardGameService.queryBoardGames(query);
  }

  selectResult(result: BoardGame) {
    this.form.get('boardGameId')?.setValue(result.name);
    this.results = [];
    this.selectedGame = result;
  }

  displayFn(boardGameId: string): string {
    const selectedGame = this.results.find((game) => game.id === boardGameId);
    return selectedGame ? selectedGame.name : '';
  }

  getSelectedGameName(): string | undefined {
    // Returns the name of the currently selected game for display
    const boardGameId = this.form.get('boardGameId')?.value;
    return this.selectedGame?.id === boardGameId ? this.selectedGame?.name : '';
  }

}
