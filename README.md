# Arcade Slot Machine Game

## Game Overview
This is a 2D arcade-style slot machine I built in Unity. The goal is simple: manage your gold balance, pick your bet size (10, 50, or 100 Gold), pull the red arcade lever, and try to match three symbols in a row. Whenever you win, your payout is calculated by multiplying your current bet by the winning symbol's specific multiplier.

## How To Play (Controls)
Set Your Bet: Click the on-screen buttons to choose your bet size (10G, 50G, or 100G).
Spin the Reels: You can start a spin using two different methods:
* Press the Spacebar on your keyboard.
* Click the red arcade lever using your Left Mouse Button (LMB).

## Instructions to Run WebGL Build
You do not need to download, extract, or install any files to play. I have hosted the final WebGL build live using GitHub Pages. 

Simply click the link below to play the game instantly in your web browser:
**https://rohitkhatrii.github.io/Slot-Game/WebGL/**

## Bonus Features
**Free Spins Mode:** I added a special bonus mechanic to reward lucky players. If you land a perfect 3-of-a-kind match using either the **Bell** or the **BAR** symbols, you instantly win 5 Free Spins! 
* A text counter will appear on the screen to track your remaining bonus spins. 
* As long as you have free spins, the game will let you pull the lever without deducting any gold from your balance, but you still keep all the winnings.

## Thought Process & Approach
My main goal was to keep the code organized and make sure the game couldn't be easily broken by the player clicking too fast. 
* **Safe Game Logic:** I built a background state tracker (Idle, Spinning, Evaluating). This guarantees that the player cannot change their bet amount or pull the lever again while the reels are already in motion.
* **Smart Performance (Infinite Reels):** Instead of creating a massive list of symbols that scroll forever, I used a teleporting trick. When a symbol drops completely out of view at the bottom of the window, the code instantly teleports it back to the top of the stack and gives it a new random picture. This keeps the game running incredibly fast because it only ever uses a handful of images at a time.
* **Building Suspense:** I used Unity Coroutines (timers) to make the reels stop one by one with a short delay between them, rather than stopping all at once. This mimics the feel of a real physical slot machine.
* **Responsive UI:** I anchored the interactive UI pieces (like the betting panel and the lever) directly to the center of the screen alongside the main artwork. This ensures the buttons never drift away or cover up the reels if the player resizes their web browser.
* **Simple Math:** I set up the payout logic to just multiply `currentBet * symbolMultiplier`. This approach makes it incredibly easy to add new symbols with different values in the future without rewriting the code.
