import { test, expect } from '@playwright/test';

test.describe('CineSynk Core Booking Flow', () => {
  test.beforeEach(async ({ page }) => {
    // Navigate to the app before each test
    await page.goto('/');
  });

  test('User should be able to login and see shows', async ({ page }) => {
    // 1. Fill Login Form
    await page.fill('input[type="text"]', '1234567890');
    await page.fill('input[type="password"]', 'Password123!');
    await page.click('button:has-text("Login")');

    // 2. Verify Dashboard redirection
    await expect(page).toHaveURL(/.*dashboard/);
    await expect(page.locator('h1')).toContainText('CineSynk');

    // 3. Check for Movie Cards
    const showCards = page.locator('.movie-card');
    await expect(showCards.first()).toBeVisible();
  });

  test('User should be able to select a seat and reserve it', async ({ page }) => {
    // Assume we are already logged in (using stored state in a real scenario)
    await page.goto('/dashboard');
    
    // 1. Click on the first movie
    await page.click('.movie-card:first-child');
    await expect(page).toHaveURL(/.*seatmap/);

    // 2. Select an available seat (green/gray)
    const availableSeat = page.locator('.seat.available').first();
    await availableSeat.click();

    // 3. Confirm selection
    await page.click('button:has-text("Reserve Selected Seat")');

    // 4. Verify Success Modal or Status Change
    await expect(page.locator('.status-badge')).toContainText('Pending');
  });
});
