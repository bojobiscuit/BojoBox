import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { StatTable } from 'src/app/dtos/stat-table';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-career-goalie-stats',
  templateUrl: './career-goalie-stats.component.html',
  styleUrls: ['./career-goalie-stats.component.scss']
})
export class CareerGoalieStatsComponent implements OnInit {

  statTable: StatTable;

  constructor(private route: ActivatedRoute, private apiService: ApiService) { }

  ngOnInit() {
    this.GetQueryParams();
  }

  private GetQueryParams() {
    this.route.queryParamMap.subscribe((params) => {
      var statParameters = new StatParameters();
      statParameters.setParams(params);
      this.GetGoalieTable(statParameters);
    });
  }

  private GetGoalieTable(statParameters: StatParameters) {
    this.apiService.getGoalieCareerTable(statParameters).subscribe((table) => {
      this.statTable = table;
    });
  }
  
}
