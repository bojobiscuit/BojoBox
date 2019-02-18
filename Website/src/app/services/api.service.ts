import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { StatTable } from '../dtos/stat-table';
import { Observable } from 'rxjs';
import { StatParameters } from '../dtos/stat-parameters';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  
  constructor(private http: HttpClient) { }

  getSkaterPlayerTable(id: number, statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getSkaterPlayerTable: ' + id);
    const url = `${environment.connectionString}/skater/${id}${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getGoaliePlayerTable(id: number, statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getGoaliePlayerTable: ' + id);
    const url = `${environment.connectionString}/goalie/${id}${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getSkaterSeasonTable(statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getSkaterSeasonTable');
    const url = `${environment.connectionString}/season/skater/${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getGoalieSeasonTable(statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getGoalieSeasonTable');
    const url = `${environment.connectionString}/season/goalie/${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getSkaterCareerTable(statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getSkaterCareerTable');
    const url = `${environment.connectionString}/career/skater/${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

  getGoalieCareerTable(statParameters: StatParameters): Observable<StatTable> {
    // TODO: Parameters
    console.log('getGoalieCareerTable');
    const url = `${environment.connectionString}/career/goalie/${statParameters.getQuery()}`;
    return this.http.get<StatTable>(url);
  }

}
