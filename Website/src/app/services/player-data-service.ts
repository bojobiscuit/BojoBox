import { Injectable } from '@angular/core';
import { StatParameters } from '../dtos/stat-parameters';
import { StatTable } from '../dtos/stat-table';

@Injectable({
  providedIn: 'root'
})
export class PlayerDataService {

  constructor() { }

  getPlayerData(statParams: StatParameters): StatTable {
    var table = new StatTable();
    table.seasons = [45, 44, 43, 42, 41, 40, 39, 38, 37, 36, 35, 34, 33, 32, 31, 30];
    table.teams = [
      { id: 1, name: "West Kendall Platoon" },
      { id: 2, name: "New England Wolfpack" },
      { id: 3, name: "Minnesota Chiefs" },
    ];
    table.statParameters = statParams;
    table.playerRows = [
      { name: 'Bojo Biscuit', rank: 1, team: { id: 1, name: "WKP" }, season: 25, stats: [50, 10, 35] },
      { name: 'Jasper Clayton', rank: 2, team: { id: 1, name: "NEW" }, season: 37, stats: [50, 12, 30] },
      { name: 'Alonzo Garbonzo', rank: 3, team: { id: 1, name: "MIN" }, season: 28, stats: [50, 8, 28] },
      { name: 'Bojo Biscuit', rank: 1, team: { id: 1, name: "WKP" }, season: 25, stats: [50, 10, 35] },
      { name: 'Jasper Clayton', rank: 2, team: { id: 1, name: "NEW" }, season: 37, stats: [50, 12, 30] },
      { name: 'Alonzo Garbonzo', rank: 3, team: { id: 1, name: "MIN" }, season: 28, stats: [50, 8, 28] },
      { name: 'Bojo Biscuit', rank: 1, team: { id: 1, name: "WKP" }, season: 25, stats: [50, 10, 35] },
      { name: 'Jasper Clayton', rank: 2, team: { id: 1, name: "NEW" }, season: 37, stats: [50, 12, 30] },
      { name: 'Alonzo Garbonzo', rank: 3, team: { id: 1, name: "MIN" }, season: 28, stats: [50, 8, 28] },
      { name: 'Bojo Biscuit', rank: 1, team: { id: 1, name: "WKP" }, season: 25, stats: [50, 10, 35] },
    ];
    table.playerTotals = [150, 30, 93];

    return table;
  }

}
