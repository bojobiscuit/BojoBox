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
        this.league = +params.get('league');
        this.seasonType = +params.get('seasonType');
        this.selectedColumnIndex = +params.get('col') || -1;
    }

    getQuery(): string {
        var queryList: string[] = [];

        this.pushArgument(queryList, "team", this.team);
        this.pushArgument(queryList, "era", this.era);
        this.pushArgument(queryList, "season", this.season);
        this.pushArgument(queryList, "league", this.league, 1);
        this.pushArgument(queryList, "seasonType", this.seasonType, 1);
        this.pushArgument(queryList, "selectedColumnIndex", this.selectedColumnIndex);

        return (queryList.length > 0) ?
            "?" + queryList.join("&") : "";
    }

    pushArgument(queryList: string[], name: string, arg: number, min: number = 0) {
        if (arg && arg > min)
            queryList.push(name + "=" + arg);
    }
}
