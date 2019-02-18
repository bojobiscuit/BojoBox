import { Component, OnInit, Input } from '@angular/core';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { BasicData } from 'src/app/dtos/basic-data';
import { Router, NavigationExtras } from '@angular/router';

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

  constructor(private router: Router) { }

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
    var linkParams = this.statParams;

    switch (name) {
      case "team": linkParams.team = arg; break;
      case "seasonType": linkParams.seasonType = arg; break;
      case "league": {
        linkParams.league = arg;
        linkParams.team = 0; 
        break;
      }
      case "era": {
        linkParams.era = arg;
        linkParams.season = 0;
        break;
      }
      case "season": {
        linkParams.season = arg;
        linkParams.era = 0;
        break;
      }
    }

    var queryParmsNew = {};
    if (linkParams.team > 0) queryParmsNew['team'] = linkParams.team;
    if (linkParams.season > 0) queryParmsNew['season'] = linkParams.season;
    if (linkParams.era > 0) queryParmsNew['era'] = linkParams.era;
    if (linkParams.league > 1) queryParmsNew['league'] = linkParams.league;
    if (linkParams.seasonType > 1) queryParmsNew['seasonType'] = linkParams.seasonType;

    let navigationExtras: NavigationExtras = {
      queryParams: queryParmsNew
    };


    var routeDirection = this.router.url;
    if (routeDirection.indexOf("?") > 0)
      routeDirection = routeDirection.substr(0, this.router.url.indexOf("?"));
    this.router.navigate([routeDirection], navigationExtras);
  }

}
