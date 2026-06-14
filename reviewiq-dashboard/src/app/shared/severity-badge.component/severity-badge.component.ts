import { Component, Input } from '@angular/core';
import { Severity } from '../../core/models';
import { CommonModule } from '@angular/common';

@Component({
    selector: 'app-severity-badge',
    imports: [CommonModule],
    templateUrl: './severity-badge.component.html',
    styleUrl: './severity-badge.component.scss',
})
export class SeverityBadgeComponent {
    @Input() severity!: Severity;

    get badgeClass(): string {
        switch (this.severity) {
            case 'Critical':
                return 'riq-badge riq-badge--critical';
            case 'Warning':
                return 'riq-badge riq-badge--warning';
            case 'Suggestion':
                return 'riq-badge riq-badge--suggestion';
        }
    }
}
