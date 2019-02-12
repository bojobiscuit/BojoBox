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

  viewName: boolean = false;
  viewRank: boolean = false;
  viewTeam: boolean = false;
  viewSeason: boolean = false;
  viewYears: boolean = false;
  viewTeams: boolean = false;
  viewTotals: boolean = false;
  viewSkater: boolean = false;
  viewGoalie: boolean = false;

  isPlayer: boolean = false;
  isCareer: boolean = false;
  isSeason: boolean = false;

  pages: number = 5;

  constructor() { }

  ngOnInit() {

    switch (this.displayType) {
      case "season": {
        this.viewName = true;
        this.viewRank = true;
        this.viewTeam = true;
        this.viewSeason = true;
        this.isSeason = true;
        break;
      }
      case "career": {
        this.viewName = true;
        this.viewRank = true;
        this.viewYears = true;
        this.viewTeams = true;
        this.isCareer = true;
        break;
      }
      case "player": {
        this.viewTeam = true;
        this.viewSeason = true;
        this.viewTotals = true;
        this.isPlayer = true;
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

  getInnerClass() {
    if (this.displayType == 'player') {
      return 'inner fixedMarginSeason';
    }
    else {
      return 'inner fixedMargin';
    }
  }

}
