import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.scss']
})
export class PaginationComponent implements OnInit {

  @Input() pages: number;

  constructor() { }

  ngOnInit() {
  }

}
