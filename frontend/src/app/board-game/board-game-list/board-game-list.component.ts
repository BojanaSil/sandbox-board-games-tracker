import { Component } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { BoardGameService } from 'src/app/services/board-game.service';
import { BoardGameEditComponent } from '../board-game-edit/board-game-edit.component';
import { BoardGame } from 'src/app/models/board-game.model';

@Component({
  selector: 'app-board-game-list',
  templateUrl: './board-game-list.component.html',
  styleUrls: ['./board-game-list.component.scss'],
})
export class BoardGameListComponent {
  displayedColumns: string[] = [
    'name',
    'releaseDate',
    'averagePlayingTimeInMinutes',
    'minNumberOfPlayers',
    'maxNumberOfPlayers',
    'difficulty',
    'categories',
    'actions',
  ];
  dataSource: BoardGame[] = [];

  constructor(
    private boardGameService: BoardGameService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.boardGameService.getBoardGames().subscribe((entities) => {
      this.dataSource = entities;
    });
  }

  create() {
    const dialogRef = this.dialog.open(BoardGameEditComponent, {
      width: '400px',
      data: null,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result)
        this.boardGameService.addBoardGame(result).subscribe((result1) => {
          this.ngOnInit;
        });
    });
  }

  edit(entity: BoardGame) {
    const dialogRef = this.dialog.open(BoardGameEditComponent, {
      width: '400px',
      data: entity,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result)
        this.boardGameService.updateBoardGame(result).subscribe((result1) => {
          this.ngOnInit();
        });
    });
  }

  delete(id: string) {
    this.boardGameService.deleteBoardGame(id);
  }
}
