import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CareerGoalieStatsComponent } from './career-goalie-stats.component';

describe('CareerGoalieStatsComponent', () => {
  let component: CareerGoalieStatsComponent;
  let fixture: ComponentFixture<CareerGoalieStatsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CareerGoalieStatsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CareerGoalieStatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
