import { Component, OnInit, Input, Output } from '@angular/core';
import { PlayerTableRow } from 'src/app/dtos/player-table-row';
import { EventEmitter } from 'events';
import { HelpService } from 'src/app/services/help.service';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { StatTable } from 'src/app/dtos/stat-table';

@Component({
  selector: 'player-table',
  templateUrl: './player-table.component.html',
  styleUrls: ['./player-table.component.scss']
})
export class PlayerTableComponent implements OnInit {

  @Input() displayType: string;
  @Input() playerType: string;
  @Input() rows: PlayerTableRow[];
  @Input() totals: number[];
  @Input() isTeam: boolean;
  @Input() selectedColumnIndex: number = -1;
  @Input() statTable: StatTable;

  viewName: boolean = false;
  viewRank: boolean = false;
  viewTeam: boolean = false;
  viewSeason: boolean = false;
  viewYears: boolean = false;
  viewTotals: boolean = false;
  viewSkater: boolean = false;
  viewGoalie: boolean = false;

  isPlayer: boolean = false;
  isCareer: boolean = false;
  isSeason: boolean = false;

  pages: number = 5;

  skaterHeaders = ["GP", "G", "A", "P", "+/-", "PIM", "PM5", "HIT", "HTT", "SHT", "OSB", "OSM", "SB", "MP", "PPG", "PPA", "PPP", "PPS", "PPM", "PKG", "PKA", "PKP", "PKS", "PKM", "GW", "GT", "FOW", "FOT", "EG", "HT", "PSG", "PSS", "FW", "FL", "FT"];
  goalieHeaders = ['GP','W','L','OTL','MP','PIM','SO','GA','SA','A','EG','PSA','ST','BG','PSS'];

  constructor(private help: HelpService) { }

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

    switch (this.playerType) {
      case "goalie": {
        this.viewSkater = false;
        this.viewGoalie = true;
        break;
      }
      case "skater": {
        this.viewSkater = true;
        this.viewGoalie = false;
        break;
      }
    }
  }

  canViewTeamCount(): boolean {
    return this.displayType == "career" && !this.isTeam;
  }

  getInnerClass() {
    var classes = "inner ";
    classes += (this.isPlayer) ? 'fixedMarginSeason' : 'fixedMargin';
    return classes;
  }

  checkIndex(i: number) {
    return i == this.selectedColumnIndex;
  }

  sortBy(i: number) {
    this.help.getLink(this.statTable.statParameters, "selectedColumnIndex", i);
  }

}
