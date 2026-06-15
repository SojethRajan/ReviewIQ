import { Component } from '@angular/core';
import { Repository } from '../../../core/models';
import { RegisterRepositoryPayload } from '../../../core/models/register-repository.payload';
import { RepoConfigPanelComponent } from '../repo-config-panel.component/repo-config-panel.component';
import { RepoListComponent } from '../repo-list.component/repo-list.component';

@Component({
    selector: 'app-settings',
    imports: [RepoConfigPanelComponent, RepoListComponent],
    templateUrl: './settings.component.html',
    styleUrl: './settings.component.scss',
})
export class SettingsComponent {

    repositories: Repository[] = [
        {
            id: 'a1b2c3d4-0000-0000-0000-000000000001',
            gitHubRepoId: '123456789',
            owner: 'SojethRajan',
            name: 'ReviewIQ',
            isActive: true,
            createdOn: '2026-05-01T10:00:00.000Z'
        }
    ];

    onRegister(payload: RegisterRepositoryPayload): void {
        const newRepo: Repository = {
            id: crypto.randomUUID(),
            gitHubRepoId: '',
            owner: payload.owner,
            name: payload.name,
            isActive: true,
            createdOn: new Date().toISOString()
        };
        this.repositories = [...this.repositories, newRepo];
    }

    onToggleActive(repo: Repository): void {
        this.repositories = this.repositories.map(r =>
            r.id === repo.id ? { ...r, isActive: !r.isActive } : r
        );
    }

    onRemove(repo: Repository): void {
        this.repositories = this.repositories.filter(r => r.id !== repo.id);
    }
}
