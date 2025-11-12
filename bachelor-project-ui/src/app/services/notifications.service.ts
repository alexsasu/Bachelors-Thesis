import { Injectable } from '@angular/core';
import { MessageService } from 'primeng/api';

@Injectable({
  providedIn: 'root'
})
export class NotificationsService {
  constructor(private _messageService: MessageService) { }

  createMessage(type: string, summary: string, message: string) {
    this._messageService.add({ severity: type, summary: summary, detail: message });
    setTimeout(() => {
      this._messageService.clear();
    }, 3000);
  }
}
