import { ComponentFixture, TestBed } from '@angular/core/testing';
import { InputFormComponent } from './input-form.component';

describe('InputFormComponent', () => {
  let component: InputFormComponent;
  let fixture: ComponentFixture<InputFormComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [InputFormComponent]
    });
    fixture = TestBed.createComponent(InputFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should have initial value for longUrl', () => {
    expect(component.longUrl).toEqual('');
  });

  it('should have initial value for shortenedUrl', () => {
    expect(component.shortenedUrl).toEqual('');
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should set shortenedUrl and showShortenedUrl to true when onSubmit is called with a valid URL', () => {
    // Arrange: Set a valid longUrl
    component.longUrl = 'https://example.com';

    // Act: Call the onSubmit method
    component.onSubmit();

    // Assert: Expect that shortenedUrl is set and showShortenedUrl is true
    expect(component.shortenedUrl).toBeDefined();
    expect(component.showShortenedUrl).toBe(true);
  });

  it('should set errormessage when onSubmit is called with an invalid URL', () => {
    // Arrange: Set an invalid longUrl
    component.longUrl = 'invalid-url';

    // Act: Call the onSubmit method
    component.onSubmit();

    // Assert: Expect that errormessage is set
    expect(component.errormessage).toBeDefined();
  });
});
