import {Directive, ElementRef, Renderer} from '@angular/core'

@Directive({
    selector: '[autoGrow]',
    host: {
        '(focus)': 'onFocus()',
        '(blur)' : 'onBlur()'
    }
})
export class AutoGrowDirective {

    constructor(private el: ElementRef, private renderer: Renderer) {

    }

    public onFocus() : void {
        this.renderer.setElementStyle(this.el.nativeElement, 'width', '400px');
    }

    public onBlur(): void {
        this.renderer.setElementStyle(this.el.nativeElement, 'width', '200px');
    }
}