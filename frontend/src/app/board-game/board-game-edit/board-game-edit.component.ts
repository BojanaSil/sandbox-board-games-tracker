import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { debounceTime, switchMap, of, Observable, finalize, tap } from 'rxjs';
import { BoardGame } from 'src/app/models/board-game.model';
import { BoardGameService } from 'src/app/services/board-game.service';

@Component({
  selector: 'app-board-game-edit',
  templateUrl: './board-game-edit.component.html',
  styleUrls: ['./board-game-edit.component.scss']
})
export class BoardGameEditComponent {
  form: FormGroup;
  results: string[] = [];
  isLoading = false;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<BoardGameEditComponent>,
    private boardGameService: BoardGameService,
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
    ).subscribe(results => this.results = results)
  }

  onSave() {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value);
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
