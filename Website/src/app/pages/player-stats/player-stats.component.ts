import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { StatTable } from 'src/app/dtos/stat-table';
import { PlayerDataService } from 'src/app/services/player-data-service';
import { ApiService } from 'src/app/services/api.service';

@Component({
  selector: 'app-player-stats',
  templateUrl: './player-stats.component.html',
  styleUrls: ['./player-stats.component.scss']
})
export class PlayerStatsComponent implements OnInit {

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
      this.GetSkaterTable(id, statParameters);
    });
  }

  private GetSkaterTable(id: number, statParameters: StatParameters) {
    this.apiService.getSkaterPlayerTable(id, statParameters).subscribe((table) => {
      this.statTable = table;
    });
  }
}
