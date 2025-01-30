import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BoardGameListComponent } from './board-game/board-game-list/board-game-list.component';

const routes: Routes = [
  { path: 'table', component: BoardGameListComponent },
  { path: '', redirectTo: '/table', pathMatch: 'full' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
