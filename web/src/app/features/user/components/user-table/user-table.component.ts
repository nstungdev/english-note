import {
  ChangeDetectionStrategy,
  Component,
  OnInit,
  signal,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { firstValueFrom } from 'rxjs';
import { UserService } from '../../services/user.service';
import { User } from '../../models/user.model';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatExpansionModule,
    MatIconModule,
    MatButtonModule,
  ],
  templateUrl: './user-table.component.html',
  styleUrls: ['./user-table.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UserTableComponent implements OnInit {
  dataSource = signal<User[]>([]);
  displayedColumns: string[] = [
    'index',
    'username',
    'email',
    'blocked',
    'actions',
  ];

  constructor(
    private readonly userService: UserService,
    private readonly router: Router
  ) {}

  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers() {
    firstValueFrom(this.userService.getList()).then((response) => {
      this.dataSource.set(response.data);
    });
  }

  toggleBlock(user: User): void {
    this.userService.toggleBlock(user.id, user.isBlocked).subscribe(() => {
      this.fetchUsers();
    });
  }

  onEdit(user: User): void {
    this.router.navigate(['/update-user', user.id]);
  }

  onDelete(user: User): void {
    // Implement delete functionality here
  }

  getIndex(idx: number): number {
    return idx + 1;
  }
}
