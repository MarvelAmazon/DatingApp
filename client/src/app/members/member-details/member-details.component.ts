import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { GalleryItem, GalleryModule, ImageItem } from 'ng-gallery';
import { Member } from 'src/app/_models/member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-details',
  templateUrl: './member-details.component.html',
  styleUrls: ['./member-details.component.css'],
  standalone: true,
  imports: [CommonModule,TabsModule,GalleryModule]
})
export class MemberDetailsComponent implements OnInit{
  
  member: Member|undefined;
 images:GalleryItem[] = []; 

  constructor(private membersService: MembersService, private router: Router, private route: ActivatedRoute) { }

  ngOnInit(): void {
    //throw new Error('Method not implemented.');
    this.loadMember();
  }

  loadMember(){
    var username = this.route.snapshot.paramMap.get('username');
    if(!username) return;
    this.membersService.getMember(username).subscribe({
      next: (member: Member) => {
        this.member = member;
        this.getImages();
      },
      error: (error: any) => {
        console.log(error);
    }
    })
  }

  getImages(): GalleryItem[] {
    if(this.member?.photos){
      for (const photo of this.member?.photos) {
        this.images.push(new ImageItem({ src: photo?.url, thumb: photo?.url }));
      
      }
    }
    return this.images;
  }


}
