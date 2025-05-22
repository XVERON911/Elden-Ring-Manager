# Elden Ring Manager (ERM)

**Elden Ring Manager (ERM)** is a tool designed to fix the Seamless Coop Mod for older versions of Elden Ring (e.g., v1.12). It does **NOT** manipulate save files and works by reading and writing memory data.

This software is intended for users who need to bypass known errors when using the Seamless Coop mod on older game versions.

---
### **Elden Ring Manager [ERM]**
![Elden Ring Manager](Screenshot.png)
![ERM in Action - GIF 1](GIF1.gif) ![ERM in Action - GIF 2](GIF2.gif)

## Table of Contents:
1. [Getting Started](#getting-started)
2. [How to Use](#how-to-use)
3. [Warnings & Important Notes](#warnings)
4. [For Developers](#FOR-DEVELOPERS-ONLY)
5. [Credits](#Credits)
6. [Licensing](#licensing)
7. [Contact Me](#contact-me)

---
## Getting Started

Before you use ERM for the first time, you need to set it up:

1. **Admin Permissions**:  
   ERM **requires administrator permissions** to run. You will not be able to use it without them.

2. **Paths Setup (First Time Setup Only)**:  
You'll need to set up the paths for **Elden Ring** and **Steam** the first time you run ERM. This step is not required for subsequent uses.

   - Example:
     - `D:\Games\ELDEN RING - Shadow Of The Erdtree`
     - `C:\Program Files (x86)\Steam`

3. **Session Setup**:  
   On the first run, you'll need to configure the session password and decide whether to allow invaders.

---

## How to Use

### 1. **Launch Seamless Coop**  
   Click the "Launch Seamless Coop" button. ERM will attempt to launch the game with Seamless Coop and bypass known errors related to older versions of the game (e.g., v1.12).


### 2. **Read Game Memory**  
   The "Read Button" fetches player stats, health, and other data directly from the game's memory. **Do not press this button while in the main menu or loading screen.**

### 3. **Freeze HP/FP**  
   This feature keeps your HP and FP at a set value. However, **do not change these values while freezing is active**, as it could cause issues.

### 4. **Set Runes**  
   ERM allows you to set a specific amount of runes, but **do not set a value higher than the max**, as this could cause the game to crash.

### 5. **Activate Rune Arc**  
   The "Rune Arc Status" button will activate the rune you have without using the in-game Rune Arc item.

---
### **How to Download ERM (Step-by-Step Guide)**
   These screenshots walk you through how to download the correct files for ERM:
   ![How to Download ERM - Step 1](Screenshot2.PNG)
   ![How to Download ERM - Step 2](Screenshot3.PNG)



---

## Warnings

- **Do not use ERM on a save that goes online**.  
   Using ERM on a save file that interacts with online features can cause problems.
  
- **Important**:  
   - Do **NOT** press **"Read"** while in the main menu or loading screen, as it could interfere with the game's initialization process.
   - Ensure that your Steam path is correct before launching Seamless Coop for the first time.
   - Do not use "Launch Seamless Coop" with a working newer version of Seamless Coop.

---

## FOR DEVELOPERS ONLY

1. Clone the repository and open the `.sln` file in Visual Studio.
2. The `test` branch is recommended for developers working without the activation code.
3. The `main` branch requires an activation code.

---

## Credits

- **Credits to [Massivetwat](https://github.com/Massivetwat)'s [Swed64](https://github.com/Massivetwat/Swed64) Library.**
---
## Licensing
- **Copyright (C) 2025 VERON911 || CHALLENGER**

**You may not redistribute, modify, or sell this software without permission.**

---
## Contact Me
**Discord**: berlin0698  
**Reddit**: [Sensitive-Arma](https://www.reddit.com/user/Sensitive-Arma/)
---