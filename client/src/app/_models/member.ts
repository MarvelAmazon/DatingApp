import { Photo } from "./photo";

 export interface Member {
    id: number;
    userName: string;
    dateOfBirth: string;
    photoUrl: string;
    knownAs: string;
    created: string;
    lastActive: string;
    gender: string;
    lookingFor: string;
    interests: string;
    introduction: string;
    city: string;
    country: string;
    photos: Photo[];
    age: number;
  }