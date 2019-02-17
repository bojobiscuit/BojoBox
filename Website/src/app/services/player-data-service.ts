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
      { id: 1, name: "West Kendall Platoon", acronym: "WKP" },
      { id: 2, name: "New England Wolfpack", acronym: "WKP" },
      { id: 3, name: "Minnesota Chiefs", acronym: "WKP" },
    ];
    table.statParameters = statParams;
    table.playerRows = [
      { skater: { id: 1, name: 'Bojo Biscuit', acronym: "Biscuit"}, rank: 1, team: { id: 1, name: "WKP", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 25, stats: [50, 10, 35] },
      { skater: { id: 1, name: 'Jasper Clayton', acronym: "Biscuit"}, rank: 2, team: { id: 1, name: "NEW", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 37, stats: [50, 12, 30] },
      { skater: { id: 1, name: 'Alonzo Garbonzo', acronym: "Biscuit"}, rank: 3, team: { id: 1, name: "MIN", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 28, stats: [50, 8, 28] },
      { skater: { id: 1, name: 'Bojo Biscuit', acronym: "Biscuit"}, rank: 1, team: { id: 1, name: "WKP", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 25, stats: [50, 10, 35] },
      { skater: { id: 1, name: 'Jasper Clayton', acronym: "Biscuit"}, rank: 2, team: { id: 1, name: "NEW", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 37, stats: [50, 12, 30] },
      { skater: { id: 1, name: 'Alonzo Garbonzo', acronym: "Biscuit"}, rank: 3, team: { id: 1, name: "MIN", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 28, stats: [50, 8, 28] },
      { skater: { id: 1, name: 'Bojo Biscuit', acronym: "Biscuit"}, rank: 1, team: { id: 1, name: "WKP", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 25, stats: [50, 10, 35] },
      { skater: { id: 1, name: 'Jasper Clayton', acronym: "Biscuit"}, rank: 2, team: { id: 1, name: "NEW", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 37, stats: [50, 12, 30] },
      { skater: { id: 1, name: 'Alonzo Garbonzo', acronym: "Biscuit"}, rank: 3, team: { id: 1, name: "MIN", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 28, stats: [50, 8, 28] },
      { skater: { id: 1, name: 'Bojo Biscuit', acronym: "Biscuit"}, rank: 1, team: { id: 1, name: "WKP", acronym: "WKP" }, seasonCount: 13, teamCount: 2, season: 25, stats: [50, 10, 35] },
    ];
    table.playerTotals = [150, 30, 93];

    return table;
  }

}
