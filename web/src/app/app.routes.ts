import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/components/login/login.component';
import { HomeComponent } from './features/home/components/home.component';
import { AddVocabularyComponent } from './features/vocabulary/components/add-vocabulary/add-vocabulary.component';
import { LayoutComponent } from './shared/components/layout/layout.component';
import { VocabularyOverviewComponent } from './features/vocabulary/components/vocabulary-overview/vocabulary-overview.component';
import { UserTableComponent } from './features/user/components/user-table/user-table.component';
import { AuthGuard } from './features/auth/auth.guard';

export const routes: Routes = [
  {
    path: 'auth',
    children: [{ path: 'login', component: LoginComponent }],
  },
  {
    path: '',
    component: LayoutComponent,
    canActivate: [AuthGuard],
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'vocabulary', component: VocabularyOverviewComponent },
      { path: 'add-vocabulary', component: AddVocabularyComponent },
      { path: 'users', component: UserTableComponent },
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: '**', redirectTo: '/home' },
    ],
  },
];
