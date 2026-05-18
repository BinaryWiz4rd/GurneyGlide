# Gurney Glide (Emergency Transport Sim)
---

## 🎮 About the Game
Gurney Glide is a 3D first-person simulation game where you play as an orderly pushing a heavy wheelchair through an abandoned asylum. The goal is to reach the invisible target zone downstairs without killing your patient. Every time you crash into walls or objects, the patient's heart rate (BPM) spikes. When you stop moving and rest, the heart rate slowly goes back down.

<img width="1416" height="734" alt="obraz" src="https://github.com/user-attachments/assets/a09cbdbb-8fcc-4cab-b971-10591b7f9877" />

<img width="1416" height="779" alt="obraz" src="https://github.com/user-attachments/assets/135d704d-3460-45f9-8e02-6d344db7b689" />

---

## 🛠️ Main Features & How They Work

### 1. Scene Management
The game uses two scenes to handle the flow:
* **`MainMenu`:** The opening screen with a "Start" button to launch the game.
* **`GameScene`:** The actual gameplay loop containing the asylum map (WIP), the wheelchair, and the user interface.

<img width="1108" height="843" alt="obraz" src="https://github.com/user-attachments/assets/4a885ddf-f6c2-4613-8896-e01e82aaffc3" />
<img width="1213" height="633" alt="obraz" src="https://github.com/user-attachments/assets/b60c14e7-177c-4808-b5b0-3e65a6855f38" />


### 2. User Interface (UI)
* **Heart Rate Monitor:** A TextMeshPro UI element anchored to the corner of the screen tracks the patient's BPM in real time. 
* **Color Warning System:** The text color automatically changes depending on the danger level. It turns bright red when the heart rate goes over 120 BPM to show that the patient is in critical danger.

### 3. Physics Simulation
* **Real Weight:** The wheelchair has `isKinematic` turned off so it obeys real-world physics and cannot pass through solid walls.
* **The Grab System:** When you press the **E key** near the handles, the game dynamically adds a `FixedJoint` component. This acts like a weld, physically connecting the chair to your character so it feels heavy when pushed.
* **Collision Detection:** The script uses `OnCollisionEnter` to check how hard you hit a wall. It calculates the force of the crash and adds it directly to the patient's heart rate.
* **The Layer Matrix Fix:** At first, the player and wheelchair colliders overlapped, creating crazy glitches. I fixed this by opening the Project Physics Settings and making sure the `Player` layer and `Wheelchair` layer completely ignore each other.

### 4. 3D Models & Licensing
Since a free clean hospital asset pack wasn't available, I used a high-quality horror asset instead to create a dark, tense atmosphere.
* **Map Asset:** Abandoned Asylum Kit (Standard Unity Asset Store EULA): https://assetstore.unity.com/packages/3d/environments/urban/abandoned-asylum-49137?srsltid=AfmBOorF0ESqGDCs8qWeCKdVKc7_hW2UknMUunV2evp4QQUwBkl1DsOC

### 5. Bug Fixes & Testing
* **No Wall Ghosting:** Changed collision detection to **Continuous** so the heavy chair never slips through thin walls.
* **Anti-Flying Fix:** Reset `linearVelocity` to zero in `FixedUpdate` when the chair is let go so it doesn't float away or glitch out.
* **UI Scaling:** Anchored the UI elements correctly so the text looks perfect on all monitor sizes (1080p, QHD, etc.).

---

## ⚖️ Changes from the Original Plan (List 8 Requirements)

During development, some things had to be changed to keep the game stable and playable:

* **ECG Wave Graph:** The original plan was to use a `LineRenderer` component. However, Line Renderers are very buggy on UI canvas screens across different resolutions. I switched to a procedural UI image graph instead to keep it looking crisp (still WIP).
* **Distance Tracker / Map:** I originally wanted a progress bar or a player map. Because the asylum layout has two floors, creating a working map was too difficult. Instead, I simplified the goal into a large invisible cube with `isTrigger` turned on. The player just needs to find this area to clear the level.
* **End Game Stats Screen:** Instead of building a whole separate scene for end-game stats, final times and stress levels are sent directly to the Unity developer console (WIP for improvement of that aspect).

---
