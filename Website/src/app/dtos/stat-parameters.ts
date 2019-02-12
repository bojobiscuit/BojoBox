import { ParamMap } from '@angular/router';

export class StatParameters {
    team: number;
    era: number;
    season: number;
    league: number;
    playerType: number;
    seasonType: number;
    selectedColumnIndex: number;

    setParams(params: ParamMap) {
        this.team = +params.get('team');
        this.era = +params.get('era');
        this.season = +params.get('season');
        this.playerType = +params.get('plyType');
        this.league = +params.get('league');
        this.seasonType = +params.get('ssnType');
        this.selectedColumnIndex = +params.get('col') || -1;
    }
}
