import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RepoConfigPanelComponent } from './repo-config-panel.component';

describe('RepoConfigPanelComponent', () => {
  let component: RepoConfigPanelComponent;
  let fixture: ComponentFixture<RepoConfigPanelComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [RepoConfigPanelComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(RepoConfigPanelComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
