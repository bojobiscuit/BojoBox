import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { StatTable } from 'src/app/dtos/stat-table';
import { PlayerDataService } from 'src/app/services/player-data-service';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-career-stats',
  templateUrl: './career-stats.component.html',
  styleUrls: ['./career-stats.component.scss']
})
export class CareerStatsComponent implements OnInit {

  statTable: StatTable;

  constructor(private route: ActivatedRoute, private dataService: PlayerDataService, private apiService: ApiService) { }

  ngOnInit() {
    this.GetQueryParams();
  }

  private GetQueryParams() {
    this.route.queryParamMap.subscribe((params) => {
      var statParameters = new StatParameters();
      statParameters.setParams(params);
      this.GetSkaterTable(statParameters);
    });
  }

  private GetSkaterTable(statParameters: StatParameters) {
    this.apiService.getSkaterCareerTable(statParameters).subscribe((table) => {
      this.statTable = table;
    });
  }
  
}
