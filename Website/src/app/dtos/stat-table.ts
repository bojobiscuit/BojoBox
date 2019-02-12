import { BasicData } from './basic-data';
import { StatParameters } from './stat-parameters';
import { PlayerTableRow } from './player-table-row';

export class StatTable {
    seasons: number[];
    teams: BasicData[];
    displayType: string;
    statParameters: StatParameters;
    playerRows: PlayerTableRow[]
    playerTotals: number[];
}
