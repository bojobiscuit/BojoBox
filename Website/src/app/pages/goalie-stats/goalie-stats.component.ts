import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { StatTable } from 'src/app/dtos/stat-table';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-goalie-stats',
  templateUrl: './goalie-stats.component.html',
  styleUrls: ['./goalie-stats.component.scss']
})
export class GoalieStatsComponent implements OnInit {

  statTable: StatTable;

  constructor(private route: ActivatedRoute, private apiService: ApiService) { }

  ngOnInit() {
    this.GetParams();
  }

  private GetParams() {
    this.route.paramMap.subscribe((params) => {
      var id = +params.get('id');
      this.GetQueryParams(id);
    });
  }

  private GetQueryParams(id: number) {
    this.route.queryParamMap.subscribe((params) => {
      var statParameters = new StatParameters();
      statParameters.setParams(params);
      this.GetGoalieTable(id, statParameters);
    });
  }

  private GetGoalieTable(id: number, statParameters: StatParameters) {
    this.apiService.getGoaliePlayerTable(id, statParameters).subscribe((table) => {
      this.statTable = table;
    });
  }
}
