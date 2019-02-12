import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SeasonStatsComponent } from './season-stats.component';

describe('SeasonStatsComponent', () => {
  let component: SeasonStatsComponent;
  let fixture: ComponentFixture<SeasonStatsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SeasonStatsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SeasonStatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
