import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private readonly THEME_KEY = 'ThemeName';
  private readonly DEFAULT_THEME_NAME = 'Ocean';

  public readonly themes: Theme[] = [
    new Theme("Ocean", "#1abc9c", "#16a085"),         // Teal Blend
    new Theme("Sunset", "#ff7675", "#fab1a0"),        // Warm Peach
    new Theme("Midnight", "#2c3e50", "#34495e"),      // Steely Night
    new Theme("Forest", "#145A32", "#1e8449"),        // Rich Green
    new Theme("Peach", "#ffb07c", "#ffd1a9"),         // Light Apricot
    new Theme("Lavender", "#a29bfe", "#dcd6f7"),      // Dreamy Purple
    new Theme("Slate", "#7f8c8d", "#95a5a6"),          // Soft Steel
    new Theme("RoseGold", "#b76e79", "#e8c3c3"),      // Elegant Blush
    // new Theme("Mint", "#98ff98", "#b2f2bb"),       // (Optional) Pastel Mint
    new Theme("Crimson", "#dc143c", "#f1948a"),       // Bright Rose
    new Theme("Sky", "#87ceeb", "#b3e5fc"),           // Clear Sky
    new Theme("Amber", "#ffc107", "#ffecb3"),         // Honey Yellow
    new Theme("Plum", "#8e44ad", "#af7ac5")           // Soft Violet
  ];

  constructor() {}

  changeTheme(themeName: string): void {
    const theme = this.themes.find(t => t.name === themeName)
      ?? this.themes.find(t => t.name === this.DEFAULT_THEME_NAME)!;

    this.applyTheme(theme);
    localStorage.setItem(this.THEME_KEY, theme.name);
  }

  getThemeName(): string {
    return localStorage.getItem(this.THEME_KEY) || this.DEFAULT_THEME_NAME;
  }

  getCurrentTheme(): Theme {
    const name = this.getThemeName();
    return this.themes.find(t => t.name === name) 
      ?? this.themes.find(t => t.name === this.DEFAULT_THEME_NAME)!;
  }

  loadTheme(): void {
    const theme = this.themes.find(t => t.name === this.getThemeName())
      ?? this.themes.find(t => t.name === this.DEFAULT_THEME_NAME)!;

    this.applyTheme(theme);
  }

  private applyTheme(theme: Theme): void {
    document.documentElement.style.setProperty('--primary-color', theme.primaryColor);
    document.documentElement.style.setProperty('--secondary-color', theme.secondaryColor);
  }

}
export class Theme {
  constructor(
    public name: string,
    public primaryColor: string,
    public secondaryColor: string
  ) {}
}