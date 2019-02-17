import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { SeasonStatsComponent } from './pages/season-stats/season-stats.component';
import { SeasonGoalieStatsComponent } from './pages/season-goalie-stats/season-goalie-stats.component';
import { CareerStatsComponent } from './pages/career-stats/career-stats.component';
import { CareerGoalieStatsComponent } from './pages/career-goalie-stats/career-goalie-stats.component';
import { SkaterStatsComponent } from './pages/skater-stats/skater-stats.component';
import { GoalieStatsComponent } from './pages/goalie-stats/goalie-stats.component';
import { NavbarComponent } from './common/navbar/navbar.component';
import { PlayerTableComponent } from './common/player-table/player-table.component';
import { PlayerTableNavComponent } from './common/player-table-nav/player-table-nav.component';
import { PaginationComponent } from './common/pagination/pagination.component';
import { FooterComponent } from './common/footer/footer.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    SeasonStatsComponent,
    SeasonGoalieStatsComponent,
    CareerStatsComponent,
    CareerGoalieStatsComponent,
    SkaterStatsComponent,
    GoalieStatsComponent,
    NavbarComponent,
    PlayerTableComponent,
    PlayerTableNavComponent,
    PaginationComponent,
    FooterComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
