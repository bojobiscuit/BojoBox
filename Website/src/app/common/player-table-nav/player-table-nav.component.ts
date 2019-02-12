import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'player-table-nav',
  templateUrl: './player-table-nav.component.html',
  styleUrls: ['./player-table-nav.component.scss']
})
export class PlayerTableNavComponent implements OnInit {

  @Input() displayType: string;
  viewEras: boolean = false;
  viewSeasons: boolean = false;
  viewTeams: boolean = false;
  viewLeagues: boolean = false;
  viewPlayerTypes: boolean = false;
  viewSeasonTypes: boolean = false;

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
  }

}
