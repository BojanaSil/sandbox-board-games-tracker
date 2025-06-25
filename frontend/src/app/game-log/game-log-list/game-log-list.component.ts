import { Component } from '@angular/core';
import { GameLog } from 'src/app/models/game-log.model';
import { GameLogService } from 'src/app/services/game-log.service';
import { GameLogEditComponent } from '../game-log-edit/game-log-edit.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-game-log-list',
  templateUrl: './game-log-list.component.html',
  styleUrls: ['./game-log-list.component.scss']
})
export class GameLogListComponent {
  displayedColumns: string[] = [
    'gameName',
    'dateOfPlay',
    'timesPlayed',
    'numberOfPlayers',
    'winner',
    'averageDuration',
    'actions'
  ];
  dataSource: GameLog[] = [];

  constructor(
      private gameLogService: GameLogService,
      private dialog: MatDialog
    ) {}

    ngOnInit() {
      this.gameLogService.getGameLogs().subscribe((entities) => {
        this.dataSource = entities;
      });
    }

    create() {
      const dialogRef = this.dialog.open(GameLogEditComponent, {
        width: '400px',
        data: null,
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result)
          this.gameLogService.addGameLog(result).subscribe((result1) => {
            this.ngOnInit;
          });
      });
    }

    edit(entity: GameLog) {
      const dialogRef = this.dialog.open(GameLogEditComponent, {
        width: '400px',
        data: entity,
      });

      dialogRef.afterClosed().subscribe((result) => {
        if (result)
          this.gameLogService.updateGameLog(result).subscribe((result1) => {
            this.ngOnInit();
          });
      });
    }

    delete(id: string) {
      this.gameLogService.deleteGameLog(id).subscribe((result) => {
        if (result)
          this.ngOnInit();
      });
    }
}
