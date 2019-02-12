import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  imgPath: string;

  constructor() { }

  ngOnInit() {
    this.getNewImage();
  }

  getFrontImage() {
    var imgPaths = ['0zaUmzm.png', '850EmPD.png', 'MKQdHib.png', 'Zqax5lk.jpg', 'gcJp6ss.png', 'p6Kv7yF.png',
      '2FKLtcW.jpg', '8Bh85g5.png', 'MZdUQ8e.jpg', 'edYJFs3.png', 'hLWxGq9.jpg', 's0fEjh6.png',
      '4ewdgS0.jpg', 'Ak4G4D6.png', 'VQ3ik6u.jpg', 'front.gif', 'ivctYgq.png', 'v3UFYpg.png',
      '7MZpARS.jpg', 'BdrGls9.jpg', 'Z0ZFNHB.jpg', 'g94Y8aK.png']
    var index = Math.floor(Math.random() * imgPaths.length) 

    // TODO: Point to imgur
    return "../../../assets/fronts/" + imgPaths[index];
  }

  getNewImage() {
    this.imgPath = this.getFrontImage();
  }

}
