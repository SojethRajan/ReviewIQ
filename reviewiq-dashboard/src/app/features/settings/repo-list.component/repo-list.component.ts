import { Component, EventEmitter, Input, Output } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatSlideToggleModule } from '@angular/material/slide-toggle';
import { Repository } from '../../../core/models';

@Component({
    selector: 'app-repo-list',
    imports: [MatSlideToggleModule, MatButtonModule],
    templateUrl: './repo-list.component.html',
    styleUrl: './repo-list.component.scss',
})
export class RepoListComponent {

    @Input() repositories: Repository[] = [];
    @Output() toggleActive = new EventEmitter<Repository>();
    @Output() remove = new EventEmitter<Repository>();

}
