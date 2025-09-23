import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { debounceTime, switchMap, of, Observable, finalize, tap } from 'rxjs';
import { BoardGame } from 'src/app/models/board-game.model';
import { BoardGameService } from 'src/app/services/board-game.service';
import { GameLogService } from 'src/app/services/game-log.service';
import { GameLog } from 'src/app/models/game-log.model';

@Component({
  selector: 'app-board-game-edit',
  templateUrl: './board-game-edit.component.html',
  styleUrls: ['./board-game-edit.component.scss']
})
export class BoardGameEditComponent {
  form: FormGroup;
  results: string[] = [];
  isLoading = false;
  hasGameLogs = false;
  isClassicGame = false;
  isFutureRelease = false;
  currentYear = new Date().getFullYear();

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<BoardGameEditComponent>,
    private boardGameService: BoardGameService,
    private gameLogService: GameLogService,
    @Inject(MAT_DIALOG_DATA) public data: BoardGame
  ) {
    this.form = this.fb.group({
      id: [data?.id || null],
      name: [data?.name || '', Validators.required],
      releaseDate: [data?.releaseDate || '', Validators.required],
      averagePlayingTimeInMinutes: [data?.averagePlayingTimeInMinutes || 0, Validators.required],
      minNumberOfPlayers: [data?.minNumberOfPlayers || 0, Validators.required],
      maxNumberOfPlayers: [data?.maxNumberOfPlayers || 0, Validators.required],
      minimumAge: [data?.minimumAge || 0, Validators.required],
      difficulty: [data?.difficulty || 0, Validators.required],
      categories: [data?.categories || ''],
    });
  }

  ngOnInit() {
    this.form.get('name')?.valueChanges.pipe(
      debounceTime(1000),
      tap(() => this.isLoading = true),
      switchMap(value => this.search(value).pipe(
        finalize(() => this.isLoading = false)
      ))
    ).subscribe(results => this.results = results);

    if (this.data?.id) {
      this.checkBusinessRules();
    }
  }

  checkBusinessRules() {
    const releaseDate = this.data.releaseDate;

    // Check if it's a future release
    this.isFutureRelease = releaseDate > this.currentYear;

    // Check if it's a classic game (over 50 years old)
    this.isClassicGame = releaseDate < (this.currentYear - 50) && releaseDate > 0;

    // Check if game has existing game logs
    this.gameLogService.getGameLogsByBoardGameId(this.data.id).subscribe(
      (logs: GameLog[]) => {
        this.hasGameLogs = logs && logs.length > 0;
        this.applyFieldRestrictions();
      },
      (error) => {
        // If endpoint doesn't exist yet, just proceed without restrictions
        console.log('Game logs endpoint not available yet');
        this.applyFieldRestrictions();
      }
    );
  }

  applyFieldRestrictions() {
    if (this.isFutureRelease) {
      // Future releases can't be edited at all
      this.form.disable();
    } else if (this.isClassicGame) {
      // Classic games: lock historical fields
      this.form.get('name')?.disable();
      this.form.get('releaseDate')?.disable();
      this.form.get('minNumberOfPlayers')?.disable();
      this.form.get('maxNumberOfPlayers')?.disable();
    } else if (this.hasGameLogs) {
      // Games with logs: lock player count fields
      this.form.get('minNumberOfPlayers')?.disable();
      this.form.get('maxNumberOfPlayers')?.disable();
    }
  }

  getFieldTooltip(fieldName: string): string {
    if (this.isFutureRelease) {
      return 'Future releases cannot be edited';
    }

    if (this.isClassicGame) {
      const lockedFields = ['name', 'releaseDate', 'minNumberOfPlayers', 'maxNumberOfPlayers'];
      if (lockedFields.includes(fieldName)) {
        return 'This field is locked for classic games to preserve historical accuracy';
      }
    }

    if (this.hasGameLogs) {
      const lockedFields = ['minNumberOfPlayers', 'maxNumberOfPlayers'];
      if (lockedFields.includes(fieldName)) {
        return 'This field is locked because game logs exist for this board game';
      }
    }

    return '';
  }

  onSave() {
    if (this.form.valid) {
      // Get all values including disabled fields
      const formValue = this.form.getRawValue();
      this.dialogRef.close(formValue);
    }
  }

  onCancel() {
    this.dialogRef.close();
  }

  search(query: string): Observable<string[]> {
    if (!query){
      return of([]);
    }

    return this.boardGameService.getBggBoardGames(query);
  }

  selectResult(result: string){
    this.form.get('name')?.setValue(result);
    this.results = [];
  }
}
