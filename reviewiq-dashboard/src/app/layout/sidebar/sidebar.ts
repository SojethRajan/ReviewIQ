import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';

@Component({
    selector: 'app-sidebar',
    imports: [CommonModule, RouterModule],
    templateUrl: './sidebar.html',
    styleUrl: './sidebar.scss',
})
export class SidebarComponent {
    navItems = [
        {
            label: 'Dashboard',
            icon: 'ti-layout-dashboard',
            route: '/dashboard',
        },
        {
            label: 'Pull Requests',
            icon: 'ti-git-pull-request',
            route: '/pull-requests',
        },
        {
            label: 'Analytics',
            icon: 'ti-chart-bar',
            route: '/analytics',
        },
    ];

    configItems = [
        {
            label: 'Settings',
            icon: 'ti-settings',
            route: '/settings'
        },
    ];
}
