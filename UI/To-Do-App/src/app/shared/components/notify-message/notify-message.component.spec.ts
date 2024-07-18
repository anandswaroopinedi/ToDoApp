import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NotifyMessageComponent } from './notify-message.component';

describe('NotifyMessageComponent', () => {
  let component: NotifyMessageComponent;
  let fixture: ComponentFixture<NotifyMessageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NotifyMessageComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NotifyMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
