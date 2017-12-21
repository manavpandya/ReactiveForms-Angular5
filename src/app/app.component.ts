import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';



@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Reactive Form With Angular 5 Demo';
  rForm: FormGroup;
  post: any;                     // A property for our submitted form
  description: string = '';
  name: string = '';
  titleAlert:string = 'This field is required';
  constructor(private fb: FormBuilder) {
    this.rForm = fb.group({
      'name': [null, Validators.required],
      'description': [null, Validators.compose([Validators.required, Validators.minLength(30), Validators.maxLength(500)])],
      'validate': ''
    });
  }

  addPost(post) {
    this.description = post.description;
    this.name = post.name;
  }

}
