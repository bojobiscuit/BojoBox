import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { StatTable } from 'src/app/dtos/stat-table';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-season-goalie-stats',
  templateUrl: './season-goalie-stats.component.html',
  styleUrls: ['./season-goalie-stats.component.scss']
})
export class SeasonGoalieStatsComponent implements OnInit {
  
  statTable: StatTable;

  constructor(private route: ActivatedRoute, private apiService: ApiService) { }

  ngOnInit() {
    this.GetQueryParams();
  }

  private GetQueryParams() {
    this.route.queryParamMap.subscribe((params) => {
      var statParameters = new StatParameters();
      statParameters.setParams(params, false);
      this.GetSkaterTable(statParameters);
    });
  }

  private GetSkaterTable(statParameters: StatParameters) {
    this.apiService.getGoalieSeasonTable(statParameters).subscribe((table) => {
      this.statTable = table;
    });
  }

}
