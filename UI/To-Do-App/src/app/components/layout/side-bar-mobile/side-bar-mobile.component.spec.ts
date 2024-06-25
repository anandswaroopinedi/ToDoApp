import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SideBarMobileComponent } from './side-bar-mobile.component';

describe('SideBarMobileComponent', () => {
  let component: SideBarMobileComponent;
  let fixture: ComponentFixture<SideBarMobileComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [SideBarMobileComponent],
    }).compileComponents();

    fixture = TestBed.createComponent(SideBarMobileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
