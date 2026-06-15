import { Component, EventEmitter, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { RegisterRepositoryPayload } from '../../../core/models';

@Component({
    selector: 'app-repo-config-panel',
    imports: [FormsModule, MatButtonModule],
    templateUrl: './repo-config-panel.component.html',
    styleUrl: './repo-config-panel.component.scss',
})
export class RepoConfigPanelComponent {
    @Output() register = new EventEmitter<RegisterRepositoryPayload>();

    owner = '';
    repoName = '';

    get isValid(): boolean {
        return this.owner.trim().length > 0 && this.repoName.trim().length > 0;
    }

    onRegister(): void {
        if (!this.isValid) return;
        this.register.emit({ owner: this.owner.trim(), name: this.repoName.trim() });
        this.owner = '';
        this.repoName = '';
    }
}
