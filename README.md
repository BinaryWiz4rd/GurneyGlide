<div align="center">
  <h1>GURNEY GLIDE</h1>
  <p><i>emergency medical transport simulation built in unity</i></p>
</div>

---

## about the game

> **project description:** gurney glide is a 3d first-person simulation game where you play as an orderly pushing a heavy wheelchair through an abandoned asylum. the objective is to safely navigate the environment and reach the target zone downstairs without causing your patient to flatline.

every time you crash into walls or objects, the patient's heart rate (bpm) spikes. when you stop moving or navigate safely, the heart rate gradually cools down back toward the baseline (70 bpm).

<img width="1416" height="734" alt="obraz" src="https://github.com/user-attachments/assets/a09cbdbb-8fcc-4cab-b971-10591b7f9877" />

<img width="1416" height="779" alt="obraz" src="https://github.com/user-attachments/assets/135d704d-3460-45f9-8e02-6d344db7b689" />

---

## main features & how they work

### 1. scene management
the project architecture separates the game flow into two distinct scenes:

| scene name | core purpose |
| :--- | :--- |
| **`MainMenu`** | entry point screen handling scene transition logic via unity's `SceneManager` |
| **`GameScene`** | primary gameplay loop including physics calculations, assets, and the ui canvas |

<img width="1108" height="843" alt="obraz" src="https://github.com/user-attachments/assets/4a885ddf-f6c2-4613-8896-e01e82aaffc3" />
<img width="1213" height="633" alt="obraz" src="https://github.com/user-attachments/assets/b60c14e7-177c-4808-b5b0-3e65a6855f38" />

### 2. user interface (ui)
* **heart rate monitor:** a textmeshpro component updating in real-time to reflect the patient's internal stress level.
* **dynamic interaction prompt:** an overlay alert (`press e to start transport`) configured to render only when the player enters the interaction radius of the wheelchair asset.
* **color warning system:** the ui shifts text color dynamically based on danger thresholds, flashing bright red when the calculated heart rate exceeds 120 bpm.

### 3. physics simulation
* **real weight:** the wheelchair has `isKinematic` disabled. it completely relies on gravity, angular drag, and environmental friction, preventing it from passing through static geometry.
* **the grab system:** pressing the interaction key dynamically instantiates a `FixedJoint` component via script. this links the wheelchair directly to the player's rigidbody, physically transferring its mass and inertia.
* **collision detection:** the controller listens to physics impacts through `OnCollisionEnter`. it calculates the relative velocity of the crash and translates the impact force directly into a heart rate spike.
* **the layer matrix fix:** to prevent severe jitter and clipping errors caused by overlapping rigidbodies, a custom physics layer matrix was implemented. the `Player` and `Wheelchair` layers ignore each other entirely while retaining full physics calculations with the asylum walls.

### 4. 3d models & licensing
due to the limitations of available free hospital assets, a modular horror asset pack was chosen to create a high-contrast, tense environment.
* **map asset:** abandoned asylum kit (standard unity asset store eula): [store link](https://assetstore.unity.com/packages/3d/environments/urban/abandoned-asylum-49137)

### 5. bug fixes & quality of life
* **no wall ghosting:** upgraded physics collision detection to **continuous** on the wheelchair rigidbody to eliminate wall-clipping at maximum velocities.
* **anti-flying fix:** the script damps and drops `linearVelocity` to zero within `FixedUpdate` the exact frame the wheelchair is released, removing inertia bugs.
* **ui scaling resolution fix:** integrated a canvas scaler set to `scale with screen size (1920x1080)`. this maintains proper anchoring and layout aspect ratios on 1080p, qhd, and 4k displays.
* **floor path triggers:** navigation marker meshes are configured with `isTrigger = true` so the wheelchair can pass through them without registering physics bumps or false impact spikes.

---

## changes from the original plan (list 8 requirements)

during development, certain architecture adjustments were introduced to preserve game performance and script stability:

* **ecg wave graph:** the original concept required a `LineRenderer`. due to scaling artifacts across different aspect ratios on a responsive ui canvas, the tracking logic was moved to a procedural layout. the visual graph wave remains disabled in the final release to prevent rendering bugs.
* **distance tracker / map:** a linear progress bar was inaccurate due to the vertical multi-floor structure of the asylum layout. instead, the objective was adapted into a large trigger volume (`WinZone`). reaching this specific zone successfully completes the level.
* **complete game loop & end screens:** instead of sending status data directly to the unity developer console, an interactive ui navigation loop handles the end-game states:
  * **victory state:** triggering the `WinZone` locks player input and freezes the current heart rate. the script checks if the patient is attached, awarding **50 points** for a solo escape or **100 points** for a successful transport. a built-in timer displays completion speed in an `MM:SS` format.
  * **failure state:** if the heart rate reaches **180 bpm**, the joint breaks, input drops, the display flatlines to `0 bpm` (red), and a **"YOU DIED"** overlay blocks the view.
  * **ui navigation:** both states unlock mouse visibility (`CursorLockMode.None`), allowing players to click **RESTART** to reload the level or **EXIT** (anchored in the top-right corner) to close the application. the **escape** key functions as a global toggle to free or lock the mouse at any point during active gameplay.

---

## controls

| input | action |
| :--- | :--- |
| **W, A, S, D** | movement controls |
| **mouse** | look around / camera rotation |
| **space** | jump |
| **E** | interact (grab / release wheelchair) |
| **escape** | toggle mouse cursor lock status |
| **ui buttons** | click **restart** or **exit** on the overlay screens |
