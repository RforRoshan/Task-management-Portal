import { Component, ElementRef, HostListener, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Location } from '@angular/common';
import { AppRouteConstants } from '../../../constants/constants';

@Component({
  selector: 'rks-no-valid-url',
  imports: [],
  templateUrl: './no-valid-url.html',
  styleUrl: './no-valid-url.scss'
})
export class NoValidURL {
  @ViewChild('gameCanvas', { static: true }) canvasRef!: ElementRef<HTMLCanvasElement>;

  private ctx!: CanvasRenderingContext2D;
  private player = { x: 180, y: 350, size: 20 };
  private asteroids: { x: number; y: number; size: number }[] = [];
  private score = 0;
  private gameOver = false;
  private animationId: number | null = null;
  private spawnInterval: any;

  constructor(private router: Router, private location: Location) {}
  
  ngAfterViewInit() {
    if (this.canvasRef?.nativeElement) {
      this.ctx = this.canvasRef.nativeElement.getContext('2d')!;
      this.startGame();
    }
  }

  goBack(): void {
    this.location.back();
  }

  goHome(): void {
    this.router.navigateByUrl(AppRouteConstants.SelectDashboard);
  }

  restartGame() {
    // reset state
    this.asteroids = [];
    this.score = 0;
    this.gameOver = false;
    this.player.x = 180;

    // cancel previous loops
    if (this.animationId) cancelAnimationFrame(this.animationId);
    if (this.spawnInterval) clearInterval(this.spawnInterval);

    this.startGame();
  }

  private startGame() {
    // spawn asteroids every 800ms
    this.spawnInterval = setInterval(() => {
      this.asteroids.push({
        x: Math.random() * 380,
        y: -20,
        size: 15 + Math.random() * 10
      });
    }, 800);

    const draw = () => {
      if (this.gameOver) return;

      // clear canvas
      this.ctx.clearRect(0, 0, 400, 400);

      // draw player
      this.ctx.fillStyle = '#00bcd4';
      this.ctx.fillRect(this.player.x, this.player.y, this.player.size, this.player.size);

      // draw asteroids
      this.ctx.fillStyle = '#ff1744';
      this.asteroids.forEach((a, i) => {
        a.y += 3;

        this.ctx.beginPath();
        this.ctx.arc(a.x, a.y, a.size, 0, Math.PI * 2);
        this.ctx.fill();

        // collision detection
        if (
          a.y + a.size > this.player.y &&
          a.x > this.player.x - a.size &&
          a.x < this.player.x + this.player.size + a.size
        ) {
          this.gameOver = true;
          this.ctx.fillStyle = 'white';
          this.ctx.font = '20px Arial';
          this.ctx.fillText('💀 Game Over 💀', 120, 200);

          // stop spawning
          clearInterval(this.spawnInterval);
          return;
        }

        // remove asteroid if it goes off screen
        if (a.y > 400) {
          this.asteroids.splice(i, 1);
          this.score++;
        }
      });

      // score display
      this.ctx.fillStyle = '#fff';
      this.ctx.font = '16px Arial';
      this.ctx.fillText(`Score: ${this.score}`, 10, 20);

      this.animationId = requestAnimationFrame(draw);
    };

    draw();
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyPress(event: KeyboardEvent) {
    if (this.gameOver) return;

    if (event.key === 'ArrowLeft' && this.player.x > 0) this.player.x -= 20;
    if (event.key === 'ArrowRight' && this.player.x < 380) this.player.x += 20;
  }
}