import { ModuleWithProviders } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomeComponent } from './components/home.component';
import { TransactionComponent } from './components/transaction.component';
import { ErrorComponent } from './components/error.component';
import { accountRoutes, accountRouting } from './components/account/routes';

const appRoutes: Routes = [
    {
        path: '',
        redirectTo: '/home',
        pathMatch: 'full'
    },
    {
        path: 'home',
        component: HomeComponent
    },
    {
        path: 'transaction',
        component: TransactionComponent
    },
    {
        path: 'error',
        component: ErrorComponent
    }
];

export const routing: ModuleWithProviders = RouterModule.forRoot(appRoutes);