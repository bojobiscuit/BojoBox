import { BasicData } from './basic-data';
import { StatParameters } from './stat-parameters';
import { PlayerTableRow } from './player-table-row';

export class StatTable {

    displayType: string;
    playerType: string;
    headerText: string;

    seasons: number[];
    teams: BasicData[];
    statParameters: StatParameters;

    playerRows: PlayerTableRow[]
    playerTotals: number[];
}
