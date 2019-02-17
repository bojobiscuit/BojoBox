import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkaterStatsComponent } from './skater-stats.component';

describe('SkaterStatsComponent', () => {
  let component: SkaterStatsComponent;
  let fixture: ComponentFixture<SkaterStatsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkaterStatsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkaterStatsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
