import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { SeasonStatsComponent } from './pages/season-stats/season-stats.component';
import { SeasonGoalieStatsComponent } from './pages/season-goalie-stats/season-goalie-stats.component';
import { CareerStatsComponent } from './pages/career-stats/career-stats.component';
import { CareerGoalieStatsComponent } from './pages/career-goalie-stats/career-goalie-stats.component';
import { SkaterStatsComponent } from './pages/skater-stats/skater-stats.component';
import { GoalieStatsComponent } from './pages/goalie-stats/goalie-stats.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'season', redirectTo:"season/skater" },
  { path: 'season/skater', component: SeasonStatsComponent },
  { path: 'season/goalie', component: SeasonGoalieStatsComponent },
  { path: 'career', redirectTo:"career/skater" },
  { path: 'career/skater', component: CareerStatsComponent },
  { path: 'career/goalie', component: CareerGoalieStatsComponent },
  { path: 'skater/:id', component: SkaterStatsComponent },
  { path: 'goalie/:id', component: GoalieStatsComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
