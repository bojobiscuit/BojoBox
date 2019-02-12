import { Component, OnInit, Input } from '@angular/core';
import { PlayerTableRow } from 'src/app/dtos/player-table-row';

@Component({
  selector: 'player-table',
  templateUrl: './player-table.component.html',
  styleUrls: ['./player-table.component.scss']
})
export class PlayerTableComponent implements OnInit {

  @Input() displayType: string;
  @Input() playerTypeId: number;
  @Input() rows: PlayerTableRow[];
  @Input() totals: number[];
  @Input() selectedColumnIndex: number = -1;

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

  skaterHeaders = ['GP', 'G', 'A'];
  goalieHeaders = ['GP', 'W', 'L'];

  constructor() { }

  ngOnInit() {
    this.setViews();
  }

  private setViews() {
    if (this.totals)
      this.viewTotals = true;

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
        this.isPlayer = true;
        this.pages = 1;
        break;
      }
    }

    switch (this.playerTypeId) {
      case 2: {
        this.viewSkater = false;
        this.viewGoalie = true;
        break;
      }
      case 1: 
      default: {
        this.viewSkater = true;
        this.viewGoalie = false;
        break;
      }
    }
  }

  getInnerClass() {
    var classes = "inner ";
    classes += (this.isPlayer) ? 'fixedMarginSeason' : 'fixedMargin';
    return classes;
  }

  checkIndex(i: number) {
    console.log("hit: " + i + " - " + this.selectedColumnIndex);
    return i == this.selectedColumnIndex;
  }

}
