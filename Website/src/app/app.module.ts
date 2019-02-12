import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './pages/home/home.component';
import { SeasonStatsComponent } from './pages/season-stats/season-stats.component';
import { CareerStatsComponent } from './pages/career-stats/career-stats.component';
import { PlayerStatsComponent } from './pages/player-stats/player-stats.component';
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
    CareerStatsComponent,
    PlayerStatsComponent,
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
