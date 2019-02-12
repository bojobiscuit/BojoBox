import { Component, OnInit, Input } from '@angular/core';
import { SkaterRow } from 'src/app/dtos/skater-row';

export const SKATERDATA: SkaterRow[] = [
  { name: 'Bojo Biscuit', rank: 1, team: 'WKP', season: 25, gp: 50, g: 10, a: 35 },
  { name: 'Jasper Clayton', rank: 2, team: 'NEW', season: 37, gp: 50, g: 12, a: 30 },
  { name: 'Alonzo Garbonzo', rank: 3, team: 'MIN', season: 28, gp: 50, g: 15, a: 25 },
  { name: 'Bojo Biscuit', rank: 1, team: 'WKP', season: 25, gp: 50, g: 10, a: 35 },
  { name: 'Jasper Clayton', rank: 2, team: 'NEW', season: 37, gp: 50, g: 12, a: 30 },
  { name: 'Alonzo Garbonzo', rank: 3, team: 'MIN', season: 28, gp: 50, g: 15, a: 25 },
  { name: 'Bojo Biscuit', rank: 1, team: 'WKP', season: 25, gp: 50, g: 10, a: 35 },
  { name: 'Jasper Clayton', rank: 2, team: 'NEW', season: 37, gp: 50, g: 12, a: 30 },
  { name: 'Alonzo Garbonzo', rank: 3, team: 'MIN', season: 28, gp: 50, g: 15, a: 25 },
  { name: 'Bojo Biscuit', rank: 1, team: 'WKP', season: 25, gp: 50, g: 10, a: 35 },
];

@Component({
  selector: 'player-table',
  templateUrl: './player-table.component.html',
  styleUrls: ['./player-table.component.scss']
})
export class PlayerTableComponent implements OnInit {

  skaterData = SKATERDATA;
  @Input() displayType: string;
  @Input() playerType: string;

  viewName: boolean;
  viewRank: boolean;
  viewTeam: boolean;
  viewSeason: boolean;
  viewYears: boolean;
  viewTeams: boolean;
  viewTotals: boolean;
  viewSkater: boolean;
  viewGoalie: boolean;

  pages: number = 5;

  constructor() { }

  ngOnInit() {

    switch (this.displayType) {
      case "season": {
        this.viewName = true;
        this.viewRank = true;
        this.viewTeam = true;
        this.viewSeason = true;
        this.viewTotals = false;
        this.viewYears = false;
        this.viewTeams = false;
        break;
      }
      case "career": {
        this.viewName = true;
        this.viewRank = true;
        this.viewTeam = false;
        this.viewSeason = false;
        this.viewTotals = false;
        this.viewYears = true;
        this.viewTeams = true;
        break;
      }
      case "player": {
        this.viewName = false;
        this.viewRank = false;
        this.viewTeam = true;
        this.viewSeason = true;
        this.viewTotals = true;
        this.viewYears = false;
        this.viewTeams = false;
        this.pages = 1;
        break;
      }
    }

    switch(this.playerType) {
      case "skater": {
        this.viewSkater = true;
        this.viewGoalie = false;
        break;
      }
      case "goalie": {
        this.viewSkater = false;
        this.viewGoalie = true;
        break;
      }
    }
  }

}
