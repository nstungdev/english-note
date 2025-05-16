import { CustomInputComponent } from '@/shared/components/custom-input/custom-input.component';
import {
  ChangeDetectionStrategy,
  Component,
  computed,
  OnInit,
  signal,
} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Group, Permission, User } from '../../models/user.model';
import { UserService } from '../../services/user.service';
import { firstValueFrom } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CustomSelectComponent } from '../../../../shared/components/custom-select/custom-select.component';

@Component({
  selector: 'app-update-user',
  imports: [
    CustomInputComponent,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    CustomSelectComponent,
  ],
  standalone: true,
  templateUrl: './update-user.component.html',
  styleUrl: './update-user.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UpdateUserComponent implements OnInit {
  userId: number = 0;
  user = signal<User | null>(null);
  permissions = signal<Permission[]>([]);
  permissionOptions = computed(() =>
    this.permissions().map((permission) => ({
      label: permission.name,
      value: permission.id,
    }))
  );

  groups = signal<Group[]>([]);
  groupOptions = computed(() =>
    this.groups().map((group) => ({
      label: group.name,
      value: group.id,
    }))
  );

  constructor(
    private readonly userService: UserService,
    private readonly route: ActivatedRoute
  ) {}

  ngOnInit(): void {
    this.route.params.subscribe((params) => {
      this.userId = +params['id'];
    });

    firstValueFrom(this.userService.getPermissions()).then((response) => {
      this.permissions.set(response.data);
    });
    firstValueFrom(this.userService.getGroups()).then((response) => {
      this.groups.set(response.data);
    });
    firstValueFrom(this.userService.getById(this.userId)).then((response) => {
      this.user.set(response.data);
    });
  }
}
