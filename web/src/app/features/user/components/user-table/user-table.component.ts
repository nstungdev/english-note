import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { firstValueFrom } from 'rxjs';
import { UserService } from '../../services/user.service';
import { UserDTO } from '../../models/user.model';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';

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
})
export class UserTableComponent implements OnInit {
  dataSource: UserDTO[] = [];
  displayedColumns: string[] = [
    'index',
    'username',
    'email',
    'blocked',
    'actions',
  ];

  constructor(private readonly userService: UserService) {}

  ngOnInit(): void {
    this.fetchUsers();
  }

  fetchUsers() {
    firstValueFrom(this.userService.getList()).then((response) => {
      this.dataSource = response.data;
    });
  }

  toggleBlock(user: UserDTO): void {
    this.userService.toggleBlock(user.id, user.isBlocked).subscribe(() => {
      this.fetchUsers();
    });
  }

  onEdit(user: UserDTO): void {
    // Implement edit functionality here
  }

  onDelete(user: UserDTO): void {
    // Implement delete functionality here
  }

  getIndex(idx: number): number {
    return idx + 1;
  }
}
