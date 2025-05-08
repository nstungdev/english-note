import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/components/login/login.component';
import { HomeComponent } from './features/home/components/home.component';
import { AddVocabularyComponent } from './features/vocabulary/components/add-vocabulary/add-vocabulary.component';
import { VocabularyTableComponent } from './features/vocabulary/components/vocabulary-table/vocabulary-table.component';
import { LayoutComponent } from './shared/components/layout/layout.component';

export const routes: Routes = [
  {
    path: 'auth',
    children: [{ path: 'login', component: LoginComponent }],
  },
  {
    path: '',
    component: LayoutComponent,
    children: [
      { path: 'home', component: HomeComponent },
      { path: 'vocabulary', component: VocabularyTableComponent },
      { path: 'add-vocabulary', component: AddVocabularyComponent },
      { path: '', redirectTo: '/home', pathMatch: 'full' },
      { path: '**', redirectTo: '/home' },
    ],
  },
];
