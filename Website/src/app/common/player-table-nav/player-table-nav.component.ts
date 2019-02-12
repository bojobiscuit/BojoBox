import { Component, OnInit, Input } from '@angular/core';
import { StatParameters } from 'src/app/dtos/stat-parameters';

@Component({
  selector: 'player-table-nav',
  templateUrl: './player-table-nav.component.html',
  styleUrls: ['./player-table-nav.component.scss']
})
export class PlayerTableNavComponent implements OnInit {

  @Input() displayType: string;
  @Input() statParams: StatParameters;

  viewEras: boolean = false;
  viewSeasons: boolean = false;
  viewTeams: boolean = false;
  viewLeagues: boolean = false;
  viewPlayerTypes: boolean = false;
  viewSeasonTypes: boolean = false;

  selectedTeam: string;
  selectedEra: string;
  selectedSeason: string;
  selectedPlayerType: string;
  selectedLeague: string;
  selectedSeasonType: string;

  constructor() { }

  ngOnInit() {
    switch (this.displayType) {
      case "season": {
        this.viewEras = true;
        this.viewSeasons = true;
        this.viewTeams = true;
        this.viewLeagues = true;
        this.viewPlayerTypes = true;
        this.viewSeasonTypes = true;
        break;
      }
      case "career": {
        this.viewTeams = true;
        this.viewLeagues = true;
        this.viewPlayerTypes = true;
        this.viewSeasonTypes = true;
        break;
      }
      case "player": {
        this.viewTeams = true;
        this.viewLeagues = true;
        this.viewSeasonTypes = true;
        break;
      }
    }

    // TODO: send request to get teams and seasons

    if (this.statParams.team == 0)
      this.selectedTeam = "All Teams";
    else {
      // TODO: set team from request
      // else throw error and reset to default
    }

    if (this.statParams.season == 0)
      this.selectedSeason = "All Seasons";
    else {
      // TODO: set season from request
      // else throw error and reset to default
    }

    switch (this.statParams.era) {
      case 1: this.selectedEra = "Modern"; break;
      case 2: this.selectedEra = "Inflation"; break;
      case 3: this.selectedEra = "Legacy"; break;
      case 0:
      default: this.selectedEra = "All Eras"; break;
    }

    switch (this.statParams.league) {
      case 2: this.selectedLeague = "SMJHL"; break;
      case 1:
      default: this.selectedLeague = "SHL"; break;
    }

    switch (this.statParams.seasonType) {
      case 2: this.selectedSeasonType = "Playoffs"; break;
      case 1:
      default: this.selectedSeasonType = "Reg. Season"; break;
    }

    switch (this.statParams.playerType) {
      case 2: this.selectedPlayerType = "Goalies"; break;
      case 1:
      default: this.selectedPlayerType = "Skaters"; break;
    }
  }

}
