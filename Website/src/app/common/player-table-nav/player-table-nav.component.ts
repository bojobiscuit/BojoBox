import { Component, OnInit, Input } from '@angular/core';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { BasicData } from 'src/app/dtos/basic-data';
import { Router, NavigationExtras } from '@angular/router';
import { HelpService } from 'src/app/services/help.service';

@Component({
  selector: 'player-table-nav',
  templateUrl: './player-table-nav.component.html',
  styleUrls: ['./player-table-nav.component.scss']
})
export class PlayerTableNavComponent implements OnInit {

  @Input() displayType: string;
  @Input() playerType: string;
  @Input() statParams: StatParameters;
  @Input() seasons: number[];
  @Input() teams: BasicData[];

  viewEras: boolean = false;
  viewSeasons: boolean = false;
  viewTeams: boolean = false;
  viewLeagues: boolean = false;
  viewPlayerTypes: boolean = false;
  viewSeasonTypes: boolean = false;

  constructor(private router: Router, private help: HelpService) { }

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
  }

  getSelectedSeasonTypeOutput(): string {
    switch (this.statParams.seasonType) {
      case 1: return "Reg. Season";
      case 2: return "Playoffs";
    }
    return "error";
  }

  getSelectedLeagueOutput(): string {
    switch (this.statParams.league) {
      case 1: return "SHL";
      case 2: return "SMJHL";
      case 3: return "IIHL";
    }
    return "error";
  }

  getSelectedPlayerTypeOutput(): string {
    switch (this.playerType) {
      case "skater": return "Skaters";
      case "goalie": return "Goalies";
    }
    return "error";
  }

  getSelectedEra(): string {
    if (this.statParams.era == 0 && this.statParams.season > 0) {
      return "s" + this.statParams.season;
    }

    switch (this.statParams.era) {
      case 0: return "All Time";
      case 1: return "Modern";
      case 2: return "Inflation";
      case 3: return "Legacy";
    }

    return "error";
  }

  getSelectedTeamOutput(): string {
    if (this.statParams.team == 0)
      return "All Teams";
    else {
      return this.teams.find(x => x.id == this.statParams.team).acronym;
    }
  }

  switchPlayerType(newType: string) {
    var routeDirection = newType;
    if (this.displayType != "player")
      routeDirection = this.displayType + "/" + routeDirection;
    this.router.navigate([routeDirection]);
  }

  getLink(name: string, arg: number) {
    this.help.getLink(this.statParams, name, arg);
  }

}
