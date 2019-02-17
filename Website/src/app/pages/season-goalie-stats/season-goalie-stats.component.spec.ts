import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeasonGoalieStatsComponent } from './season-goalie-stats.component';

describe('SeasonStatsComponent', () => {
  let component: SeasonGoalieStatsComponent;
  let fixture: ComponentFixture<SeasonGoalieStatsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeasonGoalieStatsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeasonGoalieStatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
