import { Component, Host, HostListener, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { MembersService } from 'src/app/_services/members.service';
import { AccountService } from 'src/app/_services/account.service';
import { take } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-member-edit',
  templateUrl: './member-edit.component.html',
  styleUrls: ['./member-edit.component.css']
})
export class MemberEditComponent implements OnInit{
  member: Member | undefined;
  user: User | null = null;
  @ViewChild('editForm') editForm: NgForm | undefined;
  @HostListener('window:beforeunload', ['$event']) unloadNotification($event: any) {
    if (this.editForm?.dirty) {
      $event.returnValue = true;
    }
  }
  constructor(private membersService: MembersService,private accountService:AccountService, private toatr: ToastrService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe({
      next:user => this.user = user});
   }

   loadMember() {
    if(!this.user) return;
    this.membersService.getMember(this.user?.userName).subscribe({
      next: member => this.member = member
    });

}
ngOnInit(): void {
  this.loadMember();
}
updateMember() {
  this.membersService.updateMember(this.editForm?.value)
  .subscribe({
    next: () => {
      this.toatr.success('Profile updated successfully');
      this.editForm?.reset(this.member);
    }
  });

}
  
}