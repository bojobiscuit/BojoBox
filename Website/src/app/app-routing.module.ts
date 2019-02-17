import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { PlayerStatsComponent } from './pages/player-stats/player-stats.component';
import { CareerStatsComponent } from './pages/career-stats/career-stats.component';
import { SeasonStatsComponent } from './pages/season-stats/season-stats.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'season', component: SeasonStatsComponent },
  { path: 'career', component: CareerStatsComponent },
  { path: 'skater/:id', component: PlayerStatsComponent },
  { path: 'goalie/:id', component: PlayerStatsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
