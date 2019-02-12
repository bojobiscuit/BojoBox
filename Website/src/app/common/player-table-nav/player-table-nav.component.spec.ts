import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PlayerTableNavComponent } from './player-table-nav.component';

describe('PlayerTableNavComponent', () => {
  let component: PlayerTableNavComponent;
  let fixture: ComponentFixture<PlayerTableNavComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PlayerTableNavComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PlayerTableNavComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
