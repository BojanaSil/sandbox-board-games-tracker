import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BoardGameListComponent } from './board-game/board-game-list/board-game-list.component';
import { GameLogListComponent } from './game-log/game-log-list/game-log-list.component';
import { MenuComponent } from './menu/menu.component';
import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';

const routes: Routes = [
  { path: 'board-games', component: BoardGameListComponent },
  { path: 'home', component: HomeComponent },
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'game-logs', component: GameLogListComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}

