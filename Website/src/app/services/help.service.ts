import { Injectable } from '@angular/core';
import { StatParameters } from '../dtos/stat-parameters';
import { NavigationExtras, ActivatedRoute, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class HelpService {

  constructor(private router: Router) { }

  getLink(statParams: StatParameters, name: string, arg: number) {
    var linkParams = statParams;

    switch (name) {
      case "team": linkParams.team = arg; break;
      case "seasonType": linkParams.seasonType = arg; break;
      case "selectedColumnIndex": linkParams.selectedColumnIndex = arg; break;
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
    if (linkParams.selectedColumnIndex >= 0) queryParmsNew['selectedColumnIndex'] = linkParams.selectedColumnIndex;

    let navigationExtras: NavigationExtras = {
      queryParams: queryParmsNew
    };
    
    console.log(navigationExtras);

    var routeDirection = this.router.url;
    if (routeDirection.indexOf("?") > 0)
      routeDirection = routeDirection.substr(0, this.router.url.indexOf("?"));
    this.router.navigate([routeDirection], navigationExtras);
  }
}
