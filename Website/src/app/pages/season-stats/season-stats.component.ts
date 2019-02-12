import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { StatParameters } from 'src/app/dtos/stat-parameters';
import { StatTable } from 'src/app/dtos/stat-table';
import { PlayerDataService } from 'src/app/services/player-data-service';

@Component({
  selector: 'app-season-stats',
  templateUrl: './season-stats.component.html',
  styleUrls: ['./season-stats.component.scss']
})
export class SeasonStatsComponent implements OnInit {

  statTable: StatTable;

  constructor(private route: ActivatedRoute, private dataService: PlayerDataService) { }

  ngOnInit() {
    this.route.queryParamMap.subscribe(
      (params) => {
        var statParameters = new StatParameters();
        statParameters.setParams(params);
        this.statTable = this.dataService.getPlayerData(statParameters);

        if(this.statTable.displayType != 'season')
        {
          // throw error
        }
      });
  }

}
