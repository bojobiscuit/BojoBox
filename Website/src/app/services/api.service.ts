import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StatTable } from '../dtos/stat-table';
import { Observable } from 'rxjs';
import { StatParameters } from '../dtos/stat-parameters';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  
  apiUrl = 'http://localhost:5000/api';

  constructor(private http: HttpClient) { }

  getSkaterPlayerTable(id: number, statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getSkaterPlayerTable: ' + id);
    const url = `${this.apiUrl}/skater/${id}${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getGoaliePlayerTable(id: number, statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getGoaliePlayerTable: ' + id);
    const url = `${this.apiUrl}/goalie/${id}${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getSkaterSeasonTable(statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getSkaterSeasonTable');
    const url = `${this.apiUrl}/season/skater/${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getGoalieSeasonTable(statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getGoalieSeasonTable');
    const url = `${this.apiUrl}/season/goalie/${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getSkaterCareerTable(statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getSkaterCareerTable');
    const url = `${this.apiUrl}/career/skater/${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getGoalieCareerTable(statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getGoalieCareerTable');
    const url = `${this.apiUrl}/career/goalie/${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

}
